using FluentAssertions;
using Xunit;

namespace LazyPropertyHelperTests
{
  public class LazyPropertyHelperTest
  {
    [Fact]
    public void LazyPropertyIsInitializedOnlyWhenNeeded()
    {
      var beforeInstancesCount = ExpensiveObject.InstancesCount;

      new SampleService();

      ExpensiveObject.InstancesCount.Should().Be(beforeInstancesCount);
    }
  }
}