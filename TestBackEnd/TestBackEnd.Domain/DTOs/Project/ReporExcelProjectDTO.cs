using System.ComponentModel;
using StateEnum = TestBackEnd.Domain.Enums.State;

namespace TestBackEnd.Domain.DTOs.Project
{
    public class ReporExcelProjectDTO
    {
        [DisplayName("Nombre proyecto")]
        public string Name { get; set; }

        [DisplayName("Descripción")]
        public string Description { get; set; }

        
        public int State { get; set; }

        [DisplayName("Estado")]
        public string StateString {
            get
            {
                switch (State)
                {
                    case (int)StateEnum.InActive:
                        return "Inactivo";

                    case (int)StateEnum.Active:
                        return "Activo";
                    case (int)StateEnum.pending:
                        return "Pendiente";
                    case (int)StateEnum.process:
                        return "Proceso";
                    case (int)StateEnum.Complete:
                        return "Completo";
                    case (int)StateEnum.Cancel:
                        return "Cancelada";
                    default:
                        return "Sin estado";
                }
            }
        }

        [DisplayName("Fecha inicio")]
        public DateTime StartDate { get; set; }

        [DisplayName("Fecha Final")]
        public DateTime? EndDate { get; set; }

        [DisplayName("Cantidad de tareas review")]
        public int ReviewCount { get; set; }
        
        [DisplayName("Cantidad de tareas desarrollo")]
        public int DevelopCount { get; set; }

        [DisplayName("Cantidad de tareas test")]
        public int TestCount { get; set; }

        [DisplayName("Cantidad de tareas bug")]
        public int BugCount { get; set; }
    }
}
