using Microsoft.EntityFrameworkCore;

namespace CSE325Project.Server.Data;

public class StudyRoomContext : DbContext
{
    public StudyRoomContext(DbContextOptions<StudyRoomContext> options)
        : base(options)
    {
    }

    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Reservation> Reservations => Set<Reservation>();
    public DbSet<Amenity> Amenities => Set<Amenity>();
    public DbSet<RoomAmenity> RoomAmenities => Set<RoomAmenity>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Room>(entity =>
        {

            entity.HasKey(r => r.RoomId);

            entity.Property(r => r.RoomId).HasColumnName("room_id");
            entity.Property(r => r.RoomName).HasColumnName("room_name");
            entity.Property(r => r.Location).HasColumnName("location");
            entity.Property(r => r.Capacity).HasColumnName("capacity");
            entity.Property(r => r.Description).HasColumnName("description");
            entity.Property(r => r.IsActive).HasColumnName("is_active");
            entity.Property(r => r.CreatedAt).HasColumnName("created_at");
        });

        builder.Entity<RoomAmenity>(entity =>
        {

            entity.HasKey(ra => new { ra.RoomId, ra.AmenityId });

            entity.Property(ra => ra.RoomId).HasColumnName("room_id");
            entity.Property(ra => ra.AmenityId).HasColumnName("amenity_id");
        });
    }
}