using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace EnigmatiKreations.Framework.MVVM.BaseViewModels.Contract
{
    public interface ILifecycleable
    {
        /// <summary>
        /// Executed when it's unloaded
        /// </summary>
        ICommand UnloadCommand { get; }

        /// <summary>
        /// Executed when its loaded
        /// </summary>
        ICommand LoadCommand { get; }

        void Destroy();
    }
}
