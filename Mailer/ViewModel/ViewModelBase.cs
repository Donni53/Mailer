using System.Collections.Generic;
using Mailer.Helpers;

namespace Mailer.ViewModel
{
    public class ViewModelBase : GalaSoft.MvvmLight.ViewModelBase
    {
        private bool _isWorking;

        public bool IsWorking
        {
            get => _isWorking;
            set => Set(ref _isWorking, value);
        }

        public Dictionary<string, LongRunningOperation> Tasks { get; } = new Dictionary<string, LongRunningOperation>();

        /// <summary>
        ///     Calls when viewmodel is activated
        /// </summary>
        public virtual void Activate()
        {
        }

        /// <summary>
        ///     Calls when viewmodel is deactivated
        /// </summary>
        public virtual void Deactivate()
        {
        }

        #region Long Running Operations helpers

        private void RegisterTask(string id)
        {
            Tasks.Add(id, new LongRunningOperation());
        }

        protected void RegisterTasks(params string[] ids)
        {
            foreach (var id in ids) RegisterTask(id);
        }

        protected void OnTaskStarted(string id)
        {
            Tasks[id].Error = null;
            Tasks[id].IsWorking = true;
        }

        protected void OnTaskFinished(string id)
        {
            Tasks[id].IsWorking = false;
        }

        protected void OnTaskError(string id, string error)
        {
            Tasks[id].Error = error;
            Tasks[id].IsWorking = false;
        }

        #endregion
    }
}