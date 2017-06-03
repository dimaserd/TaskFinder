using Extensions.String;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using TwoStu.Logic.Entities;
using TwoStu.Logic.Models;
using TwoStu.Logic.Models.TaskSolutions.Base;
using TwoStu.Logic.Models.WorkerResults;
using TwoStu.Logic.Workers.String;
using TwoStu.Settings.Solutions;

namespace TwoStu.Logic.Workers
{
    public class TaskSolutionsWorker : IDisposable
    {
        #region Конструкторы
        public TaskSolutionsWorker(MyDbContext db)
        {
            Db = db;
        }
        #endregion

        #region Свойства
        public MyDbContext Db { get; set; }
        #endregion

        #region Публичные методы

        #region Создание решений

        #region Методы с физикой
        
        
        /// <summary>
        /// Новая версия метода создания решений
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<WorkerResult> CreatePhysicsSolutionAsync(CreatePhysicsSolutionModel model)
        {
            int physicsId = (await Db.Subjects.FirstOrDefaultAsync(x => x.Name == "Физика")).Id;

            return await CreateSolutionAsync(model.ToCreateSolutionModel(physicsId));
        }

        /// <summary>
        /// Базовая версия метода для создания решений. Все модели являющиеся производными от базовой должны
        /// приводиться к базовой версии и вызывать этот метод для создания решения.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<WorkerResult> CreateSolutionAsync(CreateSolutionModelBase model)
        {
            int wordsCount = model.TaskDesc.GetWordsFromText().Count;

            //проверяем на кол-во слов в решении с настройкой
            if (wordsCount < SolutionSettings.NeededWordsCount)
            {
                return new WorkerResult
                    ("Слишком мало слов в условии!" +
                    $"Проверьте условие! Найдено слов {wordsCount} из нужных {SolutionSettings.NeededWordsCount}"
                    );
            }

            // получаем текст из файла в переменную textInFile
            string textInFile = new FileWorker().GetTextFromFile(model.File);

            //проверяем не существует ли уже решение с таким условием и файлом решения
            string errorText;
            if (IsThereAnyEqualSolution(model, textInFile, out errorText))
            {
                return new WorkerResult(errorText);
            }

            //получаю Id физики
            int physicsId = (await Db.Subjects.FirstOrDefaultAsync(x => x.Name == "Физика")).Id;

            //если программе не удалось найти текст 
            //из файла то мы должны
            //просто записать имеющиеся результаты
            if (string.IsNullOrEmpty(textInFile))
            {
                //записываем сущность в базу данных без проверки текста решения 
                //и файла 
                return await CreateSolutionWithDescOrNot(model, textInFile);
            }
            //иначе если программе удалось вытащить текст из файла
            //например docx то нужно прогнать проверку на введенное условие
            else
            {
                //проверяем совпадения слов в введеном тексте
                //и в том который достали из файла
                decimal percents;
                if (CheckSolutionsTexts(model.TaskDesc, textInFile, out percents))
                {
                    return await CreateSolutionWithDescOrNot(model, textInFile);
                }
                else
                {
                    //возвращаем причину того почему мы не можем записать это решение
                    //в банк решений
                    return new WorkerResult("Процент совпадения введенного условия и найденного в файле "
                        + $"менее 99 процентов а точнее {percents}");

                }
            }
        }
        #endregion

        /// <summary>
        /// Здесь нужно проверять утонения на принадлежность к разделу
        /// предмета а разделы предмета на принадлежность к самому предмету
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<WorkerResult> CreateSolutionBase(CreateSolutionModelBase model)
        {
            List<SubjectDivisionChild> solutionDivisionChilds = model.GetSubjectDivisions(await Db.SubjectDivisionChilds.ToListAsync());

            //сохраняем файл на сервер и получаем путь куда он был сохранен
            //так же при возможности класс получает текст из файла
            string textFromFile = string.Empty;
            string filePath = new FileWorker().SaveFileToSolution(model.File, out textFromFile);

            TaskSolution solution = model.ToTaskSolution(textFromFile);
            TaskSolutionVersion firstVersion = model.ToTaskSolutionVersion(textFromFile);

            //добавляем в базу решение
            Db.TaskSolutions.Add(solution);
            //добавяляем версию и делаем ее активной
            Db.TaskSolutionVersions.Add(model.ToTaskSolutionVersion(textFromFile));

            try
            {
                //пишем изменения в базу
                await Db.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                
            }
            

            return new WorkerResult
            {
                Succeeded = true
            };
        }
        #endregion

        #region Удаление 

        public async Task<WorkerResult> DeleteSolution(string id)
        {
            TaskSolution solution = await Db.TaskSolutions
                .Include(x => x.SubjectDivisionChilds)
                .FirstOrDefaultAsync(x => x.Id == id);

            if(solution == null)
            {
                return new WorkerResult("Решение с указанным Id не найдено!");
            }

            foreach(var child in solution.SubjectDivisionChilds.ToList())
            {
                solution.SubjectDivisionChilds.Remove(child);
            }
            await Db.SaveChangesAsync();
            Db.TaskSolutions.Remove(solution);
            await Db.SaveChangesAsync();

            return new WorkerResult
            {
                Succeeded = true
            };
        }

        #endregion

        #endregion

        #region Вспомогательные методы



        /// <summary>
        /// Проверка текстов по словам. (В дальнейшем вынеси в отдельный класс)
        /// </summary>
        /// <param name="userSolution"></param>
        /// <param name="fileSolution"></param>
        /// <param name="percents"></param>
        /// <returns></returns>
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



        #region Добавление в базу решения и его первой версии
        
        /// <summary>
        /// Новая версия
        /// </summary>
        /// <param name="model"></param>
        /// <param name="filePath"></param>
        /// <param name="textFromFile"></param>
        /// <returns></returns>
        async Task<WorkerResult> CreateSolutionWithDescOrNot(CreateSolutionModelBase model, string textFromFile)
        {
            
            //получаю решение из модели
            TaskSolution t = model.ToTaskSolution(textFromFile);

            //получаю первую версию решения
            TaskSolutionVersion firstVersion = model.ToTaskSolutionVersion(textFromFile);

            //добавление версии к решению
            t.Versions.Add(firstVersion);

            //добавляю решение с версией в базу данных
            Db.TaskSolutions.Add(t);

            //сохранение изменений в базе
            await Db.SaveChangesAsync();

            return new WorkerResult
            {
                Succeeded = true
            };
        }

        #endregion

        #region Методы проверки на повторы


        bool IsThereAnyEqualSolution(CreateSolutionModelBase model, string textFromFile, out string errorText)
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
            if (taskDescFromFileResult)
            {
                errorText = "Найден такой же файл решения!";
                return taskDescFromFileResult;
            }

            //если нет то проверяем просто по условиям
            if (taskDescResult)
            {
                errorText = "Найдено точно такое-же условие!";
                return taskDescResult;
            }

            //если никуда не зашли значит похожих решений не найдено
            errorText = "Ошибок нет!";
            return false;
        }

        #endregion

        #endregion


        #region IDisposable Support
        private bool disposedValue = false; // Для определения избыточных вызовов

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    //Db.Dispose();
                }

                // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить ниже метод завершения.
                // TODO: задать большим полям значение NULL.
                
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
