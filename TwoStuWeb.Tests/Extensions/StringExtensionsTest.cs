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

        [TestMethod]
        public void PreTest()
        {
            string problemS = "равна…Дано";

            problemS.Split(separator: new string[] { "." }, options: StringSplitOptions.RemoveEmptyEntries);
        }

        [TestMethod]
        public void TextWordsSplitText()
        {
            string testText = "№ 1.Цилиндр с массой  и с радиусом катится без проскальзывания и имеет в начальный момент времени кинетическую энергию 1800 Дж.Момент сил трения совершил работу 600 Дж.Кинетическая энергия поступательного движения цилиндра, продолжающего катиться без проскальзывания, стала после этого равна…Дано: , , , .Найти: Решение: Работа сил трения идёт на изменение кинетической энергии тела:.Кинетическая энергия поступательного движения определяется по формуле:.Кинетическая энергия вращательного движения определяется по формуле:.Тогда получим:  ,.Ответ: б) .";

            List<string> words = testText.GetWordsFromText();

            bool problemExists = words.Any(x => x == "равна…Дано");

            Assert.AreEqual(false, problemExists);
        }
    }
}
