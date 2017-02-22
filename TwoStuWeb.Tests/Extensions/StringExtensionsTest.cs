using Extensions.String;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoStuWeb.Tests.Extensions
{
    [TestClass]
    public class StringExtensionsTest
    {
        [TestMethod]
        public void GetWordsFromWordWithSymbolsTest()
        {
            //1.Тонкий
            //стержнем.Дано
            //Решение:Разобьем
            //равна:.Проинтегрировав
            //получим:,.Ответ
            string test = "1.Тонкий";

            List<string> result = test.GetWordsFromWordWithSymbols();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Тонкий", result[0]);
        }
    }
}
