using System;
using System.Runtime.InteropServices;

namespace ImportDllTest
{
    internal class Program
    {
        [DllImport("CSharpDll.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern string StringTest(string str);

        [DllImport("CSharpDll.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int IntTest(int a, int b);

        private static void Main()
        {
            try
            {
                var stringTest = StringTest("문자열 전달 test");
                Console.WriteLine(stringTest);

                var intTest = IntTest(1, 2);
                Console.WriteLine(intTest);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
