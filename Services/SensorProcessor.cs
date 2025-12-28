
using System.Threading.Channels;

namespace SensorApi.Services
{
    public class SensorProcessor : BackgroundService
    {
        private readonly Channel<SensorReading> _channel;
        private readonly IHttpClientFactory _httpClientFactory;

        public SensorProcessor(Channel<SensorReading> channel, IHttpClientFactory httpClientFactory)
        {
            _channel = channel;
            _httpClientFactory = httpClientFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await foreach (var item in _channel.Reader.ReadAllAsync(stoppingToken))
                {
                    try
                    {
                        var client = _httpClientFactory.CreateClient();

                        // 1. Send data to Python Microservice
                        var response = await client.PostAsJsonAsync("http://127.0.0.1:8000/analyze", item, stoppingToken);

                        if (response.IsSuccessStatusCode)
                        {
                            var result = await response.Content.ReadFromJsonAsync<AnomalyResult>(cancellationToken: stoppingToken);
                            if (result is not null && result.Anomaly)
                            {
                                Console.WriteLine($"⚠️ ANOMALY DETECTED for {item.MachineId}: Temperature {item.Temperature}");
                            }
                            else
                            {
                                Console.WriteLine($"✅ Standard reading for {item.MachineId}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing sensor data: {ex.Message}");
                    }
                }
            }
        }
    }
}
