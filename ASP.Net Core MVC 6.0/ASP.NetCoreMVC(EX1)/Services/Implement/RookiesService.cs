using ASP.NetCoreMVC_EX1_.Models;
using System.Data;
using ClosedXML.Excel;
using ASP.NetCoreMVC_EX1_.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NetCoreMVC_EX1_.Services.Implement
{
    public class RookiesService : IRookiesService
    {
        public static List<RookieModel> rookies = new List<RookieModel>()
        {
            new RookieModel(){RookieId = 1, FirstName = "Long" ,  LastName = "Bao",   Email = "LongBNash@gmail.com",  Gender =Gender.Male,  DoB = new DateTime(1995,2,2), BirthPlace ="QuangNinh",PhoneNumber="012345678", Graduated = true},
            new RookieModel(){RookieId = 2, FirstName = "Ky" ,    LastName = "Nguyen",Email = "KyNNash@gmail.com",    Gender =Gender.Male,  DoB = new DateTime(1996,2,2), BirthPlace ="Nam Dinh", PhoneNumber="012345678", Graduated = true},
            new RookieModel(){RookieId = 3, FirstName = "Hung" ,  LastName = "Hoang", Email = "HungHNash@gmail.com",  Gender =Gender.Male,  DoB = new DateTime(1991,2,2), BirthPlace ="HaNoi" ,   PhoneNumber="012345678", Graduated = true},
            new RookieModel(){RookieId = 4, FirstName = "Van" ,   LastName = "Nguyen",Email = "VanNNash@gmail.com",   Gender =Gender.Male,  DoB = new DateTime(2000,2,2), BirthPlace ="HaGiang",  PhoneNumber="012345678", Graduated = true},
            new RookieModel(){RookieId = 5, FirstName = "Trang" , LastName = "Nguyen",Email = "TrangNNash@gmail.com", Gender =Gender.Female,DoB = new DateTime(2001,2,2), BirthPlace ="HaNoi",    PhoneNumber="012345678", Graduated = true},
            new RookieModel(){RookieId = 6, FirstName = "Huong" , LastName = "Tran",  Email = "HuongTNash@gmail.com", Gender =Gender.Female,DoB = new DateTime(2000,2,2), BirthPlace ="NgheAn",   PhoneNumber="012345678", Graduated = true},
            new RookieModel(){RookieId = 7, FirstName = "Huong" , LastName = "Tran",  Email = "HuongTNash@gmail.com", Gender =Gender.Female,DoB = new DateTime(1991,5,2), BirthPlace ="NgheAn",   PhoneNumber="012345678", Graduated = true},
        };

        public async Task<RookieModel> CreateRookie(RookieModel rookie)
        {
            var idList = (from member in rookies select member.RookieId).ToList();
            var maxId = idList.Max();
            rookie.RookieId = maxId + 1;
            rookies.Add(rookie);
            return await Task.FromResult(rookie);
        }

        public async Task<List<RookieModel>> GetAllRookie()
        {
            return await Task.FromResult(rookies.ToList());
        }

        public async Task<List<string>> GetFullNameList()
        {
            return await Task.FromResult(rookies.Select(rookie => rookie.FullName).ToList());
        }

        public async Task<List<RookieModel>> GetMaleRookieList()
        {
            // return await rookies.Where(rookie => rookie.Gender == Gender.Male).ToListAsync(); 
            return await Task.FromResult(rookies.FindAll(rookie => rookie.Gender == Gender.Male));
        }

        public async Task<RookieModel> GetOldestRookie()
        {
            return await Task.FromResult(rookies.OrderBy(rookie => rookie.DoB).FirstOrDefault());
        }

        public async Task<List<RookieModel>> GetRookieByBirthYear(string condition)
        {
            var rookiesEqual = await Task.FromResult(rookies.FindAll(rookie => rookie.DoB.Year == 2000));
            var rookiesGreater = await Task.FromResult(rookies.FindAll(rookie => rookie.DoB.Year > 2000));
            var rookiesLess = await Task.FromResult(rookies.FindAll(rookie => rookie.DoB.Year < 2000));
            if (condition == "equal")
            {
                return rookiesEqual;
            }
            else if (condition == "greater")
            {
                return rookiesGreater;
            }
            else if (condition == "less")
            {
                return rookiesLess;
            }
            return rookies;
        }
        public async Task<FileContentResult> ExportExcel()
        {
            DataTable dt = new DataTable("Rookies");
            dt.Columns.AddRange(new DataColumn[8] { new DataColumn("FirstName"),
                                        new DataColumn("LastName"),
                                        new DataColumn("Gender"),
                                        new DataColumn("DoB"),
                                        new DataColumn("BirthPlace"),
                                        new DataColumn("PhoneNumber"),
                                        new DataColumn("Age"),
                                        new DataColumn("Graduated") });
            var rookiesList = await GetAllRookie();
            foreach (var rookies in rookiesList)
            {
                dt.Rows.Add(rookies.FirstName, rookies.LastName, rookies.Gender, rookies.DoB, rookies.BirthPlace, rookies.PhoneNumber, rookies.Age, rookies.Graduated);
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return new FileContentResult(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = "Rookies.xlsx"
                    };
                }
            }
        }
    }

}