using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Extensions.String
{
    public static class StringExtensions
    {
        public static string MarkManyText(this string Text, string[] words)
        {
            
            foreach (string word in words)
            {
                Text = Text.MarkText(word);
            }
            return Text;
        }

        public static string MarkManyText(this string Text, string toMarkMany)
        {
            string[] words = toMarkMany.Split(separator: new string[] { "," }, options: StringSplitOptions.RemoveEmptyEntries);

            foreach (string word in words)
            {
                Text = Text.MarkText(word);
            }
            return Text;
        }

        public static string MarkText(this string Text, string toMark)
        {
            int start = Text.ToLower().IndexOf(toMark.ToLower());

            if (start == -1)
            {
                return Text;
            }
            return $"{Text.Substring(0, start)}<mark>{Text.Substring(start, toMark.Length)}</mark>{Text.Substring(start + toMark.Length)}";
        }

        public static string ForToolTip(this string s)
        {
            int enterSetting = 7;
            int lengthSetting = 50;

            int count = 0;
            int beforeEnterLength = 0;


            string result = string.Empty;

            for (int i = 0; i < s.Length; i++)
            {
                beforeEnterLength++;

                if (s[i].CheckCharForSymbols())
                {
                    count++;
                }

                if (count >= enterSetting && beforeEnterLength >= lengthSetting)
                {
                    count = 0;
                    beforeEnterLength = 0;
                    result += s[i] + "\n";
                }
                else
                {
                    result += s[i];
                }
            }

            return result;
        }

        public static bool CheckCharForSymbols(this char ch)
        {
            char[] symbols = new char[] { '.', ',', ' ', ':' };

            bool result = false;

            foreach (char c in symbols)
            {
                if (c == ch)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }


        #region returning MvcHtmlString
        public static MvcHtmlString ToHtml(this string s)
        {
            return MvcHtmlString.Create(s);
        }
        #endregion

        #region returning string methoods
        public static string GetExtensionFromFileName(this string fileName)
        {
            return fileName.Split(separator: new char[] { '.' }).Last();
        }
        #endregion

        #region returning bool methods
        public static bool HasAnyOf(this string s, IEnumerable<string> subStrings)
        {
            bool result = false;

            foreach(string subString in subStrings)
            {
                if(s.Contains(subString))
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        public static bool HasAnyOf(this string s, params string[] subStrings)
        {
            bool result = false;

            foreach (string subString in subStrings)
            {
                if (s.Contains(subString))
                {
                    result = true;
                    break;
                }
            }

            return result;
        }
        #endregion

        #region returning IEnumerable<string> methods
        /// <summary>
        /// Метод удаляет слишком короткие слова
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        public static List<string> CleanWords(this List<string> words)
        {
            List<string> result = new List<string>();
            foreach (string word in words)
            {
                if (word.Length > 2)
                {
                    result.Add(word);
                }

            }
            return result;
        }

        //разбивает текст на слова и возвращает список из слов
        //из слов удаляются слишком короткие слова
        public static List<string> GetWordsFromText(this string text)
        {
            if(string.IsNullOrEmpty(text))
            {
                return null;
            }
            //var text = "'Oh, you can't help that,' said the Cat: 'we're all mad here. I'm mad. You're mad.'";
            char[] punctuation = text.Where(Char.IsPunctuation).Distinct().ToArray();

            List<string> words = text.Split().Select(x => x.Trim(punctuation)).ToList().CleanWords();

            //запуск вспомогательного метода
            return HelpForGetWordsFromTextMethod(words);
        }

        public static List<string>  HelpForGetWordsFromTextMethod(List<string> words)
        {
            //1.Тонкий
            //стержнем.Дано
            //Решение:Разобьем
            //равна:.Проинтегрировав
            //получим:,.Ответ

            List<string> result = new List<string>();

            foreach(string word in words)
            {
                result.AddRange(GetWordsFromWordWithSymbols(word));
            }
            return result;
        }

        #region Help Methods

        public static List<string> GetWordsFromWordWithSymbols(this string word)
        {
            string[] toReplaces = new string[] 
            {
                "0", "1", "2", "3", "4",
                "5", "6", "7", "8", "9",
                ":", ",", ".", "?", ";",
                "\\", "|", "~", "-", "'",
                "\"", ">", "<", "`", "*",
                "(", ")"

            };
            return word.Split(separator: toReplaces, options: StringSplitOptions.RemoveEmptyEntries).ToList();
        }
        #endregion
        #endregion


    }
}
