using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace EnigmatiKreations.Framework.MVVM.BaseViewModels
{

    /// <summary>
    /// Base class for view models, encapsulating the model
    /// </summary>
    public class BaseViewModel<T> : BaseViewModel
    {

        #region Fields
        protected T _Model;
        #endregion

        #region Constructor(s)
        public BaseViewModel(T model)
        {
            Model = model;
        }

        #endregion

        #region Properties
        /// <summary>
        /// The model of this ViewModel
        /// </summary>
        public T Model
        {
            get => _Model;
            set => SetProperty(ref _Model, value);
        }


        #endregion

        #region Methods
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj == this) return true;
            if (!(obj is BaseViewModel<T> other)) return false;
            var isEqual = Model.Equals(other.Model);
            return isEqual;
        }

        public override int GetHashCode()
        {
            return Model.GetHashCode();
        }
        #endregion
    }


    /// <summary>
    /// Class serving as the base for any ViewModel
    /// </summary>
    public class BaseViewModel : BindableObject, INotifyPropertyChanged
    {
     
        #region SetValue
        /// <summary>
        /// Sets the backing store field to the value and raises <see cref="RaisePropertyChanged(string)"/>
        /// if the field has a different value than the property corresponding to 'propertyName'
        /// </summary>
        /// <typeparam name="T">The type of the property to set</typeparam>
        /// <param name="backingStore">The field that contains the value</param>
        /// <param name="value">The new value to set</param>
        /// <param name="propertyName">The name of the property to be set (by default we use the <see cref="CallerMemberNameAttribute"/></param>
        /// <param name="onChanged">An action to invoke whenever the property changes</param>
        /// <returns></returns>
        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            RaisePropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Sets the value to the model's property of the name only if it is different from the value already stored in the model.
        /// If the field has a different value than the property corresponding to 'propertyName' we raise <see cref="RaisePropertyChanged(string)"/> 
        /// </summary>
        /// <typeparam name="T">The type of the property to set</typeparam>
        /// <typeparam name="M">The type of the model we want to set the new value if it is different</typeparam>
        /// <param name="newValue">The new value</param>
        /// <param name="model">The model we want to set</param>
        /// <param name="propertyName">The name of the property to be set (by default we use the <see cref="CallerMemberNameAttribute"/></param>
        /// <param name="onChanged">An action to invoke whenever the property changes</param>
        /// <returns></returns>
        public bool SetValueOnModel<T, M>(T newValue, M model, [CallerMemberName]string propertyName = "", Action onChanged = null)
        {
            var res = false;

            try
            {
                var oldValue = (T)model.GetType().GetProperty(propertyName).GetValue(model);
                if (EqualityComparer<T>.Default.Equals(newValue, oldValue))
                    return false;
                model.GetType().GetProperty(propertyName).SetValue(model, newValue);
                RaisePropertyChanged(propertyName);
                onChanged?.Invoke();
                res = true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"SetValueOnModel Failed for proprety {propertyName} on model {model}" + e.StackTrace);
                res = false;
            }

            return res;
        }

        #endregion

        #region INotifyPropertyChanged
        public new event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
