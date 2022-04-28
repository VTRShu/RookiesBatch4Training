using System;
using System.Collections.Generic;
namespace C_Fundamental_Exer_
{
    class CommonBusinessLogic
    {
         // A list of members who is Male
        public static void GetMaleMembersList()
        {
            var members = Member.members;
            List<Member> newMaleList = new List<Member>();
            // for (int i = 0; i < members.Count; i++)
            // {
            //     if (members[i].Gender == "male")
            //     {
            //         var member = members[i].FirstName + " " + members[i].LastName;
            //         newMaleList.Add(member);
            //     }
            // }
            members.ForEach(rookie => {if(rookie.Gender == Genders.Male){
                newMaleList.Add(rookie);
            }});
            newMaleList.ForEach(rookie => Console.WriteLine(rookie.FullName));
        }
        
        //Oldest member
        public static void GetOldestMember()
        {
            var members = Member.members;
            int maxAge = 0;
            List<Member> listOldestMember = new List<Member>();
            foreach(Member rookie in members)
            {
                if(rookie.Age > maxAge)
                {
                    maxAge = rookie.Age;
                }
            }
            foreach(Member rookie in members)
            {
                if(rookie.Age == maxAge)
                {
                    listOldestMember.Add(rookie);
                }
            }

            // foreach (Member member in members)
            // {
            //     var age = CalculateAge(member);
            //     if (age > maxAge) maxAge = age;
            // }
            // Console.WriteLine(maxAge);
            // for (int i = 0; i < members.Count; i++)
            // {
            //     var ageMember = CalculateAge(members[i]);
            //     if (ageMember == maxAge)
            //     {
            //         listOldestMember.Add(members[i]);
            //     }
            // }
            Console.WriteLine("\n"+"Oldest member: " + listOldestMember[0].FullName +" " + listOldestMember[0].Age);
        }
        // public static int CalculateAge (Member member)
        // {
        //     DateTime now = DateTime.Now;
        //     int age = now.Year - member.DoB.Year;
        //     if (member.DoB > now.AddYears(-age))
        //     {
        //         age--;
        //     };
        //     return age;
        // }

        // A new List contains FullName
        public static void GetFullNameList()
        {
            var members = Member.members;
            List<string> NewMembers = new List<string>();
            members.ForEach(rookie => {NewMembers.Add(rookie.FullName);});
            NewMembers.ForEach(rookie => Console.WriteLine(rookie));
        }
        
        //First Person who was born in Ha Noi
        public static void GetBirthPlaceHaNoiList()
        {
            var members = Member.members;
            int i = 0;
            while (true)
            {
                i++;
                if (members[i].BirthPlace == "HaNoi")
                {
                    Console.WriteLine(members[i].FirstName + " " + members[i].LastName);
                    break;
                }
            }
        }
    }
}
