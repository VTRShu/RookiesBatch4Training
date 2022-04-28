using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace C_Sharp_Ex3_AsyncAwait
{
    class BusinessLogic
    {
        static List<int> listPrime = new List<int>();
        static List<int> range1 = new List<int>();
        static List<int> range2 = new List<int>();
        static List<int> range3 = new List<int>();
        static List<int> range4 = new List<int>();
  
        // public static List<List<int>> SeparateRange(int start, int end)
        // {   
        //     var listNumbers = new List<int>();
        //     for (int k = start; k <= end;k++)
        //     {
        //         listNumbers.Add(k);
        //     }       
        //     int i = 0;
        //     var range = (from name in listNumbers
        //                  group name by i++ %10 into part
        //                  select part.ToList()).ToList();
        //     return range;
        // }
        public static void SeparateRange(int start, int end)
        {
            for (int i = start; i < (start + (end / 4)); i++)
            {  
                if(i != 0)
                {
                    range1.Add(i);
                }
            }
            for (int k = (start + (end / 4)); k < (end/2); k++)
            {
                range2.Add(k);
            }
            for (int l = (end/2); l < ((end/2) + (end/4)); l++)
            {
                range3.Add(l);
            }
            for (int m = ((end/2) + (end/4)); m <= end; m++)
            {
                range4.Add(m);
            }
        }

        public static async Task<List<int>> GetPrimeFromRange()
        {     
            var task1 = new Task(() =>
            {
                range1.ForEach(number =>
                {
                    if (CheckPrime(number))
                    {
                        listPrime.Add(number);
                        Console.WriteLine("Task1: {0}",number);
                    }
                });
                  Task.Delay(1000);
                
            });

            var task2 =  new Task(() =>
            {
                range2.ForEach(number =>
                {
                    if (CheckPrime(number))
                    {
                        listPrime.Add(number);
                         Console.WriteLine("Task2: {0}",number);
                    }
                });
                Task.Delay(1000);
            });

            var task3 =  new Task(() =>
            {
                range3.ForEach(number =>
                {
                    if (CheckPrime(number))
                    {
                        listPrime.Add(number);
                         Console.WriteLine("Task3: {0}",number);
                    }
                });
                  Task.Delay(1000);
            });
         
            var task4 =  new Task(() =>
            {
                range4.ForEach(number =>
                {
                    if (CheckPrime(number))
                    {
                        listPrime.Add(number);
                         Console.WriteLine("Task4: {0}",number);
                    }
                });
                  Task.Delay(1000);
              
            });
            task1.Start();
            task2.Start();                                                                
            task3.Start();
            task4.Start();  
            await Task.WhenAll(task1, task2, task3, task4);
            return listPrime;
        }
        
        // public static async Task<List<int>> GetPrimeFromRange(int start,int end)
        // {    
        //     List<int> listPrime = new List<int>();
        //     return await Task.Factory.StartNew(()=>{
        //         for (int i = start; i <= end; i++)
        //         {
        //             if (CheckPrime(i))
        //             {
        //                 listPrime.Add(i);
        //             }
        //         }
        //         return listPrime;
        //     });
        // }
        public static bool CheckPrime(int number)
        {
            bool isPrime = true;
            for (int i = 2; i <= number / 2; i++)
            {
                if (number % i == 0)
                {
                    isPrime = false;
                    break;
                }
            }
            return isPrime;
        }
    }
}