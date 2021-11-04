using System.Windows.Forms;
using ExeWithDLL.Model;
using Newtonsoft.Json;

namespace ExeWithDLL
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            label1.Text = $"{(JsonTest() ? "성공" : "실패")}";
        }

        private static bool JsonTest()
        {
            var testModel = new TestModel
            {
                Test1 = "test1",
                Test2 = "test2"
            };
            return "{\"Test1\":\"test1\",\"Test2\":\"test2\"}".Equals(JsonConvert.SerializeObject(testModel));
        }
    }
}
