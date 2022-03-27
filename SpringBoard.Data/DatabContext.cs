using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SpringBoard.Domaine;

namespace SpringBoard.Data
{
    public class DatabContext: IdentityDbContext<Utilisateur>
    {
        public DatabContext() : base()
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=DB_SpringBoard");
        }

        public DbSet<Utilisateur> utilisateurs { get; set; }
        public DbSet<Administrateur> administrateurs { get; set; }
        public DbSet<Commercial> Commercials { get; set; }
        public DbSet<Consultant> Consultants { get; set; }
        public DbSet<GestionnaireRH> gestionnaires { get; set; }
        public DbSet<CompteRendu> compteRendus { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Consultant>(x => x.HasMany(c => c.CompteRendus).WithOne(c => c.Consultant));

            builder.Entity<CompteRendu>(_ => _.HasMany(r => r.Rapports).WithOne(x => x.CompteRendu));

            base.OnModelCreating(builder);
        }



    }
}