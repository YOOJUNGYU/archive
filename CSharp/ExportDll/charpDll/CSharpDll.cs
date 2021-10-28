using System.Runtime.InteropServices;

namespace CSharpDll
{
    public class CSharpDll
    {
        [ExportDll("Test", CallingConvention.StdCall)]
        public static string Test(string str)
            => $"받은 내용: {str}";
    }
}
