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

      ExpensiveObject.InstancesCount.Should().BeGreaterThan(beforeInstancesCount);
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
  }
}