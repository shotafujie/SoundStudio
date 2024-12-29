using Microsoft.Extensions.DependencyInjection;
using SoundStudio.ViewModels;
using SoundStudio.Views;

namespace SoundStudio
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var builder = MauiApp.CreateBuilder();
            builder.Services.AddTransient<MainViewModel>();
            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddSingleton<MainPage>();

            MainPage = builder.Build().Services.GetService<AppShell>();
        }
    }
}