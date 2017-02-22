using Extensions.String;
using System.Collections.Generic;

namespace TwoStu.Logic.Workers.String
{
    public static class MainStringWorker
    {
        public static int GetNumberOfEqualWords(string textA, string textB)
        {
            List<string> setOfWordsA = textA.GetWordsFromText();
            List<string> setOfWordsB = textB.GetWordsFromText();

            return GetCountOfMatchingWords(setOfWordsA, setOfWordsB);
        }

        public static int GetCountOfMatchingWords(List<string> ASetOfWords, List<string> BSetOfWords)
        {
            int result = 0;

            for (int i = 0; i < ASetOfWords.Count; i++)
            {
                for (int j = 0; j < BSetOfWords.Count; j++)
                {
                    if (AreWordsEqual(ASetOfWords[i], BSetOfWords[j]))
                    {
                        result++;
                        break;
                    }
                }

            }

            return result;
        }

        public static bool AreWordsEqual(string wordA, string wordB)
        {
            return string.Compare(wordA, wordB, true) == 0;
        }
    }
}
