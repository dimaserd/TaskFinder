using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoStu.Logic.Workers
{
    public static class StringWorker
    {
        //первый рейтинг будет прогонять совпадения подряд
        //по двум строкам: которые будут без пробелов ипрочих знаков препинания
        //также этот метод должен работать не только с начальным условием
        public static int FirstRating(string s1, string s2)
        {
            int result = 0;
            string str1, str2;
            //str1 >= str2
            if (s1.Length > s2.Length)
            {
                str1 = s1.ToLowerInvariant();
                str2 = s2.ToLowerInvariant();
            }
            else
            {
                str1 = s2.ToLowerInvariant();
                str2 = s1.ToLowerInvariant();
            }
            for (int i = 0; i < str2.Length; i++)
            {
                if (str1[i] == str2[i])
                {
                    result++;
                }
                else
                {
                    break;
                }
            }
            return result;
        }

        public static bool IsWordMatch(string wordToCompare, string wordToMatch)
        {
            return string.Compare(wordToCompare, wordToMatch, true) == 0;
        }
        //не готово
        public static bool Main(List<string> wordsInSearchQuery, List<string> wordsInSolution)
        {
            for (int i = 0; i < wordsInSearchQuery.Count; i++)
            {
                IsWordMatch(wordsInSearchQuery[i], wordsInSolution[i]);

            }
            return false;
        }

        //получает индекс первого совпадения слова для проверки из листа слов для сравнений
        //возвращает -1 если слово не найдено
        public static int GetFirstIndexOfMatchInList(string wordToCheck, List<string> wordsToCompare)
        {
            int result = -1;

            for (int i = 0; i < wordsToCompare.Count; i++)
            {
                if (IsWordMatch(wordToCheck, wordsToCompare[i]))
                {
                    result = i;
                    break;
                }
            }

            return result;
        }

        public static string RemoveSymbols(string s)
        {
            string[] toRemove = new string[] { " ", ",", ".", ":", "!", "-", "\r", "\n" };

            string result = s;
            foreach (string remove in toRemove)
            {
                result = result.Replace(remove, string.Empty);
            }

            return result;
        }
    }
}
