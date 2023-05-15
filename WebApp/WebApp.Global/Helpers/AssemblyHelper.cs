using System.Reflection;

namespace WebApp.Global.Helpers
{
    public static class AssemblyHelper
    {
        //public static Assembly GetWebHostAssembly()
        //{
        //    return Assembly.GetAssembly(GetTypeByName("Program").First());
        //}

        //public static string AssemblyDirectory()
        //{
        //    return Path.GetDirectoryName(new Uri(GetWebHostAssembly().CodeBase).LocalPath);
        //}

        public static Type[] GetTypeByName(string className)
        {
            var matches = new List<Type>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                matches.AddRange(assembly.GetTypes().Where(type => type.Name == className));
            }
            return matches.ToArray();
        }

        public static Assembly GetWebHostAssembly()
        {
            var result = Assembly.GetAssembly(GetTypeByName("BaseController").FirstOrDefault());
            return result;
        }

        public static string AssemblyDirectory()
        {
            var assembly = GetWebHostAssembly();

            if (assembly == null)
            {
                return null;
            }

            var codeBase = assembly.Location;

            string[] urlSplits = codeBase.Split("bin/");

            var uriAssembly = new Uri(urlSplits[0]).LocalPath;

            var pathDirectory = Path.GetDirectoryName(uriAssembly);

            return pathDirectory;
        }
    }
}
