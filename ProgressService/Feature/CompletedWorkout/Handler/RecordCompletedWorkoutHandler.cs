using ProgressService.Domains.Entities;
using ProgressService.Domains.Enums.CompletedWorkout;
using ProgressService.Feature.CompletedWorkout.Command;
using ProgressService.Infrastructure.Persistence;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ProgressService.Feature.CompletedWorkout.Handler
{
    public class RecordCompletedWorkoutHandler
    : IRequestHandler<CreateCompletedWorkoutCommand, RecordResult>
    {
        private readonly ProgressDbContext _db;
        public RecordCompletedWorkoutHandler(ProgressDbContext db) => _db = db;
        
        public async Task<RecordResult> Handle(CreateCompletedWorkoutCommand request, CancellationToken cancellationToken)
        {
            // Check  Duplicate Record
            var existingRecord = await _db.CompletedWorkoutRecords
               .AnyAsync(x => x.SessionId == request.SessionId , cancellationToken);
            if(existingRecord)
            {
                return RecordResult.Duplicate;
            }

            _db.CompletedWorkoutRecords.Add(
                CompletedWorkoutRecord.Create(
                    request.SessionId, request.UserId, request.ProgramId,
                    request.ProgramWorkoutId, request.CompletedAt, request.Version));

            try
            {
                await _db.SaveChangesAsync(cancellationToken);
                return RecordResult.Created;

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
