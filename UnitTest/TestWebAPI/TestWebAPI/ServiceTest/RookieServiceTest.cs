using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;
using WebAPI.Models.Enums;
using WebAPI.Services;
using WebAPI.Services.Implement;
using NUnit.Framework;

namespace TestWebAPI;

public class RookieServiceTest
{   
    private IRookieService _rookieService;
    [SetUp]
    public void Setup()
    {
        _rookieService = new RookieService();
    }

    [Test]
    public async Task Test_RookieService_ReturnListRookie()
    {   
        var result = await _rookieService.GetRookieListAsync();

        Assert.IsNotNull(result);
        Assert.AreEqual(7,result.Count);
    }
}