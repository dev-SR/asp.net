# Design Patterns and Priciples

- [Design Patterns and Priciples](#design-patterns-and-priciples)
  - [⚠️ Our Problem: Lack of Maintainable Architecture](#️-our-problem-lack-of-maintainable-architecture)
    - [❌ Identified Issues with all in one approach](#-identified-issues-with-all-in-one-approach)
      - [🔴 1. **Violation of Single Responsibility Principle (SRP)**](#-1-violation-of-single-responsibility-principle-srp)
      - [2. **Tight Coupling**](#2-tight-coupling)
      - [3. **Poor Testability**](#3-poor-testability)
      - [4. **No Support for Parallel Development**](#4-no-support-for-parallel-development)
      - [5. **Violation of Open-Closed Principle (OCP)**](#5-violation-of-open-closed-principle-ocp)
    - [✅ Refactored Version Using Services](#-refactored-version-using-services)
      - [✅ Improvements in the Refactored Version](#-improvements-in-the-refactored-version)
        - [1. 🔹 **Code Reusability Improved**](#1--code-reusability-improved)
        - [2. 🔹 **Separation of Concerns (SoC) Achieved**](#2--separation-of-concerns-soc-achieved)
      - [🟨 Partially Solved Issues](#-partially-solved-issues)
        - [1. ⚠ **SRP Compliance is Better but Not Perfect**](#1--srp-compliance-is-better-but-not-perfect)
      - [❌ Remaining Issues](#-remaining-issues)
        - [1. 🔴 **No Dependency Injection Yet**](#1--no-dependency-injection-yet)
        - [2. 🔴 **Still Not Fully Testable**](#2--still-not-fully-testable)
        - [3. 🔴 **Tight Coupling Still Exists**](#3--tight-coupling-still-exists)
        - [4. 🔴 **Parallel Development Still Blocked**](#4--parallel-development-still-blocked)
  - [✅ The Solution: SOLID + Dependency Inversion Principle (DIP) + Inversion of Control (IoC)](#-the-solution-solid--dependency-inversion-principle-dip--inversion-of-control-ioc)
    - [🛠 Define an Interface](#-define-an-interface)
    - [🛠 Use the Interface in the Controller](#-use-the-interface-in-the-controller)
    - [🔁 Inversion of Control (IoC)](#-inversion-of-control-ioc)
      - [🧠 What is Inversion of Control?](#-what-is-inversion-of-control)
      - [Asp's IoC Container](#asps-ioc-container)
    - [🔄 Service Lifetimes in ASP.NET Core](#-service-lifetimes-in-aspnet-core)
      - [1. **Transient**  ](#1-transient-)
      - [2. **Scoped**  ](#2-scoped-)
      - [3. **Singleton**  ](#3-singleton-)
  - [✅ Final Solution of our problem:](#-final-solution-of-our-problem)
    - [Step 1: Create Interfaces](#step-1-create-interfaces)
    - [Step 2: Inject Services into Controller](#step-2-inject-services-into-controller)
    - [Register Services in Startup.cs](#register-services-in-startupcs)
  - [🎯 How This Solves the Original Problems](#-how-this-solves-the-original-problems)


## ⚠️ Our Problem: Lack of Maintainable Architecture

Let's say we have a simple ASP.NET Core MVC application that fetches data from an API and displays it in a paginated view. The code might look something like this:

```csharp
public async Task<ActionResult> Index(int page = 1, int pageSize = 5)
{
    string apiUrl = "http://localhost:5004/WeatherForecast/GetNames";

    using var httpClient = new HttpClient();
    var response = await httpClient.GetAsync(apiUrl);

    if (!response.IsSuccessStatusCode)
    {
        return StatusCode((int)response.StatusCode, "Error fetching data from API");
    }

    var apiData = await response.Content.ReadFromJsonAsync<List<string>>();

    var paginatedNames = apiData
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .Select(name => name.ToUpper())
        .ToList();

    ViewData["CurrentPage"] = page;
    ViewData["TotalPages"] = (int)Math.Ceiling((double)apiData.Count / pageSize);

    return View(paginatedNames);
}
```

Above code is a simple controller action that fetches data from an API, paginates it, and transforms it to uppercase before returning it to the view. However, this code has several issues:

### ❌ Identified Issues with all in one approach

#### 🔴 1. **Violation of Single Responsibility Principle (SRP)**

👉 The `Index()` method is supposed to only return a view, but it also:
- Makes an HTTP call  
- Implements pagination  
- Transforms data (e.g., to uppercase)  

🔧 **Solution**: Each responsibility should be delegated to a separate service class, like `DataFetchService`, `PaginationService`, and `TransformationService`.

---

#### 2. **Tight Coupling**

👉 The method depends directly on `HttpClient` and `List<string>`.  
If the data structure or the fetching logic changes - i.e change from `List<string>` to `List<int>` or switch to a different API client library- you'll need to modify the controller method.

🔧 **Solution**: Introduce interfaces like `IDataFetchService` to decouple the implementation.

---

#### 3. **Poor Testability**

- 🔗 **Tightly Coupled Logic**  
  All logic (data fetch, pagination, transformation) lives inside the controller — can't be tested separately.
  Therefore, you need to write integration tests instead of isolated unit tests.

```csharp
using Xunit;
using Moq;
using FluentAssertions;

public class HomeControllerTests
{
    [Fact]
    public async Task Index_ReturnsPaginatedUppercaseNames_WhenApiSucceeds()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<HomeController>>();
        var controller = new HomeController(loggerMock.Object);

        // Act
        var result = await controller.Index(page: 1, pageSize: 5);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<List<string>>(viewResult.Model);

        model.Should().OnlyContain(name => name == name.ToUpper());
        model.Count.Should().BeLessThanOrEqualTo(5);

        controller.ViewData["CurrentPage"].Should().Be(1);
        controller.ViewData["TotalPages"].Should().BeOfType<int>();
    }
}
```

Thefore the test is not isolated and depends on the actual API call. This leads to:

- 🧪 **Limited Test Coverage**  
  Only full-flow behavior can be tested — no isolated unit tests for pagination, transformation, or error handling.
- 🔌 **Dependency on External API**  
  Test results depend on whether the real API is running and available, leading to flaky tests.
- ⚠️ **Hard to Simulate Edge Cases**  
  Difficult to test scenarios like empty responses, malformed data, or partial data from the API.
- 🎭 **No Mocking of Dependencies**  
  You can't mock `HttpClient` or simulate different API behaviors (success, failure, timeout) without hacks.
- 🔁 **Repetition and Boilerplate**  
  If you want to test different cases, you need to repeat the entire controller execution flow every time.
- 🧱 **Difficult to Extend or Reuse Logic**  
  Any change in one part (e.g., changing how names are transformed) requires editing and retesting the full method.

🔧 **Solution**: Move each piece of logic to its own `interface/service` so they can be mocked and tested independently.

---

#### 4. **No Support for Parallel Development**

👉 The controller tightly handles data fetching, processing, and UI rendering.  
🔻 As a result:
- Frontend teams must wait until the backend is fully implemented.  
- Agile workflow becomes difficult to maintain.

🔧 **Solution**: Use service-based architecture to decouple frontend and backend progress.

---
#### 5. **Violation of Open-Closed Principle (OCP)**

👉 If you want to switch from uppercase to lowercase transformation, you must modify the controller code.

🔧 **Solution**: Move transformation logic to a separate interface to allow extending functionality without changing existing code.


### ✅ Refactored Version Using Services

Let separate the concerns into different service classes:

```csharp
public class DataFetchService
{
    private readonly HttpClient _httpClient;
    public DataFetchService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<List<string>> FetchDataAsync(string apiUrl)
    {
        var response = await _httpClient.GetAsync(apiUrl);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<string>>();
        }
        return new List<string>();
    }
}

public class PaginationService
{
    public List<T> Paginate<T>(List<T> data, int page, int pageSize)
    {
        return data.Skip((page - 1) * pageSize).Take(pageSize).ToList();
    }

    public int CalculateTotalPages(int totalItems, int pageSize)
    {
        return (int)Math.Ceiling((double)totalItems / pageSize);
    }
}

public class StringTransformationService
{
    public List<string> TransformToUpper(List<string> data)
    {
        return data.Select(x => x.ToUpper()).ToList();
    }
}
```

Updated Controller Using These Services:

```csharp
public class Home2Controller : Controller
{
    public async Task<IActionResult> Index(int page = 1, int pageSize = 5)
    {
        string apiUrl = "http://localhost:5004/WeatherForecast/GetNames";

        var dataFetchService = new DataFetchService(new HttpClient());
        var paginationService = new PaginationService();
        var stringTransformationService = new StringTransformationService();

        var apiData = await dataFetchService.FetchDataAsync(apiUrl);
        var paginatedData = paginationService.Paginate(apiData, page, pageSize);
        var transformedData = stringTransformationService.TransformToUpper(paginatedData);

        ViewData["CurrentPage"] = page;
        ViewData["TotalPages"] = paginationService.CalculateTotalPages(apiData.Count, pageSize);

        return View(transformedData);
    }
}
```

---

#### ✅ Improvements in the Refactored Version

##### 1. 🔹 **Code Reusability Improved**
- Services like `PaginationService` are generic and reusable across different data types.
- `StringTransformationService` can be used with other data sources.

---

##### 2. 🔹 **Separation of Concerns (SoC) Achieved**
- The controller now only coordinates services, rather than implementing logic itself.

---

#### 🟨 Partially Solved Issues

##### 1. ⚠ **SRP Compliance is Better but Not Perfect**
- Logic is somewhat separated, but controller still instantiates service objects (`new DataFetchService(...)`), keeping it coupled to implementations.

---

#### ❌ Remaining Issues

##### 1. 🔴 **No Dependency Injection Yet**

```csharp
var dataFetchService = new DataFetchService(new HttpClient());
```
- Services are manually created inside the controller.
- This results in tight coupling and limits testability.

🔧 **Solution**: Inject services via the controller constructor.

---

##### 2. 🔴 **Still Not Fully Testable**
- Direct instantiation (`new HttpClient()`) leads to actual HTTP calls in tests.
- Unit testing remains difficult.

🔧 **Solution**: Use interfaces for mocking.

---

##### 3. 🔴 **Tight Coupling Still Exists**
- Changing a service means also modifying the controller.

🔧 **Solution**: Use interfaces for loose coupling.

---

##### 4. 🔴 **Parallel Development Still Blocked**
- Because the controller still relies on actual services, frontend cannot proceed independently with mocked data.

🔧 **Solution**: Introduce interfaces + DI + mock implementations.

---

## ✅ The Solution: SOLID + Dependency Inversion Principle (DIP) + Inversion of Control (IoC)

We can solve the tight coupling issue by introducing an *abstraction* (an **interface/contract**), and making **both the controller and the service depend** on it. This is the core idea behind the Dependency Inversion Principle (DIP).


The **Dependency Inversion Principle** is one of the SOLID principles of object-oriented design. It says:

> **High-level modules should not depend on low-level modules. Both should depend on abstractions.**  
> **Abstractions should not depend on details. Details should depend on abstractions.**

> This means we should depend on interfaces or abstract classes, not concrete implementations.

### 🛠 Define an Interface

```csharp
public interface IBusinessLogicService
{
    void DoSomething();
}
```

### 🛠 Use the Interface in the Controller

```csharp
public class MyController
{
    private readonly IBusinessLogicService _service;

    public MyController(IBusinessLogicService service)
    {
        _service = service;
    }

    public void MyAction()
    {
        _service.DoSomething();
    }
}
```


Once we’ve inverted the dependency onto an interface, the controller no longer depends on the concrete `BusinessLogicService`, doesn’t care which implementation it gets—it could be your real `BusinessLogicService`, a new `BusinessLogicService2`, a `DummyBusinessLogicService` for tests… even a yet‐to‐be‐implemented stub. Also now both both the controller and the service depend on the same abstraction, which is a good design practice.

```csharp
// Production implementation #1
public class BusinessLogicService : IBusinessLogicService{}
//or
// Production implementation #2
public class BusinessLogicService2 : IBusinessLogicService{}
// Test/dummy implementation
public class DummyBusinessLogicService : IBusinessLogicService{}
//...
// Stub/unimplemented placeholder
public class UnimplementedService : IBusinessLogicService
{
    public void DoSomething()
        => throw new NotImplementedException("This service isn’t ready yet");
}
```

By inverting the dependency onto IBusinessLogicService, we’ve achieved full decoupling, maximum flexibility, and rock‑solid testability—all without a single new inside MyController.


💡 Benefits of Applying DIP

- ✅ **Loose Coupling**: Easily swap implementations without touching the controller.
- ✅ **Testability**: Pass a mock service during unit tests.
- ✅ **Flexibility**: Add new service implementations with zero impact on high-level modules.
- ✅ **Follows SOLID Principles**: Especially SRP, OCP, and DIP.
- ✅ **Cleaner and Maintainable Code**: Components become easier to understand, modify, and extend.

### 🔁 Inversion of Control (IoC)

Now that our controller depends on an abstraction (`IBusinessLogicService`) instead of a concrete class, we need some mechanism to:

- **Create the correct implementation**
- **Inject it where needed (e.g., into the controller)**

That mechanism is called Inversion of Control (IoC).


#### 🧠 What is Inversion of Control?

> Inversion of Control means we **delegate the responsibility of object creation** to a container/provider/framework, instead of creating them manually using `new`.

In simpler terms:

- Without IoC:  
  You manually control everything — creating services, passing them around.

- With IoC:  
  You **define what you need** (via `interfaces`), and the **IoC container** figures out *how* to provide it.

> It inverses the control by shifting the control (object creation) to IoC container
> "Don't call us, we'll call you" Pattern
> It can be implemented by other design patterns such as **Events**, **Service Locator**, and **Dependency Injection (DI)**.

#### Asp's IoC Container

With asp.net core, All dependencies should be added into the `IServiceCollection` (act as IoC container). The framework will then take care of injecting them into your controllers or other classes that need them.

```csharp
builder.Services.Add(
    new ServiceDescriptor(
        typeof(Interface),            // e.g., IBusinessLogicService
        typeof(Service),              // e.g., BusinessLogicService2
        ServiceLifetime.Transient     // Lifetime - Transient, Singleton, Scoped
    )
);

// 🧠 It's equivalent to:
builder.Services.AddTransient<Interface, Service>();
```

This registers a mapping in the IoC container:

- “When someone asks for Interface, give them a Service.”
- Lifetime controls how long the instance lives.

✅ Benefits of Using IoC

- 🔄 **Centralized control** of how dependencies are created and shared
- 🧪 **Easier testing** by swapping real services with fakes/mocks
- 🔌 **Plug-and-play architecture**—add/remove/change services without touching the consumers
- 🧼 **Cleaner code**—no `new`, no object graph management inside your business logic


### 🔄 Service Lifetimes in ASP.NET Core

#### 1. **Transient**  
🧪 **New instance every time** the service is requested.

- ✅ Use for **lightweight, stateless** services.
- 📦 Good for things like business logic, helpers, small utilities.

```csharp
builder.Services.AddTransient<IMyService, MyService>();
```

> Every `MyController`, or any class that uses `IMyService`, gets its **own fresh** `MyService` instance.

---

#### 2. **Scoped**  

🔄 **One instance per request/HTTP call**.

- ✅ Use for services that need to **share data across layers** during a single request.
- 📦 Common for **DbContext**, user/session data, unit-of-work patterns.

```csharp
builder.Services.AddScoped<IMyService, MyService>();
```

> Same instance is used throughout the **lifetime of one request**, but a new one is created for each new request.

---

#### 3. **Singleton**  
♾️ **One instance for the whole application lifetime**.

- ✅ Use for **caching**, logging, configuration, or services with **no internal state per request**.
- ⚠️ Be careful: shared instance means shared state across all users!

```csharp
builder.Services.AddSingleton<IMyService, MyService>();
```

> Created **once**, reused **forever**.

## ✅ Final Solution of our problem:

### Step 1: Create Interfaces

```csharp
public interface IDataFetchService
{
    Task<List<string>> FetchDataAsync(string apiUrl);
}

public interface IPaginationService
{
    List<T> Paginate<T>(List<T> data, int page, int pageSize);
    int CalculateTotalPages(int totalItems, int pageSize);
}

public interface IStringTransformationService
{
    List<string> TransformToUpper(List<string> data);
}
```

### Step 2: Inject Services into Controller

```csharp
public class WeatherForecastController : Controller
{
    private readonly IDataFetchService _dataFetchService;
    private readonly IPaginationService _paginationService;
    private readonly IStringTransformationService _stringTransformationService;

    public WeatherForecastController(
        IDataFetchService dataFetchService,
        IPaginationService paginationService,
        IStringTransformationService stringTransformationService)
    {
        _dataFetchService = dataFetchService;
        _paginationService = paginationService;
        _stringTransformationService = stringTransformationService;
    }

    public async Task<ActionResult> Index(int page = 1, int pageSize = 5)
    {
        string apiUrl = "http://localhost:5004/WeatherForecast/GetNames";

        var apiData = await _dataFetchService.FetchDataAsync(apiUrl);
        var paginatedNames = _paginationService.Paginate(apiData, page, pageSize);
        paginatedNames = _stringTransformationService.TransformToUpper(paginatedNames);

        ViewData["CurrentPage"] = page;
        ViewData["TotalPages"] = _paginationService.CalculateTotalPages(apiData.Count, pageSize);

        return View(paginatedNames);
    }
}
```

### Register Services in Startup.cs

```csharp
builder.Services.AddTransient<IDataFetchService, DataFetchService>();
builder.Services.AddTransient<IPaginationService, PaginationService>();
builder.Services.AddTransient<IStringTransformationService, StringTransformationService>();
```

---

## 🎯 How This Solves the Original Problems

By applying the Dependency Inversion Principle through interfaces and utilizing ASP.NET Core's built-in Inversion of Control container for Dependency Injection, the final solution effectively addresses the issues identified in the original "all-in-one" approach and the partially refactored version:

✅ **Violation of Single Responsibility Principle (SRP):** The controller no longer performs multiple tasks (fetching, pagination, transformation). Each of these responsibilities is now confined to dedicated service classes (`DataFetchService`, `PaginationService`, `StringTransformationService`), making the controller responsible only for handling the request, coordinating the services, and preparing the view.

✅ **Tight Coupling:** The controller now depends on **interfaces** (`IDataFetchService`, `IPaginationService`, `IStringTransformationService`) instead of concrete implementations. This eliminates the tight coupling present in both the initial and partially refactored versions. You can change the implementation of any service (e.g., switch from `HttpClient` to another library in `DataFetchService`) without altering the controller code.

✅ **Poor Testability:** Because the controller depends on interfaces, we can easily provide **mock** implementations of these interfaces during unit testing. This allows for true unit tests of the controller's logic in isolation, without making actual API calls or depending on the real service implementations. This significantly improves test reliability, speed, and the ability to test edge cases.

✅ **No Support for Parallel Development:** With dependencies defined as interfaces, frontend or other development teams can work against mock implementations of the services while the actual backend services are being developed. This decouples development streams and facilitates parallel work, adhering better to agile principles.

✅ **Violation of Open-Closed Principle (OCP):** The controller is now open for extension but closed for modification regarding the transformation logic. To add a new transformation (e.g., lowercase, trimming), you simply create a new class that implements `IStringTransformationService` and register it in the IoC container. The controller code remains unchanged.

✅ **No Dependency Injection Yet (Remaining Issue):** The final solution explicitly uses **constructor injection**. The ASP.NET Core IoC container is configured to build and provide the necessary service instances when the controller is created, eliminating the need for manual `new`ing inside the controller action.

✅ **Still Not Fully Testable (Remaining Issue):** As mentioned under "Poor Testability," using interfaces and DI makes the controller fully testable by allowing dependencies to be mocked.

✅ **Tight Coupling Still Exists (Remaining Issue):** As mentioned under "Tight Coupling," depending on interfaces solves this remaining issue.

✅ **Parallel Development Still Blocked (Remaining Issue):** As mentioned under "No Support for Parallel Development," using interfaces, DI, and mock implementations unlocks parallel development.

In summary, by embracing Dependency Inversion Principle (DIP) through interfaces and leveraging Inversion of Control (IoC) via Dependency Injection (DI), the architecture becomes significantly more maintainable, testable, flexible, and scalable, demonstrating core principles of good software design.
