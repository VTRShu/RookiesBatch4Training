using LibraryManagementBE.Repositories.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementBE.Repositories.EFContext;
public class LibraryManagementDBContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    public LibraryManagementDBContext(DbContextOptions options) : base(options){}
    public DbSet<AppUser> AppUser { get; set; }
    public DbSet<AppRole> AppRole { get; set; }
    public DbSet<BookBorrowingRequest> BookBorrowingRequest { get; set; }
    public DbSet<BookBorrowingRequestDetails> BookBorrowingRequestDetails { get; set; }
    public DbSet<CategoryEntity> CategoryEntity { get; set; }
    public DbSet<BookEntity> BookEntity { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaims");
        modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRoles").HasKey(x => new { x.UserId, x.RoleId });
        modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogins").HasKey(x => x.UserId);
        modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims");
        modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens").HasKey(x => x.UserId);

        // modelBuilder.Entity<AppUser>().Property(c => c.NormalizedUserName).HasComputedColumnSql("UPPER([UserName])");
        // modelBuilder.Entity<AppUser>().Property(c => c.NormalizedEmail).HasComputedColumnSql("UPPER([Email])");
        modelBuilder.Entity<AppUser>().HasMany(c => c.OwnRequested);

        modelBuilder.Entity<BookBorrowingRequest>().HasMany(c=>c.BooksRequested).WithOne(c=>c.DetailOfRequest).OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<BookBorrowingRequest>().HasOne(c => c.RequestedBy).WithMany(c => c.OwnRequested).OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<BookBorrowingRequest>().HasOne(c => c.ResponseBy);

        modelBuilder.Entity<BookBorrowingRequestDetails>().HasOne(c=>c.Book);
        modelBuilder.Entity<BookBorrowingRequestDetails>().HasOne(c=>c.DetailOfRequest);
        
        modelBuilder.Entity<BookEntity>().HasOne(c => c.Category).WithMany(c => c.Books).OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<CategoryEntity>().HasMany(c => c.Books).WithOne(c=>c.Category).OnDelete(DeleteBehavior.SetNull);

        LibraryManagementSeedData.Seed(modelBuilder);
    }
}