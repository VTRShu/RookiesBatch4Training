using System;
using System.Collections.Generic;
using System.Linq;
namespace C_Fundamental_Exer1LinqVerson_
{
    class DOBBusinessLogic
    {
        public static void DoBMenu()
        {
            var members = Member.members;
            int options2;
            Console.WriteLine("1) Year : 2000");
            Console.WriteLine("2) Year > 2000");
            Console.WriteLine("3) Year < 2000");
            Console.WriteLine("4) Back to main menu");
            Console.Write("\r\nSelect an option: ");
            options2 = int.Parse(Console.ReadLine());
            switch (options2)
            {
                case 1:
                    Equal();
                    break;
                case 2:
                    Greater();
                    break;
                case 3:
                    Less();
                    break;
                case 4:
                    Menu.RunMenu();
                    break;
                default:
                    Console.WriteLine(" Wrong option, pls choose again! ");
                    break;
            }
        }

        public static void Equal()
        {
            var members = Member.members;
            // var rookieEqualList = (from rookie in members where rookie.DoB.Year == 2000 select rookie).ToList();

            // var rookieEqualList = members.FindAll(rookie => rookie.DoB.Year == 2000);
            // rookieEqualList.ForEach(rookie => Console.WriteLine(rookie.FullName));
            members.FindAll(rookie => rookie.DoB.Year == 2000)
            .ForEach(rookie => Console.WriteLine(rookie.FullName));
        }

        public static void Greater()
        {
            var members = Member.members;
            // var rookieGreaterList = (from rookie in members where rookie.DoB.Year > 2000 select rookie).ToList();

            // var rookieGreaterList = members.FindAll(rookie => rookie.DoB.Year > 2000);
            // rookieGreaterList.ForEach( rookie => Console.WriteLine(rookie.FullName)); 

            members.FindAll(rookie => rookie.DoB.Year > 2000)
            .ForEach(rookie => Console.WriteLine(rookie.FullName));
        }

        public static void Less()
        {
            var members = Member.members;
            // var rookieLessList = (from rookie in members where rookie.DoB.Year < 2000 select rookie).ToList();

            // var rookieLessList = members.FindAll(rookie => rookie.DoB.Year < 2000);
            // rookieLessList.ForEach( rookie => Console.WriteLine(rookie.FullName)); 

            members.FindAll(rookie => rookie.DoB.Year < 2000)
            .ForEach(rookie => Console.WriteLine(rookie.FullName));
        }

    }
}