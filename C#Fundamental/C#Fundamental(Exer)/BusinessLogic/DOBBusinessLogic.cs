using System;
using System.Collections.Generic;
namespace C_Fundamental_Exer_
{
    class DOBBusinessLogic
    {
        public static void DoBMenu()
        {
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
            
            //List<string> memberEqualList = new List<string>();
            // for (int i = 0; i < members.Count; i++)
            // {
            //     if (members[i].DoB.Year == 2000)
            //     {
            //         var memberEqual = members[i].FirstName + " " + members[i].LastName;
            //         memberEqualList.Add(memberEqual);
            //     }
            // }
            // foreach (string member in memberEqualList)
            // {
            //     Console.WriteLine(member);
            // }
            List<Member> memberEqualList = new List<Member>();
            members.ForEach(rookie => {
                if(rookie.DoB.Year == 2000) memberEqualList.Add(rookie);
            });
            memberEqualList.ForEach(rookie => Console.WriteLine(rookie.FullName));
        }

        public static void Greater()
        {
            var members = Member.members;
            // List<string> memberGreaterList = new List<string>();
            // for (int i = 0; i < members.Count; i++)
            // {

            //     if (members[i].DoB.Year > 2000)
            //     {
            //         var memberGreater = members[i].FirstName + " " + members[i].LastName;
            //         memberGreaterList.Add(memberGreater);
            //     }
            // }
            // foreach (string member in memberGreaterList)
            // {
            //     Console.WriteLine(member);
            // }
            List<Member> memberEqualList = new List<Member>();
            members.ForEach(rookie => {
                if(rookie.DoB.Year>2000) 
                    memberEqualList.Add(rookie);
            });
            memberEqualList.ForEach(rookie => Console.WriteLine(rookie.FullName));
        }

        public static void Less()
        {
            var members = Member.members;
            // List<string> memberLessList = new List<string>();
            // for (int i = 0; i < members.Count; i++)
            // {
            //     if (members[i].DoB.Year < 2000)
            //     {
            //         var memberLess = members[i].FirstName + " " + members[i].LastName;
            //         memberLessList.Add(memberLess);
            //     }
            // }
            // foreach (string member in memberLessList)
            // {
            //     Console.WriteLine(member);
            // }
            List<Member> memberLessList = new List<Member>();
            members.ForEach(rookie => {
                if(rookie.DoB.Year < 2000) 
                    memberLessList.Add(rookie);
            });
            memberLessList.ForEach(rookie => Console.WriteLine(rookie.FullName));
        }

    }
}