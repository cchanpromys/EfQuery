using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using NUnit.Framework;

namespace EfQuery
{
    [TestFixture]
    public class TaskTest
    {
        [Test]
        public void LouieGor()
        {
            var sw = Stopwatch.StartNew();

            sw.Start();
            var thread = new Thread(ValidateUrlAsync);
            thread.IsBackground = true;
            thread.Start();
            sw.Stop();

            Console.WriteLine("We done! {0}ms", sw.ElapsedMilliseconds);
        }

        public void ValidateUrlAsync()
        {
            Thread.Sleep(1000);
            using (var sw = new StreamWriter("c:\\file.txt", true))
            {
                sw.WriteLine("Chi");
            }
        }
    }
}
