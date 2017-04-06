using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TwoStu.Logic;
using TwoStu.Logic.Entities;
using TwoStu.Logic.Models;
using TwoStu.Logic.Models.WorkerResults;

namespace TwoStuWeb.Controllers.Base
{
    public class BaseController : Controller
    {
        #region Поля
        protected static MyDbContext _db;

        /// <summary>
        /// Переопределяется в классе наследнике
        /// </summary>
        IDisposable[] toDisposes = new IDisposable[]
        {
                    
        };

        
        #endregion

        #region Свойства
        protected static MyDbContext Db
        {
            get
            {
                if (_db == null)
                {
                    _db = new MyDbContext();
                }
                return _db;
            }
        }
        #endregion

        #region Методы

        /// <summary>
        /// По этой функции мы узнаем можно ли пользователю пользоваться сервисом
        /// если дата вышла значит его надо разлогинить
        /// </summary>
        /// <returns></returns>
        protected bool IsDateExpired()
        {
            if(!Request.IsAuthenticated)
            {
                return false;
            }

            DateTime dateNow = DateTime.Now;

            DateTime expDate = User.Identity.GetExpirationDate();
            if (expDate == null)
            {
                return false;
            }

            return expDate > dateNow;
        }

        protected WorkerResult UserHasRightsToBeThere()
        {
            if (!User.IsInRole("Admin"))
            {
                return new WorkerResult("У вас недостаточно прав!");
            }

            return new WorkerResult
            {
                Succeeded = true
            };
        }

        protected async Task<WorkerResult> UserHasRightsToCreateThatSubjectSectionAsync(SubjectSection subjectSection)
        {
            List<Subject> subjects = await Db.Subjects.ToListAsync();

            //получили список предметов пользователя
            List<Subject> userSubjects = User.Identity.GetUserSubjects(subjects).ToList();

            if (userSubjects.Any(x => x.Id == subjectSection.SubjectId))
            {
                return new WorkerResult
                {
                    Succeeded = true
                };

            }

            return new WorkerResult("У вас недостаточно прав для создания раздела по данному предмету!");
        }


        protected async Task<WorkerResult> UserHasRightsForThatSubjectAsync(int subjectId)
        {
            List<Subject> subjects = await Db.Subjects.ToListAsync();

            List<Subject> userSubjects = User.Identity.GetUserSubjects(subjects).ToList();


            if (userSubjects.Any(x => x.Id == subjectId))
            {
                return new WorkerResult
                {
                    Succeeded = true
                };
            }


            return new WorkerResult($"У вас недостаточно прав для создания решения по предмету {subjects.FirstOrDefault(x => x.Id == subjectId).Name}!\n"
                + $"Вы можете создавать решения только по предметам {userSubjects.GetSubjectNamesString()}");
        }


        protected void AddErrors(WorkerResult workerResult)
        {
            foreach (string error in workerResult.ErrorsList)
            {
                ModelState.AddModelError("", error);
            }
        }

        #region Dispose
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {

                for (int i = 0; i < toDisposes.Length; i++)
                {
                    if (toDisposes[i] != null)
                    {
                        toDisposes[i].Dispose();
                        toDisposes[i] = null;
                    }
                }
            }
            base.Dispose(disposing);
        }
        #endregion

        #endregion


    }
}