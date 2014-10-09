using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            LouieGor();
        }

        public static void LouieGor()
        {
            var sw = Stopwatch.StartNew();

            sw.Start();
            var thread = new Thread(ValidateUrlAsync) {IsBackground = true};
            thread.Start();
            sw.Stop();

            Console.WriteLine("We done! {0}ms", sw.ElapsedMilliseconds);

            Thread.Sleep(3000);
        }

        public static void ValidateUrlAsync()
        {
            Thread.Sleep(2000);
            using (var sw = new StreamWriter("c:\\file.txt", true))
            {
                sw.WriteLine("Chi");
            }
        }
    }
}
