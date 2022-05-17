using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace DataModel
{
    public class DataContext : DbContext
    {
        public DbSet<Hospital> Hospitals { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Bed> Beds { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ReservationRequest> ReservationRequests { get; set; }

        public DataContext()
        {
            Database.EnsureCreated();
            Database.OpenConnection();
            if (!Users.AnyAsync().Result)
            {
                Users.Add(new User()
                {
                    Name = "admin",
                    Password = "admin",
                    Address = "Center",
                    DateOfBirth = new DateTime(),
                    IsAdmin = true,
                    Mobile = "000000"
                });
            }
            SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=Hospitals.db");
            if (!File.Exists("Hospitals.db")) CreateFile("Hospitals.db");
            //The Below Comment Added by me
            optionsBuilder.EnableSensitiveDataLogging();
        }

        private void CreateFile(string databaseFileName)
        {
            Directory.CreateDirectory(new FileInfo(databaseFileName).DirectoryName);
            FileStream fs = File.Create(databaseFileName);
            fs.Close();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Bed)
                .WithMany(b=>b.Reservations);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.User)
                .WithMany(b => b.Reservations);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.ReservationRequest)
                .WithOne();

        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Reservation>().HasKey(sc => new { sc.UserId, sc.BedId });

        //    modelBuilder.Entity<Reservation>()
        //        .HasOne<User>(sc => sc.User)
        //        .WithMany(s => s.Reservations)
        //        .HasForeignKey(sc => sc.UserId);


        //    modelBuilder.Entity<Reservation>()
        //        .HasOne<Bed>(sc => sc.Bed)
        //        .WithMany(s => s.Reservations)
        //        .HasForeignKey(sc => sc.BedId);
        //}

    }
}
