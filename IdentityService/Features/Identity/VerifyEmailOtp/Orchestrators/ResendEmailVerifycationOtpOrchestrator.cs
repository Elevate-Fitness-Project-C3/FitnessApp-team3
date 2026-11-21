using IdentityService.Domain.Repositories;
using IdentityService.Common.Middleware;
using IdentityService.Domain.Entities.Identity;
using IdentityService.Domain.Services;
using IdentityService.Features.Identity.Register.Commands;
using IdentityService.Features.Identity.Register.Queries;
using IdentityService.Features.Identity.VerifyEmailOtp.Commands;
using MediatR;

namespace IdentityService.Features.Identity.VerifyEmailOtp.Orchestrators
{
    public record ResendEmailVerifycationOtpOrchestrator(string UserEmail):IRequest<bool>;

    public class ResendEmailVerifycationOtpOrchestratorHandler(IUnitOfWork unitOfWork,ISender sender) : IRequestHandler<ResendEmailVerifycationOtpOrchestrator, bool>
    {
        
        public async Task<bool> Handle(ResendEmailVerifycationOtpOrchestrator request, CancellationToken cancellationToken)
        {
            var userId = await sender.Send(new GetUserIdByEmailQuery(request.UserEmail), cancellationToken);
            if (userId is null)
                return false;

            SendVerificationEmailCommand? email = null;
            await unitOfWork.ExecuteAsync(async () =>
            {
                if (!await sender.Send(new BeginEmailOtpUpdateCommand(userId.Value), cancellationToken))
                    return;

                var user = await unitOfWork.GetRepository<ApplicationUser>().GetByIdAsync(userId.Value, cancellationToken);
                if (user is null)
                    return;

                await sender.Send(new RevokeAllUnusedOtpsCommand(user.Id), cancellationToken);
                var verification = await sender.Send(new CreateEmailVerificationOtpCommand(user.Id), cancellationToken);
                email = new SendVerificationEmailCommand(user.Email, verification.Code, verification.Challenge.ExpiresAt);
            }, cancellationToken);

            if (email is null)
                return false;

            var delivery = await sender.Send(email, cancellationToken);
            if (delivery != EmailDeliveryStatus.Accepted)
                throw new EmailDeliveryException(delivery, "Verification email delivery could not be confirmed. If the email arrives, use the code before it expires. Otherwise, request another code.");

            return true;
        }
     
    }

}
