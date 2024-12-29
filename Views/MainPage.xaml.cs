using ScottPlot.Maui;
using SoundStudio.ViewModels;
using System.Diagnostics;
using System.Windows.Input;

namespace SoundStudio.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            InitializePlot();
            BindingContext = new MainViewModel();
            var viewModel = (MainViewModel)BindingContext;

            if (viewModel == null)
            {
                Debug.WriteLine("BindingContextがMainViewModelに設定されていません");
                return;
            }

            if (inputWaveform == null)
            {
                inputWaveform = new MauiPlot();
            }
            viewModel.MauiPlot = inputWaveform;
        }
        private void InitializePlot()
        {
            inputWaveform.Plot.XLabel("Time (s)");
            inputWaveform.Plot.YLabel("Amplitude");
            inputWaveform.Plot.Axes.SetLimits(0, 60, -1, 1);
            inputWaveform.Refresh();
        }
        private async void OnLoadAudioCommandExecuted(object sender, EventArgs e)
        {
            var viewModel = (MainViewModel)BindingContext;
            if (viewModel.LoadAudioCommand.CanExecute(null))
            {
                await Task.Run(() => viewModel.LoadAudioCommand.Execute(null));
            }
        }
    }
}

