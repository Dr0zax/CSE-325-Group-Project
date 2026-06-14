using Microsoft.EntityFrameworkCore;

public class StudyRoomContext : DbContext
{
    public StudyRoomContext(
        DbContextOptions<StudyRoomContext> options)
        : base(options)
    {
    }

    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Reservation> Reservations => Set<Reservation>();
    public DbSet<Amenity> Amenities => Set<Amenity>();
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<RoomAmenity>()
            .HasKey(ra => new { ra.RoomId, ra.AmenityId });
    }
    public DbSet<User> Users => Set<User>();
}