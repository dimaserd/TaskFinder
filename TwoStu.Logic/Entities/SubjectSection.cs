

using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwoStu.Logic.Entities
{
    public class SubjectSection
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Name { get; set; }

        [Required]
        [ForeignKey("FromSubject")]
        public int SubjectId { get; set; }
        [JsonIgnore]
        public virtual Subject FromSubject { get; set; }

        [JsonIgnore]
        public virtual IList<SubjectDivision> SubjectDivisions { get; set; }

        [JsonIgnore]
        public virtual IList<TaskSolution> TaskSolutions { get; set; }
    }

    public static class SubjectSectionExtensions
    {
        public static string GetSearchParamsString(this SubjectSection subjectSection)
        {
            return $"?subjectId={subjectSection.FromSubject.Id}&subjectSectionId={subjectSection.Id}&needSearch=true";
        }

        public static string GetCreateParamsString(this SubjectSection subjectSection)
        {
            return $"?subjectId={subjectSection.FromSubject.Id}&subjectSectionId={subjectSection.Id}";
        }
    }
}
