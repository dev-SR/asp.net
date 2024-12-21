using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/test")]
[ApiController]
public class TestController : ControllerBase
{

    [HttpGet]
    public Person GetPerson()
    {
        var person1 = new Person("John Doe", 30);
        return person1;
    }
}

public record Person(string Name, int Age);