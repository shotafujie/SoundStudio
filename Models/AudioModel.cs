using CommunityToolkit.Mvvm.ComponentModel;

namespace SoundStudio.Models
{
    public partial class AudioModel : ObservableObject
    {
        [ObservableProperty]
        private string filePath;
    }
}
