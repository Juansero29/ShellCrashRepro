using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Randomizer.Framework.ViewModels.Commanding
{

    /// <summary>
    /// Allows to bind a method without parameters to a Command property of a control by binding in XAML.
    /// </summary>
    public class SimpleCommand : ICommand
    {

        private bool _IsExecuting = false;
        private readonly Command _InternalCommand;
        private System.Threading.Timer _timer;

        /// <summary>
        /// Constructor without support for the CanExecute mechanism
        /// </summary>
        /// <param name="execute">Method called by the command</param>
        /// <remarks></remarks>
        public SimpleCommand(Action execute)
        {
            _InternalCommand = new Command(execute);
        }

        /// <summary>
        /// Constructeur avec prise en charge du mécanisme canExecute
        /// </summary>
        /// <param name="execute">Method to be called</param>
        /// <param name="canExecute">Function allowing to evaluate wether to execute the command or not</param>
        /// <remarks>
        /// Use-case :
        /// ToggledCommand = new SimpleCommand(Method, CanExecuteMethod)
        /// CanExecuteMethod is either a methodthat decides if we can execute the method or not
        /// </remarks>
        public SimpleCommand(Action execute, System.Func<bool> canExecute)
        {
            _InternalCommand = new Command(execute, canExecute);
        }

        event EventHandler ICommand.CanExecuteChanged
        {
            add
            {
                _InternalCommand.CanExecuteChanged += value;
            }

            remove
            {
                _InternalCommand.CanExecuteChanged -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            return !_IsExecuting && _InternalCommand.CanExecute(parameter);
        }

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter)) return;

            _IsExecuting = true;
            _InternalCommand.Execute(parameter);
            _timer = new System.Threading.Timer(ChangeIsExecuting);
            _timer.Change(200, Timeout.Infinite);
        }

        private void ChangeIsExecuting(object state)
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
            _timer.Dispose();
            _IsExecuting = false;
        }

        public void CallCanExecute()
        {
            _InternalCommand.ChangeCanExecute();
        }
    }

}
