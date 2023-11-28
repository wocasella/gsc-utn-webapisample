using EjWebApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace EjWebApi.DataAccess
{
    public class AcademiaDbContext : DbContext
    {
        public DbSet<Estudiante> Estudiantes { get; set; }

        public AcademiaDbContext(DbContextOptions options) : base(options) => this.Database.EnsureCreated();
    }
}
