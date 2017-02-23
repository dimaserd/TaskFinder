using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwoStu.Logic.Entities
{
    /// <summary>
    /// Конкретный пример уточнения по предмету связан со многими решениями
    /// и с одним родительским уточнением
    /// Интеграл->Двойной,Тройной
    /// где Двойной,Тройной - экземпляры класса SubjectDivisionChild
    /// а Интеграл SubjectDivisionParent
    /// </summary>
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
