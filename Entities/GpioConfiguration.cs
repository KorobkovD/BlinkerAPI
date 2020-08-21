namespace BlinkerAPI.Entities
{
    /// <summary>
    /// Конфигурация GPIO для приложения
    /// </summary>
    public static class GpioConfiguration
    {
        /// <summary>
        /// Пин для светодиода
        /// </summary>
        public static int LedPin = 6;

        /// <summary>
        /// Время свечения светодиода в мс
        /// </summary>
        public static int LightTimeInMilliseconds = 200;

        /// <summary>
        /// Время выключенного светодиода в мс
        /// </summary>
        public static int DimTimeInMilliseconds = 1000;
    }
}
