using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TrainingService.Domain.Entities;
using TrainingService.Dtos.Exercises;
using TrainingService.Dtos.Shared;
using TrainingService.Features.Exercises.CreateExercise;
using static Azure.Core.HttpHeader;

namespace TrainingService.Features.Exercises.GetExercises
{
    public class GetExercisesQueryHandler : IRequestHandler<GetExercisesQuery, RequestResponse<PaginatedResult<GetExercisesDto>>>
    {
        private readonly IGenericRepository<Exercise> _exerciseRepository;

        public GetExercisesQueryHandler(IGenericRepository<Exercise> exerciseRepository)
        {
            _exerciseRepository = exerciseRepository;
        }


        public async Task<RequestResponse<PaginatedResult<GetExercisesDto>>> Handle(GetExercisesQuery request, CancellationToken cancellationToken)
        {
            var exercises = _exerciseRepository.GetAll().AsNoTracking().Where(e => e.IsDeleted == false);

            var items = await exercises.OrderBy(e => e.Id)
                                   .Skip((request.RequestDto.PageIndex - 1) * request.RequestDto.PageSize)
                                   .Take(request.RequestDto.PageSize)
                                   .Select(d => new GetExercisesDto
                                   {
                                       Id = d.Id,
                                       Name = d.Name,
                                       Description = d.Description,
                                       TargetMuscleGroup = d.TargetMuscleGroup,
                                       TrackingTypeID = (int?)d.TrackingType,
                                       TrackingType = d.TrackingType.ToString(),
                                       IsActive = d.IsActive
                                   }).ToListAsync(cancellationToken);



           var result = PaginatedResult<GetExercisesDto>.Create(items, await exercises.CountAsync(cancellationToken), request.RequestDto.PageIndex, request.RequestDto.PageSize);
            return RequestResponse<PaginatedResult<GetExercisesDto>>.Ok(result, "Exercises retrieved successfully");
        }
    }
}
