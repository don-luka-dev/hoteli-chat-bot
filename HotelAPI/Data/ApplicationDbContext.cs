using HotelAPI.Models.Constants;
using HotelAPI.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<Korisnik, KorisnikovaUloga, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<PostanskiUred> PostanskiUredi { get; set; }
        public DbSet<Mjesto> Mjesta { get; set; }
        public DbSet<StatusHotela> StatusiHotela { get; set; }
        public DbSet<StatusRezervacije> StatusiRezervacija { get; set; }
        public DbSet<Korisnik> Korisnici { get; set; }
        public DbSet<KorisnikovaUloga> KorisnikovaUloga { get; set; }
        public DbSet<Hotel> Hoteli { get; set; }
        public DbSet<Soba> Sobe { get; set; }
        public DbSet<Rezervacija> Rezervacije { get; set; }
        public DbSet<SlikaSobe> SlikeSoba { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StatusHotela>().HasData(
                new StatusHotela { Id = StatusHotelaConstants.Nepotvrden, Naziv = StatusHotelaConstants.Parse(StatusHotelaConstants.Nepotvrden) },
                new StatusHotela { Id = StatusHotelaConstants.Aktivan, Naziv = StatusHotelaConstants.Parse(StatusHotelaConstants.Aktivan) },
                new StatusHotela { Id = StatusHotelaConstants.Neaktivan, Naziv = StatusHotelaConstants.Parse(StatusHotelaConstants.Neaktivan) },
                new StatusHotela { Id = StatusHotelaConstants.Odbijen, Naziv = StatusHotelaConstants.Parse(StatusHotelaConstants.Odbijen) }
            );


            modelBuilder.Entity<StatusRezervacije>().HasData(
                new StatusRezervacije { Id = StatusRezervacijeConstants.UObradi, Naziv = StatusRezervacijeConstants.Parse(StatusRezervacijeConstants.UObradi) },
                new StatusRezervacije { Id = StatusRezervacijeConstants.Prihvacena, Naziv = StatusRezervacijeConstants.Parse(StatusRezervacijeConstants.Prihvacena) },
                new StatusRezervacije { Id = StatusRezervacijeConstants.Odbijena, Naziv = StatusRezervacijeConstants.Parse(StatusRezervacijeConstants.Odbijena) },
                new StatusRezervacije { Id = StatusRezervacijeConstants.Otkazana, Naziv = StatusRezervacijeConstants.Parse(StatusRezervacijeConstants.Otkazana) }
            );

            modelBuilder.Entity<KorisnikovaUloga>().HasData(
                new KorisnikovaUloga
                {
                    Id = KorisnikoveUlogeConstants.SuperAdministrator,
                    Name = KorisnikoveUlogeConstants.Parse(KorisnikoveUlogeConstants.SuperAdministrator),
                    NormalizedName = KorisnikoveUlogeConstants.ParseNormalized(KorisnikoveUlogeConstants.SuperAdministrator)
                },
                new KorisnikovaUloga
                {
                    Id = KorisnikoveUlogeConstants.Administrator,
                    Name = KorisnikoveUlogeConstants.Parse(KorisnikoveUlogeConstants.Administrator),
                    NormalizedName = KorisnikoveUlogeConstants.ParseNormalized(KorisnikoveUlogeConstants.Administrator)
                },
                new KorisnikovaUloga
                {
                    Id = KorisnikoveUlogeConstants.UpraviteljHotela,
                    Name = KorisnikoveUlogeConstants.Parse(KorisnikoveUlogeConstants.UpraviteljHotela),
                    NormalizedName = KorisnikoveUlogeConstants.ParseNormalized(KorisnikoveUlogeConstants.UpraviteljHotela)
                },
                new KorisnikovaUloga
                {
                    Id = KorisnikoveUlogeConstants.RegistriraniKorisnik,
                    Name = KorisnikoveUlogeConstants.Parse(KorisnikoveUlogeConstants.RegistriraniKorisnik),
                    NormalizedName = KorisnikoveUlogeConstants.ParseNormalized(KorisnikoveUlogeConstants.RegistriraniKorisnik)
                }
);

            modelBuilder.Entity<Rezervacija>()
                 .HasOne(r => r.Soba)
                 .WithMany(s => s.Rezervacije)
                 .HasForeignKey(r => r.SobaId)
                 .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ChatMessage>().ToTable("ChatMessages");
            base.OnModelCreating(modelBuilder);

        }
    }
}
