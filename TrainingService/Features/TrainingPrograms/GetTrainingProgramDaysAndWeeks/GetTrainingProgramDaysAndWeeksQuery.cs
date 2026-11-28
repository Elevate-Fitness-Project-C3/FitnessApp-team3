using MediatR;
using TrainingService.Dtos.Shared;
using TrainingService.Dtos.TrainingPrograms;

namespace TrainingService.Features.TrainingPrograms.CheckTrainingProgramIsExist {
    public record GetTrainingProgramDaysAndWeeksQuery(Guid TrainingProgramId) : IRequest<RequestResponse<GetTrainingProgramDaysAndWeeksDto>>;
 }
