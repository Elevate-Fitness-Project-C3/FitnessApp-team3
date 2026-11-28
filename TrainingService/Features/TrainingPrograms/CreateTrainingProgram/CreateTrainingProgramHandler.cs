using MediatR;
using TrainingService.Domain.Entities;
using TrainingService.Dtos.Shared;
using TrainingService.Infrastructure.DataAccess;

namespace TrainingService.Features.TrainingPrograms.CreateTrainingProgram {
    public class GetTrainingProgramQueryHandler : IRequestHandler<GetTrainingProgramCommand, RequestResponse<Guid>> {

        private readonly IGenericRepository<TrainingProgram> trainingProgramRepository;
        private readonly IUnitOfWork unitOfWork;


        public GetTrainingProgramQueryHandler(IGenericRepository<TrainingProgram> trainingProgramRepository, IUnitOfWork unitOfWork) {
            this.trainingProgramRepository = trainingProgramRepository;
            this.unitOfWork = unitOfWork;

        }

        public async Task<RequestResponse<Guid>> Handle(GetTrainingProgramCommand request, CancellationToken cancellationToken) {

            var dto = request.trainingProgramDto;
            if (dto == null) {
                throw new ArgumentNullException(nameof(dto), "Training program Data cannot be null.");
            }
            var trainingProgram = new TrainingProgram {
                Title = dto.Title,
                Description = dto.Description,
                Goal = dto.Goal,
                RequiredLevel = dto.RequiredLevel,
                DurationWeeks = dto.DurationWeeks,
                DaysPerWeek = dto.DaysPerWeek,
                AccessType = dto.AccessType
            };

            await unitOfWork.ExecuteAsync(async () => {
                await trainingProgramRepository.AddAsync(trainingProgram);

            }, cancellationToken);


            return new RequestResponse<Guid> {
                Data = trainingProgram.Id,
                StatusCode = 201,
                Success = true,
                Message = "Training program created successfully."
            };
        }

    }
}

