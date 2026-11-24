using MediatR;
using TrainingService.Dtos.Exercises;
using TrainingService.Dtos.Shared;

namespace TrainingService.Features.Exercises.GetExercises
{

    public record GetExercisesQuery(GetExercisesRequestDto RequestDto): IRequest<RequestResponse<PaginatedResult<GetExercisesDto>>>;
}
