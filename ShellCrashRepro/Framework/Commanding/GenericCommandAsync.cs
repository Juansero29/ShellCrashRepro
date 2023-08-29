using log4net.Core;
using EnigmatiKreations.Framework.Utils.Extensions;
using Randomizer.Framework.ViewModels.Commanding.Contract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using log4net;

namespace Randomizer.Framework.ViewModels.Commanding
{

    public interface IGenericCommandAsync<T> : ICommand
    {
        Task ExecuteAsync(T param);
        Task<bool> CanExecute(T param);
    }

    public class GenericCommandAsync<T> : IGenericCommandAsync<T>, IReportProgressCommand
    {
        public event EventHandler CanExecuteChanged;

        private bool _isExecuting;
        private readonly Func<T, Task> _execute;
        private readonly Func<T, Task<bool>> _canExecute;
        private readonly ILog _errorHandler;
        private readonly Action<Exception> _onExceptionAction;

        public GenericCommandAsync(Func<T, Task> execute, Func<T, Task<bool>> canExecute = null, Action<Exception> onException = null, ILog errorHandler = null)
        {
            _execute = execute;
            _canExecute = canExecute;
            _errorHandler = errorHandler;
            _onExceptionAction = onException;
        }



        public async Task<bool> CanExecute(T obj)
        {
            var canExecute = _canExecute == null || await _canExecute?.Invoke(obj);
            return !_isExecuting && canExecute;
        }

        public void Execute(T parameter)
        {
            ExecuteAsync(parameter).FireAndForgetSafeAsync(_errorHandler);
        }

        public async Task ExecuteAsync(T param)
        {
            if (await CanExecute(param))
            {
                try
                {
                    _isExecuting = true;
                    await  _execute(param);
                }
                catch(Exception e)
                {
                    _errorHandler.Error(e.Message, e);
                    _onExceptionAction?.Invoke(e);
                }
                finally
                {
                    _isExecuting = false;
                }
            }

            RaiseCanExecuteChanged();
        }

        public void ReportProgress(Action action)
        {
            Task.Run(action);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        #region Explicit Implementations

        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute((T)parameter).Result;
        }

        void ICommand.Execute(object parameter)
        {
            ExecuteAsync((T)parameter).FireAndForgetSafeAsync(_errorHandler);
        }
        #endregion
    }
}
