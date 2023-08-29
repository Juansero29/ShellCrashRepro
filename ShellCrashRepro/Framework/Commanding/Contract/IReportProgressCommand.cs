using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Randomizer.Framework.ViewModels.Commanding.Contract
{
    /// <summary>
    /// A command that uses a method to report its progress
    /// </summary>
    interface IReportProgressCommand : ICommand
    {
        void ReportProgress(Action action);
    }
}
