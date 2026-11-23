using MediatR;
using Microsoft.EntityFrameworkCore;
using TrainingService.Domain.Entities;
using TrainingService.Domain.Enums;
using TrainingService.Dtos.Shared;
using TrainingService.Infrastructure.DataAccess;

namespace TrainingService.Features.Workouts.CreateWorkout
{
    public class CreateWorkoutCommandHandler : IRequestHandler<CreateWorkoutCommand, RequestResponse<Guid>>
    {

        private readonly IGenericRepository<Workout> _workoutRepository;
        private readonly IGenericRepository<Exercise> _exerciseRepository;
        private readonly IUnitOfWork _unitOfWork;


        public CreateWorkoutCommandHandler(IGenericRepository<Workout> workoutRepository, IGenericRepository<Exercise> exerciseRepository, IUnitOfWork unitOfWork)
        {
            _workoutRepository = workoutRepository;
            _exerciseRepository = exerciseRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RequestResponse<Guid>> Handle(CreateWorkoutCommand request, CancellationToken cancellationToken)
        {
            var dto = request.CreateWorkoutDto;

            var exerciseIds = dto.Exercises.Select(x => x.ExerciseId).Distinct().ToList();

            var activeExercises = await _exerciseRepository.Get(x => exerciseIds.Contains(x.Id) && x.IsActive && !x.IsDeleted)
                .AsNoTracking()
                .Select(x => new
                {
                    x.Id,
                    x.TrackingType
                }).ToListAsync(cancellationToken);

            if (activeExercises.Count != exerciseIds.Count)
            {
                return RequestResponse<Guid>.Fail(
                    "One or more exercises do not exist or are inactive.", 400);
            }

            var trackingTypes = activeExercises.ToDictionary(x => x.Id,
                                                             x => x.TrackingType);

            foreach (var item in dto.Exercises)
            {
                var trackingType = trackingTypes[item.ExerciseId];

                bool isValid = trackingType switch
                {
                    ExerciseTrackingType.Reps =>
                        item.Reps > 0 &&
                        item.Duration == null &&
                        item.Weight == null,

                    ExerciseTrackingType.Duration =>
                        item.Duration > 0 &&
                        item.Reps == null &&
                        item.Weight == null,

                    ExerciseTrackingType.RepsAndWeight =>
                        item.Reps > 0 &&
                        item.Weight > 0 &&
                        item.Duration == null,

                    _ => false
                };

                if (!isValid)
                {
                    return RequestResponse<Guid>.Fail(
                        $"Invalid targets for exercise {item.ExerciseId}. " +
                        $"Tracking type: {trackingType}.",
                        400);
                }
            }


            var workout = new Workout
            {
                Title = dto.Title,
                Difficulty = (TrainingLevel)dto.Difficulty,
                EstimatedDurationMinutes = dto.EstimatedDuration,

                WorkoutExercises = dto.Exercises
                    .Select(item => new WorkoutExercise
                    {
                        ExerciseId = item.ExerciseId,
                        Order = item.Order,
                        TargetSets = item.Sets,
                        TargetReps = item.Reps,
                        TargetDurationSeconds = item.Duration
                    })
                    .ToList()
            };

            await _unitOfWork.ExecuteAsync(async () =>
            {
                await _workoutRepository.AddAsync(workout);

            }, cancellationToken);

            return RequestResponse<Guid>.Created(
                workout.Id,
                "Workout created successfully.");
        }
    }
}
         