using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Data
{
    public class PayrollDbContext : DbContext
    {
        public PayrollDbContext(DbContextOptions<PayrollDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Dependent> Dependents { get; set; }

        public DbSet<Deduction> Deductions { get; set; }

        public DbSet<Paycheck> Paychecks { get; set; }

        public DbSet<TimePeriod> TimePeriods { get; set; }

        public DbSet<Salary> Salaries { get; set; }
    }
}
