using System;
using EnigmatiKreations.Framework.MVVM.BaseViewModels;

namespace ShellCrashRepro.ViewModels
{
	public class AppShellVM : BasePageViewModel
	{

        private MainPageVM _MainPageViewModel;
        public MainPageVM MainPageViewModel
        {
            get => _MainPageViewModel;
            set => SetProperty(ref _MainPageViewModel, value);
        }


        private PageOneVM _PageOneViewModel;
        public PageOneVM PageOneViewModel
        {
            get => _PageOneViewModel;
            set => SetProperty(ref _PageOneViewModel, value);
        }

        private PageTwoVM _PageTwoViewModel;
        public PageTwoVM PageTwoViewModel
        {
            get => _PageTwoViewModel;
            set => SetProperty(ref _PageTwoViewModel, value);
        }

        public AppShellVM()
		{
		}

        public override async void Load(object parameter = null)
        {
            base.Load(parameter);

            MainPageViewModel = new MainPageVM();
            PageOneViewModel = new PageOneVM();
            PageTwoViewModel = new PageTwoVM();

            MainPageViewModel.IsBusy = true;

            // simulating some long process load
            // in MainPageViewModel before navigating
            await Task.Delay(5000);


            MainPageViewModel.IsBusy = false;


            await Shell.Current.GoToAsync("//PageOne");
        }
    }
}

