using CleanArchitecture.Domain;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Data
{
    public class StreamerDbContext : DbContext
    {

        private string connectionString = stringConnectionSecret.secretString;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        {
            //En este caso al options builder le damos la opción de que nos muestr por consola
            // lo que pasa en la BDD con un cierto nivel de log
            // Además nos permite describir cada una de las operaciones con la opcion enableSensitive
            optionsBuilder
                .UseSqlServer(connectionString)
                .LogTo(Console.WriteLine, 
                new[] {DbLoggerCategory.Database.Command.Name}, Microsoft.Extensions.Logging.LogLevel.Information )
                .EnableSensitiveDataLogging();

        }


        //Nomenclatura de Fluent API
        // Si sigues las convenciones de EF usando el tema de las anclas y las nomenclaturas
        // y usas correctamente las clases, no necesitas esta nomeclatura adicional
        // aunque muchos programadores lo usan como una buean práctica para asegurarse
        // de la mantenibilidad del código
        // estarás obligado a usarlo cuadno veas que en un código otro programador 
        // no ha usado la nomenclatura de las FK y por lo tanto EF no reconoce las 
        // FK, de manera que tenemos que forzarlas nosotros
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Streamer>()
                .HasMany(m => m.Videos)
                .WithOne(m => m.Streamer)
                .HasForeignKey(m => m.StreamerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Video>()
                .HasMany(p => p.Actores)
                .WithMany(t => t.Videos)
                .UsingEntity<VideoActor>(
                    pt => pt.HasKey(e => new { e.ActorId, e.VideoId })
                );
        }

     
        
        
        
        
        // Nullable
        public DbSet<Streamer>? Streamers { get; set; }

        public DbSet<Video>? Videos { get; set; }

        public DbSet<Actor>? Actor { get; set; }
        





    }
}
