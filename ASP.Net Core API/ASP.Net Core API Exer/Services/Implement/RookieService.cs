using ASP.Net_Core_API_Exer.Models;
using ASP.Net_Core_API_Exer.Models.Enums;
using ASP.Net_Core_API_Exer.Request;

namespace ASP.Net_Core_API_Exer.Services.Implement;

public class RookieService : IRookieService
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
    public RookieModel GetById(int id) => rookies.FirstOrDefault(x => x.RookieId == id);
    public async Task<RookieModel> CreateNewRookieAsync(RookieModel rookie)
    {
        rookie.RookieId = rookies.OrderBy(x => x.RookieId).Select(x => x.RookieId).LastOrDefault() + 1;
        rookies.Add(rookie);
        return await Task.FromResult(rookie);
    }

    public async Task<bool> DeleteRookieAsync(int id)
    {
        var rookie = GetById(id);
        return await Task.FromResult(rookie != null && rookies.Remove(rookie));
    }

    public async Task<RookieModel> EditRookieAsync(int id, RookieModel rookie)
    {
        var currentRookie = GetById(id);
        if (currentRookie != null)
        {
            currentRookie.FirstName = rookie.FirstName;
            currentRookie.LastName = rookie.LastName;
            currentRookie.DoB = rookie.DoB;
            currentRookie.BirthPlace = rookie.BirthPlace;
            currentRookie.PhoneNumber = rookie.PhoneNumber;
            currentRookie.Graduated = rookie.Graduated;
            return rookie;
        }
        return null;
    }

    public async Task<RookieModel> GetRookieDetailsAsync(int id)
    {
        var rookie = GetById(id);
        if (rookie != null)
        {
            return await Task.FromResult(rookie);
        }
        return null;
    }

    public async Task<List<RookieModel>> GetRookieListAsync()
    {
        return await Task.FromResult(rookies.ToList());
    }
    public async Task<List<RookieModel>> FilterRookie(FilterRequest request = null)
    {
        var result = rookies;
        result = AddFilterQuery(request, result);
        return await Task.FromResult(result);
    }

    private static List<RookieModel> AddFilterQuery(FilterRequest request, List<RookieModel> rookiesFilter)
    {
        if (!string.IsNullOrEmpty(request?.BirthPlace))
        {
            rookiesFilter = rookiesFilter.Where(x => x.BirthPlace.Equals(request.BirthPlace, StringComparison.CurrentCultureIgnoreCase)).ToList();
        }
        if (!string.IsNullOrEmpty(request?.Name))
        {
            rookiesFilter = rookiesFilter.Where(x => x.LastName.Equals(request.Name, StringComparison.CurrentCultureIgnoreCase)
            || x.FirstName.Equals(request.Name, StringComparison.CurrentCultureIgnoreCase)).ToList();
        }
        if (request.Gender != null)
        {
            rookiesFilter = rookiesFilter.Where(x => x.Gender == request.Gender).ToList();
        }
        return rookiesFilter;
    } 
}