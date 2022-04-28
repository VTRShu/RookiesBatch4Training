using AutoMapper;
using EFCore_Ex.Repositories.EFContext;
using EFCore_Ex.Repositories.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCore_Ex.Services.Implement;

public class RookieService : IRookieService
{
    private RookieDBContext _rookieDBContext;
    private readonly ILogger<RookieService> _logger;
     private readonly IMapper _mapper;
    public RookieService(RookieDBContext rookieDBContext, ILogger<RookieService> logger,IMapper mapper)
    {
        _rookieDBContext = rookieDBContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<RookieEntity> GetRookieByIdAsync(Guid id) => await _rookieDBContext.Rookies.FirstOrDefaultAsync(x => x.Id == id);
    public async Task<RookieDTO> CreateRookieAsync(RookieDTO rookie)
    {
        RookieDTO result = null;
        try
        {
            var newRookie =  _mapper.Map<RookieEntity>(rookie);
            await _rookieDBContext.Rookies.AddAsync(newRookie);
            await _rookieDBContext.SaveChangesAsync();
            result = _mapper.Map<RookieDTO>(newRookie);
            return result;
        }
        catch (Exception)
        {
            _logger.LogError("Can't Create rookie! Pls try again.");
        }
        return result;
    }

    public async Task<bool> DeleteRookieAsync(Guid id)
    {
        var currentRookie = await GetRookieByIdAsync(id);
        if(currentRookie != null)
        {
            _rookieDBContext.Rookies.Remove(currentRookie);
            await _rookieDBContext.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<RookieDTO> GetRookieDetailsAsync(Guid id)
    {
        var rookie = await GetRookieByIdAsync(id);
        if(rookie != null)
        {   
            var result = _mapper.Map<RookieDTO>(rookie);
            return await Task.FromResult(result);
        }
        return null;
    }
                                                            
    public async Task<List<RookieDTO>> GetRookieListAsync()
    {   
       return await _rookieDBContext.Rookies.Select(x=> _mapper.Map<RookieDTO>(x)).ToListAsync();
    }

    public async Task<RookieDTO> EditRookieAsync(Guid id, RookieDTO rookie)
    {
        RookieDTO result = null;
        try{
            var existRookie = await GetRookieByIdAsync(id);
            if(existRookie != null)
            {
                existRookie.FirstName = rookie.FirstName;
                existRookie.LastName = rookie.LastName;
                existRookie.PhoneNumber = rookie.PhoneNumber;
                existRookie.State = rookie.State;
                existRookie.City = rookie.City;
                await _rookieDBContext.SaveChangesAsync();
                result = _mapper.Map<RookieDTO>(existRookie);
                return result;
            }
        }catch(Exception)
        {
            _logger.LogError("Couldn't Edit Rookie");
        }
        return result;
    }
}
