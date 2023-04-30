using DMed_Razor.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DMed_Razor.Data
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, int,
        IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Module> Modules { get; set; }
        public DbSet<ModulePreReqs> ModulePreReqs { get; set; }
        public DbSet<CourseModules> CourseModules { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<ModuleAssignment> ModuleAssignments { get; set; }
        public DbSet<Registration> Registration { get; set; }
        public DbSet<Session> Session { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<AppUser>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            builder.Entity<AppRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();
            //MODULE
            builder.Entity<Module>()
               .HasMany(m => m.ModulePreReqs)
               .WithOne(mp => mp.Module)
               .HasForeignKey(mp => mp.ModuleId);

            builder.Entity<ModulePreReqs>()
                .HasOne(mp => mp.PreReq)
                .WithMany()
                .HasForeignKey(mp => mp.PreReqId);
            //ORGANIZATION
            /*builder.Entity<Organization>()
                .HasMany(ur => ur.Courses)
                .WithOne(e => e.Organization)
                .HasForeignKey(e => e.OrgId);*/
            //COURSE
            /*builder.Entity<Course>()
                .HasMany(u => u.Sessions)
                .WithOne(e => e.Course)
                .HasForeignKey(e => e.CourseId);*/
            /*builder.Entity<Course>()
                 .HasMany(ur => ur.Modules)
                 .WithOne()
                 .IsRequired();
            builder.Entity<Course>()
                .HasMany(ur => ur.ModulesPreReq)
                .WithOne();
            builder.Entity<Course>()
                .HasMany(ur => ur.CoursePreReq)
                .WithOne();*/
            //MODULE ASSIGNMENT
            builder.Entity<ModuleAssignment>()
                .HasOne(u => u.Lecturer)
                .WithMany()
                .HasForeignKey(ma => ma.LecturerId);
            builder.Entity<ModuleAssignment>()
                .HasOne(u => u.Module)
                .WithMany()
                .HasForeignKey(u => u.ModuleId);

            //REGISTRATION
            builder.Entity<Registration>()
                .HasOne(ur => ur.Student)
                .WithMany()
                .HasForeignKey(reg => reg.StudentId);
        }
    }
}