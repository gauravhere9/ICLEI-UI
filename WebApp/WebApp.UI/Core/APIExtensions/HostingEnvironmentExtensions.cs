using WebApp.Global.Helpers;

namespace WebApp.UI.Core.APIExtensions
{
    public static class HostingEnvironmentExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static string GetPathForConfigurationFile(this IWebHostEnvironment @this) =>
            Path.Combine(
                AssemblyHelper.AssemblyDirectory(),
                //@this.EnvironmentName != "Development" ? "config" : string.Empty
                string.Empty
                );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static string GetPathForSecretConfigurationFile(this IWebHostEnvironment @this) =>
            Path.Combine(
                AssemblyHelper.AssemblyDirectory(),
                //@this.EnvironmentName != "Development" ? "config" : string.Empty
                string.Empty
                );
    }
}
