using System.Runtime.InteropServices;

namespace CSharpDll
{
    public class CSharpDll
    {
        [ExportDll("StringTest", CallingConvention.StdCall)]
        public static string StringTest(string str)
            => $"받은 내용: {str}";

        [ExportDll("IntTest", CallingConvention.StdCall)]
        public static int IntTest(int a, int b)
            => a + b;
    }
}
