using System;
using System.Collections.Generic;
using System.Collections;
using System.Threading;
namespace C_Sharp3_Fundamental_ClockDelegate
{
     public class ClockPublisher{
        //Publisher de raise event
        public delegate void OnSecondChangeHandler( Clock time);
        //Create event base on delegate
        public event OnSecondChangeHandler SecondChange;
        public void OnSecondChange(Clock time){
            SecondChange(time);
        }

        public void Run(){
            while(true){
                Thread.Sleep(1000);
                DateTime now = DateTime.Now;
                Clock time = new Clock(now.Hour,now.Minute,now.Second);
                OnSecondChange( time);
            }
        }
    }
}