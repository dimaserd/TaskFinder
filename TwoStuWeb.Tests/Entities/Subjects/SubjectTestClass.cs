using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoStu.Logic.Entities;

namespace TwoStuWeb.Tests.Entities.Subjects
{
    [TestClass]
    public class SubjectTestClass
    {
        [TestMethod]
        public void TestGetSubjectNamesString()
        {
            List<Subject> subjects = new List<Subject>();

            subjects.Add(new Subject { Name = "Subject1" });
            subjects.Add(new Subject { Name = "Subject2" });

            string result = subjects.GetSubjectNamesString();

            Assert.AreEqual("Subject1,Subject2", result);
        }
    }
}
