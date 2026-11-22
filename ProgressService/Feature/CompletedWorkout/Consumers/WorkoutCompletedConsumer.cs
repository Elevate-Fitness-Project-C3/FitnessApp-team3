using ProgressService.Feature.CompletedWorkout.Command;
using ProgressService.Feature.Shared.EventContracts;
using MassTransit;
using MediatR;

namespace ProgressService.Feature.CompletedWorkout.Consumers
{
    public class WorkoutCompletedConsumer : IConsumer<WorkoutCompleted>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<WorkoutCompletedConsumer> _log;
        public WorkoutCompletedConsumer(IMediator mediator, ILogger<WorkoutCompletedConsumer> log)
        { _mediator = mediator; _log = log; }

        public async Task Consume(ConsumeContext<WorkoutCompleted> context)
        {
            var m = context.Message;

            if (m.SessionId == Guid.Empty || m.UserId == Guid.Empty ||
                m.ProgramId == Guid.Empty || m.ProgramWorkoutId == Guid.Empty ||
                m.CompletedAt == default)
            {
                // exception sends it to the _error queue for inspection.
                throw new InvalidEventException($"Invalid WorkoutCompleted: {m.SessionId}");
            }

            var result = await _mediator.Send(new CreateCompletedWorkoutCommand(
            m.SessionId, m.UserId, m.ProgramId,
            m.ProgramWorkoutId, m.CompletedAt, m.Version), context.CancellationToken);

            _log.LogInformation("WorkoutCompleted {SessionId} => {Result}", m.SessionId, result);
        }
    }
    public class InvalidEventException(string msg) : Exception(msg);
}
