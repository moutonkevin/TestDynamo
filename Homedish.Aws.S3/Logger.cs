namespace Homedish.Logging
{
    public class Logger
    {
        public readonly string CallingClassFullName;
        private readonly NLog.Logger _logger;

        public Logger(string callingClassFullName)
        {
            CallingClassFullName = callingClassFullName;
            _logger = NLog.LogManager.GetLogger(CallingClassFullName);
        }

        public void Info(string message)
        {
            _logger.Info(message);
        }

        public void Info<TArgument>(string message, TArgument argument)
        {
            _logger.Info(message, argument);
        }

        public void Warn(string message)
        {
            _logger.Warn(message);
        }

        public void Warn<TArgument>(string message, TArgument argument)
        {
            _logger.Warn(message, argument);
        }

        public void Error(string message)
        {
            _logger.Error(message);
        }

        public void Error<TArgument>(string message, TArgument argument)
        {
            _logger.Error(message, argument);
        }
    }
}
