using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace ExeWithDLL
{
    static class Program
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            // 리소스 dll 취득
            AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        private static Assembly ResolveAssembly(object sender, ResolveEventArgs args)
        {
            var thisAssembly = Assembly.GetExecutingAssembly();
            var name = args.Name.Substring(0, args.Name.IndexOf(',')) + ".dll";

            var resources = thisAssembly.GetManifestResourceNames().Where(s => s.EndsWith(name));
            var enumerable = resources.ToList();
            if (!enumerable.Any()) return null;
            var resourceName = enumerable.First();
            using var stream = thisAssembly.GetManifestResourceStream(resourceName);
            if (stream == null) return null;
            var assembly = new byte[stream.Length];
            stream.Read(assembly, 0, assembly.Length);
            return Assembly.Load(assembly);
        }
    }
}