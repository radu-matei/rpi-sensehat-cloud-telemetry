using System;
using System.Threading;
using System.Threading.Tasks;
using Emmellsoft.IoT.Rpi.SenseHat;
using RPiSenseHatTelemetry.Common;

namespace RPiSenseHatTelemetry.SenseHatCommunication
{
    public class SenseHat : IDisposable
    {
        private ISenseHat _senseHat { get; set; }

        public async Task Activate()
        {
            _senseHat = await SenseHatFactory.GetSenseHat().ConfigureAwait(false);

            _senseHat.Display.Clear();
            _senseHat.Display.Update();
        }

        public TemperatureTelemetry GetTemperature()
        {
            while (true)
            {
                _senseHat.Sensors.HumiditySensor.Update();

                if (_senseHat.Sensors.Temperature.HasValue)
                {
                    return new TemperatureTelemetry()
                    {
                        Time = DateTime.Now,
                        Temperature = _senseHat.Sensors.Temperature.Value
                    };
                }

                else new ManualResetEventSlim(false).Wait(TimeSpan.FromSeconds(0.5));
            }
        }

        public void Dispose()
        {
            _senseHat.Dispose();
        }

    }
}
