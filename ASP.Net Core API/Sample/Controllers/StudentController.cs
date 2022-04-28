using Microsoft.AspNetCore.Mvc;

namespace Sample.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class StudentController : ControllerBase
{
    List<Student> studentList = new List<Student>(){
        new Student{BirthPlace = "HaNoi", Gender = "Male", StudentName = "LOL", University = "GW"},
        new Student{BirthPlace = "NamDinh", Gender = "Male", StudentName = "UWU", University = "GW"},
    };
    [HttpGet]
    public List<Student> GetStudent(string? birthPlace)
    {
        if(string.IsNullOrEmpty(birthPlace))
        return studentList.Where(x=>x.BirthPlace == "HaNoi").ToList();
        else
        return studentList.Where(x=>x.BirthPlace == birthPlace).ToList();
    }
}