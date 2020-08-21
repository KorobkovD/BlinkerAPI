using BlinkerAPI.Entities;
using BlinkerAPI.Enums;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Device.Gpio;
using System.Threading;
using System.Threading.Tasks;

namespace BlinkerAPI.Controllers
{
    [Route("led")]
    [ApiController]
    public class LedController : ControllerBase
    {
        private const int lightTimeInMilliseconds = 1000;
        private const int dimTimeInMilliseconds = 200;
        private readonly GpioController _gpioController;
        private readonly int LedPin = GpioConfiguration.LedPin;

        private static CancellationTokenSource CancellationSource;

        private static Task BlinkerTask;

        public LedController()
        {
            _gpioController = new GpioController();
            _gpioController.OpenPin(LedPin, PinMode.Output);

            if (CancellationSource?.Token == null)
            {
                CancellationSource = new CancellationTokenSource();
            }
        }

        /// <summary>
        /// Метод мигания светодиодом
        /// </summary>
        /// <param name="state">Состояние мигания</param>
        [Route("blink")]
        public void LedBlinking(
            [Required(ErrorMessage = "State is not specified")]
            [Range(0, 1, ErrorMessage = "Unknown state")]
            BlinkState state)
        {
            switch (state)
            {
                case BlinkState.OFF:
                    if (BlinkerTask != null && BlinkerTask.Status == TaskStatus.Running)
                    {
                        CancellationSource.Cancel();
                    }
                    break;

                case BlinkState.BLINK:
                    if (BlinkerTask == null || BlinkerTask.Status != TaskStatus.Running)
                    {
                        //BlinkerTask.Dispose();
                        BlinkerTask = new Task(() => BlinkerLoop());

                        CancellationSource = new CancellationTokenSource();
                        BlinkerTask.Start();
                    }
                    break;
            }
        }

        /// <summary>
        /// Мигание светодиодом в бесконечном цикле
        /// </summary>
        private void BlinkerLoop()
        {
            while (true)
            {
                if (CancellationSource.IsCancellationRequested)
                {
                    _gpioController.Write(LedPin, PinValue.Low);
                    return;
                }

                _gpioController.Write(LedPin, PinValue.Low);
                Thread.Sleep(lightTimeInMilliseconds);

                _gpioController.Write(LedPin, PinValue.High);
                Thread.Sleep(dimTimeInMilliseconds);
            }
        }
    }
}
