using IdentityService.Domain.Repositories;
using IdentityService.Features.Identity.Register.Queries;
using IdentityService.Common.Middleware;
using IdentityService.Features.Identity.VerifyEmailOtp.Commands;
using IdentityService.Features.Identity.VerifyEmailOtp.Queries;
using MediatR;

namespace IdentityService.Features.Identity.VerifyEmailOtp.Orchestrators
{
    public record VerifyOtpOrchestrator(
        string Email,
        string Otp
    ):IRequest<bool>;
    public class VerifyOtpOrchestratorHandler (IUnitOfWork unitOfWork,ISender sender): IRequestHandler<VerifyOtpOrchestrator, bool>
    {
        public async Task<bool> Handle(VerifyOtpOrchestrator request, CancellationToken cancellationToken)
        {
            var userId = await sender.Send(new GetUserIdByEmailQuery(request.Email), cancellationToken);
            if (userId is null)
                return false;

            var verified = false;
            await unitOfWork.ExecuteAsync(async () =>
            {
                if (!await sender.Send(new BeginEmailOtpUpdateCommand(userId.Value), cancellationToken))
                    return;

                var validation = await sender.Send(new ChickIfOtpValidQuery(userId.Value, request.Otp), cancellationToken);
                if (!validation.IsValid)
                {
                    if (validation.FailedOtpId is int failedOtpId)
                        await sender.Send(new RecordFailedEmailOtpAttemptCommand(userId.Value, failedOtpId), cancellationToken);

                    return;
                }

                var isMarkedAsUsed = await sender.Send(new MarkOtpAsUsedCommand(userId.Value, request.Otp), cancellationToken);
                if (!isMarkedAsUsed)
                    return;

                if (!await sender.Send(new ActivateUserCommend(userId.Value), cancellationToken))
                    throw new NotFoundException("User not found.");

                verified = true;
            }, cancellationToken);
            return verified;
        }
    }   

}
