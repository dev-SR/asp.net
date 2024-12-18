using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/test")]
public class TestController : BaseApiController
{


    [HttpGet]
    public Person GetPerson()
    {
        var person1 = new Person("John Doe", 30);
        return person1;

    }
}

public record Person(string Name, int Age);