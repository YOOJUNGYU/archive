using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace CSharpDll
{
    public class ExportDll
    {
        static ExportDll()
        {
            AppDomain.CurrentDomain.AssemblyResolve += Resolver;
        }


        public static Assembly Resolver(object sender, ResolveEventArgs args)
        {
            string[] DllFiles = new string[] { "Ionic.Zip.dll", "Newtonsoft.Json.dll" };

            try
            {
                foreach (var DllFile in DllFiles)
                {
                    if (args.Name.ToUpper().Contains(Path.GetFileNameWithoutExtension(DllFile).ToUpper()))
                    {
                        return Assembly.LoadFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), DllFile));
                    }
                }
            }
            catch (Exception)
            {

            }
            return null; // load failure
        }

        [ExportDll("StringTest", CallingConvention.StdCall)]
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
