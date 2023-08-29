using CommunityToolkit.Maui.Alerts;
using EnigmatiKreations.Framework.MVVM.BaseViewModels.Contract;
using EnigmatiKreations.Framework.Services.Alerts;
using EnigmatiKreations.Framework.Services.Navigation;
using Microsoft.AppCenter.Crashes;
using Randomizer.Framework.ViewModels.Commanding;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EnigmatiKreations.Framework.MVVM.BaseViewModels
{
    /// <summary>
    /// A base view model for a page
    /// </summary>
    public class BasePageViewModel : BaseViewModel, ILifecycleable
    {
        #region Fields
        bool _IsBusy = false;
        string _Title = string.Empty;

        #endregion

        #region Constructor(s)
        /// <summary>
        /// Initializes a new instance of the <see cref="BasePageViewModel"/> class.
        /// </summary>
        public BasePageViewModel()
        {
            LoadCommand = new GenericCommand<object>(PreLoad);
            UnloadCommand = new GenericCommand<object>(PreUnLoad);
            LoadCommandAsync = new GenericCommandAsync<object>(PreLoadAsync);
            UnloadCommandAsync = new GenericCommandAsync<object>(PreUnLoadAsync);
        }

        private async Task PreUnLoadAsync(object arg)
        {
            await Task.Run(() => PreUnLoad(arg));
        }

        private async Task PreLoadAsync(object arg)
        {
            await Task.Run(() => PreLoad(arg));
        }

        /// <summary>
        /// Method called when this view model has been navigated to
        /// </summary>
        /// <param name="sender">The page that called this method</param>
        /// <param name="e">The arguments to treat the navigation</param>
        public virtual void Navigated(object sender, object e)
        {
        }

        /// <summary>
        /// Method called when this view model is navigating somewhere
        /// </summary>
        /// <param name="sender">The page that called this methods</param>
        /// <param name="args">The arguments to treat the navigation</param>
        /// <remarks>
        /// Usually used to cancel the navigation of some conditions aren't met
        /// </remarks>
        public virtual void Navigating(object sender, object args)
        {

        }



        #endregion

        #region Properties



        private bool _HasLoaded;

        public DateTime LastLoadDate { get; private set; }
        public bool HasLoaded
        {
            get => _HasLoaded;
            set => SetProperty(ref _HasLoaded, value);
        }



        /// <summary>
        /// The title of this page 
        /// </summary>
        public string Title
        {
            get => _Title;
            set => SetProperty(ref _Title, value);
        }

        /// <summary>
        /// Property indicating if the ViewModel is busy loading.
        /// If it's true, this should show a visual feedback to the user.
        /// </summary>
        public bool IsBusy
        {
            get => _IsBusy;
            set => SetProperty(ref _IsBusy, value);
        }



        private bool _IsRefreshing;

        public bool IsRefreshing
        {
            get => _IsRefreshing;
            set => SetProperty(ref _IsRefreshing, value);
        }



        public bool IsDestroyed { get; private set; }


        private bool _NeedsToRefresh;

        public bool NeedsToRefresh
        {
            get => _NeedsToRefresh;
            set => SetProperty(ref _NeedsToRefresh, value);
        }


        public bool IsCreateFromContextMode => PageMode == FormPageMode.CreateWithContext;

        public bool IsCreateOrCreateFromContext => PageMode == FormPageMode.Create || PageMode == FormPageMode.CreateWithContext;

        public bool IsCreateMode => PageMode == FormPageMode.Create;

        public bool IsCreateOrEditMode => PageMode == FormPageMode.Create || PageMode == FormPageMode.Edit;

        public bool IsEditMode => PageMode == FormPageMode.Edit;

        public bool IsDisplayMode => PageMode == FormPageMode.Display;
        public bool IsDisplayOrEditMode => PageMode == FormPageMode.Display || PageMode == FormPageMode.Edit;

        private FormPageMode _pageMode;
        public FormPageMode PageMode
        {
            get => _pageMode;
            set
            {

                if (!SetProperty(ref _pageMode, value)) return;

                RaisePropertyChanged(nameof(IsCreateMode));
                RaisePropertyChanged(nameof(IsEditMode));
                RaisePropertyChanged(nameof(IsDisplayMode));
                RaisePropertyChanged(nameof(IsCreateOrEditMode));
                RaisePropertyChanged(nameof(IsCreateOrCreateFromContext));
                RaisePropertyChanged(nameof(IsCreateFromContextMode));
                RaisePropertyChanged(nameof(IsDisplayOrEditMode));
            }
        }


        #endregion

        #region Commands
        /// <summary>
        /// Command executed when this view model needs to be loaded
        /// </summary>
        public ICommand LoadCommand
        {
            get => _loadCommand;
            set => SetProperty(ref _loadCommand, value);
        }


        private IGenericCommandAsync<object> _LoadCommandAsync;

        /// <summary>
        /// Command executed async when this view model needs to be loaded
        /// </summary>
        public IGenericCommandAsync<object> LoadCommandAsync
        {
            get => _LoadCommandAsync;
            set => SetProperty(ref _LoadCommandAsync, value);
        }



        private IGenericCommandAsync<object> _unloadCommandAsync;

        /// <summary>
        /// Command executed async when this view model needs to be unloaded
        /// </summary>
        public IGenericCommandAsync<object> UnloadCommandAsync
        {
            get => _unloadCommandAsync;
            set => SetProperty(ref _unloadCommandAsync, value);
        }



        private ICommand _unloadCommand;
        private ICommand _loadCommand;

        /// <summary>
        /// Command executed when this view model needs to be unloaded
        /// </summary>
        public ICommand UnloadCommand
        {
            get => _unloadCommand;
            set => SetProperty(ref _unloadCommand, value);
        }

        public ICommand SaveCommand { get; set; }

        public ICommand CancelConfirmCommand { get; set; }

        public ICommand CancelCommand { get; set; }

        public ICommand EditCommand { get; set; }

        public ICommand DeleteCommand { get; set; }

        private ICommandAsync _GoBack;
        public ICommandAsync GoBack
        {
            get => _GoBack;
            set => SetProperty(ref _GoBack, value);
        }
        #endregion

        #region Lifecycle Methods
        private void PreLoad(object parameter)
        {
            if (HasLoaded)
            {
                ReLoad(parameter);
            }
            else
            {
                Load(parameter);
                LastLoadDate = DateTime.UtcNow;
                HasLoaded = true;
            }
        }

        /// <summary>
        /// Method called when the view model needs to be loaded (called only once)
        /// </summary>
        public virtual void Load(object parameter = null)
        {
        }

        /// <summary>
        /// Method called when the view model needs to be reloaded (called if the <see cref="LoadCommand"/> is executed twice or more)
        /// </summary>
        public virtual void ReLoad(object parameter = null)
        {

        }


        /// <summary>
        /// Method called when the viewmodel needs to be unloaded
        /// </summary>
        private void PreUnLoad(object parameter)
        {
            UnLoad(parameter);
        }


        /// <summary>
        /// Method called when the viewmodel needs to be unloaded
        /// It can be overriden
        /// </summary>
        public virtual void UnLoad(object parameter)
        {

        }

        void ILifecycleable.Destroy()
        {
            PreDestroy();
        }

        internal void PreDestroy()
        {
            IsDestroyed = true;
            Destroy();
        }

        /// <summary>
        /// Called when its not possible to come to this viewmodel anymore
        /// Override to liberate mecessay resssources
        /// </summary>
        public virtual void Destroy()
        { }

        public virtual async void OnAsyncCommandException(Exception exception)
        {
            await Application.Current.MainPage.DisplaySnackbar($"Une erreur est survenue. Vérifiez votre connection internet et veuillez réssayer votre action dans quelques secondes.");
            if (exception == null) return;
            Crashes.TrackError(exception);
        }
        #endregion
    }

}

