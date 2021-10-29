using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace TestClassLibrary
{
    public class Export
    {
        static Export()
        {
            AppDomain.CurrentDomain.AssemblyResolve += Resolver;
        }


        public static Assembly Resolver(object sender, ResolveEventArgs args)
        {
            var dllFiles = new[] { "Newtonsoft.Json.dll" };

            foreach (var dllFile in dllFiles)
            {
                var fileName = Path.GetFileNameWithoutExtension(dllFile);
                if (string.IsNullOrEmpty(fileName)) continue;
                if (!args.Name.ToUpper().Contains(fileName.ToUpper())) continue;
                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                if (string.IsNullOrEmpty(path)) continue;
                return Assembly.LoadFile(Path.Combine(path, dllFile));
            }

            return null;
        }

        [ExportDllAttribute("StringTest", CallingConvention.StdCall)]
        public static string StringTest(string str)
            => $"받은 내용: {str}";

        [ExportDll("IntTest", CallingConvention.StdCall)]
        public static int IntTest(int a, int b)
            => a + b;

        [ExportDll("JsonTest", CallingConvention.StdCall)]
        public static string JsonTest(string str)
        {
            var jsonObj = JsonConvert.DeserializeObject(str);
            return $"JsonTest 성공 => {JsonConvert.SerializeObject(jsonObj)}";
        }
    }
}
