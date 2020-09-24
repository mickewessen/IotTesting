using DeviceApp.Services;
using Microsoft.Azure.Devices.Client;
using System;
using Xunit;

namespace DeviceApp.Tests
{
    public class SetTelemetryIntervallTests
    {

        private DeviceClient deviceClient = DeviceClient.CreateFromConnectionString("HostName=ec-win-iothub.azure-devices.net;DeviceId=ConsoleApp;SharedAccessKey=3zsfBvJy2yTIaBEoTQpnnbD1jX4Z1Juz3g8jhLSjX8M=", TransportType.Mqtt);


        [Theory]
        [InlineData("SetTelemetryInterval", "10", 200)]
        [InlineData("SetInterval", "10", 501)]
        public void SetTelemetryInterval_ShouldChangeTheIntervall(string methodName, string payload, int statusCode)
        {
            deviceClient.SetMethodHandlerAsync("methodName", DeviceService.SetTelemetryInterval, null).Wait();
        }
    }
}
  