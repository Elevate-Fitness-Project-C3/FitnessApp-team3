using MediatR;
using Microsoft.EntityFrameworkCore;
using TrainingService.Domain.Entities;
using TrainingService.Dtos.Shared;
using TrainingService.Dtos.TrainingPrograms;
using TrainingService.Infrastructure.DataAccess;

namespace TrainingService.Features.TrainingPrograms.CheckTrainingProgramIsExist {
    public class GetTrainingProgramDaysAndWeeksQueryHandler : IRequestHandler<GetTrainingProgramDaysAndWeeksQuery, RequestResponse<GetTrainingProgramDaysAndWeeks>> {

        private readonly IGenericRepository<TrainingProgram> trainingProgramRepository;

        public GetTrainingProgramDaysAndWeeksQueryHandler(IGenericRepository<TrainingProgram> trainingProgramRepository) {
            this.trainingProgramRepository = trainingProgramRepository;
        }



        public async Task<RequestResponse<GetTrainingProgramDaysAndWeeks>> Handle(GetTrainingProgramDaysAndWeeksQuery request, CancellationToken cancellationToken) {
            if (request == null)
                throw new ArgumentNullException(nameof(request));


            if (request.TrainingProgramId == Guid.Empty)
                return new RequestResponse<GetTrainingProgramDaysAndWeeks> {
                    Success = false,
                    StatusCode = 400,
                    Message = "Training program ID cannot be empty."
                };

            var trianingProgram = await trainingProgramRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == request.TrainingProgramId&& !x.IsDeleted,cancellationToken);

            if (trianingProgram == null)
                return new RequestResponse<GetTrainingProgramDaysAndWeeks> {
                    Success = false,
                    StatusCode = 404,
                    Message = "Training program not found."
                };


            return new RequestResponse<GetTrainingProgramDaysAndWeeks> {
                Data = new GetTrainingProgramDaysAndWeeks {
                    Id = trianingProgram.Id,
                    DurationWeeks = trianingProgram.DurationWeeks,
                    DaysPerWeek = trianingProgram.DaysPerWeek
                },
                StatusCode = 200,
                Success = true,
                Message = "Training Program retrieved successfully."
            };
        }
    }
}
