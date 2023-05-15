namespace WebApp.Global.Logging
{
    public interface ILoggerManager
    {
        void SetScopeId(string scopeId);
        void SetCurrentUserId(int userId);
        void LogInterfaceEntry(string method, string query, string body, string appName);
        void LogInterfaceExit(string method, string responseCode, string body, string appName);
        void LogDebug(string message);
        void LogDebug(string message, Exception exception, params object[] args);
        void LogSecurity(string message);
        void LogSecurity(string message, Exception exception, params object[] args);

        void Fatal(string message);
        void Fatal(string message, Exception exception, params object[] args);


        void LogInformation(string message);
        void LogInformation(string message, Exception exception, params object[] args);

        void LogWarning(string message);
        void LogWarning(string message, Exception exception, params object[] args);

        void LogError(string message);
        void LogError(string message, Exception exception);
        void LogError(string message, Exception exception, params object[] args);
    }
}
