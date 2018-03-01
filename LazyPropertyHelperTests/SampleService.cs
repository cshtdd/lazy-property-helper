using System;

namespace LazyPropertyHelperTests
{
  public class SampleService
  {
    private static volatile int _createdCount;
    private static volatile int _destroyedCount;

    public static int CreatedCount => _createdCount;
    public static int DestroyedCount => _destroyedCount;
    
    private readonly Func<ExpensiveObject> _expensiveLoad = LazyPropertyHelper.LazyPropertyHelper.Create(() => new ExpensiveObject());
    public ExpensiveObject ExpensiveLoad => _expensiveLoad();

    public SampleService() => _createdCount++;

    public void DoWork(int n) => ExpensiveLoad.Move(n*10 - 100, n*10 + 100);

    ~SampleService() => _destroyedCount++;
  }
}