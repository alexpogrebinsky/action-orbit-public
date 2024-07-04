using System.ComponentModel.DataAnnotations;

namespace mmDailyPlanner.Server.DTO
{
    public class TaskDetailDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Title { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Range(1, 5)]
        public int Priority { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [StringLength(50)]
        public string Category { get; set; }
    }
}
