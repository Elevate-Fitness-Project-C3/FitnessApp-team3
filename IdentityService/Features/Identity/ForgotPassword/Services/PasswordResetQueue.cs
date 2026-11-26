using System.Threading.Channels;

namespace IdentityService.Features.Identity.ForgotPassword.Services;

public class PasswordResetQueue
{
    private readonly Channel<string> _requests = Channel.CreateBounded<string>(new BoundedChannelOptions(64)
    {
        SingleReader = true
    });

    public bool TryEnqueue(string email) => _requests.Writer.TryWrite(email);

    public IAsyncEnumerable<string> ReadAllAsync(CancellationToken cancellationToken)
        => _requests.Reader.ReadAllAsync(cancellationToken);
}
