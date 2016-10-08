using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;

namespace RPiSenseHatTelemetry.CloudCommunication
{
    public class IoTHubConnection : IDisposable
    {
        private DeviceClient _deviceClient { get; set; }

        public IoTHubConnection()
        {
            _deviceClient = DeviceClient.CreateFromConnectionString(GetConnectionString(), TransportType.Amqp);
        }

        public async Task SendEventAsync(string payload)
        {
            await _deviceClient.SendEventAsync(new Message(Encoding.ASCII.GetBytes(payload)));
        }

        public async Task<string> ReceiveEventAsync()
        {
            while (true)
            {
                var receivedMessage = await _deviceClient.ReceiveAsync();

                if (receivedMessage != null)
                {
                    var messageData = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                    await _deviceClient.CompleteAsync(receivedMessage);
                    return messageData;
                }
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }


        private string GetConnectionString()
        {
            return "your-connection-string";
        }

        public void Dispose()
        {
            _deviceClient.Dispose();
        }
    }
}
