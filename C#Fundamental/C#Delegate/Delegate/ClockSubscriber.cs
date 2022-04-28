using System;
using System.Collections.Generic;
using System.Collections;
using System.Threading;
namespace C_Sharp3_Fundamental_ClockDelegate
{
     public class ClockSubscriber{
        //Thuc thi viec hien thi thong tin thay doi
        public void Subscribe(ClockPublisher publisher){
            publisher.SecondChange += new ClockPublisher.OnSecondChangeHandler(TimeHasChanged);
        }
        
        private void TimeHasChanged(Clock time){
            Console.WriteLine($"The current time is {time.Hour} : {time.Minute} : {time.Second}");
        }

    }


}