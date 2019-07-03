using System;

namespace PostcardApp.Infrastructure.Logging
{
    /// <summary>
    /// Common contract for trace instrumentation. You 
    /// can implement this contrat with several frameworks.
    /// .NET Diagnostics API, EntLib, Log4Net, NLog etc.
    /// <remarks>
    /// The use of this abstraction depends on the real needs you have and the specific features  
    /// you want to use of a particular existing implementation. 
    /// </remarks>
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Log message information 
        /// </summary>
        /// <param name="message">The information message to write</param>
        /// <param name="args">The arguments values</param>
        void Info(string message, params object[] args);

        /// <summary>
        /// Log warning message
        /// </summary>
        /// <param name="message">The warning message to write</param>
        /// <param name="args">The argument values</param>
        void Warn(string message, params object[] args);

        /// <summary>
        /// Log error message
        /// </summary>
        /// <param name="message">The error message to write</param>
        /// <param name="args">The arguments values</param>
        void Error(string message, params object[] args);

        /// <summary>
        /// Log error message
        /// </summary>
        /// <param name="exception">The exception associated with this error</param>
        /// <param name="args">The arguments values</param>
        void Error(Exception exception, params object[] args);
    }
}
