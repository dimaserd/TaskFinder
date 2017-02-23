using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwoStu.Logic.Entities
{
    public class SubjectDivisionChild
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        
        public string Description { get; set; }

        [Required]
        [ForeignKey("SubjectDivisionParent")]
        public int SubjectDivisionId { get; set; }
        [JsonIgnore]
        public virtual SubjectDivision SubjectDivisionParent
        {
            get; set;
        }

        public virtual ICollection<TaskSolution> TaskSolutions { get; set; }
    }
}
