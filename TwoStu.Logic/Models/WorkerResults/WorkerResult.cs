using System.Collections.Generic;

namespace TwoStu.Logic.Models.WorkerResults
{
    public class WorkerResult
    {
        #region Constructors
        public WorkerResult()
        {
            Construct();
        }

        public WorkerResult(string Error)
        {
            Construct();
            ErrorsList.Add(Error);
        }

        void Construct()
        {
            Succeeded = false;
            ErrorsList = new List<string>();
        }
        #endregion

        #region Private Fields

        #endregion

        #region Properties
        public bool Succeeded { get; set; }

        public List<string> ErrorsList { get; set; }
        #endregion
    }
}
