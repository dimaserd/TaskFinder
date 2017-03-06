using System.Collections.Generic;
using TwoStu.Logic.Entities;

namespace TwoStu.Logic.Models.Search
{
    /// <summary>
    /// Недоделано
    /// </summary>
    public class SearchParamsModel
    {
        public Subject SubjectParam { get; set; }

        public SubjectSection SubjectSectionParam { get; set; }

        public IEnumerable<object> Searchs { get; set; }
    }
}
