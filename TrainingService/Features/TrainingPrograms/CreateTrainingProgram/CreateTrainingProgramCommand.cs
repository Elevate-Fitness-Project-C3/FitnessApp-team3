using MediatR;
using TrainingService.Dtos.Shared;
using TrainingService.Dtos.TrainingPrograms;

namespace TrainingService.Features.TrainingPrograms.CreateTrainingProgram {
    public record GetTrainingProgramCommand(CreateTrainingProgramDto trainingProgramDto) : IRequest<RequestResponse<Guid>>;
}