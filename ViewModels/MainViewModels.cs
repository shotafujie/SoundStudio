using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ScottPlot.Maui;
using SoundStudio.Models;
using System.Windows.Input;

namespace SoundStudio.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly AudioLoader _audioLoader;

        private double[] audioData;
        public double[] AudioData
        {
            get => audioData;
            set => SetProperty(ref audioData, value);
        }

        private string errorMessage;
        public string ErrorMessage
        {
            get => errorMessage;
            set => SetProperty(ref errorMessage, value);
        }


        private string inputFilePath;
        public string InputFilePath
        {
            get => inputFilePath;
            set => SetProperty(ref inputFilePath, value);
        }   

        public bool IsErrorVisible => !string.IsNullOrEmpty(ErrorMessage);

        public MauiPlot MauiPlot { get; set; }

        public ICommand LoadAudioCommand { get; }

        public MainViewModel()
        {
            _audioLoader = new AudioLoader();
            LoadAudioCommand = new AsyncRelayCommand(LoadAudio);
        }

        private async Task LoadAudio()
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                        { DevicePlatform.WinUI, new[] { ".mp3", ".wav" } },
                        { DevicePlatform.Android, new[] { "audio/mpeg", "audio/x-wav" } },
                        { DevicePlatform.iOS, new[] { "public.mp3", "public.wav" } },
                        { DevicePlatform.MacCatalyst, new[] { "public.mp3", "public.wav" } }
                    })
                });

                if (result != null)
                {
                    var audioData = _audioLoader.LoadAudioData(result.FullPath);

                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        if (audioData != null)
                        {
                            AudioData = audioData;
                            InputFilePath = result.FullPath;
                            ErrorMessage = null;
                            _audioLoader.PlotAudioData(result.FullPath, MauiPlot);
                            
                        }
                        else
                        {
                            ErrorMessage = "音声ファイルの読み込みに失敗しました。";
                            InputFilePath = null;
                        }
                    });
                }
                else
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        ErrorMessage = "ファイル選択がキャンセルされました。";
                        InputFilePath = null;
                    });
                }
            }
            catch (Exception ex)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    ErrorMessage = $"エラーが発生しました: {ex.Message}";
                    InputFilePath = null;
                });
            }
        }
    }
}