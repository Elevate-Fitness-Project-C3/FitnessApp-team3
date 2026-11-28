using MediatR;
using Microsoft.EntityFrameworkCore;
using TrainingService.Domain.Entities;
using TrainingService.Dtos.Shared;
using TrainingService.Dtos.TrainingPrograms;
using TrainingService.Infrastructure.DataAccess;

namespace TrainingService.Features.TrainingPrograms.CheckTrainingProgramIsExist {
    public class GetTrainingProgramDaysAndWeeksQueryHandler : IRequestHandler<GetTrainingProgramDaysAndWeeksQuery, RequestResponse<GetTrainingProgramDaysAndWeeksDto>> {

        private readonly IGenericRepository<TrainingProgram> trainingProgramRepository;

        public GetTrainingProgramDaysAndWeeksQueryHandler(IGenericRepository<TrainingProgram> trainingProgramRepository) {
            this.trainingProgramRepository = trainingProgramRepository;
        }



        public async Task<RequestResponse<GetTrainingProgramDaysAndWeeksDto>> Handle(GetTrainingProgramDaysAndWeeksQuery request, CancellationToken cancellationToken) {
            if (request == null)
                throw new ArgumentNullException(nameof(request));


            if (request.TrainingProgramId == Guid.Empty)
                return new RequestResponse<GetTrainingProgramDaysAndWeeksDto> {
                    Success = false,
                    StatusCode = 400,
                    Message = "Training program ID cannot be empty."
                };

            var trianingProgram = await trainingProgramRepository.GetAll()
                .Where(x => x.Id == request.TrainingProgramId && !x.IsDeleted).Select(x => new GetTrainingProgramDaysAndWeeksDto {
                    Id = x.Id , 
                    Status = x.Status,
                    DaysPerWeek = x.DaysPerWeek,
                    DurationWeeks = x.DurationWeeks
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (trianingProgram == null)
                return new RequestResponse<GetTrainingProgramDaysAndWeeksDto> {
                    Success = false,
                    StatusCode = 404,
                    Message = "Training program not found."
                };

            return new RequestResponse<GetTrainingProgramDaysAndWeeksDto> {
                Data = trianingProgram,
                StatusCode = 200,
                Success = true,
                Message = "Training Program retrieved successfully."
            };
        }
    }
}
