using Microsoft.EntityFrameworkCore;

namespace Api.Data
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

        //could have TimePeriods table where admin can set different start, end and paydates
        //public DbSet<TimePeriod> TimePeriods { get; set; }
    }
}
