using FluentAssertions;
using Xunit;

namespace LazyPropertyHelperTests
{
  public class LazyPropertyHelperTest
  {
    [Fact]
    public void LazyPropertyIsNotInitializedOnConstruction()
    {
      var beforeInstancesCount = ExpensiveObject.InstancesCount;

      new SampleService();

      ExpensiveObject.InstancesCount.Should().Be(beforeInstancesCount);
    }
    
    [Fact]
    public void LazyPropertyIsInitializedWhenWorkNeedsToBeDone()
    {
      var beforeInstancesCount = ExpensiveObject.InstancesCount;

      new SampleService().DoWork(1);

      ExpensiveObject.InstancesCount.Should().Be(beforeInstancesCount + 1);
    }
    
    [Fact]
    public void LazyPropertyIsInitializedWhenRead()
    {
      var beforeInstancesCount = ExpensiveObject.InstancesCount;

      var load = new SampleService().ExpensiveLoad;

      ExpensiveObject.InstancesCount.Should().Be(beforeInstancesCount + 1);
    }
    
    [Fact]
    public void LazyPropertyIsInitializedWhenReadOnlyOnce()
    {
      var beforeInstancesCount = ExpensiveObject.InstancesCount;

      var service = new SampleService();

      var load1 = service.ExpensiveLoad;
      var load2 = service.ExpensiveLoad;
      var load3 = service.ExpensiveLoad;

      ExpensiveObject.InstancesCount.Should().Be(beforeInstancesCount + 1);
    }
    
    [Fact]
    public void LazyPropertyIsInitializedOnlyOnce()
    {
      var beforeInstancesCount = ExpensiveObject.InstancesCount;

      var service = new SampleService();
      
      service.DoWork(1);
      service.DoWork(1);
      service.DoWork(1);

      ExpensiveObject.InstancesCount.Should().Be(beforeInstancesCount + 1);
    }

    [Fact]
    public void LazyPropertyIsInitializedOncePerInstance()
    {
      var beforeInstancesCount = ExpensiveObject.InstancesCount;

      var service1 = new SampleService();
      var service2 = new SampleService();
      
      service1.DoWork(1);
      service1.DoWork(1);
      service1.DoWork(1);

      service2.DoWork(1);
      service2.DoWork(1);
      service2.DoWork(1);
      
      ExpensiveObject.InstancesCount.Should().Be(beforeInstancesCount + 2);
    }
    
  }
}