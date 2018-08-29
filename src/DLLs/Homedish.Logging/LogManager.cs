namespace Homedish.Logging
{
    public class LogManager
    {
        public static ILogger GetLogger()
        {
            return new Logger();
        }
    }
}
