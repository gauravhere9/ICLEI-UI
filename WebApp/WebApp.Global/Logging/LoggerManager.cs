using NLog;
using NLog.Targets;
using WebApp.Global.Options;

namespace WebApp.Global.Logging
{
    public class LoggerManager : ILoggerManager
    {
        private readonly Logger _logger;
        private readonly Logger _integrationLogger;
        private readonly LogLevel _minLogLevel;

        private readonly string _activityId;
        private int _userId;
        private string _scopeId = string.Empty;
        private readonly string _appName;

        public LoggerManager(DatabaseOptions databaseOptions, ApplicationOptions applicationOptions)
        {
            _activityId = Guid.NewGuid().ToString();

            _logger = LogManager.GetLogger("logger");
            _integrationLogger = LogManager.GetLogger("integrationLogger");

            _appName = applicationOptions.AppName;

            //LogManager.ReconfigExistingLoggers();

            foreach (var targets in LogManager.Configuration.AllTargets.Where(t => t is DatabaseTarget))
            {
                var target = (DatabaseTarget)targets;
                target.ConnectionString = databaseOptions.NLogConnection;
            }


            LogManager.ReconfigExistingLoggers();

            _minLogLevel = LogLevel.FromString(applicationOptions.Logging.LogLevel.Default);

        }

        private void MergeEventProperties(LogEventInfo logEvent)
        {
            foreach (var item in logEvent.Parameters)
            {
                if (item.GetType() == typeof(LogEventInfo))
                {

                    foreach (var propertyItem in ((LogEventInfo)item).Properties)
                    {
                        logEvent.Properties.Remove(propertyItem.Key);
                        logEvent.Properties.Add(propertyItem);
                    }
                }
            }
        }

        private void Write(LogLevel level, string message, params object[] args)
        {
            LogEventInfo logEvent = new LogEventInfo(level, _logger.Name, null, message, args);
            logEvent.Properties["scopeId"] = _scopeId;
            logEvent.Properties["activityId"] = _activityId;
            logEvent.Properties["application"] = _appName;
            logEvent.Properties["userId"] = _userId.ToString();

            _logger.Log(typeof(LoggerManager), logEvent);

        }

        private void WriteWithEx(LogLevel level, string format, Exception ex, params object[] args)
        {
            LogEventInfo logEvent = new LogEventInfo(level, _logger.Name, null, format, args, ex) { Exception = ex };
            logEvent.Properties["scopeId"] = _scopeId;
            logEvent.Properties["activityId"] = _activityId;
            logEvent.Properties["application"] = _appName;
            logEvent.Properties["userId"] = _userId.ToString();

            _logger.Log(typeof(LoggerManager), logEvent);
        }


        public void Fatal(string message)
        {
            if (!_logger.IsFatalEnabled) { return; }

            Write(LogLevel.Fatal, message);
        }

        public void Fatal(string message, Exception exception, params object[] args)
        {
            if (!_logger.IsFatalEnabled) { return; }

            WriteWithEx(LogLevel.Fatal, message, exception);
        }

        public void LogDebug(string message)
        {
            if (!_logger.IsDebugEnabled || LogLevel.Debug < _minLogLevel) { return; }

            Write(LogLevel.Debug, message);
        }

        public void LogDebug(string message, Exception exception, params object[] args)
        {
            if (!_logger.IsDebugEnabled || LogLevel.Debug < _minLogLevel) { return; }

            WriteWithEx(LogLevel.Debug, message, exception);
        }

        public void LogError(string message)
        {
            if (!_logger.IsErrorEnabled) { return; }

            Write(LogLevel.Error, message);
        }

        public void LogError(string message, Exception exception)
        {
            if (!_logger.IsErrorEnabled) { return; }

            WriteWithEx(LogLevel.Error, message, exception);
        }

        public void LogError(string message, Exception exception, params object[] args)
        {
            if (!_logger.IsErrorEnabled) { return; }

            WriteWithEx(LogLevel.Error, message, exception);
        }

        public void LogInformation(string message)
        {
            if (!_logger.IsInfoEnabled) { return; }

            Write(LogLevel.Info, message);
        }

        public void LogInformation(string message, Exception exception, params object[] args)
        {
            if (!_logger.IsInfoEnabled) { return; }

            WriteWithEx(LogLevel.Info, message, exception);
        }

        public void LogInterfaceEntry(string method, string query, string body, string appName)
        {
            LogEventInfo le = new LogEventInfo(LogLevel.Info, _integrationLogger.Name, "");

            le.Properties["scopeId"] = _scopeId;
            le.Properties["activityId"] = _activityId;
            le.Properties["application"] = _appName;
            le.Properties["userId"] = _userId.ToString();

            le.Properties["direction"] = "Inbound";
            le.Properties["method"] = method;
            le.Properties["query"] = query;
            le.Properties["payload"] = body;
            _integrationLogger.Log(typeof(LoggerManager), le);
        }

        public void LogInterfaceExit(string method, string responseCode, string body, string appName)
        {
            LogEventInfo le = new LogEventInfo(LogLevel.Info, _integrationLogger.Name, "");

            le.Properties["scopeId"] = _scopeId;
            le.Properties["activityId"] = _activityId;
            le.Properties["application"] = _appName;
            le.Properties["userId"] = _userId.ToString();

            le.Properties["direction"] = "Outbound";
            le.Properties["method"] = method;
            le.Properties["code"] = responseCode;
            le.Properties["payload"] = body;
            _integrationLogger.Log(typeof(LoggerManager), le);
        }

        public void LogSecurity(string message)
        {
            if (!_logger.IsTraceEnabled) { return; }

            Write(LogLevel.Trace, message);
        }

        public void LogSecurity(string message, Exception exception, params object[] args)
        {
            if (!_logger.IsTraceEnabled) { return; }

            WriteWithEx(LogLevel.Trace, message, exception);
        }

        public void LogWarning(string message)
        {
            if (!_logger.IsWarnEnabled) { return; }

            Write(LogLevel.Warn, message);
        }

        public void LogWarning(string message, Exception exception, params object[] args)
        {
            if (!_logger.IsWarnEnabled) { return; }

            WriteWithEx(LogLevel.Warn, message, exception);
        }

        public void SetCurrentUserId(int userId)
        {
            _userId = userId;
        }

        public void SetScopeId(string scopeId)
        {
            _scopeId = scopeId;
        }
    }
}
