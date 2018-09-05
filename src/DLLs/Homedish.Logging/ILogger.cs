namespace Homedish.Logging
{
    public interface ILogger
    {
        void Info(string message);
        void Info<TArgument>(string message, TArgument argument);
        void Info(string message, params object[] args);
        void Warn(string message);
        void Warn<TArgument>(string message, TArgument argument);
        void Warn(string message, params object[] args);
        void Error(string message);
        void Error<TArgument>(string message, TArgument argument);
        void Error(string message, params object[] args);
    }
}