using MediatR;
using TrainingService.Domain.Entities;
using TrainingService.Dtos.Shared;
using TrainingService.Infrastructure.DataAccess;

namespace TrainingService.Features.Exercises.CreateExercise
{
    public class CreateExerciseCommandHandler : IRequestHandler<CreateExerciseCommand, RequestResponse<Guid>>
    {

        private readonly IGenericRepository<Exercise> _genericRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateExerciseCommandHandler(IGenericRepository<Exercise> genericRepository, IUnitOfWork unitOfWork)
        {
            _genericRepository = genericRepository;
            _unitOfWork = unitOfWork;
        }


        public async Task<RequestResponse<Guid>> Handle(CreateExerciseCommand request, CancellationToken cancellationToken)
        {
            var exercise = new Exercise
            {
                Id = Guid.NewGuid(),
                Name = request.CreateExerciseDto.Name,
                Description = request.CreateExerciseDto.Description,
                TargetMuscleGroup = request.CreateExerciseDto.TargetMuscleGroup,
                TrackingType = request.CreateExerciseDto.TrackingType,
                IsActive = true
            };

            await _unitOfWork.ExecuteAsync(async () => { await _genericRepository.AddAsync(exercise); }, cancellationToken);

            return RequestResponse<Guid>.Created( exercise.Id,"Exercise created successfully");
        }
    }
}
