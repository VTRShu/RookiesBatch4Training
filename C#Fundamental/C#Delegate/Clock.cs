using System;
using System.Collections.Generic;
using System.Collections;
using System.Threading;
namespace C_Sharp3_Fundamental_ClockDelegate
{
        public class Clock{
        public int Hour{ get; set; }
        public int Minute{ get; set; }
        public int Second{ get; set; }

        public Clock(int hour, int minute, int second){
            Hour = hour;
            Minute = minute;
            Second = second;
        }
    }
}