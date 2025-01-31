using System.ComponentModel.DataAnnotations;

namespace TestBackEnd.Domain.Enums
{
    public enum TypeTask
    {
        [Display(Name = "Revisión")]
        Review = 1,

        [Display(Name = "Desarrollo")]
        Develop = 2,

        [Display(Name = "Prueba")]
        Test = 3,

        [Display(Name = "Bug")]
        Bug = 4
    }
}
