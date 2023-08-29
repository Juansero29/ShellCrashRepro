using ShellCrashRepro.ViewModels;

namespace ShellCrashRepro;

public partial class App : Application
{
	public App(AppShellVM appShellVM)
	{
		InitializeComponent();

		MainPage = new AppShell(appShellVM);
	}
}

