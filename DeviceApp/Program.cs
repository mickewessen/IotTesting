using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace DeviceApp
{
    class Program
    {
        private static DeviceClient deviceClient = DeviceClient.CreateFromConnectionString("HostName=ec-win-iothub.azure-devices.net;DeviceId=ConsoleApp;SharedAccessKey=3zsfBvJy2yTIaBEoTQpnnbD1jX4Z1Juz3g8jhLSjX8M=", TransportType.Mqtt);
        private static int telemetryInterval = 5;
        private static Random rnd = new Random();

        static void Main(string[] args)
        {
            deviceClient.SetMethodHandlerAsync("SetTelemetryInterval", SetTelemetryInterval, null).Wait();
            SendMessageAsync().GetAwaiter();

            Console.ReadKey();
        }

        private static Task<MethodResponse> SetTelemetryInterval(MethodRequest request, object userContext)
        {
            var payload = Encoding.UTF8.GetString(request.Data).Replace("\"","");


            if (Int32.TryParse(payload, out telemetryInterval))
            {
                Console.WriteLine($"Interval set to: {telemetryInterval} seconds.");

                string json = "{\"result\": \"Execute direct method: " + request.Name + "\"}";
                return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(json), 200));
            }
            else
            {
                string json = "{\"result\": \"Method not implemented\"}";
                return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(json), 501));
            }
        }

        private static async Task SendMessageAsync()
        {
            while (true)
            {
                double temp = 10 + rnd.NextDouble() * 15;
                double hum = 40 + rnd.NextDouble() * 20;

                var data = new
                {
                    temperature = temp,
                    humidty = hum
                };

                var json = JsonConvert.SerializeObject(data);
                var payload = new Message(Encoding.UTF8.GetBytes(json));
                payload.Properties.Add("temperatureAlert", (temp > 30) ? "true" : "false");

                await deviceClient.SendEventAsync(payload);
                Console.WriteLine($"Message sent {json}");

                await Task.Delay(telemetryInterval * 1000);
            }

        }
    }
}
