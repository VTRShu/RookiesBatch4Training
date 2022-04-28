using System;
using System.Collections.Generic;
using System.Linq;
namespace C_Fundamental_Exer1LinqVerson_
{
    public enum Genders { Male, Female };
    class Member
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Genders Gender { get; set; }
        public DateTime DoB { get; set; }
        public string PhoneNum { get; set; }
        public int Age
        {
            get
            {
                DateTime now = DateTime.Now;
                int age = now.Year - DoB.Year;
                if (DoB > now.AddYears(-age))
                {
                    age--;
                };
                return age;
            }
        }
        public string BirthPlace { get; set; }
        public Boolean Graduated { get; set; }
        public string FullName { get { return $"{FirstName} {LastName}"; } }

        public static List<Member> members = new List<Member>()
        {
            new Member(){ FirstName = "Long" ,  LastName = "Bao",    Gender = Genders.Male,  DoB = new DateTime(1995,2,2), BirthPlace ="QuangNinh",PhoneNum="5555555", Graduated = true},
            new Member(){ FirstName = "Ky" ,    LastName = "Nguyen", Gender = Genders.Male,  DoB = new DateTime(1991,1,10), BirthPlace ="Nam Dinh", PhoneNum="5555555", Graduated = true},
            new Member(){ FirstName = "Hung" ,  LastName = "Hoang",  Gender = Genders.Male,  DoB = new DateTime(1991,1,11), BirthPlace ="HaNoi" ,   PhoneNum="5555555", Graduated = true},
            new Member(){ FirstName = "Van" ,   LastName = "Nguyen", Gender = Genders.Male,  DoB = new DateTime(2000,2,2), BirthPlace ="HaGiang",  PhoneNum="5555555", Graduated = true},
            new Member(){ FirstName = "Trang" , LastName = "Nguyen", Gender = Genders.Female,DoB = new DateTime(2001,2,2), BirthPlace ="HaNoi",    PhoneNum="5555555", Graduated = true},
            new Member(){ FirstName = "Huong" , LastName = "Tran",   Gender = Genders.Female,DoB = new DateTime(2000,2,2), BirthPlace ="NgheAn",   PhoneNum="5555555", Graduated = true},
            new Member(){ FirstName = "Huong" , LastName = "Tran",   Gender = Genders.Female,DoB = new DateTime(1991,1,12), BirthPlace ="NgheAn",   PhoneNum="5555555", Graduated = true},
        };
    }
}