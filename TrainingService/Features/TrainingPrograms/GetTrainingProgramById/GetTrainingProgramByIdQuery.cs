using MediatR;
using TrainingService.Dtos.Shared;
using TrainingService.Dtos.TrainingPrograms;

namespace TrainingService.Features.TrainingPrograms.GetTrainingProgramById {
    public record GetTrainingProgramByIdQuery (Guid TrainingProgramId) : IRequest<RequestResponse<GetTrainingProgramDto>>;
 }
