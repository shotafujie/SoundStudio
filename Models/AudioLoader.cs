using NAudio.Wave;
using ScottPlot.Maui;
using ScottPlot.Plottables;
using SoundStudio.ViewModels;

namespace SoundStudio.Models
{
    public class AudioLoader
    {
        public double[] LoadAudioData(string filePath)
        {
            try
            {
                using (var reader = new AudioFileReader(filePath))
                {
                    var samples = new List<float>();
                    float[] buffer = new float[reader.WaveFormat.SampleRate];
                    int samplesRead;
                    while ((samplesRead = reader.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        samples.AddRange(buffer.Take(samplesRead));
                    }

                    return samples.Select(s => (double)s).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading audio file: {ex.Message}");
                return null;
            }
        }

        public void PlotAudioData(string filePath, MauiPlot mauiPlot)
        {
            try
            {
                using (var reader = new AudioFileReader(filePath))
                {
                    double[] audioData = LoadAudioData(filePath);
                    if (audioData != null)
                    {
                        mauiPlot.Plot.Clear();
                        mauiPlot.Plot.XLabel("Time (s)");
                        mauiPlot.Plot.YLabel("Amplitude");
                        mauiPlot.Plot.Axes.SetLimits(0, 60, -1, 1);
                        int sampleRate = reader.WaveFormat.SampleRate;

                        double[] time = Enumerable.Range(0, audioData.Length)
                                                .Select(i => (double)i / sampleRate)
                                                .ToArray();
                       
                        var scatter = mauiPlot.Plot.Add.Scatter(time, audioData, ScottPlot.Color.FromHtml("#0095d9"));
                        mauiPlot.Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error plotting audio data: {ex.Message}");
            }
        }
    }
}
