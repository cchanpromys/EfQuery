using System.IO;
using System.Threading;
using System.Web.Mvc;

namespace AsyncMvcTest.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            var x = 20000;

            new Thread(() => LongRunningTask.InsertXRecordsToDb(x)).Start();

            return View();
        }
    }

    public class LongRunningTask
    {
        public static void InsertXRecordsToDb(int max)
        {
            for (int i = 0; i < max; i++)
            {
                Write(i);
            }
        }

        private static void Write(int i)
        {
            Thread.Sleep(1000);
            using (var fs = new StreamWriter(@"c:\file.txt", true))
            {
                fs.WriteLine(i);
            }
        }
    }
}