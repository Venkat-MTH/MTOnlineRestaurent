using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using NLog;
using System;
using System.Diagnostics.CodeAnalysis;

namespace LoggingManagement
{
    [ExcludeFromCodeCoverage]
    public class LoggerService : ILogService
    {
        private static string InstrumentationKey { get; set; }
        private static bool LoggingParameters
        {
            get
            {
                return !string.IsNullOrEmpty(InstrumentationKey);
            }
        }

        private static TelemetryClient _LoggerObject = null;

        public static void InitializeAppInsights(string instrumentationKey)
        {
            InstrumentationKey = instrumentationKey;
        }
        
        private static TelemetryClient LoggerObject
        {
            get
            {
                if (LoggingParameters && _LoggerObject == null &&
                    !string.IsNullOrEmpty(InstrumentationKey))
                {
                    //TelemetryConfiguration config = new TelemetryConfiguration { InstrumentationKey = InstrumentationKey };
                    _LoggerObject = new TelemetryClient();
                    _LoggerObject.Context.InstrumentationKey = InstrumentationKey;
                }
                return _LoggerObject;
            }
        }

        /// <summary>
        /// Log custom log messages
        /// </summary>
        /// <param name="exception"></param>
        public void LogMessage(string logmessage)
        {

            TelemetryConfiguration configuration = TelemetryConfiguration.CreateDefault();
            configuration.InstrumentationKey = "cb9d7c69-7cb6-4f15-a29d-123ff4e3250f";
            var telemetryClient = new TelemetryClient(configuration);
            telemetryClient.TrackTrace(logmessage);
        }

        /// <summary>
        /// Log exception events
        /// </summary>
        /// <param name="exception"></param>
        public void LogException(Exception exception)
        {
            if (!LoggingParameters)
            {
                var logger = LogManager.GetLogger("ExceptionEvent");
                logger.Log(LogLevel.Fatal, exception);
            }
            else
            {
                LoggerObject.TrackException(exception);
                LoggerObject.Flush();
            }
        }         
    }
}
