using Extensions.String;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace TwoStuWeb.Tests.Extensions
{
    [TestClass]
    public class StringWordEndingExtensionTests
    {
        [TestMethod]
        public void OnlyUniqueEndings()
        {
            bool result = false;
            string duplicate = string.Empty;

            List<string> endings = WordEndingStringExtensions.AllWordEndings.ToList();

            foreach(string ending in endings)
            {
                if(endings.Count(x => x == ending) > 1)
                {
                    result = true;
                    duplicate = ending;
                }
            }

            Assert.AreEqual(string.Empty, duplicate);
            Assert.AreEqual(false, result);
        }
    }
}
