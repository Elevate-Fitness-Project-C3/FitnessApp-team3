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
        public const string ClientName = "FitnessProfile";
        private readonly IHttpClientFactory _httpFactory;
        private readonly ILogger<GetUserTdeeQueryHandler> _logger;

        public GetUserTdeeQueryHandler(IHttpClientFactory httpFactory, ILogger<GetUserTdeeQueryHandler> logger)
        {
            _httpFactory = httpFactory;
            _logger = logger;
        }
        public async Task<FitnessProfileTdeeDto> Handle(GetUserTdeeQuery request, CancellationToken ct)
        {
            try
            {
                var client = _httpFactory.CreateClient(ClientName);
                using var response = await client.GetAsync("api/fitness-profiles/me/tdee", ct);
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
                var payload = await response.Content.ReadFromJsonAsync<TdeeResponseDto>(ct);
                if (payload?.tdee is null or <= 0)
                {
                    return new FitnessProfileTdeeDto(
                        FitnessProfileTdeeStatus.ProfileIncomplete,
                        Tdee: null,
                        ErrorMessage: "TDEE value is missing or invalid in fitness profile.");

                }
                return new FitnessProfileTdeeDto(
                    FitnessProfileTdeeStatus.Available,
                    Tdee: payload.tdee.Value,
                    CalculatedAt: payload.CalculatedAt);
            }
            catch (Exception ex) when (
            ex is HttpRequestException or
                  JsonException or
                 TaskCanceledException && !ct.IsCancellationRequested)
            {
                _logger.LogError(ex, "Failed To Retrieve TDEE From fitness-Profile Service.");
                return new FitnessProfileTdeeDto(
                    FitnessProfileTdeeStatus.ServiceUnavailable,
                    Tdee: null,
                    ErrorMessage: "Unable To reach Fitness-Profile Service.");
            }
        }
        private sealed record TdeeResponseDto(decimal? tdee, DateTime? CalculatedAt);
    }

}
