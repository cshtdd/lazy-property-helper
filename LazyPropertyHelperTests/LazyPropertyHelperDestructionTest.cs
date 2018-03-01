using System;
using FluentAssertions;
using Xunit;

namespace LazyPropertyHelperTests
{
  public class LazyPropertyHelperDestructionTest
  {
    private readonly int beforeExpensiveInstancesDestroyedCount; 
    private int ExpensiveInstancesDestroyed => ExpensiveObject.DestroyedCount - beforeExpensiveInstancesDestroyedCount;
    
    private readonly int beforeServiceInstancesDestroyedCount;
    private int ServiceInstancesDestroyed => SampleService.DestroyedCount - beforeServiceInstancesDestroyedCount;  
    
    public LazyPropertyHelperDestructionTest()
    {
      CleanupResources();
      
      beforeExpensiveInstancesDestroyedCount = ExpensiveObject.DestroyedCount;
      beforeServiceInstancesDestroyedCount = SampleService.DestroyedCount;
    }
    
    private void TestBody(Action callback)
    {
      callback();
      
      CleanupResources();
    }

    private static void CleanupResources()
    {
      GC.Collect();
      GC.WaitForPendingFinalizers();
    }
    
    [Fact]
    public void LazyPropertyIsNotDestroyedIfNotCreated()
    {
      TestBody(() =>
      {
        new SampleService();
      });

      ServiceInstancesDestroyed.Should().Be(1);
      ExpensiveInstancesDestroyed.Should().Be(0);
    }
    
    [Fact]
    public void LazyPropertyIsDestroyedIfCreated()
    {
      TestBody(() =>
      {
        new SampleService().DoWork(1);
      });
     
      ServiceInstancesDestroyed.Should().Be(1);
      ExpensiveInstancesDestroyed.Should().Be(1);
    }
    
    [Fact]
    public void LazyPropertyIsDestroyIfRead()
    {
      TestBody(() =>
      {
        var load = new SampleService().ExpensiveLoad;
      });

      ServiceInstancesDestroyed.Should().Be(1);
      ExpensiveInstancesDestroyed.Should().Be(1);
    }
    
    [Fact]
    public void LazyPropertyIsDestroyedOnlyOnceWhenRead()
    {
      TestBody(() =>
      {
        var service = new SampleService();

        var load1 = service.ExpensiveLoad;
        var load2 = service.ExpensiveLoad;
        var load3 = service.ExpensiveLoad;
      });
      
      ServiceInstancesDestroyed.Should().Be(1);
      ExpensiveInstancesDestroyed.Should().Be(1);
    }
    
    [Fact]
    public void LazyPropertyIsDestroyedOnlyOnce()
    {
      TestBody(() =>
      {
        var service = new SampleService();
      
        service.DoWork(1);
        service.DoWork(1);
        service.DoWork(1);
      });
      
      ServiceInstancesDestroyed.Should().Be(1);
      ExpensiveInstancesDestroyed.Should().Be(1);
    }
    
    [Fact]
    public void LazyPropertyIsDestroyedOncePerInstance()
    {
      TestBody(() =>
      {
        var service1 = new SampleService();
        var service2 = new SampleService();
      
        service1.DoWork(1);
        service1.DoWork(1);
        service1.DoWork(1);

        service2.DoWork(1);
        service2.DoWork(1);
        service2.DoWork(1);
      });
     
      ServiceInstancesDestroyed.Should().Be(2);
      ExpensiveInstancesDestroyed.Should().Be(2);
    }
  }
}