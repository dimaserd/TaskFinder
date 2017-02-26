using System;
using System.Collections.Generic;
using TwoStu.Logic.Entities;

namespace TwoStu.Logic.Models.Api
{
    public class SubjectsApiResult
    {
        public List<Subject> Subjects { get; set; }

        public List<SubjectSection> SubjectSections { get; set; }

        public List<SubjectDivision> SubjectDivisions { get; set; }

        public List<SubjectDivisionChild> SubjectDivisionChilds { get; set; }
    }
}
