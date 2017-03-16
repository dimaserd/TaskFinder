using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

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
        #region Many to many relationship
        [JsonIgnore]
        public virtual IList<TaskSolution> TaskSolutions { get; set; }

        [JsonIgnore]
        public virtual IList<TaskSolutionVersion> TaskSolutionVersions { get; set; }
        #endregion
    }

    #region Extensions
    public static class SubjectDivisionChildExtensions
    {
        public static IEnumerable<SubjectDivisionChild> GetSubjectDivisionsFromString(this IEnumerable<SubjectDivisionChild> divisionChilds, string divisionsString)
        {
            if(string.IsNullOrEmpty(divisionsString))
            {
                return new List<SubjectDivisionChild>();
            }


            List<int> divisionIds = divisionsString
                .Split(separator: new string[] { "," }, options: StringSplitOptions.RemoveEmptyEntries)
                .Select(x =>
                {
                    int temp;
                    if (int.TryParse(x, out temp))
                    {
                        return temp;
                    }
                    else
                    {
                        return 0;
                    }
                }).Where(x => x != 0)
                .ToList();

            return divisionChilds.Where(x => divisionIds.Contains(x.Id)).ToList();
        }
    }

    #endregion
}
