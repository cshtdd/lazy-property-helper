using System;

namespace LazyPropertyHelperTests
{
  public class SampleService
  {
    private readonly Func<ExpensiveObject> _expensiveLoad = LazyPropertyHelper.LazyPropertyHelper.Create(() => new ExpensiveObject());
    public ExpensiveObject ExpensiveLoad => _expensiveLoad();
      
    public void DoWork(int n)
    { 
      ExpensiveLoad.Move(n*10 - 100, n*10 + 100);
    }
  }
}