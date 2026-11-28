using MediatR;
using TrainingService.Dtos.Exercises;
using TrainingService.Dtos.Shared;
using TrainingService.Dtos.TrainingPrograms;

namespace TrainingService.Features.TrainingPrograms.GetTrainingPrograms {
    public record GetTrainingProgramQuery(GetTrainingProgramRequestDto requestDto) : IRequest<RequestResponse<PaginatedResult<GetTrainingProgramDto>>>;


}