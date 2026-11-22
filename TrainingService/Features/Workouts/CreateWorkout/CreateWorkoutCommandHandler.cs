using MediatR;
using TrainingService.Domain.Entities;
using TrainingService.Dtos.Shared;
using TrainingService.Infrastructure.DataAccess;

namespace TrainingService.Features.Workouts.CreateWorkout
{
    public class CreateWorkoutCommandHandler : IRequestHandler<CreateWorkoutCommand, RequestResponse<Guid>>
    {
        private readonly IGenericRepository<Exercise> _genericRepository;
        private readonly IUnitOfWork _unitOfWork;
        public Task<RequestResponse<Guid>> Handle(CreateWorkoutCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
