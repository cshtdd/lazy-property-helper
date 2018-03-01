using FluentAssertions;
using Xunit;

namespace LazyPropertyHelperTests
{
  public class LazyPropertyHelperTest
  {
    private readonly int beforeInstancesCount = ExpensiveObject.InstancesCount;
    private readonly int beforeDestroyedCount = ExpensiveObject.DestroyedCount;

    private int CreatedInstances => ExpensiveObject.InstancesCount - beforeInstancesCount;
    private int DestroyedInstances => ExpensiveObject.DestroyedCount - beforeDestroyedCount;
    
    [Fact]
    public void LazyPropertyIsNotInitializedOnConstruction()
    {
      new SampleService();

      CreatedInstances.Should().Be(0);
    }
    
    [Fact]
    public void LazyPropertyIsInitializedWhenWorkNeedsToBeDone()
    {
      new SampleService().DoWork(1);

      CreatedInstances.Should().Be(1);
    }
    
    [Fact]
    public void LazyPropertyIsInitializedWhenRead()
    {
      var load = new SampleService().ExpensiveLoad;

      CreatedInstances.Should().Be(1);
    }
    
    [Fact]
    public void LazyPropertyIsInitializedWhenReadOnlyOnce()
    {
      var service = new SampleService();

      var load1 = service.ExpensiveLoad;
      var load2 = service.ExpensiveLoad;
      var load3 = service.ExpensiveLoad;

      CreatedInstances.Should().Be(1);
    }
    
    [Fact]
    public void LazyPropertyIsInitializedOnlyOnce()
    {
      var service = new SampleService();
      
      service.DoWork(1);
      service.DoWork(1);
      service.DoWork(1);

      CreatedInstances.Should().Be(1);
    }

    [Fact]
    public void LazyPropertyIsInitializedOncePerInstance()
    {
      var service1 = new SampleService();
      var service2 = new SampleService();
      
      service1.DoWork(1);
      service1.DoWork(1);
      service1.DoWork(1);

      service2.DoWork(1);
      service2.DoWork(1);
      service2.DoWork(1);
      
      CreatedInstances.Should().Be(2);
    }
  }
}