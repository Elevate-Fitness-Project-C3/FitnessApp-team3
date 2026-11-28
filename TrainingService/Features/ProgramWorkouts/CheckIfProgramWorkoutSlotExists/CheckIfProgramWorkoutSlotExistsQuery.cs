using MediatR;
using TrainingService.Dtos.Shared;

namespace TrainingService.Features.ProgramWorkouts.CheckIfProgramWorkoutSlotExists {
    public record CheckIfProgramWorkoutSlotExistsQuery(Guid TrainingProgramId,int WeekNumber,int DayNumber) : IRequest<RequestResponse<bool>>;
}
