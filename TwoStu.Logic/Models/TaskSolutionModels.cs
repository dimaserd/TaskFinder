using System.ComponentModel.DataAnnotations;
using System.Web;

namespace TwoStu.Logic.Models
{
    #region Subject Types Enums
    public enum PhysicsType
    {
        Mechanics, Kinеmatics, ThermoDynamics,
    }
    #endregion

    public class CreatePhysicsSolutionModel
    {
        [Required]
        [Display(Name = "Условие задачи")]
        public string TaskDesc { get; set; }

        [Required]
        [Display(Name = "Раздел физики")]
        public int SubjectSectionId { get; set; }

        [Required]
        [Display(Name = "Тип работы")]
        public int WorkTypeId { get; set; }

        [Required]
        [Display(Name = "Файл решения")]
        public HttpPostedFileBase File { get; set; }
    }

    public class CreateSolutionModel
    {

        [Required]
        [Display(Name = "Условие задачи")]
        public string TaskDesc { get; set; }

        [Required]
        [Display(Name = "Файл решения")]
        public HttpPostedFileBase File { get; set; }
    }
}
