using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Randomizer.Framework.ViewModels.Commanding
{
    /// <summary>
    /// Allows to bind a method with a generic parameter to a Command property of a Xamarin control by binding in XAML.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericCommand<T> : ICommand
    {
        private readonly Command<T> _InternalCommand;
        private bool _IsExecuting = false;
        private System.Threading.Timer _timer;

        /// <summary>
        /// Constructor without support for the CanExecute mechanism
        /// </summary>
        /// <param name="execute">Method called by the command</param>
        /// <remarks></remarks>
        public GenericCommand(Action<T> execute)
        {
            _InternalCommand = new Command<T>(execute);
        }

        /// <summary>
        /// Constructor with support for the CanExecute mechanism
        /// </summary>
        /// <param name="execute">Méthode appelée par la commande avec un paramètre typé</param>
        /// <param name="canExecute">Fonction permettant d'évaluer la possibilité d'exécuter une commande</param>
        /// <remarks></remarks>
        public GenericCommand(Action<T> execute, System.Func<T, bool> canExecute)
        {
            _InternalCommand = new Command<T>(execute, canExecute);
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

        private void ChangeIsExecuting(object state)
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
            MainThread.BeginInvokeOnMainThread(() => _IsExecuting = false);
        }

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter)) return;
            _IsExecuting = true;
            _InternalCommand.Execute(parameter);
            _timer = new System.Threading.Timer(ChangeIsExecuting);
            _timer.Change(200, Timeout.Infinite);
        }
        
        public void CallCanExecute()
        {
            _InternalCommand.ChangeCanExecute();
        }
    }
}
