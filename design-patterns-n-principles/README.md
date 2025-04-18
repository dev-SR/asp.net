# Design Patterns and Priciples

- [Design Patterns and Priciples](#design-patterns-and-priciples)
  - [Dependency Inversion Principle](#dependency-inversion-principle)
    - [🔧 Dependency Problem](#-dependency-problem)
  - [✅ The Solution: Apply Dependency Inversion Principle](#-the-solution-apply-dependency-inversion-principle)
    - [🛠 Define an Interface](#-define-an-interface)
    - [🛠 Use the Interface in the Controller](#-use-the-interface-in-the-controller)
    - [💡 Benefits of Applying DIP](#-benefits-of-applying-dip)
  - [🔁 Inversion of Control (IoC)](#-inversion-of-control-ioc)
    - [🧠 What is Inversion of Control?](#-what-is-inversion-of-control)
    - [Asp's IoC Container](#asps-ioc-container)
  - [🔄 Service Lifetimes in ASP.NET Core](#-service-lifetimes-in-aspnet-core)
    - [1. **Transient**](#1-transient)
    - [2. **Scoped**](#2-scoped)
    - [3. **Singleton**](#3-singleton)

## Dependency Inversion Principle

### 🔧 Dependency Problem

Let's say we have a service class that contains our business logic. Without applying any design principles, our controller might look like this:

```csharp
class BusinessLogicService
{
    public void DoSomething()
    {
        // Business logic here
    }
}

class MyController
{
    private BusinessLogicService _service;

    public MyController()
    {
        _service = new BusinessLogicService();
    }
    
    public void MyAction()
    {
        _service.DoSomething();
    }
}
```


At first glance, this looks fine — but there are hidden problems here:

**❌ What's wrong with this approach?**

1. **Tight Coupling**  
   The controller directly depends on the concrete service - we cant change `BusinessLogicService` class with i.e. `BusinessLogicService2`. Any change in the service can affect the controller.

2. **Hard to Test**  
   You can't easily replace `BusinessLogicService` with a mock class i.e. `DummyBusinessLogicService` during unit testing.

3. **Low Flexibility**  
   Want to switch to a new version of the service? You have to go back and modify the controller code.

4. **Violates SOLID Principles**  
   - **Single Responsibility Principle (SRP)**: The controller is doing two things — handling requests *and* creating its dependencies.
   - **Open/Closed Principle (OCP)**: The controller is not open for extension — you must modify it to change the service.
   - **Dependency Inversion Principle (DIP)** is completely broken — the high-level module depends directly on the low-level one.


## ✅ The Solution: Apply Dependency Inversion Principle

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


### 💡 Benefits of Applying DIP

- ✅ **Loose Coupling**: Easily swap implementations without touching the controller.
- ✅ **Testability**: Pass a mock service during unit tests.
- ✅ **Flexibility**: Add new service implementations with zero impact on high-level modules.
- ✅ **Follows SOLID Principles**: Especially SRP, OCP, and DIP.
- ✅ **Cleaner and Maintainable Code**: Components become easier to understand, modify, and extend.

## 🔁 Inversion of Control (IoC)

Now that our controller depends on an abstraction (`IBusinessLogicService`) instead of a concrete class, we need some mechanism to:

- **Create the correct implementation**
- **Inject it where needed (e.g., into the controller)**

That mechanism is called Inversion of Control (IoC).


### 🧠 What is Inversion of Control?

> Inversion of Control means we **delegate the responsibility of object creation** to a container/provider/framework, instead of creating them manually using `new`.

In simpler terms:

- Without IoC:  
  You manually control everything — creating services, passing them around.

- With IoC:  
  You **define what you need** (via `interfaces`), and the **IoC container** figures out *how* to provide it.

> It inverses the control by shifting the control (object creation) to IoC container
> "Don't call us, we'll call you" Pattern
> It can be implemented by other design patterns such as **Events**, **Service Locator**, and **Dependency Injection (DI)**.

### Asp's IoC Container

With asp.net core, All dependencies should be added into the `IServiceCollection` (act as IoC container). The framework will then take care of injecting them into your controllers or other classes that need them.

```csharp
builder.Services.Add(
    new ServiceDescriptor(
        typeof(Interface),            // e.g., IBusinessLogicService
        typeof(Service),              // e.g., BusinessLogicService2
        ServiceLifetime.Transient     // Lifetime - Transient, Singleton, Scoped
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


## 🔄 Service Lifetimes in ASP.NET Core

### 1. **Transient**  
🧪 **New instance every time** the service is requested.

- ✅ Use for **lightweight, stateless** services.
- 📦 Good for things like business logic, helpers, small utilities.

```csharp
builder.Services.AddTransient<IMyService, MyService>();
```

> Every `MyController`, or any class that uses `IMyService`, gets its **own fresh** `MyService` instance.

---

### 2. **Scoped**  

🔄 **One instance per request/HTTP call**.

- ✅ Use for services that need to **share data across layers** during a single request.
- 📦 Common for **DbContext**, user/session data, unit-of-work patterns.

```csharp
builder.Services.AddScoped<IMyService, MyService>();
```

> Same instance is used throughout the **lifetime of one request**, but a new one is created for each new request.

---

### 3. **Singleton**  
♾️ **One instance for the whole application lifetime**.

- ✅ Use for **caching**, logging, configuration, or services with **no internal state per request**.
- ⚠️ Be careful: shared instance means shared state across all users!

```csharp
builder.Services.AddSingleton<IMyService, MyService>();
```

> Created **once**, reused **forever**.

