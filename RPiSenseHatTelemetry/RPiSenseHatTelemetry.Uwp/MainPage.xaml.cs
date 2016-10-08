using System;
using RPiSenseHatTelemetry.SenseHatCommunication;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using RPiSenseHatTelemetry.CloudCommunication;
using Newtonsoft.Json;

namespace RPiSenseHatTelemetry.Uwp
{

    public sealed partial class MainPage : Page
    {
        private SenseHat _senseHat { get; set; }
        private IoTHubConnection _iotHubConnection { get; set; }

        public MainPage()
        {
            this.InitializeComponent();

            _senseHat = new SenseHat();
            _iotHubConnection = new IoTHubConnection();

            this.ActivateSenseHat();

            this.Loaded += (sender, e) =>
            {
                DispatcherTimer timer = new DispatcherTimer();
                timer.Tick += async (x, y) => await _iotHubConnection.SendEventAsync(JsonConvert.SerializeObject(_senseHat.GetTemperature()));
                timer.Interval = TimeSpan.FromSeconds(3);
                timer.Start();
            };
        }

        private async void ActivateSenseHat()
        {
           await _senseHat.Activate();
        }
    }
}
