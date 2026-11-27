using MediatR;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TrainingService.Domain.Entities;
using TrainingService.Dtos.Shared;
using TrainingService.Features.Exercises.CreateExercise;
using TrainingService.Infrastructure.DataAccess;

namespace TrainingService.Features.Exercises.UpdateExercise
{
    public class UpdateExerciseCommandHandler : IRequestHandler<UpdateExerciseCommand, RequestResponse<bool>>
    {
        private readonly IGenericRepository<Exercise> _genericRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateExerciseCommandHandler(IGenericRepository<Exercise> genericRepository, IUnitOfWork unitOfWork)
        {
            _genericRepository = genericRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RequestResponse<bool>> Handle(UpdateExerciseCommand request, CancellationToken cancellationToken)
        {

            var exercise = await _genericRepository.AnyAsync(x => x.Id == request.ExerciseId && !x.IsDeleted);

            if (!exercise)
            {
                return RequestResponse<bool>.Fail("Exercise not found", 404);
            }

            var updatedExercise = new Exercise
            {
                Id = request.ExerciseId,
                Name = request.UpdateExerciseDto.Name,
                Description = request.UpdateExerciseDto.Description,
                TargetMuscleGroup = request.UpdateExerciseDto.TargetMuscleGroup,
                TrackingType = request.UpdateExerciseDto.TrackingType,
                IsActive = request.UpdateExerciseDto.IsActive,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.ExecuteAsync(async () => {
                _genericRepository.SaveInclude(
                updatedExercise,
                nameof(Exercise.Name),
                nameof(Exercise.Description),
                nameof(Exercise.TargetMuscleGroup),
                nameof(Exercise.TrackingType),
                nameof(Exercise.IsActive),
                nameof(Exercise.UpdatedAt)
            );
            }, cancellationToken);
            return RequestResponse<bool>.Ok(true, "Exercise updated successfully.");
        }
    }
}
