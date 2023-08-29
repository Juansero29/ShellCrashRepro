using log4net.Core;
using EnigmatiKreations.Framework.Utils.Extensions;
using Randomizer.Framework.ViewModels.Commanding.Contract;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using EnigmatiKreations.Framework.MVVM.BaseViewModels;
using log4net;

namespace Randomizer.Framework.ViewModels.Commanding
{

    public interface ICommandAsync : ICommand
    {
        Task ExecuteAsync();
        bool CanExecute();
    }



    public class SimpleCommandAsync : BaseViewModel, ICommandAsync, IReportProgressCommand, INotifyPropertyChanged
    {
        public event EventHandler CanExecuteChanged;

        private readonly Func<Task> _execute;
        private readonly Func<bool> _canExecute;
        private readonly ILog _errorHandler;



        private bool _IsExecuting;
        private Action<Exception> _onExceptionAction;

        /// <summary>
        /// Is this command executing
        /// </summary>
        public bool IsExecuting
        {
            get => _IsExecuting;
            set => SetProperty(ref _IsExecuting, value);
        }

        public SimpleCommandAsync(Func<Task> execute, Func<bool> canExecute = null, Action<Exception> onException = null, ILog errorHandler = null)
        {
            _execute = execute;
            _canExecute = canExecute;
            _errorHandler = errorHandler;
            _onExceptionAction = onException;
        }

        public bool CanExecute()
        {
            return !_IsExecuting && (_canExecute?.Invoke() ?? true);
        }

        public async Task ExecuteAsync()
        {
            if (CanExecute())
            {
                try
                {
                    if(_execute.Target is BasePageViewModel vm)
                    {
                        vm.IsBusy = true;
                    }

                    IsExecuting = true;
                    await _execute();

                    if (_execute.Target is BasePageViewModel vm2)
                    {
                        vm2.IsBusy = false;
                    }
                }
                catch(Exception e)
                {
                    _errorHandler?.Error(e.Message, e);
                    _onExceptionAction?.Invoke(e);
                }
                finally
                {
                    IsExecuting = false;
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

        #region Explicit implementations
        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute();
        }

        void ICommand.Execute(object parameter)
        {
            ExecuteAsync().FireAndForgetSafeAsync(_errorHandler);
        }
        #endregion
    }
}
