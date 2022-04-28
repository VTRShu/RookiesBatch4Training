using System;
using System.Collections.Generic;
using System.Collections;
using System.Threading;
namespace C_Sharp3_Fundamental_ClockDelegate
{   
   
    class Program
    {
 
        static void Main(string[] args)
        {
            ClockPublisher clockPublisher = new ClockPublisher();
            ClockSubscriber clockSubscriber = new ClockSubscriber();
            clockSubscriber.Subscribe(clockPublisher);
            clockPublisher.Run();
            
            // easiest way
            // while(true){
            //     Thread.Sleep(1000);
            //     DateTime now = DateTime.Now;
            //     Console.WriteLine($"Hour:{now.Hour}- Minute: {now.Minute}- Second: {now.Second}");
            // }
        }
    }
}
