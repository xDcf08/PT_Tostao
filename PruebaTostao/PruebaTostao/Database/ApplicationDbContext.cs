using Microsoft.EntityFrameworkCore;
using PruebaTostao.Entities.Abstractions;
using PruebaTostao.Entities.DocumentEntity;
using PruebaTostao.Exceptions;
using System.Data;

namespace PruebaTostao.Database
{
    public class ApplicationDbContext : DbContext, IUnitOfWork
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Document> Documents { get; set; }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return base.SaveChangesAsync(cancellationToken);
            }
            catch (DBConcurrencyException ex)
            {
                throw new ConcurrencyException("concurrency exception", ex);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
