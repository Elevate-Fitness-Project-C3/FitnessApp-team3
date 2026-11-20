using MediatR;
using NutritionService.Features.CalorieTarget.DTOs;
using NutritionService.Features.CalorieTarget.Enums;
using NutritionService.Features.CalorieTarget.Queries;
using System.Net;
using System.Text.Json;

namespace NutritionService.Features.CalorieTarget.Handlers
{
    public class GetUserTdeeQueryHandler : IRequestHandler<GetUserTdeeQuery, FitnessProfileTdeeDto>
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<GetUserTdeeQueryHandler> _logger;

        public GetUserTdeeQueryHandler(HttpClient httpClient, ILogger<GetUserTdeeQueryHandler> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<FitnessProfileTdeeDto> Handle(GetUserTdeeQuery request, CancellationToken ct)
        {
            try
            {
                using var response = await _httpClient.GetAsync("/api/fitness-profiles/me/tdee", ct);
                if (response.StatusCode is HttpStatusCode.NotFound or HttpStatusCode.UnprocessableEntity)
                {
                    return new FitnessProfileTdeeDto(
                        FitnessProfileTdeeStatus.ProfileIncomplete,
                        Tdee: null,
                        ErrorMessage: "User fitness profile is incomplete or missing.");
                }
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning(
                        "FitnessProfileService returned non-success status code {StatusCode}.",
                        (int)response.StatusCode);
                    return new FitnessProfileTdeeDto(
                        FitnessProfileTdeeStatus.ServiceUnavailable,
                        Tdee: null,
                        ErrorMessage: "Fitness profile service is currently unavailable.");
                }
                var payload= await response.Content.ReadFromJsonAsync<TdeeResponseDto>(ct);
                if(payload?.tdee is null or <= 0)
                {
                    return new FitnessProfileTdeeDto(
                        FitnessProfileTdeeStatus.ProfileIncomplete,
                        Tdee: null,
                        ErrorMessage: "TDEE value is missing or invalid in fitness profile.");

                }
                return new FitnessProfileTdeeDto(
                    FitnessProfileTdeeStatus.Available,
                    Tdee: payload.tdee.Value,
                    CalculatedAt:payload.CalculatedAt);
            }
            catch (Exception ex) when (
            ex is HttpRequestException or
            JsonException or 
            TaskCanceledException && !ct.IsCancellationRequested)
            {
                _logger.LogError("Failed To Retrive TDEE From fitness-Profile Service.");
                return new FitnessProfileTdeeDto(
                    FitnessProfileTdeeStatus.ServiceUnavailable,
                    Tdee: null,
                    ErrorMessage: "Unable To reach Fitness-Profile Service.");
            }
        }
        private sealed record TdeeResponseDto(decimal? tdee , DateTime ? CalculatedAt);
    }

}
