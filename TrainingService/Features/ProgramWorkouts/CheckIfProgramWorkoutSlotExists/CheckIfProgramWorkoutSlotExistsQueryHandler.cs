using MediatR;
using Microsoft.EntityFrameworkCore;
using TrainingService.Domain.Entities;
using TrainingService.Dtos.Shared;
using TrainingService.Infrastructure.DataAccess;

namespace TrainingService.Features.ProgramWorkouts.CheckIfProgramWorkoutSlotExists {
    public class CheckIfProgramWorkoutSlotExistsQueryHandler : IRequestHandler<CheckIfProgramWorkoutSlotExistsQuery, RequestResponse<bool>> {

        private readonly IGenericRepository<ProgramWorkout>
       programWorkoutRepository;

        public CheckIfProgramWorkoutSlotExistsQueryHandler(IGenericRepository<ProgramWorkout> programWorkoutRepository) {
            this.programWorkoutRepository = programWorkoutRepository;
        }

        public async Task<RequestResponse<bool>> Handle(CheckIfProgramWorkoutSlotExistsQuery request, CancellationToken cancellationToken) {

            var exists = await programWorkoutRepository.GetAll()
                       .AnyAsync(x =>
                           x.TrainingProgramId == request.TrainingProgramId &&
                           x.WeekNumber == request.WeekNumber &&
                           x.DayNumber == request.DayNumber,
                           cancellationToken);

            return new RequestResponse<bool> {
                Data = exists,
                Success = true,
                StatusCode = 200,
                Message = exists
                    ? "Slot already exists."
                    : "Slot is available."
            };
        }
    }
}
