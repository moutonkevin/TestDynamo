using System.Diagnostics;

namespace Homedish.Logging
{
    public class LogManager
    {
        public static Logger GetCurrentClassLogger()
        {
            var callingClassName = new StackFrame(1).GetMethod().DeclaringType.FullName;

            return new Logger(callingClassName);
        }
    }
}
