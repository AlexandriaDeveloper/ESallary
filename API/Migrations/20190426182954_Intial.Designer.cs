﻿// <auto-generated />
using System;
using API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DatingApp.API.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20190426182954_Intial")]
    partial class Intial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.2-rtm-30932")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("API.Models.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.ToTable("Account");
                });

            modelBuilder.Entity("API.Models.Bank", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccountMaxLength");

                    b.Property<int>("AccountMinLength");

                    b.Property<string>("BankName")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.HasKey("Id");

                    b.ToTable("Banks");
                });

            modelBuilder.Entity("API.Models.BankBranch", b =>
                {
                    b.Property<int>("BranchCode")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BankId");

                    b.Property<string>("BranchName")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.HasKey("BranchCode");

                    b.HasIndex("BankId");

                    b.ToTable("BankBranchs");
                });

            modelBuilder.Entity("API.Models.Daily", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Details");

                    b.Property<string>("EntryNumber");

                    b.Property<DateTime?>("MaturityDate");

                    b.Property<decimal?>("Total")
                        .HasColumnType("decimal(9,2)");

                    b.HasKey("Id");

                    b.ToTable("Dailies");
                });

            modelBuilder.Entity("API.Models.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(70);

                    b.HasKey("Id");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("API.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .HasMaxLength(10);

                    b.Property<string>("Collage")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("DOB");

                    b.Property<bool>("Deleted");

                    b.Property<int?>("DepartmentId");

                    b.Property<string>("Email")
                        .HasMaxLength(150);

                    b.Property<string>("Gender")
                        .HasMaxLength(10);

                    b.Property<string>("Grade")
                        .HasMaxLength(150);

                    b.Property<bool>("HasATM");

                    b.Property<bool>("HasBank");

                    b.Property<bool>("HasOrder");

                    b.Property<bool>("HasPost");

                    b.Property<string>("KnownAs")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<string>("NationalId")
                        .IsRequired()
                        .HasMaxLength(14);

                    b.Property<string>("OtherOption")
                        .HasMaxLength(150);

                    b.Property<string>("Phone")
                        .HasMaxLength(11);

                    b.Property<string>("SallaryOption")
                        .HasMaxLength(150);

                    b.Property<string>("Section")
                        .HasMaxLength(150);

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("API.Models.EmployeeBank", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BankCode")
                        .HasMaxLength(10);

                    b.Property<string>("BankName")
                        .HasMaxLength(50);

                    b.Property<string>("BranchCode")
                        .HasMaxLength(10);

                    b.Property<string>("BranchName")
                        .HasMaxLength(50);

                    b.Property<int?>("EmployeeId");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId")
                        .IsUnique()
                        .HasFilter("[EmployeeId] IS NOT NULL");

                    b.ToTable("EmployeesBank");
                });

            modelBuilder.Entity("API.Models.EmployeeDeductions", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<float?>("EMokamel");

                    b.Property<float?>("EWazifi");

                    b.Property<int>("EmployeeId")
                        .HasMaxLength(14);

                    b.Property<float?>("MMokamel");

                    b.Property<float?>("MWazifi");

                    b.Property<int>("Mokafaa");

                    b.Property<string>("OrderNum")
                        .HasMaxLength(6);

                    b.Property<float?>("Sanctions");

                    b.Property<int?>("Vacation");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.ToTable("EmployeeDeductions");
                });

            modelBuilder.Entity("API.Models.EmployeeDeductionsPaied", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<float?>("EMokamel");

                    b.Property<float?>("EWazifi");

                    b.Property<int>("EmployeeId");

                    b.Property<float?>("MMokamel");

                    b.Property<float?>("MWazifi");

                    b.Property<int>("Mokafaa");

                    b.Property<int?>("PaymentFileId");

                    b.Property<float?>("Sanctions");

                    b.Property<int?>("Vacation");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.ToTable("EmployeeDeductionsPaied");
                });

            modelBuilder.Entity("API.Models.EmployeeFinincialData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccountId");

                    b.Property<string>("AccountState")
                        .HasMaxLength(50);

                    b.Property<string>("AdditionalValue")
                        .HasMaxLength(200);

                    b.Property<int>("EmployeeId");

                    b.Property<float>("Value");

                    b.Property<short>("Year");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("EmployeeFinincialData");
                });

            modelBuilder.Entity("API.Models.EmployeeOrder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BranchCode");

                    b.Property<int?>("EmployeeId");

                    b.Property<string>("OrderAccountNumber")
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.HasIndex("BranchCode");

                    b.HasIndex("EmployeeId")
                        .IsUnique()
                        .HasFilter("[EmployeeId] IS NOT NULL");

                    b.ToTable("EmployeesOrder");
                });

            modelBuilder.Entity("API.Models.EmployeePartTime", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasMaxLength(150);

                    b.Property<int>("EmployeeId");

                    b.Property<DateTime>("EndDate");

                    b.Property<DateTime>("StartDate");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.ToTable("EmployeePartTime");
                });

            modelBuilder.Entity("API.Models.EmployeePost", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("EmployeeId");

                    b.Property<string>("PostAddress")
                        .HasMaxLength(100);

                    b.Property<string>("PostPhone")
                        .HasMaxLength(11);

                    b.Property<string>("PostTo")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId")
                        .IsUnique()
                        .HasFilter("[EmployeeId] IS NOT NULL");

                    b.ToTable("EmployeesPost");
                });

            modelBuilder.Entity("API.Models.File", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CollageName")
                        .IsRequired();

                    b.Property<int?>("DailyId");

                    b.Property<string>("EntryName");

                    b.Property<string>("FileNum224");

                    b.Property<string>("FileNum55")
                        .IsRequired();

                    b.Property<int?>("FileTypeId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("PaymentMethod")
                        .IsRequired();

                    b.Property<string>("PaymentType")
                        .IsRequired();

                    b.Property<decimal>("TotalSum")
                        .HasColumnType("decimal(9,2)");

                    b.HasKey("Id");

                    b.HasIndex("DailyId");

                    b.HasIndex("FileTypeId");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("API.Models.FileDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("EmployeeId");

                    b.Property<string>("EmployeeName");

                    b.Property<int>("FileId");

                    b.Property<decimal>("Net")
                        .HasColumnType("decimal(9,2)");

                    b.Property<string>("PaymentMethod");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("FileId");

                    b.ToTable("FilesDetail");
                });

            modelBuilder.Entity("API.Models.FileType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("FileType");
                });

            modelBuilder.Entity("API.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("API.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("Gender");

                    b.Property<string>("KnownAs");

                    b.Property<DateTime>("LastActive");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("API.Models.UserRole", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<int>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("API.Models.BankBranch", b =>
                {
                    b.HasOne("API.Models.Bank", "Bank")
                        .WithMany("Branches")
                        .HasForeignKey("BankId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("API.Models.Employee", b =>
                {
                    b.HasOne("API.Models.Department", "Department")
                        .WithMany("Employees")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("API.Models.EmployeeBank", b =>
                {
                    b.HasOne("API.Models.Employee", "Employee")
                        .WithOne("EmployeeBank")
                        .HasForeignKey("API.Models.EmployeeBank", "EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("API.Models.EmployeeDeductions", b =>
                {
                    b.HasOne("API.Models.Employee", "Employee")
                        .WithMany("EmployeeDeductions")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("API.Models.EmployeeDeductionsPaied", b =>
                {
                    b.HasOne("API.Models.Employee", "Employee")
                        .WithMany("EmployeeDeductionsPaied")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("API.Models.EmployeeFinincialData", b =>
                {
                    b.HasOne("API.Models.Account", "Account")
                        .WithMany("EmployeeFinincialData")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("API.Models.Employee", "Employee")
                        .WithMany("EmployeeFinincialData")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("API.Models.EmployeeOrder", b =>
                {
                    b.HasOne("API.Models.BankBranch", "BankBranch")
                        .WithMany("EmployeesOrder")
                        .HasForeignKey("BranchCode")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("API.Models.Employee", "Employee")
                        .WithOne("EmployeeOrder")
                        .HasForeignKey("API.Models.EmployeeOrder", "EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("API.Models.EmployeePartTime", b =>
                {
                    b.HasOne("API.Models.Employee", "Employee")
                        .WithMany("EmployeePartTime")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("API.Models.EmployeePost", b =>
                {
                    b.HasOne("API.Models.Employee", "Employee")
                        .WithOne("EmployeePost")
                        .HasForeignKey("API.Models.EmployeePost", "EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("API.Models.File", b =>
                {
                    b.HasOne("API.Models.Daily", "Daily")
                        .WithMany("Files")
                        .HasForeignKey("DailyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("API.Models.FileType", "FileType")
                        .WithMany("Files")
                        .HasForeignKey("FileTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("API.Models.FileDetail", b =>
                {
                    b.HasOne("API.Models.Employee", "Employee")
                        .WithMany("FileDetails")
                        .HasForeignKey("EmployeeId");

                    b.HasOne("API.Models.File", "File")
                        .WithMany("FileDetails")
                        .HasForeignKey("FileId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("API.Models.UserRole", b =>
                {
                    b.HasOne("API.Models.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("API.Models.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("API.Models.Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("API.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("API.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("API.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
