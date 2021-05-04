using Microsoft.EntityFrameworkCore;

namespace AspNetTemplate.Infrastructure.DataAccess
{
    public class AspNetTemplateContext : DbContext
    {
        public AspNetTemplateContext(DbContextOptions<AspNetTemplateContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}