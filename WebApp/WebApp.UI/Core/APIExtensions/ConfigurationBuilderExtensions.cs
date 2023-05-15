namespace WebApp.UI.Core.APIExtensions
{
    public static class ConfigurationBuilderExtensions
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="this"></param>
        /// <param name="environment"></param>
        /// <param name="name"></param>
        /// <param name="optional"></param>
        /// <param name="reloadOnChange"></param>
        /// <returns></returns>
        public static IConfigurationBuilder AddAppJsonFile(this IConfigurationBuilder @this, IWebHostEnvironment environment,
            string name, bool optional = true, bool reloadOnChange = false) =>
            @this.AddJsonFile(Path.Combine(
                environment.GetPathForConfigurationFile(),
                $"{name.Split('.')[0]}.{environment.EnvironmentName}.json"), optional, reloadOnChange);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="this"></param>
        /// <param name="environment"></param>
        /// <param name="name"></param>
        /// <param name="optional"></param>
        /// <param name="reloadOnChange"></param>
        /// <returns></returns>
        public static IConfigurationBuilder AddSecretJsonFile(this IConfigurationBuilder @this, IWebHostEnvironment environment,
            string name, bool optional = true, bool reloadOnChange = false) =>
            @this.AddJsonFile(Path.Combine(
                environment.GetPathForConfigurationFile(),
                $"{name.Split('.')[0]}.{environment.EnvironmentName}.json"), optional, reloadOnChange);
    }
}
