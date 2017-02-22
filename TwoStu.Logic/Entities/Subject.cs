using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwoStu.Logic.Entities
{
    public class Subject
    {
        public Subject()
        {
            SubjectSections = new List<SubjectSection>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<SubjectSection> SubjectSections { get; set; }
    }
}
