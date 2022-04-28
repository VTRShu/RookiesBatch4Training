using System;
using System.Collections.Generic;
namespace C_Fundamental_Exer_
{   
    public enum Genders{Male,Female}
    class Member
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Genders Gender { get; set; }
        public DateTime DoB { get; set; }
        public string PhoneNum { get; set; }
        public int Age { get
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
        
        public string FullName{ get { return $"{FirstName} {LastName}"; } }
        public static List<Member> members = new List<Member>()
        {
            new Member(){ FirstName = "Dai",  LastName = "Bao",    Gender = Genders.Male,   DoB = new DateTime(1995,2,2), BirthPlace ="QuangNinh",PhoneNum="5555555", Graduated = true},
            new Member(){ FirstName = "Dat",  LastName = "Nguyen", Gender = Genders.Male,   DoB = new DateTime(1996,2,2), BirthPlace ="Nam Dinh", PhoneNum="5555555", Graduated = true},
            new Member(){ FirstName = "Hai",  LastName = "Hoang",  Gender = Genders.Male,   DoB = new DateTime(1991,2,2), BirthPlace ="HaNoi" ,   PhoneNum="5555555", Graduated = false},
            new Member(){ FirstName = "Toang",LastName = "Nguyen", Gender = Genders.Male,   DoB = new DateTime(2000,2,2), BirthPlace ="HaGiang",  PhoneNum="5555555", Graduated = true},
            new Member(){ FirstName = "OhMan",LastName = "Nguyen", Gender = Genders.Female, DoB = new DateTime(2001,2,2), BirthPlace ="HaNoi",    PhoneNum="5555555", Graduated = false},
            new Member(){ FirstName = "John", LastName = "Tran",   Gender = Genders.Female, DoB = new DateTime(2000,2,2), BirthPlace ="NgheAn",   PhoneNum="5555555", Graduated = true},
            new Member(){ FirstName = "Alex", LastName = "Tran",   Gender = Genders.Female, DoB = new DateTime(1991,1,2), BirthPlace ="NgheAn",   PhoneNum="5555555", Graduated = false},
        };
    }
}