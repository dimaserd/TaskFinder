using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoStu.Logic.Entities;

namespace TwoStu.Logic.Models.Search
{
    public class TaskSearchResult
    {
        public string SolutionId { get; set; }

        public string TaskDesc { get; set; }

        public decimal DescPercentage { get; set; }

        public decimal FilePercentage { get; set; }

        public int WordsFoundInDesc { get; set; }

        public int WordsFoundInFile { get; set; }
    }
}
