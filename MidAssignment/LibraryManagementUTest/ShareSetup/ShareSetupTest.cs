using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using EFCore_Ex2.Mappings;
using LibraryManagementBE.Repositories.EFContext;
using LibraryManagementBE.Repositories.Entities;
using LibraryManagementBE.Repositories.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;

namespace LibraryManagementUTest;
public class ShareSetupTest
{
    private LibraryManagementDBContext? _libraryMDBInMemoryContext;
    private DbContextOptions<LibraryManagementDBContext>? _dbContextOptions;
    private IMapper? _mapper;
    private List<AppUser>? _userTestList;
    private List<BookEntity>? _bookTestList;
    private List<CategoryEntity>? _categoryTestList;
    private List<AppRole>? _roleTestList;
    private List<IdentityUserRole<Guid>>? _identityUserTestRoles;
    private List<BookBorrowingRequest>? _borrowRequestTestList;
    private List<BookBorrowingRequestDetails>? _borrowRequestDetailTestList;
    public LibraryManagementDBContext InMemoryDatabaseSetup()
    {
        _dbContextOptions = new DbContextOptionsBuilder<LibraryManagementDBContext>()
        .UseInMemoryDatabase("LibraryManagement")
        .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)).Options;

        _libraryMDBInMemoryContext = new LibraryManagementDBContext(_dbContextOptions);
        _libraryMDBInMemoryContext.Database.EnsureCreated();
        _libraryMDBInMemoryContext.Database.EnsureDeleted();
        SeedInMemoryDatabase();

        return _libraryMDBInMemoryContext;
    }
    private void SeedInMemoryDatabase()
    {   
        //Role
        _roleTestList = new List<AppRole>(){
            new AppRole
            {
                Id = new Guid("077aaabb-ac69-4ab5-abe5-902dd5120fd9"),
                Name = "NormalUser",
                NormalizedName = "NORMALUSER",
                ConcurrencyStamp = "31BF5413-8303-4E21-8D3A-10099FCA95FE",
                Description = "Normal user",
            }, new AppRole
            {
                Id = new Guid("eb994f87-ed00-477e-ab20-d66214de73cc"),
                Name = "SuperUser",
                NormalizedName = "SUPERUSER",
                ConcurrencyStamp = "94BD65EE-DE64-4476-91AA-6258155DE018",
                Description = "Act like Admin",
            }
        };
        _libraryMDBInMemoryContext.AppRole.AddRange(_roleTestList);
        //User
        _userTestList = new List<AppUser>(){
            new AppUser()
            {
                Id = new Guid("1725b63b-f707-4b49-4ed2-08da06f835d7"),
                FullName = "Le Trung Nghia",
                Dob = new DateTime(2000,2,2),
                IsDisabled = false,
                Gender = (Gender)0,
                UserName = "nghialt",
                NormalizedUserName = "NGHIALT",
                Email = "nghia@gmail.com",
                NormalizedEmail = "NGHIA@GMAIL.COM",
                PasswordHash = "AQAAAAEAACcQAAAAEIlZvLNbRbavKqtei6MSGTYZmh3s0juNAmlKvOXMQ0DP+YDBmpCN9ryMHOFh3hlbAw==",
                SecurityStamp = "QXZYXIFQIFFM7TYFWNTSFT32V2J2Y7HM",
                ConcurrencyStamp = "79c962b7-4035-4016-bb71-f8a69e2deda3",
            },
            new AppUser()
            {
                Id = new Guid("7f9f0ea3-a755-4d38-4ed1-08da06f835d7"),
                FullName = "Pham Ngoc Dai",
                Dob = new DateTime(2000,1,1),
                IsDisabled = true,
                Gender = (Gender)0,
                UserName = "daipn",
                NormalizedUserName = "DAIPN",
                Email = "dai@gmail.com",
                NormalizedEmail = "DAI@GMAIL.COM",
                PasswordHash = "AQAAAAEAACcQAAAAELvSL0FZEmF+U1eOPOPxZmlypBIxliBJCcynTzFGQmVd6FhiyAG9m56lFv6MNH4lNQ==",
                SecurityStamp = "ST4X4QYRAABOJUBI2OA2F6CSNATJF7WB",
                ConcurrencyStamp = "65525777-0cc0-4364-8598-cdf93f0d5b14",
            },
            new AppUser()
            {
                Id = new Guid("478fd9c5-fa34-439e-a0c4-0a4e25f65fdf"),
                FullName = "Pham Ngoc Thang",
                Dob = new DateTime(2000,1,1),
                IsDisabled = false,
                Gender = (Gender)0,
                UserName = "thangpn",
                NormalizedUserName = "THANGPN",
                Email = "thang@gmail.com",
                NormalizedEmail = "THANG@GMAIL.COM",
                PasswordHash = "AQAAAAEAACcQAAAAEE3e12XgzC33WxYXEB11TbBZqSJ35LJ+39qVN48nwoj8fFjcuuH/XNnf9LnwS6mUrg==",
                SecurityStamp = "YN3AW7572GROFA6AJI5BEBQPWMKJOC2S",
                ConcurrencyStamp = "6c9f9a9c-b92c-45a0-bb1a-9375c3850504",
            },
        };
        _libraryMDBInMemoryContext.AppUser.AddRange(_userTestList);
        //Identity User Role
        _identityUserTestRoles = new List<IdentityUserRole<Guid>>(){
            new IdentityUserRole<Guid>{
                UserId = new Guid("7f9f0ea3-a755-4d38-4ed1-08da06f835d7"),
                RoleId = new Guid("077aaabb-ac69-4ab5-abe5-902dd5120fd9")
            }, 
            new IdentityUserRole<Guid>{
                UserId = new Guid("478fd9c5-fa34-439e-a0c4-0a4e25f65fdf"),
                RoleId = new Guid("077aaabb-ac69-4ab5-abe5-902dd5120fd9")
            }, 
            new IdentityUserRole<Guid>{
                UserId = new Guid("1725b63b-f707-4b49-4ed2-08da06f835d7"),
                RoleId = new Guid("eb994f87-ed00-477e-ab20-d66214de73cc")
            }
        };
        _libraryMDBInMemoryContext.UserRoles.AddRange(_identityUserTestRoles);
        //Categories
        _categoryTestList = new List<CategoryEntity>(){
            new CategoryEntity(){Id = Guid.NewGuid() , CategoryName = "Mecha"},
            new CategoryEntity(){Id = Guid.NewGuid() , CategoryName = "Isekai"}
        };
        _libraryMDBInMemoryContext.CategoryEntity.AddRange(_categoryTestList);
        //Books
        _bookTestList = new List<BookEntity>(){
            new BookEntity(){
                Id = Guid.NewGuid(),
                Name = "Eighty Six", 
                Description = "Mecha for life", 
                CategoryId =_categoryTestList[0].Id, 
                PublishedAt = DateTime.Now,
                CoverSrc = "https://static.wikia.nocookie.net/86-eighty-six/images/4/4c/Light_Novel_Volume_7_Cover.jpg"},
            new BookEntity(){
                Id = Guid.NewGuid(),
                Name = "Overlord", 
                Description="Badass dude", 
                CategoryId =_categoryTestList[1].Id, 
                PublishedAt = DateTime.Now,
                CoverSrc = "https://kbimages1-a.akamaihd.net/a1c7fe63-0c2d-4217-a823-5d0119a919a8/1200/1200/False/overlord-vol-11-light-novel.jpg"},
        };
        _libraryMDBInMemoryContext.BookEntity.AddRange(_bookTestList);
        //Borrow Request

        _borrowRequestTestList = new List<BookBorrowingRequest>(){
            new BookBorrowingRequest(){
                Id = Guid.NewGuid(),
                RequestedById = _userTestList[2].Id,
                RequestedAt = DateTime.Now,
                Status = (Status)2,
                ResponseById = null,
                ResponseAt = null,
            },
            new BookBorrowingRequest(){
                Id = Guid.NewGuid(),
                RequestedById = _userTestList[2].Id,
                RequestedAt = DateTime.Now,
                Status = (Status)2,
                ResponseById = null,
                ResponseAt = null,
            },
            new BookBorrowingRequest(){
                Id = Guid.NewGuid(),
                RequestedById = _userTestList[1].Id,
                RequestedAt = DateTime.Now,
                Status = (Status)2,
                ResponseById = null,
                ResponseAt = null,
            }
        };
        _libraryMDBInMemoryContext.BookBorrowingRequest.AddRange(_borrowRequestTestList);
        //Borrow Request Detail
        _borrowRequestDetailTestList = new List<BookBorrowingRequestDetails>{
            new BookBorrowingRequestDetails()
            {
                Id = Guid.NewGuid(),
                DetailOfRequestId = _borrowRequestTestList[0].Id, 
                BookName = _bookTestList[0].Name,
                BookId = _bookTestList[0].Id,
            },
            new BookBorrowingRequestDetails()
            {
                Id = Guid.NewGuid(),
                DetailOfRequestId = _borrowRequestTestList[0].Id, 
                BookName = _bookTestList[1].Name,
                BookId = _bookTestList[1].Id,
            },
            new BookBorrowingRequestDetails()
            {
                Id = Guid.NewGuid(),
                DetailOfRequestId = _borrowRequestTestList[1].Id, 
                BookName = _bookTestList[1].Name,
                BookId = _bookTestList[1].Id,
            },
        };
        _libraryMDBInMemoryContext.BookBorrowingRequestDetails.AddRange(_borrowRequestDetailTestList);
        _libraryMDBInMemoryContext.SaveChanges();
    } 
    public IMapper MappingForTesting()
    {
         var mapperConfiguration = new MapperConfiguration(
            cfg => cfg.AddProfile(new MappingProfile()));
        _mapper = mapperConfiguration.CreateMapper();

        return _mapper;
    }                                                                                   
    public IList<ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var ctx = new ValidationContext(model, null, null);
        Validator.TryValidateObject(model, ctx, validationResults, true);
        return validationResults;
    }
}