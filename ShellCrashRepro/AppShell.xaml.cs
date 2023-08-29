using ShellCrashRepro.ViewModels;

namespace ShellCrashRepro;

public partial class AppShell : Shell
{
	public AppShell(AppShellVM viewModel)
	{
		InitializeComponent();

        BindingContext = viewModel;

        Routing.RegisterRoute(nameof(PageOne), typeof(PageOne));
        Routing.RegisterRoute(nameof(PageTwo), typeof(PageTwo));
        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
    }
}

