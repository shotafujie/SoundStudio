using Xunit;
using SoundStudio.Models;

namespace SoundStudio.Tests
{
    public class AudioLoaderTests
    {
        [Fact]
        public void LoadAudioData_ValidFilePath_ReturnsAudioData()
        {
            string validFilePath = Path.Combine(AppContext.BaseDirectory, "TestData", "valid_sine.wav");
            Assert.True(File.Exists(validFilePath), $"File not found:{validFilePath}");

            var audioLoader = new AudioLoader();
            var audioData = audioLoader.LoadAudioData(validFilePath);

            Assert.NotNull(audioData);
        }
        

        [Fact]
        public void LoadAudioData_InvalidFilePath_ReturnsNull()
        {
            string invalidFilePath = Path.Combine(AppContext.BaseDirectory, "TestData", "invalid_audio.wav");
            Assert.False(File.Exists(invalidFilePath));

            var audioLoader = new AudioLoader();            
            var audioData = audioLoader.LoadAudioData(invalidFilePath);

            Assert.Null(audioData);
        }
    }
}