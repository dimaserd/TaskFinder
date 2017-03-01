using System.ComponentModel.DataAnnotations;

namespace TwoStu.Logic.Models.TaskSolutions
{
    public class SearchSolutionsModel
    {
        [Required]
        [Display(Name = "Условие задачи")]
        public string TaskDesc { get; set; }

        [Required]
        [Display(Name = "Предмет")]
        public int SubjectId
        {
            get; set;
        }

        [Required]
        [Display(Name = "Раздел предмета")]
        public int SubjectSectionId { get; set; }

        [Required]
        [Display(Name = "Тип работы")]
        public int WorkTypeId { get; set; }

        [Required]
        public string DivisionChildsString { get; set; }
    }
}
