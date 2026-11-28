using MediatR;
using Microsoft.EntityFrameworkCore;
using TrainingService.Domain.Entities;
using TrainingService.Dtos.Shared;
using TrainingService.Dtos.WorkoutPrograms;
using TrainingService.Infrastructure.DataAccess;

namespace TrainingService.Features.ProgramWorkouts.GetProgramWorkouts {
    public class GetProgramWorkoutsQueryHandler : IRequestHandler<GetProgramWorkoutsQuery, RequestResponse<List<GetProgramWorkoutDto>>> {

        private readonly IGenericRepository<ProgramWorkout> programWorkoutRepository;
        public GetProgramWorkoutsQueryHandler(IGenericRepository<ProgramWorkout> programWorkoutRepository) {
            this.programWorkoutRepository = programWorkoutRepository;
        }
        
            public async Task<RequestResponse<List<GetProgramWorkoutDto>>> Handle(GetProgramWorkoutsQuery request,CancellationToken cancellationToken) {

            var workouts = await programWorkoutRepository.GetAll().Where(x =>
                                     x.TrainingProgramId == request.Guid &&
                                    !x.IsDeleted &&
                                    !x.TrainingProgram.IsDeleted)
                                    .OrderBy(x => x.WeekNumber)
                                    .ThenBy(x => x.DayNumber)
                                    .Select(x => new GetProgramWorkoutDto {
                                             Id = x.Id,
                                            WorkoutId = x.WorkoutId,
                                            WeekNumber = x.WeekNumber,
                                            DayNumber = x.DayNumber }).ToListAsync(cancellationToken);

            if (workouts is null) {
                    return new RequestResponse<List<GetProgramWorkoutDto>> {
                        Success = false,
                        StatusCode = 404,
                        Message = "Training program not found."
                    };
                }

                return new RequestResponse<List<GetProgramWorkoutDto>> {
                    Data = workouts,
                    Success = true,
                    StatusCode = 200,
                    Message = "Program workouts retrieved successfully."
                };
            }
        }   
}
