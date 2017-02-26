using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TwoStu.Logic.Models.TaskSolutions
{

    public class CreateSolutionModel
    {
        [Required]
        public int SubjectId { get; set; }

        [Required]
        public int SubjectSectionId { get; set; }

        [Required]
        public int WorkTypeId { get; set; }

        

        [Required]
        public string DivisionChildsString { get; set; }

        [Required]
        [Display(Name = "Условие задачи")]
        public string TaskDesc { get; set; }

        [Required]
        [Display(Name = "Файл решения")]
        public HttpPostedFileBase File { get; set; }

        
    }

}
