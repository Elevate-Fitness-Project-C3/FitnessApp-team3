using MediatR;
using Microsoft.EntityFrameworkCore;
using TrainingService.Domain.Entities;
using TrainingService.Dtos.Shared;
using TrainingService.Dtos.TrainingPrograms;
using TrainingService.Infrastructure.DataAccess;

namespace TrainingService.Features.Workouts.CheckWorkoutIsExist {
    public class CheckWorkoutIsExistQueryHandler : IRequestHandler<CheckWorkoutIsExistQuery, RequestResponse<bool>> {


        private readonly IGenericRepository<Workout> workoutRepository;

        public CheckWorkoutIsExistQueryHandler(IGenericRepository<Workout> workoutRepository) {
             this.workoutRepository = workoutRepository;

        }
        public async Task<RequestResponse<bool>> Handle(CheckWorkoutIsExistQuery request, CancellationToken cancellationToken) {

            if (request == null) 
                throw new ArgumentNullException(nameof(request));

            if (request.WorkoutId == Guid.Empty)
                return new RequestResponse<bool> {
                    Success = false,
                    StatusCode = 400,
                    Message = "Workout ID cannot be empty."
                };


            var exists = await workoutRepository.GetAll().AnyAsync(
                x => x.Id == request.WorkoutId && !x.IsDeleted, cancellationToken);


            return new RequestResponse<bool> {
                Data = exists,
                StatusCode = 200,
                Success = true,
                Message = exists? "Workout exists.": "Workout does not exist."
            };
        }
    }
}
