using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui.Markup;
using CommunityToolkit.Maui;
using ShellCrashRepro.ViewModels;

namespace ShellCrashRepro;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkitMarkup()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddTransient<AppShell>();
		builder.Services.AddTransient<AppShellVM>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}

