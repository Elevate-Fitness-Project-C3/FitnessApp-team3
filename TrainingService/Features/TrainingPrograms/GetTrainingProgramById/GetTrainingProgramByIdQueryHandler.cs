using MediatR;
using Microsoft.EntityFrameworkCore;
using TrainingService.Domain.Entities;
using TrainingService.Dtos.Shared;
using TrainingService.Dtos.TrainingPrograms;
using TrainingService.Infrastructure.DataAccess;

namespace TrainingService.Features.TrainingPrograms.GetTrainingProgramById {
    public class CheckTrainingProgramIsExistQueryHandler : IRequestHandler<GetTrainingProgramByIdQuery, RequestResponse<GetTrainingProgramDto>> {

        private readonly IGenericRepository<TrainingProgram> trainingProgramRepository;
        public CheckTrainingProgramIsExistQueryHandler(IGenericRepository<TrainingProgram> trainingProgramRepository) {
            this.trainingProgramRepository = trainingProgramRepository;
        }

        public async Task<RequestResponse<GetTrainingProgramDto>> Handle(GetTrainingProgramByIdQuery request, CancellationToken cancellationToken) {

            if (request == null) 
                throw new ArgumentNullException(nameof(request));
            

            if (request.TrainingProgramId == Guid.Empty)
                return new RequestResponse<GetTrainingProgramDto> {
                    Success = false,
                    StatusCode = 400,
                    Message = "Training program ID cannot be empty."
                };

            var trainingProgram = await trainingProgramRepository.GetAll()
                .Where(x => x.Id == request.TrainingProgramId &&
                            !x.IsDeleted)
                .Select(x => new GetTrainingProgramDto {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    Status = x.Status,
                    RequiredLevel = x.RequiredLevel,
                    AccessType = x.AccessType,
                    DurationWeeks = x.DurationWeeks,
                    DaysPerWeek = x.DaysPerWeek
                })
                .FirstOrDefaultAsync(cancellationToken); 
            
            
            if (trainingProgram == null)
                    return new RequestResponse<GetTrainingProgramDto> {
                        Success = false,
                        StatusCode = 404,
                        Message = "Training program not found."};
                
            return new RequestResponse<GetTrainingProgramDto> {
                Data = trainingProgram,
                StatusCode = 200,
                Success = true,
                Message = "Training program retrieved successfully."
            };
        }
    }
}
