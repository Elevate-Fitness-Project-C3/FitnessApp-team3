using MediatR;
using Microsoft.EntityFrameworkCore;
using TrainingService.Domain.Entities;
using TrainingService.Dtos.Exercises;
using TrainingService.Dtos.Shared;
using TrainingService.Dtos.TrainingPrograms;
using TrainingService.Infrastructure.DataAccess;

namespace TrainingService.Features.TrainingPrograms.GetTrainingPrograms {
    public class GetTrainingProgramQueryHandler : IRequestHandler<GetTrainingProgramQuery, RequestResponse<PaginatedResult<GetTrainingProgramDto>>> {

        private readonly IGenericRepository<TrainingProgram> trainingProgramRepository;


        public GetTrainingProgramQueryHandler(IGenericRepository<TrainingProgram> trainingProgramRepository) {
            this.trainingProgramRepository = trainingProgramRepository;

        }



        async Task<RequestResponse<PaginatedResult<GetTrainingProgramDto>>> IRequestHandler<GetTrainingProgramQuery, RequestResponse<PaginatedResult<GetTrainingProgramDto>>>.Handle(GetTrainingProgramQuery request, CancellationToken cancellationToken) {

            var training = trainingProgramRepository.GetAll().AsNoTracking().Where(e => !e.IsDeleted);

            var totalCount = await training.CountAsync(cancellationToken);

            var items = await training
                .OrderBy(e => e.Id)
                .Skip(request.requestDto.PageIndex * request.requestDto.PageSize)
                .Take(request.requestDto.PageSize)
                .Select(d => new GetTrainingProgramDto {
                    Id = d.Id,
                    Title = d.Title,
                    Description = d.Description,
                    Status = d.Status,
                    Goal = d.Goal,
                    RequiredLevel = d.RequiredLevel,
                    DurationWeeks = d.DurationWeeks,
                    DaysPerWeek = d.DaysPerWeek,
                    AccessType = d.AccessType
                })
                .ToListAsync(cancellationToken);

            var result = PaginatedResult<GetTrainingProgramDto>.Create(
                items,
                totalCount,
                request.requestDto.PageIndex,
                request.requestDto.PageSize);

         

            return new RequestResponse<PaginatedResult<GetTrainingProgramDto>> {
                Data = result,
                StatusCode = 200,
                Success = true,
                Message = $"Training programs retrieved successfully. with total count {totalCount}"
            };
        }
    }
}
