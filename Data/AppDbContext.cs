using AdminDashboard.Models;
using Microsoft.EntityFrameworkCore;

namespace AdminDashboard.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Company> Companies { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<TransportRoute> Routes { get; set; }
    public DbSet<Stop> Stops { get; set; }
    public DbSet<RouteStop> RouteStops { get; set; }
    public DbSet<RoutePassenger> RoutePassengers { get; set; }
    public DbSet<Trip> Trips { get; set; }
    public DbSet<SystemSetting> SystemSettings { get; set; } = null!;
    public DbSet<TripLocationLog> TripLocationLogs { get; set; }
    public DbSet<VehicleDocument> VehicleDocuments { get; set; }
    public DbSet<TripSchedule> TripSchedules { get; set; }
    public DbSet<TripAttendance> TripAttendances { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ---------- PRIMARY KEYS ----------
        modelBuilder.Entity<TransportRoute>().HasKey(x => x.RouteId);
        modelBuilder.Entity<Stop>().HasKey(x => x.StopId);
        modelBuilder.Entity<RouteStop>().HasKey(x => x.RouteStopId);

        // ---------- COMPANY RELATIONSHIPS ----------
        // Explicit mapping prevents EF from creating "CompanyId1"
        modelBuilder.Entity<User>()
            .HasOne(u => u.Company).WithMany(c => c.Users)
            .HasForeignKey(u => u.CompanyId).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Vehicle>()
            .HasOne(v => v.Company).WithMany(c => c.Vehicles)
            .HasForeignKey(v => v.CompanyId).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TransportRoute>()
            .HasOne(r => r.Company).WithMany(c => c.Routes)
            .HasForeignKey(r => r.CompanyId).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Stop>()
            .HasOne(s => s.Company).WithMany(c => c.Stops)
            .HasForeignKey(s => s.CompanyId)
            .OnDelete(DeleteBehavior.NoAction);

        // ---------- ROUTE <-> STOP (MAPPING TABLE) ----------
        modelBuilder.Entity<RouteStop>()
            .HasOne(rs => rs.Route).WithMany(r => r.RouteStops)
            .HasForeignKey(rs => rs.RouteId).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RouteStop>()
            .HasOne(rs => rs.Stop).WithMany()
            .HasForeignKey(rs => rs.StopId).OnDelete(DeleteBehavior.Restrict);

        //modelBuilder.Entity<RouteStop>()
        //    .HasIndex(x => new { x.RouteId, x.StopId }).IsUnique();

        modelBuilder.Entity<RouteStop>()
    .HasIndex(x => new { x.RouteId, x.StopOrder });

        // ---------- PRECISION SETTINGS ----------
        modelBuilder.Entity<Stop>().Property(s => s.Latitude).HasPrecision(10, 7);
        modelBuilder.Entity<Stop>().Property(s => s.Longitude).HasPrecision(10, 7);

        // ---------- CONSTRAINTS & INDEXES ----------
        modelBuilder.Entity<Vehicle>().HasIndex(v => v.RegistrationNumber).IsUnique();

        modelBuilder.Entity<VehicleDocument>()
    .HasOne(vd => vd.Vehicle)
    .WithMany(v => v.Documents)
    .HasForeignKey(vd => vd.VehicleId)
    .OnDelete(DeleteBehavior.Cascade);


        // ---------- ROUTE PASSENGER ----------
        modelBuilder.Entity<RoutePassenger>(entity =>
        {
            entity.HasKey(x => x.RoutePassengerId);
            entity.HasOne(x => x.Company).WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(x => x.Route).WithMany().HasForeignKey(x => x.RouteId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(x => x.Stop).WithMany().HasForeignKey(x => x.StopId).OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(x => new { x.CompanyId, x.UserId, x.RouteId }).IsUnique();
        });

        // ---------- TRIP ----------
        modelBuilder.Entity<Trip>(entity =>
        {
            entity.HasOne(t => t.Driver).WithMany().HasForeignKey(t => t.DriverId).OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(t => t.Vehicle).WithMany().HasForeignKey(t => t.VehicleId).OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(t => t.Route).WithMany().HasForeignKey(t => t.RouteId).OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Trip>()
    .HasOne(t => t.Vehicle)
    .WithMany()
    .HasForeignKey(t => t.VehicleId)
    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Trip>()
            .HasOne(t => t.Driver)
            .WithMany()
            .HasForeignKey(t => t.DriverId)
            .OnDelete(DeleteBehavior.Restrict);


        // ---------- SEED DATA ----------
        modelBuilder.Entity<Company>().HasData(
            new Company { CompanyId = 1, CompanyName = "Default Corp", Status = 'Y', CreatedAt = new DateTime(2025, 1, 1) }
        );

        modelBuilder.Entity<SystemSetting>().HasData(
            new SystemSetting { Id = 1, CompanyId = 1, SettingKey = "ForceFirstLoginPasswordChange", SettingValue = "Y", Description = "...", CreatedAt = new DateTime(2025, 11, 27) },
            new SystemSetting { Id = 2, CompanyId = 1, SettingKey = "MaxLoginAttempts", SettingValue = "5", Description = "...", CreatedAt = new DateTime(2025, 11, 27) }
        );
    }
}
