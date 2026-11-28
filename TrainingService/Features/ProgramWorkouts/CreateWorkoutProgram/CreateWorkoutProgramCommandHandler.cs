using MediatR;
using TrainingService.Domain.Entities;
using TrainingService.Dtos.Shared;
using TrainingService.Infrastructure.DataAccess;

namespace TrainingService.Features.ProgramWorkouts.CreateWorkoutProgram {
    public class CreateWorkoutProgramCommandHandler : IRequestHandler<CreateWorkoutProgramCommand, RequestResponse<Guid>> {

        private readonly IGenericRepository<ProgramWorkout> programWorkoutRepository;
        private readonly IUnitOfWork unitOfWork;

        public CreateWorkoutProgramCommandHandler(IGenericRepository<ProgramWorkout> programWorkoutRepository , IUnitOfWork unitOfWork) {
            this.programWorkoutRepository = programWorkoutRepository;
            this.unitOfWork = unitOfWork;
        }
        public async Task<RequestResponse<Guid>> Handle(CreateWorkoutProgramCommand request, CancellationToken cancellationToken) {

            var dto = request.RequestDto;

            var programWorkout = new ProgramWorkout {
                TrainingProgramId = dto.TrainingProgramId,
                WorkoutId = dto.WorkoutId,
                WeekNumber = dto.WeekNumber,
                DayNumber = dto.DayNumber
            };

            await programWorkoutRepository.AddAsync(programWorkout);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new RequestResponse<Guid> {
                Data = programWorkout.Id,
                Success = true,
                StatusCode = 201,
                Message = "Workout assigned successfully."
            };
        }
    }
}
