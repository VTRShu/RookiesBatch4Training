using System;
using System.Collections.Generic;
using System.Linq;

namespace C_Fundamental_Exer1LinqVerson_
{
    class CommonBusinessLogic
    {
        public static void GetMaleMembersList()
        {
            var members = Member.members;
            // var newMaleList = (from member in members where member.Gender == Genders.Male select member).ToList();
            var newMaleList = members.FindAll(rookie => rookie.Gender == Genders.Male);
            newMaleList.ForEach(rookie => Console.WriteLine(rookie.FullName + " " + rookie.Gender));
        }

        public static void GetOldestMember()
        {
            var members = Member.members;
            // var oldestRookie = (from rookie in members orderby rookie.DoB descending select rookie).LastOrDefault();
            // Console.WriteLine(oldestRookie.FullName);
            var oldestRookie = members.OrderBy(rookie => rookie.DoB).FirstOrDefault();
            Console.WriteLine(oldestRookie.FullName);
        }

        public static void GetFullNameList()
        {
            var members = Member.members;
            // var fullNameList = (from member in members select member.FirstName + " " + member.LastName).ToList();
            members.ForEach(member => Console.WriteLine(member.FullName));
         
        }

        public static void GetBirthPlaceHaNoiList()
        {
            var members = Member.members;
            // var haNoiMemberList = (from member in members where member.BirthPlace == "HaNoi" select member).ToList();
            // Console.WriteLine(haNoiMemberList[0].FirstName + " " + haNoiMemberList[0].LastName);          
            var firstHaNoiRookie = members.FirstOrDefault(rookie => rookie.BirthPlace == "HaNoi");
            Console.WriteLine(firstHaNoiRookie.FullName);
        }
    }
}