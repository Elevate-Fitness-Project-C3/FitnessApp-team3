using MediatR;
using TrainingService.Domain.Entities;
using TrainingService.Dtos.Shared;
using TrainingService.Features.Exercises.CreateExercise;
using TrainingService.Infrastructure.DataAccess;

namespace TrainingService.Features.Exercises.ChangeExerciseStatus
{
    public class ChangeExerciseStatusCommandHandler : IRequestHandler<ChangeExerciseStatusCommand, RequestResponse<bool>>
    {
        private readonly IGenericRepository<Exercise> _exerciseRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ChangeExerciseStatusCommandHandler(IGenericRepository<Exercise> exerciseRepository, IUnitOfWork unitOfWork)
        {
            _exerciseRepository = exerciseRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<RequestResponse<bool>> Handle(ChangeExerciseStatusCommand request, CancellationToken cancellationToken)
        {
            
            var exists = await _exerciseRepository.AnyAsync( x => x.Id == request.ExerciseId && !x.IsDeleted);
            if (!exists)
            {
                return RequestResponse<bool>.Fail("Exercise not found.", 404);
            }

            var exercise = new Exercise
            {
                Id = request.ExerciseId,
                IsActive = false,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.ExecuteAsync(async () => {
                _exerciseRepository.SaveInclude(
                exercise,
                nameof(Exercise.IsActive),
                nameof(Exercise.UpdatedAt)
            );
            }, cancellationToken);
            return RequestResponse<bool>.Ok(true, "Exercise status changed successfully.");
        }
    }
}
