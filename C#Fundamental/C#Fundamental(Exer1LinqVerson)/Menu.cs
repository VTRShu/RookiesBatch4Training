using System;
using System.Collections.Generic;
using System.Linq;
namespace C_Fundamental_Exer1LinqVerson_
{
    class Menu
    {
        public static void RunMenu()
        {
            int options;
            do
            {
                Console.WriteLine("\nChoose an option:");
                Console.WriteLine("1) Return a list of Male");
                Console.WriteLine("2) Oldest");
                Console.WriteLine("3) FullName only");
                Console.WriteLine("4) return 3 list");
                Console.WriteLine("5) 1st person who was born in HaNoi");
                Console.WriteLine("6) Exit program");
                Console.Write("\r\nSelect an option: ");

                options = int.Parse(Console.ReadLine());
                switch (options)
                {
                    case 1:
                        CommonBusinessLogic.GetMaleMembersList();
                        break;
                    case 2:
                        CommonBusinessLogic.GetOldestMember();
                        break;
                    case 3:
                        CommonBusinessLogic.GetFullNameList();
                        break;
                    case 4:
                        DOBBusinessLogic.DoBMenu();
                        break;
                    case 5:
                        CommonBusinessLogic.GetBirthPlaceHaNoiList();
                        break;
                    case 6:
                        Console.WriteLine("Exit....");
                        break;
                    default:
                        Console.WriteLine(" Wrong option, pls choose again! ");
                        break;
                }
            } while (options != 6);
        }
    }
}