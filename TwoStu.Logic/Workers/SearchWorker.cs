using Extensions.String;
using System;
using System.Collections.Generic;
using System.Linq;
using TwoStu.Logic.Entities;
using TwoStu.Logic.Models.Search;
using TwoStu.Logic.Workers.String;
using System.Data.Entity;

namespace TwoStu.Logic.Workers
{
    public class SearchWorker : IDisposable
    {
        #region Constructor
        public SearchWorker(MyDbContext db)
        {
            Db = db;
            TaskSolutions = Db.TaskSolutions.Include(x => x.TypeOfWork)
                .Include(x => x.TaskSubject).Include(x => x.TaskSubjectSection)
                .ToList();
        }
        #endregion

        

        #region Properties
        MyDbContext Db { get; set; }

        List<TaskSolution> TaskSolutions { get; set; }
        #endregion

        #region Public Methods
        public IEnumerable<TaskSearchResult> SearchByTaskDesc(string desc, bool searchFile = false, bool markText = false)
        {
            if(desc == string.Empty)
            {
                return null;
            }

            List<string> searchWords = desc.GetWordsFromText();

            List<TaskSearchResult> result = TaskSolutions
                .Select(x =>
                {
                    int countOfMatchesInDesc = 0;
                    decimal descPercentage = GetDescPercentage(x, searchWords, out countOfMatchesInDesc);

                    int countOfMatchesInFile = 0;
                    decimal filePercentage = GetFilePercentage(x, searchWords, out countOfMatchesInFile);

                    return new TaskSearchResult
                    {
                        SolutionId = x.Id,
                        TaskDesc = (markText) ? x.TaskDesc.MarkManyText(searchWords.ToArray()) : x.TaskDesc,

                        DescPercentage = descPercentage,
                        FilePercentage = filePercentage,
                        
                        WordsFoundInDesc = countOfMatchesInDesc,
                        WordsFoundInFile = countOfMatchesInFile
                    };
                })
                .Where(x => x.WordsFoundInDesc > 0).ToList();

            return result;
        }
        #endregion

        #region Help Methods
        decimal GetDescPercentage(TaskSolution solution, List<string> searchWords, out int countOfMatches)
        {
            //получаем слова из решения
            List<string> solutionWords = solution.TaskDesc.GetWordsFromText();

            countOfMatches = MainStringWorker.GetCountOfMatchingWords(searchWords, solutionWords);

            return (decimal)countOfMatches / (decimal)searchWords.Count;
        }

        decimal GetFilePercentage(TaskSolution solution, List<string> searchWords, out int countOfMatches)
        {
            //получаем слова из решения
            List<string> fileWords = solution.TaskDescFromFile.GetWordsFromText();

            countOfMatches = MainStringWorker.GetCountOfMatchingWords(searchWords, fileWords);

            return (decimal)countOfMatches / (decimal)searchWords.Count;
        }

        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // Для определения избыточных вызовов

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if(Db != null)
                    {
                        Db.Dispose();
                        Db = null;
                    }
                }

                // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить ниже метод завершения.
                // TODO: задать большим полям значение NULL.

                disposedValue = true;
            }
        }

        // TODO: переопределить метод завершения, только если Dispose(bool disposing) выше включает код для освобождения неуправляемых ресурсов.
        // ~SearchWorker() {
        //   // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
        //   Dispose(false);
        // }

        // Этот код добавлен для правильной реализации шаблона высвобождаемого класса.
        public void Dispose()
        {
            // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
            Dispose(true);
            // TODO: раскомментировать следующую строку, если метод завершения переопределен выше.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
