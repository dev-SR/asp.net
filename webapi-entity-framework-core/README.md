# Entity Framework (EF) Core

- [Entity Framework (EF) Core](#entity-framework-ef-core)
  - [Defining a Model](#defining-a-model)
    - [EF Core conventions](#ef-core-conventions)
    - [EF Core annotation attributes](#ef-core-annotation-attributes)
    - [EF Core Fluent API](#ef-core-fluent-api)
    - [**Configuring nonrelational properties**](#configuring-nonrelational-properties)
      - [Setting database column type, size, and nullability](#setting-database-column-type-size-and-nullability)
      - [The different ways of configuring the primary key](#the-different-ways-of-configuring-the-primary-key)
        - [Configuring a primary key via Data Annotations](#configuring-a-primary-key-via-data-annotations)
        - [Configuring a primary key via the Fluent API](#configuring-a-primary-key-via-the-fluent-api)
      - [Adding indexes to database columns](#adding-indexes-to-database-columns)
      - [Excluding properties and classes from the database](#excluding-properties-and-classes-from-the-database)
        - [Excluding a class or property via Data Annotations](#excluding-a-class-or-property-via-data-annotations)
        - [Excluding a class or property via Fluent API](#excluding-a-class-or-property-via-fluent-api)
    - [**Configuring relationships**](#configuring-relationships)
      - [Defining some relationship terms](#defining-some-relationship-terms)
        - [What navigational properties do you need?](#what-navigational-properties-do-you-need)
      - [Configuring relationships By Convention](#configuring-relationships-by-convention)
        - [One-to-Many Relationship](#one-to-many-relationship)
        - [One-to-One Relationship](#one-to-one-relationship)
        - [Many-to-Many Relationship](#many-to-many-relationship)
      - [Creating an instance of the application’s DbContext](#creating-an-instance-of-the-applications-dbcontext)

## Defining a Model

EF Core builds an entity model at runtime using **conventions**, **annotation attributes**, and **Fluent API**, enabling class actions to translate automatically into database actions. **An entity class defines a table's structure, and its instance represents a row in the table.**

<p align="center">
<img src="img/three-conv.jpg" alt="three-conv.jpg" width="800px"/>
</p>

In summary, EF Core offers three configuration approaches:  
1. **By Convention**: Automatically configures features based on property types and names; simple but limited.  
2. **Data Annotations**: Adds .NET attributes to classes/properties for additional configuration and validation.  
3. **Fluent API**: Overrides `OnModelCreating` to define configurations using commands; the most comprehensive option, enabling features exclusive to this method.

<p align="center">
<img src="img/three-conv-ex.jpg" alt="three-conv-ex.jpg" width="800px"/>
</p>


### EF Core conventions

The code follows these conventions:  
- A table's name matches the corresponding `DbSet<T>` property in `DbContext` (e.g., `Products`).  
- EF Core **expects one primary-key property**. The property is called `Id` or `<class name>id` (such as `ProductId`).If it's an integer or `Guid`, it is treated as an IDENTITY column (auto-generated values on insertion).
- Column names match the property names in the entity class (e.g., `Title`).  
- The .NET `string` type maps to `nvarchar` in the database. The .NET `int` type maps to `int` in the database.  

<p align="center">
<img src="img/convention1.jpg" alt="convention1.jpg" width="400px"/>
</p>


<p align="center">
<img src="img/entityconvention.png" alt="entityconvention.png" width="600px"/>
</p>

### EF Core annotation attributes

Conventions often aren't enough to completely map the classes to the database objects. **Data annotation attributes** are used to provide additional configuration and validation. They are placed on the entity class or its properties.

For example, in the database, the maximum length of a product name is 40, and the value cannot be null. In a Product class, we could apply attributes to specify this, as shown in the following code:


```csharp
[Required]
[StringLength(40)]
public string ProductName { get; set; }
```

In the Category table, the Description column can be longer than the 8,000 characters that can be stored in a `nvarchar` variable, so it needs to map to `ntext` instead, as shown in the following code:

```csharp
[Column(TypeName = "ntext")]
public string Description { get; set; }
```

<p align="center">
<img src="img/annotation-attributes.jpg" alt="annotation-attributes.jpg" width="600px"/>
</p>

### EF Core Fluent API

The last way that the model can be defined is by using the Fluent API. This API can be used instead of attributes, as well as being used in addition to them. For example, let's look at the following two attributes in a Product class, as shown in the following code:

```csharp
[Required]
[StringLength(40)]
public string ProductName { get; set; }
```

The attributes could be removed from the class to keep it simpler, and replaced with
an equivalent Fluent API statement in the OnModelCreating method of a database
context class, as shown in the following code:

```csharp
modelBuilder.Entity<Product>()
.Property(product => product.ProductName)
.IsRequired()
.HasMaxLength(40);
```

### **Configuring nonrelational properties**

#### Setting database column type, size, and nullability

<p align="center">
<img src="img/setnulltytpesize.jpg" alt="setnulltytpesize.jpg" width="700px"/>
</p>


#### The different ways of configuring the primary key

You’ve already seen the By Convention approach of setting up the primary key of an entity : `<class name>Id` or `Id`.

But You need to configure the primary key explicitly in two situations:
- When the key name doesn’t fit the By Convention naming rules; not `Id` or `<class name>Id`
- When the primary key is made up of more than one property/column, called a **composite key**

##### Configuring a primary key via Data Annotations

The `[Key]` attribute allows you to designate one property as the primary key in a class. Use this annotation when you don’t use the By Convention primary key name, as shown in the following listing.

```csharp
private class SomeEntity
{
    [Key]
    public int NonStandardKeyName { get; set; }
}
```

##### Configuring a primary key via the Fluent API

You can also configure a primary key via the Fluent API’s `HasKey` method, which is useful for primary keys that don’t fit the By Convention patterns. 

```csharp
protected override void
OnModelCreating(ModelBuilder modelBuilder)
{
    // single primary key
    modelBuilder.Entity<SomeEntity>().HasKey(x => x.NonStandardKeyName);
    // composite primary key
    modelBuilder.Entity<BookAuthor>().HasKey(x => new {x.BookId, x.AuthorId});
}
```

#### Adding indexes to database columns

Relational databases have a feature called an index, which provides quicker searching and sorting of rows based on the column, or columns, in the index. In addition, an index may have a constraint, which ensures that each entry in the index is unique.

To add an index to a column, you can use the `Index` attribute, as shown in the following code:

```csharp
[Index]
public string ProductName { get; set; }
```

To add an index to a column with a unique constraint, you can use the `Index` attribute with the `IsUnique` property set to `true`, as shown in the following code:

```csharp
[Index(IsUnique = true)]
public string ProductName { get; set; }
```

To add an index to a column using the Fluent API, you can use the `HasIndex` method, as shown in the following code:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Product>()
        .HasIndex(p => p.ProductName);
    // multiple columns
    modelBuilder.Entity<Product>()
        .HasIndex(p => new { p.ProductName, p.ProductId });
}
```

#### Excluding properties and classes from the database

Sometimes, you might want to exclude a property or an entire class from the database. 

##### Excluding a class or property via Data Annotations

EF Core will exclude a property or a class that has a `[NotMapped]` data attribute applied to it.

```csharp
public class MyEntityClass
{
    public int MyEntityClassId { get; set; }
    public string NormalProp{ get; set; }
    [NotMapped]
    public string LocalString { get; set; }
    public ExcludeClass LocalClass { get; set; }
}
[NotMapped]
public class ExcludeClass
{
    public int LocalInt { get; set; }
}
```

##### Excluding a class or property via Fluent API

To exclude a property or class from the database using the Fluent API, you can use the `Ignore` method. The following code shows how to exclude a property from the database:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<MyEntityClass>()
        .Ignore(e => e.LocalString);
}
```


### **Configuring relationships**

#### Defining some relationship terms

<p align="center">
<img src="img/terms.jpg" alt="terms.jpg" width="800px"/>
</p>

Here are concise descriptions of key EF Core terms:  
- **Principal Key**: A unique key in an entity, either a primary or alternate key.  
- **Principal Entity**: The entity containing the principal key that dependent entities reference via foreign keys.  
- **Dependent Entity**: The entity with foreign keys referencing a principal entity.  
- **Navigational Property**: A property linking entity classes, containing either a single entity or a collection.  
- **Foreign Key**: A property holding the value of a principal key (nullable in optional relationships).  
- **Required Relationship**: A relationship where the foreign key is non-nullable, requiring the principal entity.  
- **Optional Relationship**: A relationship with a nullable foreign key, allowing the principal entity to be absent.

##### What navigational properties do you need?

Configuring relationships should align with your project's business needs. While navigational properties can be added at both ends, not all are useful. Only include those that make sense from a business or design perspective.

For example, in a Book App:  
- A `Book` class with an `ICollection<Review>` navigational property is essential to calculate average review scores.  
- A `Review` class doesn't need a `Book` navigational property if the relationship isn't used in the app.  

This selective approach simplifies entity classes and prevents misuse by junior developers, focusing only on meaningful relationships.

#### Configuring relationships By Convention

The By Convention approach is a real time-saver when it comes to configuring relationships. EF Core automatically detects relationships based on property types and names, as shown in the following code:

<p align="center">
<img src="img/allr.jpg" alt="allr.jpg" width="700px"/>
</p>


##### One-to-Many Relationship  

```csharp
public class Book
{
    public int BookId { get; set; }
    public required string Title { get; set; }
    public virtual ICollection<Review>? Reviews { get; set; } // 1-M Navigation property
}

public class Review
{
    public int ReviewId { get; set; }
    public required string ReviewText { get; set; }
    public int BookId { get; set; } // Foreign key
    public required Book Book { get; set; }  // Navigation property
}
```

<p align="center">
<img src="img/otm1.jpg" alt="otm1.jpg" width="800px"/>
</p>

In this case, EF Core detects that `Book` has many `Reviews`, while each `Review` belongs to one `Book`.  

---

##### One-to-One Relationship  

```csharp
public class Book
{
    public int BookId { get; set; }
    public string Title { get; set; }
    public virtual PriceOffer? PriceOffer { get; set; } // 1-0/1 Navigation property
}

public class PriceOffer
{
    public int PriceOfferId { get; set; }
    public decimal NewPrice { get; set; }
    public int BookId { get; set; } // Foreign key
    public required Book Book { get; set; }  // Navigation property
}
```

<p align="center">
<img src="img/img/oto1.jpg" alt="img/oto1.jpg" width="700px"/>
</p>

Here, each `Book` has one `PriceOffer`, and each `PriceOffer` belongs to one `Book`.  



##### Many-to-Many Relationship  

```csharp
public class Book
{
    public int BookId { get; set; }
    public required string Title { get; set; }
    public virtual ICollection<Review>? Reviews { get; set; } // 1-M Navigation property
    public virtual PriceOffer? PriceOffer { get; set; } // 1-0/1 Navigation property
    public virtual required ICollection<Author> Authors { get; set; } = new List<Author>(); // Navigation property
    public ICollection<Tag>? Tags { get; set; } = new List<Tag>(); // Navigation property
}

public class Author
{
    public int AuthorId { get; set; }
    public required string Name { get; set; }
    public ICollection<Book>? Books { get; set; } = new List<Book>(); // Navigation property
}

public class Tag
{
    public int TagId { get; set; }
    public required string Name { get; set; }
    public ICollection<Book>? Books { get; set; } = new List<Book>(); // Navigation property
}

```

For many-to-many relationships, EF Core automatically creates junction tables (`BookAuthors` and `BookTags`) to represent the links.  

<p align="center">
<img src="img/mtm0.jpg" alt="img/mtm0.jpg" width="700px"/>
</p>

<p align="center">
<img src="img/mtm1.jpg" alt="img/mtm1.jpg" width="700px"/>
</p>


> Foreign keys: What happens if you leave them out?

If you don't include a foreign key property in the dependent entity, EF Core will create a shadow property for you. This property will be named `<navigation property name><principal key property name>`. For example, if you have a `Book` entity with a `Reviews` navigation property, and the primary key of the `Book` entity is `BookId`, EF Core will create a shadow property named `BookId` in the `Review` entity.

> When does By Convention configuration not work?

If you’re going to use the By Convention configuration approach, you need to know when it’s not going to work so that you can use other means to configure your relationship. Here’s my list of scenarios that won’t work, with the most common listed first:
- You have composite foreign keys
- You want to create a one-to-one relationship without navigational links going both ways 
- You want to override the default delete-behavior setting
- You have two navigational properties going to the same class
- You want to define a specific database constraint

#### Creating an instance of the application’s DbContext

```csharp
public class RepositoryContext : DbContext
{

    public RepositoryContext(DbContextOptions options) : base(options) { }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<Review> Reviews { get; set; }
    
    public DbSet<PriceOffer> PriceOffers { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Tag> Tags { get; set; }

}
```

<p align="center">
<img src="img/dbcon.jpg" alt="dbcon.jpg" width="600px"/>
</p>
