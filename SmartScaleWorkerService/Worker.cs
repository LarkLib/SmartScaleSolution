using MQTTnet.Client;
using MQTTnet.Protocol;
using MQTTnet;
using System.Collections.Concurrent;
using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;

namespace SmartScaleWorkerService
{
    public partial class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration Configuration;
        private readonly SmartScaleApiClient ApiClient;
        private readonly ConcurrentDictionary<string, Action<string>> ActipnDictonary;
        public Worker(HostBuilderContext hostContext, ILogger<Worker> logger)
        {
            Directory.SetCurrentDirectory(AppContext.BaseDirectory);
            Configuration = hostContext.Configuration;
            _logger = logger;
            var httpClient = new HttpClient();
            var apiUrl = Configuration.GetValue<string>("ApiUrl", "http://192.168.1.220:8080/");
            var imagePath = Configuration.GetValue<string>("ImagePath", "FaceImages");
            ApiClient = new SmartScaleApiClient(apiUrl, httpClient);
#pragma warning disable CS8604 // Possible null reference argument.
            ActipnDictonary = new ConcurrentDictionary<string, Action<string>>
            {
                ["SmartScale/FaceEigen"] = (message) => { ApiClient.FaceEigen2Async(JsonConvert.DeserializeObject<FaceEigen>(message)); },
                ["SmartScale/Telemetry"] = (message) => { ApiClient.ScaleItems2Async(JsonConvert.DeserializeObject<SmartScale>(message)); },
                ["SmartScale/FaceImage"] = (message) => { SaveFaceImage(imagePath, JsonConvert.DeserializeObject<FaceImage>(message)); },
                ["SmartScale/FaceFeture"] = (message) => { },
            };
#pragma warning restore CS8604 // Possible null reference argument.
        }


        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("StartAsync running at: {time}", DateTimeOffset.Now);
            return base.StartAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await MqttSubscriptionTopic();
            }
            catch (Exception e)
            {
                _logger.LogError("Worker Error at: {time}", DateTimeOffset.Now);
                _logger.LogError(e.Message);
            }
        }

        private async Task MqttSubscriptionTopic()
        {
            // Create TCP based options using the builder.
            var mqttClientInfo = new MqttClient(Configuration, _logger);
            var client = new MqttFactory().CreateMqttClient();
            var clientOptions = new MqttClientOptionsBuilder()
                .WithTcpServer(mqttClientInfo.Server, mqttClientInfo.Port)
                .WithCredentials(mqttClientInfo.UserName, mqttClientInfo.Password)
                .WithCleanSession(mqttClientInfo.CleanSession)
                .WithClientId(mqttClientInfo.ClientId)
                .Build();
            var result = await client.ConnectAsync(clientOptions);

            client.ApplicationMessageReceivedAsync += e =>
            {
                var topic = e.ApplicationMessage.Topic;
                var message = e.ApplicationMessage.ConvertPayloadToString();
                _logger.LogInformation("Received application message at: {time}", DateTimeOffset.Now);
                _logger.LogInformation($"{Environment.NewLine}Topic:{topic}{Environment.NewLine}Message:{Environment.NewLine}{message}");
                if (ActipnDictonary.ContainsKey(topic)) ActipnDictonary[topic](message);
                return Task.CompletedTask;
            };

            await client.SubscribeAsync("SmartScale/+", MqttQualityOfServiceLevel.AtLeastOnce);
        }

        private void SaveFaceImage(string imagePath, FaceImage faceImage)
        {
            var path = imagePath.IndexOf(':') > 0 ? imagePath : Path.Combine(AppContext.BaseDirectory, imagePath);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var fileName = Path.Combine(path, $"{faceImage.FaceNo}-{DateTime.Now:yyyyMMddHHmmssfff}.{faceImage.ImageType}");
            _logger.LogInformation("Save Image {fileName} at: {time}", fileName, DateTimeOffset.Now);
            File.WriteAllBytes(fileName, Convert.FromBase64String(faceImage.Image));
        }
    }
}