using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HwaidakAPI.CheckInModels;

public partial class HwaidakHotelsOnlineCheckInDbContext : DbContext
{
    public HwaidakHotelsOnlineCheckInDbContext()
    {
    }

    public HwaidakHotelsOnlineCheckInDbContext(DbContextOptions<HwaidakHotelsOnlineCheckInDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CheckInCountry> CheckInCountries { get; set; }

    public virtual DbSet<CheckInHotel> CheckInHotels { get; set; }

    public virtual DbSet<CheckInHotelsRoom> CheckInHotelsRooms { get; set; }

    public virtual DbSet<CheckInLanguage> CheckInLanguages { get; set; }

    public virtual DbSet<CheckInReservationThrough> CheckInReservationThroughs { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<RequestsGuest> RequestsGuests { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json")
                        .Build();

            string connectionString = configuration.GetConnectionString("OnlineCheckInDBConnectionString");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("titDevCheckInDB2024");

        modelBuilder.Entity<CheckInCountry>(entity =>
        {
            entity.HasKey(e => e.RequestCountryId).HasName("PK_tbl_Countries");

            entity.ToTable("CheckIn_Countries", "dbo");

            entity.Property(e => e.RequestCountryId).HasColumnName("RequestCountryID");
            entity.Property(e => e.Langid).HasColumnName("langid");
            entity.Property(e => e.RequestCountryName).HasMaxLength(250);
            entity.Property(e => e.RequestCountryNameAr)
                .HasMaxLength(250)
                .HasColumnName("RequestCountryNameAR");
            entity.Property(e => e.RequestCountryNameFr).HasMaxLength(250);
        });

        modelBuilder.Entity<CheckInHotel>(entity =>
        {
            entity.HasKey(e => e.HotelId);

            entity.ToTable("CheckIn_Hotels", "dbo");

            entity.Property(e => e.HotelId).HasColumnName("HotelID");
            entity.Property(e => e.HotelName).HasMaxLength(250);
            entity.Property(e => e.HotelReceivedEmail).HasMaxLength(250);
            entity.Property(e => e.HotelTermsConditions).HasColumnType("ntext");
        });

        modelBuilder.Entity<CheckInHotelsRoom>(entity =>
        {
            entity.HasKey(e => e.RoomTypesId);

            entity.ToTable("CheckIn_Hotels_Rooms", "dbo");

            entity.Property(e => e.RoomTypesId).HasColumnName("RoomTypesID");
            entity.Property(e => e.HotelId).HasColumnName("HotelID");
            entity.Property(e => e.RoomTypesEn)
                .HasMaxLength(250)
                .HasColumnName("RoomTypes_EN");
        });

        modelBuilder.Entity<CheckInLanguage>(entity =>
        {
            entity.HasKey(e => e.LangId);

            entity.ToTable("CheckIN_Languages", "dbo");

            entity.Property(e => e.LangId).HasColumnName("LangID");
            entity.Property(e => e.LangName).HasMaxLength(250);
        });

        modelBuilder.Entity<CheckInReservationThrough>(entity =>
        {
            entity.HasKey(e => e.ReservationThroughId);

            entity.ToTable("CheckIn_ReservationThrough", "dbo");

            entity.Property(e => e.ReservationThroughId).HasColumnName("ReservationThroughID");
            entity.Property(e => e.ReservationThroughEn)
                .HasMaxLength(250)
                .HasColumnName("ReservationThrough_EN");
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.ToTable("Requests", "dbo");

            entity.Property(e => e.RequestId).HasColumnName("RequestID");
            entity.Property(e => e.ArrivalFlight).HasMaxLength(250);
            entity.Property(e => e.ChannelName).HasMaxLength(250);
            entity.Property(e => e.CheckInDate).HasMaxLength(250);
            entity.Property(e => e.CheckoutDate).HasMaxLength(250);
            entity.Property(e => e.Chronicdiseases).HasColumnName("chronicdiseases");
            entity.Property(e => e.Chronicdiseasesdescription)
                .HasColumnType("ntext")
                .HasColumnName("chronicdiseasesdescription");
            entity.Property(e => e.DepartureFlight).HasMaxLength(250);
            entity.Property(e => e.DepositReceipt).HasMaxLength(250);
            entity.Property(e => e.EmailAddress).HasMaxLength(250);
            entity.Property(e => e.GuestBirthDate).HasMaxLength(250);
            entity.Property(e => e.GuestName).HasMaxLength(250);
            entity.Property(e => e.HotelName).HasMaxLength(250);
            entity.Property(e => e.Last14days).HasColumnName("last14days");
            entity.Property(e => e.Last14daysdescription)
                .HasColumnType("ntext")
                .HasColumnName("last14daysdescription");
            entity.Property(e => e.MarriageCertificate).HasMaxLength(250);
            entity.Property(e => e.MobileNumber).HasMaxLength(250);
            entity.Property(e => e.Nationality).HasMaxLength(250);
            entity.Property(e => e.NumberofGuest).HasMaxLength(250);
            entity.Property(e => e.NumberofRooms).HasMaxLength(250);
            entity.Property(e => e.Passport).HasMaxLength(250);
            entity.Property(e => e.RequestDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.ReservationThrough).HasMaxLength(250);
            entity.Property(e => e.RoomTypes).HasMaxLength(250);
            entity.Property(e => e.ScanFile).HasMaxLength(250);
            entity.Property(e => e.ScanFileWife).HasMaxLength(250);
            entity.Property(e => e.SpecialRequest).HasColumnType("ntext");
        });

        modelBuilder.Entity<RequestsGuest>(entity =>
        {
            entity.HasKey(e => e.GuestId);

            entity.ToTable("Requests_Guests", "dbo");

            entity.Property(e => e.GuestId).HasColumnName("GuestID");
            entity.Property(e => e.GuestBirthDate).HasMaxLength(250);
            entity.Property(e => e.GuestName).HasMaxLength(250);
            entity.Property(e => e.GuestPassport).HasMaxLength(250);
            entity.Property(e => e.GuestUploadFile).HasMaxLength(250);
            entity.Property(e => e.RequestId).HasColumnName("RequestID");

            entity.HasOne(d => d.Request).WithMany(p => p.RequestsGuests)
                .HasForeignKey(d => d.RequestId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Requests_Guests_Requests");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
