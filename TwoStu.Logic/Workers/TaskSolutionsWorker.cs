using Extensions.String;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwoStu.Logic.Entities;
using TwoStu.Logic.Models;
using TwoStu.Logic.Models.WorkerResults;
using TwoStu.Logic.Workers.String;

namespace TwoStu.Logic.Workers
{
    public class TaskSolutionsWorker : IDisposable
    {
        #region Constructors
        public TaskSolutionsWorker(MyDbContext db)
        {
            Db = db;
        }
        #endregion
        #region Properties
        public MyDbContext Db { get; set; }
        #endregion

        #region Public Methods
        public async Task<WorkerResult> CreatePhysicsSolution(CreatePhysicsSolutionModel model)
        {
            int wordsCount = model.TaskDesc.GetWordsFromText().Count;
            int wordsSetting = 20 - 12;

            if (wordsCount < wordsSetting)
            {
                return new WorkerResult
                    ("Слишком мало слов в условии!" + 
                    $"Проверьте условие! Найдено слов {wordsCount} из нужных {wordsSetting}"
                    ); 
            }

            //сохраняем файл и получаем текст из файла
            //в переменную textInFile
            string textInFile;
            string filePath = new FileWorker().SaveFileToSolution(model.File, out textInFile);

            //проверяем не существует ли уже заказ с таким условием и файлом решения
            string errorText;
            if(IsThereAnyEqualOrder(model, textInFile, out errorText))
            {
                return new WorkerResult(errorText);
            }

            //если программе не удалось найти текст 
            //из файла то мы должны
            //просто записать имеющиеся результаты
            if(string.IsNullOrEmpty(textInFile))
            {
                //записываем сущность в базу данных без проверки текста решения 
                //и файла 
                return await CreatePhysicsSolutionWithDescOrNot(model, filePath, textInFile);
            }
            //иначе если программе удалось вытащить текст из файла
            //например docx то нужно прогнать проверку на введенное условие
            else
            {
                //проверяем совпадения слов в введеном тексте
                //и в том который достали из файла
                decimal percents;
                if(CheckSolutionsTexts(model.TaskDesc, textInFile, out percents))
                {
                    return await CreatePhysicsSolutionWithDescOrNot(model, filePath, textInFile);
                }
                else
                {
                    //удаляем только что записанный файл из системы
                    System.IO.File.Delete(filePath);
                    
                    //возвращаем причину того почему мы не можем записать это решение
                    //в банк решений
                    return new WorkerResult("Процент совпадения введенного условия и найденного в файле "
                        + $"менее 99 процентов а точнее {percents}");
                    
                }
            }

            

            

            
        }
        #endregion

        #region Help Methods
        bool CheckSolutionsTexts(string userSolution, string fileSolution, out decimal percents)
        {
            List<string> userWords = userSolution.GetWordsFromText();
            List<string> fileWords = fileSolution.GetWordsFromText();

            int wordsCountInUserSolution = userSolution.GetWordsFromText().Count;
            
            int equalsCount = MainStringWorker.GetNumberOfEqualWords(userSolution, fileSolution);

            //все слова содержищиеся в условии должны быть в файле решения
            percents = ((decimal)equalsCount / (decimal)wordsCountInUserSolution);
            return percents == 1;
        }

        async Task<WorkerResult> CreatePhysicsSolutionWithDescOrNot(CreatePhysicsSolutionModel model, string filePath, string textFromFile)
        {
            int physicsId = Db.Subjects.FirstOrDefault(x => x.Name == "Физика").Id;

            string mark = $"{physicsId}|{model.SubjectSectionId}";

            

            TaskSolution t = new TaskSolution
            {
                Id = Guid.NewGuid().ToString(),
                SubjectId = physicsId,
                SubjectSectionId = model.SubjectSectionId,
                CreationDate = DateTime.Now,

                FileName = model.File.FileName,
                Mark = mark,
                TaskDesc = model.TaskDesc,
                WorkTypeId = model.WorkTypeId,
                TrimmedTaskDesc = StringWorker.RemoveSymbols(model.TaskDesc).ToLowerInvariant(),
                TaskDescFromFile = textFromFile,
                //то записываем без слов полученных из файла
                FilePath = filePath,

            };
            Db.TaskSolutions.Add(t);
            await Db.SaveChangesAsync();

            return new WorkerResult
            {
                Succeeded = true
            };
        }
        
        bool IsThereAnyEqualOrder(CreatePhysicsSolutionModel model, string textFromFile, out string errorText)
        {
            //если текст из файла пустой
            //то есть его не удалось прочесть
            //то проверяем по условию задачи
            if (string.IsNullOrEmpty(textFromFile))
            {
                errorText = "Найдено точно такое-же условие!";
                //если что-то нашлось с тем-же описанием
                return Db.TaskSolutions.Any(x => x.TaskDesc == model.TaskDesc);
            }


            bool taskDescResult = Db.TaskSolutions.Any(x => x.TaskDesc == model.TaskDesc);
            bool taskDescFromFileResult = Db.TaskSolutions.Any(x => x.TaskDescFromFile == textFromFile);
            
            //если текст из файлов идентичен
            if(taskDescFromFileResult)
            {
                errorText = "Найден такой же файл решения!";
                return taskDescFromFileResult;
            }

            //если нет то проверяем просто по условиям
            if(taskDescResult)
            {
                errorText = "Найдено точно такое-же условие!";
                return taskDescResult;
            }

            //если никуда не зашли значит похожих решений не найдено
            errorText = "Ошибок нет!";
            return false;
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
                    Db.Dispose();
                }

                // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить ниже метод завершения.
                // TODO: задать большим полям значение NULL.
                Db = null;
                disposedValue = true;
            }
        }

        // TODO: переопределить метод завершения, только если Dispose(bool disposing) выше включает код для освобождения неуправляемых ресурсов.
        // ~TaskSolutionsWorker() {
        //   // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
        //   Dispose(false);
        // }

        // Этот код добавлен для правильной реализации шаблона высвобождаемого класса.
        public void Dispose()
        {
            // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
