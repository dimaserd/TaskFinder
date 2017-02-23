using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwoStu.Logic.Entities
{
    /// <summary>
    /// Уточнение по предмету это раздел предмета
    /// Например Интеграл->Двойной,Тройной
    /// Где интеграл, это SubjectDivision
    /// а Двойной,Тройной - SubjectDivisionChilds
    /// </summary>
    public class SubjectDivision
    {
        #region Constructor
        public SubjectDivision()
        {
            SubjectDivisionChilds = new List<SubjectDivisionChild>();
        }
        #endregion

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        [Required]
        [ForeignKey("FromSubjectSection")]
        public int SubjectSectionId { get; set; }

        [JsonIgnore]
        public virtual SubjectSection FromSubjectSection { get; set; }

        [JsonIgnore]
        public virtual ICollection<SubjectDivisionChild> SubjectDivisionChilds
        {
            get; set;
        }
    }
}
