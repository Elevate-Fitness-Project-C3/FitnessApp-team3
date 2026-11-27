using MediatR;
using Microsoft.EntityFrameworkCore;
using TrainingService.Domain.Entities;
using TrainingService.Dtos.Shared;
using TrainingService.Features.TrainingPrograms.CheckTrainingProgramIsExist;
using TrainingService.Features.Workouts.CheckWorkoutIsExist;
using TrainingService.Infrastructure.DataAccess;

namespace TrainingService.Features.TrainingPrograms.AssignWorkoutToProgram {
    public class AssignWorkoutToProgramOrchestratorHandler : IRequestHandler<AssignWorkoutToProgramOrchestrator, RequestResponse<Guid>> {

        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;
        private readonly IGenericRepository<ProgramWorkout> programWorkoutRepository;


        public AssignWorkoutToProgramOrchestratorHandler(IUnitOfWork unitOfWork, IMediator mediator, IGenericRepository<ProgramWorkout> programWorkoutRepository) {
            this.unitOfWork = unitOfWork;
            this.mediator = mediator;
            this.programWorkoutRepository = programWorkoutRepository;
        }

        public async Task<RequestResponse<Guid>> Handle(AssignWorkoutToProgramOrchestrator request, CancellationToken cancellationToken) {

            // 1. Check training program
            var trainingProgram = await mediator.Send(new GetTrainingProgramDaysAndWeeksQuery(request.RequestDto.TrainingProgramId), cancellationToken);

            if (!trainingProgram.Success || trainingProgram.Data == null) {
                return new RequestResponse<Guid> {
                    Success = false,
                    StatusCode = 404,
                    Message = "Training program not found."
                };
            }

            // 2. Check workout 
            var workout = await mediator.Send(new CheckWorkoutIsExistQuery(request.RequestDto.WorkoutId), cancellationToken);

            if (!workout.Success || !workout.Data) {
                return new RequestResponse<Guid> {
                    Success = false,
                    StatusCode = 404,
                    Message = "Workout  not found."
                };
            }


            // 3. Validate week number
            if (request.RequestDto.WeekNumber < 1 || request.RequestDto.WeekNumber > trainingProgram.Data.DurationWeeks)

                return new RequestResponse<Guid> {
                    Success = false,
                    StatusCode = 400,
                    Message = $"Week number must be between 1 and {trainingProgram.Data.DurationWeeks}."
                };


            // 4. Validate day number
            if (request.RequestDto.DayNumber < 1 || request.RequestDto.DayNumber > trainingProgram.Data.DaysPerWeek) {
                return new RequestResponse<Guid> {
                    Success = false,
                    StatusCode = 400,
                    Message = $"Day number must be between 1 and {trainingProgram.Data.DaysPerWeek}."
                };
            }
            // 5. Validate duplicate slot
            var slotExists = await programWorkoutRepository
                .GetAll()
                .AnyAsync(x =>
                    x.TrainingProgramId == request.RequestDto.TrainingProgramId &&
                    x.WeekNumber == request.RequestDto.WeekNumber &&
                    x.DayNumber == request.RequestDto.DayNumber,
                    cancellationToken);

            if (slotExists)
                return new RequestResponse<Guid> {
                    Success = false,
                    StatusCode = 409,
                    Message = "A workout is already assigned to this slot."
                };
            // 6. Create assignment
            var programWorkout = new ProgramWorkout {
                TrainingProgramId = request.RequestDto.TrainingProgramId,
                WorkoutId = request.RequestDto.WorkoutId,
                WeekNumber = request.RequestDto.WeekNumber,
                DayNumber = request.RequestDto.DayNumber
            };

            // 7. Persist assignment
            await programWorkoutRepository.AddAsync(programWorkout);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new RequestResponse<Guid> {
                Data = programWorkout.Id,
                StatusCode = 201,
                Success = true,
                Message = "Workout assigned to training program successfully."
            };
        }
    }
}
