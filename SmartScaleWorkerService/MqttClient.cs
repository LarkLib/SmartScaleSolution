using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartScaleWorkerService
{
    internal class MqttClient
    {
        public string Server { get; private set; }
        public int Port { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }
        public bool CleanSession { get; private set; }
        public string ClientId { get; private set; }
        public MqttClient(IConfiguration confiuration, ILogger<Worker> logger)
        {
            logger.LogInformation("MqttClient starting at: {time}", DateTime.Now);
            Server = confiuration.GetValue<string>("MqttClient:Server") ?? "192.168.1.220";
            Port = confiuration.GetValue<int>("MqttClient:Port1", 1883);
            UserName = confiuration.GetValue<string>("MqttClient:UserName") ?? "djadmin";
            Password = confiuration.GetValue<string>("MqttClient:Password") ?? "dj123456";
            CleanSession = confiuration.GetValue<bool>("MqttClient:CleanSession", false);
            ClientId = confiuration.GetValue<string>("MqttClient:ClientId") ?? "5f4cbadd7b1aff7a0ba5a303cce1eddf";
            logger.LogInformation("MqttClient end at: {time}", DateTime.Now);
        }
    }
}
