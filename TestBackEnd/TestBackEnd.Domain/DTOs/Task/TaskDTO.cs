using TestBackEnd.Domain.Enums;

namespace TestBackEnd.Domain.DTOs.Task
{
    public class TaskDTO
    {
        public Guid? Id { get; set; }

        public Guid ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Type { get; set; }
        public int State { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string UserId  { get; set; }

        public string Icon
        {
            get
            {
                switch (Type)
                {
                    case (int)TypeTask.Review:
                        return "btn-warning fa fa-eye";
                    case (int)TypeTask.Develop:
                        return "btn-info fa fa-code-fork";
                    case (int)TypeTask.Test:
                        return "btn-warning no-hover fa fa-flask";
                    case (int)TypeTask.Bug:
                        return "btn-danger no-hover fa fa-bug";
                    default:
                        return string.Empty;
                }
            }
        }

        public string color
        {
            get
            {
                switch (Type)
                {
                    case (int)TypeTask.Review:
                        return "widget-color-orange";
                    case (int)TypeTask.Develop:
                        return "widget-color-blue2";
                    case (int)TypeTask.Test:
                        return "widget-color-orange";
                    case (int)TypeTask.Bug:
                        return "widget-color-red2";
                    default:
                        return string.Empty;
                }
            }

        }
    }
}
