# Lazy Property Helper Nuget  

Nuget package to implement the [Lazy Initialization pattern](https://en.wikipedia.org/wiki/Lazy_initialization) in a [thread-safe](https://en.wikipedia.org/wiki/Thread_safety) and efficient manner.  

## Usage  

```csharp
public class SampleService
{ 
  private readonly Func<ExpensiveObject> _expensiveLoad = LazyProperty.Create(() => new ExpensiveObject());
  public ExpensiveObject ExpensiveLoad => _expensiveLoad();

  public void DoWork(int n) => ExpensiveLoad.Move(n*10 - 100, n*10 + 100);
}
```

`SampleService` depends on `ExpensiveObject` to do some work.  
The creation of a `ExpensiveObject` is a computational intensive task.  
The `SampleService`'s `ExpensiveLoad` property will return a `new ExpensiveObject` when read the first time. Moreover, It will cache this result in a thread-safe and efficient manner.  

## Installation  

Follow the steps from the [Nuget Package Url](https://www.nuget.org/packages/LazyPropertyHelper/).  

## FAQ  

### Can this be used in dotnet core?  

Yes. [Here's a sample project that uses](https://github.com/camilin87/ThreadSafeEfficientLazyProperty).  

### Who needs this?  

Developers that have written code similar to this one  

```csharp
public class MyServiceNaive
{
  private ExpensiveObject expensiveLoad;

  public ExpensiveObject ExpensiveLoad
  {
    get
    {
      if (expensiveLoad == null)
      {
        expensiveLoad = new ExpensiveObject();
      }

      return expensiveLoad;
    }
  }

  //more code
}
```

The `ExpensiveLoad` property is not thread-safe. That code can be subject to weird race conditions when more than one `ExpensiveObject` are created.  

### Why is this better than `lock`?  

The following code is thread-safe. However, it is very inefficient because it needs to acquire a lock whenever `ExpensiveLoad` is read. The `LazyProperty` from this nuget solves this problem.  

```csharp
public class MyLockedService
{
  private object criticalSection = new object();
  
  private ExpensiveObject expensiveLoad;

  public ExpensiveObject ExpensiveLoad
  {
    get
    {
      lock (criticalSection)
      {
        if (expensiveLoad == null)
        {
          expensiveLoad = new ExpensiveObject();
        }
      }

      return expensiveLoad;
    }
  }
}
```

### How does it work?  

`LazyPropertyHelper` leverages the advantages of lambdas and functional programming to cache the result of an expensive computation. The computation is executed only once in a thread-safe context. Subsequent reads to the result don't require a lock.  
[Here's the code](https://github.com/camilin87/lazy-property-helper/blob/master/LazyPropertyHelper/LazyProperty.cs) where all of this takes place. The important piece is the `CalculateAndCacheExpensiveComputation` method that replaces the `_expensiveComputationReader` with a lambda that always return the cached value stored in the `_cachedResult` field.  

### How do you know it works?  

Testing concurrent code is hard _[citation needed]_. We didn't even try to simulate concurrency problems in unit tests.  
However, we wrote dozens of unit tests to make sure that the `LazyPropertyHelper` behaves as expected.  

[Take a look at the tests](https://github.com/camilin87/lazy-property-helper/tree/master/LazyPropertyHelperTests)  

### How did you come up with this?  

I did not invented this pattern. I was inspired by a similar implementation from [this great book about functional programming in Java](https://www.tddapps.com/2018/02/27/functional-programming-in-java/).  

### Isn't this the same as [`Lazy<T>`](https://msdn.microsoft.com/en-us/library/dd642331(v=vs.110).aspx)?  

[`Lazy<T>`](https://msdn.microsoft.com/en-us/library/dd642331(v=vs.110).aspx) solves many of the problems around lazy initialization. You could even argue how much richer it is.  
However, `Lazy<T>` adopts an all or nothing approach to locking. It can be configured to never lock or to always lock. These extremes can be highly inefficient or dangerous in certain situations.  
`LazyPropertyHelper` takes a simpler approach with no compromises on thread-safety and performance.  

### Will it create memory leaks?  

No. There are [several unit tests to cover the destruction of the cached objects](https://github.com/camilin87/lazy-property-helper/blob/master/LazyPropertyHelperTests/DestructionTest.cs).  
The result or the expensive computation can be explicitly disposed from the `Dispose` method of the class with the lazy property.  
