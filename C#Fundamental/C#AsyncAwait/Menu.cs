using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
namespace C_Sharp_Ex3_AsyncAwait
{
    class Menu
    {
        public static async Task RunMenu()
        {
            int start;
            int end;

            Console.WriteLine("Input Range: ");
            Console.WriteLine("Start: ");
            start = int.Parse(Console.ReadLine());
            Console.WriteLine("End: ");
            end = int.Parse(Console.ReadLine());
            // var range = BusinessLogic.SeparateRange(start, end);
            BusinessLogic.SeparateRange(start, end);
            var sw = new Stopwatch();
            sw.Start();
            // Task<List<int>>[] listTask = new Task<List<int>>[10];
            // for (int i = 0; i < 10; i++)
            // {
            //     range.ForEach(num => {
            //         listTask[i] = BusinessLogic.GetPrimeFromRange(num[0],num.Last());
            //     });
            // }
            // var primeResults = await Task.WhenAll(listTask);
            // Console.Write("Total Prime found: {0}\n Sum prime list: {1}\n Runtime: {2}",
            // primeResults.Sum(p=>p.Count),
            // primeResults.Sum(p=>p.Sum()),
            // sw.ElapsedMilliseconds); 
            var primeResults = await BusinessLogic.GetPrimeFromRange();
            // Console.Write("Total Prime found: {0}\n Sum prime list: {1}\n Runtime: {2}",
            // primeResults.Count,
            // primeResults.Sum(),
            // sw.ElapsedMilliseconds);
            // sw.Stop();
        }
    }
}