﻿using System.Data.Entity;
using TwoStu.Logic.Entities;
using TwoStu.Logic.Models;

namespace TwoStu.Logic
{
    public class MyDbContext : ApplicationDbContext
    {
        public DbSet<TaskSolution> TaskSolutions { get; set; }

        public DbSet<Subject> Subjects { get; set; }

        public DbSet<SubjectSection> SubjectSections { get; set; }



        public DbSet<WorkType> WorkTypes { get; set; }
    }
}