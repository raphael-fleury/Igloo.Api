namespace Igloo.Infrastructure.Persistence;

using Igloo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class IglooDbContext : DbContext
{
    public IglooDbContext(DbContextOptions<IglooDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Profile> Profiles => Set<Profile>();
    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
    public DbSet<Follow> Follows => Set<Follow>();
    public DbSet<Mute> Mutes => Set<Mute>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Like> Likes => Set<Like>();
    public DbSet<Group> Groups => Set<Group>();
    public DbSet<GroupMembership> GroupMemberships => Set<GroupMembership>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User
        modelBuilder.Entity<User>()
            .ToTable("User")
            .HasKey(u => u.Id);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Profile
        modelBuilder.Entity<Profile>()
            .ToTable("Profile")
            .HasKey(p => p.Id);

        modelBuilder.Entity<Profile>()
            .HasIndex(p => p.Username)
            .IsUnique();

        // UserProfile - composite key
        modelBuilder.Entity<UserProfile>()
            .ToTable("UserProfile")
            .HasKey(up => new { up.UserId, up.ProfileId });

        modelBuilder.Entity<UserProfile>()
            .HasOne(up => up.User)
            .WithMany(u => u.UserProfiles)
            .HasForeignKey(up => up.UserId);

        modelBuilder.Entity<UserProfile>()
            .HasOne(up => up.Profile)
            .WithMany(p => p.UserProfiles)
            .HasForeignKey(up => up.ProfileId);

        // Follow - composite key
        modelBuilder.Entity<Follow>()
            .ToTable("Follow")
            .HasKey(f => new { f.FollowerProfileId, f.FollowedProfileId });

        modelBuilder.Entity<Follow>()
            .HasOne(f => f.FollowerProfile)
            .WithMany(p => p.Followings)
            .HasForeignKey(f => f.FollowerProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Follow>()
            .HasOne(f => f.FollowedProfile)
            .WithMany(p => p.Followers)
            .HasForeignKey(f => f.FollowedProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        // Mute - composite key
        modelBuilder.Entity<Mute>()
            .ToTable("Mute")
            .HasKey(m => new { m.MuterProfileId, m.MutedProfileId });

        modelBuilder.Entity<Mute>()
            .HasOne(m => m.MuterProfile)
            .WithMany(p => p.Muters)
            .HasForeignKey(m => m.MuterProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Mute>()
            .HasOne(m => m.MutedProfile)
            .WithMany(p => p.Muteds)
            .HasForeignKey(m => m.MutedProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        // Post
        modelBuilder.Entity<Post>()
            .ToTable("Post")
            .HasKey(p => p.Id);

        modelBuilder.Entity<Post>()
            .Property(p => p.Visibility)
            .HasConversion<string>();

        modelBuilder.Entity<Post>()
            .HasOne(p => p.Profile)
            .WithMany(pf => pf.Posts)
            .HasForeignKey(p => p.ProfileId);

        modelBuilder.Entity<Post>()
            .HasOne(p => p.ReplyToPost)
            .WithMany(p => p.Replies)
            .HasForeignKey(p => p.ReplyToPostId)
            .OnDelete(DeleteBehavior.Cascade);

        // Like - composite key
        modelBuilder.Entity<Like>()
            .ToTable("Like")
            .HasKey(l => new { l.ProfileId, l.PostId });

        modelBuilder.Entity<Like>()
            .HasOne(l => l.Profile)
            .WithMany()
            .HasForeignKey(l => l.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Like>()
            .HasOne(l => l.Post)
            .WithMany(p => p.Likes)
            .HasForeignKey(l => l.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        // Group
        modelBuilder.Entity<Group>()
            .ToTable("Group")
            .HasKey(g => g.Id);

        // GroupMembership - composite key
        modelBuilder.Entity<GroupMembership>()
            .ToTable("GroupMembership")
            .HasKey(gm => new { gm.GroupId, gm.ProfileId });

        modelBuilder.Entity<GroupMembership>()
            .HasOne(gm => gm.Group)
            .WithMany(g => g.GroupMemberships)
            .HasForeignKey(gm => gm.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<GroupMembership>()
            .HasOne(gm => gm.Profile)
            .WithMany(p => p.GroupMemberships)
            .HasForeignKey(gm => gm.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
