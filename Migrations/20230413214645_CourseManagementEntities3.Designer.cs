﻿// <auto-generated />
using System;
using DMed_Razor.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DMedRazor.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230413214645_CourseManagementEntities3")]
    partial class CourseManagementEntities3
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.2");

            modelBuilder.Entity("DMed_Razor.Entities.AppRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("DMed_Razor.Entities.AppUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CNIC")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DOB")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("EmailVerificationToken")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("RegistrationRegId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("UnqiueHash")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.HasIndex("RegistrationRegId");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("DMed_Razor.Entities.AppUserRole", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RoleId")
                        .HasColumnType("INTEGER");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("DMed_Razor.Entities.Course", b =>
                {
                    b.Property<int>("CourseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CourseId1")
                        .HasColumnType("INTEGER");

                    b.Property<int>("OrgId")
                        .HasColumnType("INTEGER");

                    b.HasKey("CourseId");

                    b.HasIndex("CourseId1");

                    b.HasIndex("OrgId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("DMed_Razor.Entities.Lecturer", b =>
                {
                    b.Property<int>("LecturerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Name")
                        .HasColumnType("INTEGER");

                    b.HasKey("LecturerId");

                    b.ToTable("Lecturers");
                });

            modelBuilder.Entity("DMed_Razor.Entities.Module", b =>
                {
                    b.Property<int>("ModuleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CourseId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CourseId1")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ModuleId1")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("maxDurationHours")
                        .HasColumnType("INTEGER");

                    b.HasKey("ModuleId");

                    b.HasIndex("CourseId");

                    b.HasIndex("CourseId1");

                    b.HasIndex("ModuleId1");

                    b.ToTable("Modules");
                });

            modelBuilder.Entity("DMed_Razor.Entities.ModuleAssignment", b =>
                {
                    b.Property<int>("AssignmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("LecturerId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ModuleId")
                        .HasColumnType("INTEGER");

                    b.HasKey("AssignmentId");

                    b.HasIndex("LecturerId")
                        .IsUnique();

                    b.HasIndex("ModuleId")
                        .IsUnique();

                    b.ToTable("ModuleAssignments");
                });

            modelBuilder.Entity("DMed_Razor.Entities.Organization", b =>
                {
                    b.Property<int>("OrgId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("OrgId");

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("DMed_Razor.Entities.Registration", b =>
                {
                    b.Property<int>("RegId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Completed")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("LectureId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("TEXT");

                    b.Property<bool>("isModule")
                        .HasColumnType("INTEGER");

                    b.HasKey("RegId");

                    b.ToTable("Registration");
                });

            modelBuilder.Entity("DMed_Razor.Entities.Session", b =>
                {
                    b.Property<int>("SessionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CourseId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("NumStudents")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("TEXT");

                    b.HasKey("SessionId");

                    b.HasIndex("CourseId");

                    b.ToTable("Session");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<int>("RoleId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("DMed_Razor.Entities.AppUser", b =>
                {
                    b.HasOne("DMed_Razor.Entities.Registration", null)
                        .WithMany("Students")
                        .HasForeignKey("RegistrationRegId");
                });

            modelBuilder.Entity("DMed_Razor.Entities.AppUserRole", b =>
                {
                    b.HasOne("DMed_Razor.Entities.AppRole", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DMed_Razor.Entities.AppUser", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DMed_Razor.Entities.Course", b =>
                {
                    b.HasOne("DMed_Razor.Entities.Course", null)
                        .WithMany("CoursePreReq")
                        .HasForeignKey("CourseId1");

                    b.HasOne("DMed_Razor.Entities.Organization", "Organization")
                        .WithMany("Courses")
                        .HasForeignKey("OrgId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("DMed_Razor.Entities.Module", b =>
                {
                    b.HasOne("DMed_Razor.Entities.Course", null)
                        .WithMany("Modules")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DMed_Razor.Entities.Course", null)
                        .WithMany("ModulesPreReq")
                        .HasForeignKey("CourseId1");

                    b.HasOne("DMed_Razor.Entities.Module", null)
                        .WithMany("ModulesPreReq")
                        .HasForeignKey("ModuleId1");
                });

            modelBuilder.Entity("DMed_Razor.Entities.ModuleAssignment", b =>
                {
                    b.HasOne("DMed_Razor.Entities.Lecturer", "Lecturer")
                        .WithOne()
                        .HasForeignKey("DMed_Razor.Entities.ModuleAssignment", "LecturerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DMed_Razor.Entities.Module", "Module")
                        .WithOne()
                        .HasForeignKey("DMed_Razor.Entities.ModuleAssignment", "ModuleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lecturer");

                    b.Navigation("Module");
                });

            modelBuilder.Entity("DMed_Razor.Entities.Session", b =>
                {
                    b.HasOne("DMed_Razor.Entities.Course", "Course")
                        .WithMany("Sessions")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("DMed_Razor.Entities.AppRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("DMed_Razor.Entities.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("DMed_Razor.Entities.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("DMed_Razor.Entities.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DMed_Razor.Entities.AppRole", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("DMed_Razor.Entities.AppUser", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("DMed_Razor.Entities.Course", b =>
                {
                    b.Navigation("CoursePreReq");

                    b.Navigation("Modules");

                    b.Navigation("ModulesPreReq");

                    b.Navigation("Sessions");
                });

            modelBuilder.Entity("DMed_Razor.Entities.Module", b =>
                {
                    b.Navigation("ModulesPreReq");
                });

            modelBuilder.Entity("DMed_Razor.Entities.Organization", b =>
                {
                    b.Navigation("Courses");
                });

            modelBuilder.Entity("DMed_Razor.Entities.Registration", b =>
                {
                    b.Navigation("Students");
                });
#pragma warning restore 612, 618
        }
    }
}
