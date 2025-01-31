using System.ComponentModel.DataAnnotations;

namespace TestBackEnd.Domain.Entities
{
    public class Project
    {
        public Project()
        {
            Tasks = new HashSet<Task>();
        }


        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int State { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }
    }
}
