using System;
using System.Runtime.InteropServices;

namespace ImportDllTest
{
    internal class Program
    {
        [DllImport("TestClassLibrary.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern string StringTest(string str);

        [DllImport("TestClassLibrary.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int IntTest(int a, int b);

        [DllImport("TestClassLibrary.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern string JsonTest(string str);

        private static void Main()
        {
            try
            {
                var stringTest = StringTest("문자열 전달 test");
                Console.WriteLine(stringTest);

                var intTest = IntTest(1, 2);
                Console.WriteLine(intTest);

                var jsonTest = JsonTest("{\"test\": \"테스트\"}");
                Console.WriteLine(jsonTest);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
