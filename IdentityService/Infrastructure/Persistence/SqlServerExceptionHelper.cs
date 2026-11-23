using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Persistence;

public static class SqlServerExceptionHelper
{
    public const string NormalizedEmailIndex = "UX_ApplicationUser_NormalizedEmail";

    public static bool IsNormalizedEmailConflict(DbUpdateException exception)
        => exception.InnerException is SqlException sqlException
            && sqlException.Errors.Cast<SqlError>().Any(error =>
                error.Number is 2601 or 2627
                && error.Message.Contains($"'{NormalizedEmailIndex}'", StringComparison.Ordinal));
}
