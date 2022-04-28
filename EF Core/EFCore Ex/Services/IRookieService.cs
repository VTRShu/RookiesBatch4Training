using EFCore_Ex.Repositories.Entities;

namespace EFCore_Ex.Services;

public interface IRookieService 
{
    Task<List<RookieDTO>> GetRookieListAsync();
    Task<RookieDTO> CreateRookieAsync(RookieDTO rookie);
    Task<RookieDTO> EditRookieAsync(Guid id,RookieDTO rookie);
    Task<bool> DeleteRookieAsync(Guid id);
    Task<RookieDTO> GetRookieDetailsAsync(Guid id);
}
