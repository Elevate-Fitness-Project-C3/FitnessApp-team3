

using ProgressService.Infrastructure.Persistence;

namespace ProgressService.Infrastructure.Persistence
{
    public static class DbInitializer
    {
        public static void Seed(ProgressDbContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}