using ProgressService.Domains.Entities;
using ProgressService.Domains.Enums.CompletedWorkout;
using ProgressService.Domains.Repositories;
using ProgressService.Feature.CompletedWorkout.Command;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ProgressService.Feature.CompletedWorkout.Handler
{
    public class RecordCompletedWorkoutHandler
    : IRequestHandler<CreateCompletedWorkoutCommand, RecordResult>
    {
        private readonly IGenericRepository<CompletedWorkoutRecord> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public RecordCompletedWorkoutHandler(
            IGenericRepository<CompletedWorkoutRecord> repository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        
        public async Task<RecordResult> Handle(CreateCompletedWorkoutCommand request, CancellationToken cancellationToken)
        {
            var result = RecordResult.Created;

            try
            {
                await _unitOfWork.ExecuteAsync(async () =>
                {
                    var existingRecord = await _repository.GetAll()
                        .AnyAsync(x => x.SessionId == request.SessionId, cancellationToken);
                    if (existingRecord)
                    {
                        result = RecordResult.Duplicate;
                        return;
                    }

                    _repository.Add(CompletedWorkoutRecord.Create(
                        request.SessionId, request.UserId, request.ProgramId,
                        request.ProgramWorkoutId, request.CompletedAt, request.Version));
                }, cancellationToken);

                return result;
            }
            catch (DbUpdateException ex) when (IsUniqueViolation(ex))
            {
                return RecordResult.Duplicate;
            }
        }

        private static bool IsUniqueViolation(DbUpdateException ex) =>
        ex.InnerException is SqlException { Number: 2601 or 2627 };
    }
}
