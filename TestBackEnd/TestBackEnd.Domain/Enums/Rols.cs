using System.ComponentModel.DataAnnotations;

namespace TestBackEnd.Domain.Enums
{
    public enum Rols
    {
        [Display(Name = "Administrador")]
        administrator = 1,

        [Display(Name = "Líder de Proyecto")]
        LeaderProject = 2,

        [Display(Name = "Desarrollador")]
        developer = 3,

        [Display(Name = "Tester")]
        Tester = 4
    }
}
