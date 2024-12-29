using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SoundStudio.Models;

namespace SoundStudio.ViewModels
{
    public partial class AudioViewModel : ObservableObject
    {
        [ObservableProperty]
        private AudioModel audioModel;

        [ObservableProperty]
        private string errorMessage;

        public bool IsErrorVisible => !string.IsNullOrEmpty(ErrorMessage);

        [RelayCommand]
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
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        AudioModel = new AudioModel { FilePath = result.FullPath };
                        ErrorMessage = null;
                    });
                }
                else
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        AudioModel = new AudioModel { FilePath = "ファイル選択がキャンセルされました" };
                    });
                }
            }
            catch (Exception ex)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    ErrorMessage = $"エラーが発生しました: {ex.Message}";
                    AudioModel = null;
                });
            }
        }
    }
}