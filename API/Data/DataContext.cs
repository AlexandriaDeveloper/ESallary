using System;
using System.Threading;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data {
    public class DataContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>> {

        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeBank> EmployeesBank { get; set; }
        public DbSet<EmployeeOrder> EmployeesOrder { get; set; }
        public DbSet<EmployeePost> EmployeesPost { get; set; }
        public DbSet<EmployeeFinincialData> EmployeeFinincialData { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<BankBranch> BankBranchs { get; set; }
        public DbSet<Daily> Dailies { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<FileDetail> FilesDetail { get; set; }
        public DbSet<FileType> FilesType { get; set; }
        public DbSet<FileType> PaymentType { get; set; }

        public DataContext (DbContextOptions<DataContext> options) : base (options) {

        }

        protected override void OnModelCreating (ModelBuilder builder) {
            base.OnModelCreating (builder);
            builder.Entity<UserRole> (userRole => {
                userRole.HasKey (ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne (ur => ur.User)
                    .WithMany (r => r.UserRoles)
                    .HasForeignKey (f => f.UserId)
                    .IsRequired ();

                userRole.HasOne (ur => ur.Role)
                    .WithMany (r => r.UserRoles)
                    .HasForeignKey (f => f.RoleId)
                    .IsRequired ();

            });
            builder.Entity<Employee> (emp => {
                emp.HasOne (emp1 => emp1.Department)
                    .WithMany (dep => dep.Employees).IsRequired (false)
                    .HasForeignKey (k => k.DepartmentId).OnDelete (DeleteBehavior.SetNull);

            });

            builder.Entity<Employee> ()
                .HasOne (empBank => empBank.EmployeeBank)
                .WithOne (emp => emp.Employee).IsRequired (false)
                .HasForeignKey<EmployeeBank> (k => k.EmployeeId)
                .OnDelete (DeleteBehavior.Cascade);

            builder.Entity<Employee> ().HasOne (empOrder => empOrder.EmployeeOrder)
                .WithOne (emp => emp.Employee).IsRequired (false)
                .HasForeignKey<EmployeeOrder> (k => k.EmployeeId).OnDelete (DeleteBehavior.Cascade);

            builder.Entity<Employee> ().HasOne (empOrder => empOrder.EmployeePost)
                .WithOne (emp => emp.Employee).IsRequired (false)
                .HasForeignKey<EmployeePost> (k => k.EmployeeId).OnDelete (DeleteBehavior.Cascade);

            builder.Entity<EmployeeOrder> ().HasOne (empBankBranch => empBankBranch.BankBranch)
                .WithMany (emp => emp.EmployeesOrder)
                .HasForeignKey (k => k.BranchCode)
                .OnDelete (DeleteBehavior.Cascade);

            builder.Entity<Employee> ().HasMany (EmpFinincialData => EmpFinincialData.EmployeeFinincialData)
                .WithOne (emp => emp.Employee)
                .HasForeignKey (k => k.EmployeeId)
                .OnDelete (DeleteBehavior.Cascade);

            builder.Entity<Employee> ().HasMany (EmployeeDeductions => EmployeeDeductions.EmployeeDeductions)
                .WithOne (emp => emp.Employee)
                .HasForeignKey (k => k.EmployeeId)
                .OnDelete (DeleteBehavior.Cascade);

            builder.Entity<Account> ().HasMany (acc => acc.EmployeeFinincialData)
                .WithOne (emp => emp.Account)
                .HasForeignKey (k => k.AccountId)
                .OnDelete (DeleteBehavior.Cascade);

            builder.Entity<Bank> ().HasMany (bank => bank.Branches)
                .WithOne (branch => branch.Bank)
                .HasForeignKey (k => k.BankId).OnDelete (DeleteBehavior.Cascade);

            builder.Entity<Daily> ().HasMany (file => file.Files)
                .WithOne (d => d.Daily).IsRequired (false)
                .HasForeignKey (k => k.DailyId)
                .OnDelete (DeleteBehavior.Cascade);

            builder.Entity<File> ().HasMany (fileDetail => fileDetail.FileDetails)
                .WithOne (d => d.File)
                .IsRequired (true)
                .HasForeignKey (k => k.FileId)
                .OnDelete (DeleteBehavior.Cascade);
                
            builder.Entity<Employee> ().HasMany (fileDetail => fileDetail.FileDetails)
                .WithOne (d => d.Employee)
                .IsRequired (false)
                .HasForeignKey (k => k.EmployeeId)
                .OnDelete (DeleteBehavior.ClientSetNull);

            builder.Entity<FileType>().HasMany (files => files.Files)
                .WithOne (d => d.FileType)
                .IsRequired (false)
                .HasForeignKey (fk => fk.FileTypeId)
                .OnDelete (DeleteBehavior.Cascade);    

            // builder.Entity<File>().HasIndex(x=> x.FileNum55).IsUnique(true);
            // builder.Entity<File>().HasIndex(x=> x.FileNum224).IsUnique(true);

        }
        public override int SaveChanges () {
            try { return base.SaveChanges (); } catch (Exception ex) {
                throw new Exception (ex.InnerException.Message.ToString ());
            }

        }
        public override async Task<int> SaveChangesAsync (CancellationToken cancellationToken = default (CancellationToken)) {
            try { return (await base.SaveChangesAsync (true, cancellationToken)); } catch (Exception ex) {
                throw new Exception (ex.InnerException.Message.ToString ());
            }

        }
    }
}