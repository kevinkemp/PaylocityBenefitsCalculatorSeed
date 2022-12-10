using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Data
{
    public class TimePeriod
    {
        [Key]
        public int PeriodId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public DateTime PayDate { get; set; }
    }
}
