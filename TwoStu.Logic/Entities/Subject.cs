using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

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
        public virtual IList<SubjectSection> SubjectSections { get; set; }
    }

    public static class SubjectExtensions
    {
        public static string GetSubjectNamesString(this IEnumerable<Subject> subjects)
        {
            StringBuilder sb = new StringBuilder();

            foreach(Subject subject in subjects)
            {
                sb.Append($"{subject.Name},");
            }

            string result = sb.ToString();

            return result.Substring(0, result.Length - 1);
        }
    }
}
