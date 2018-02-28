namespace LazyPropertyHelperTests
{
  public class SampleService
  {
    private ExpensiveObject _expensiveLoad;
    public ExpensiveObject ExpensiveLoad {
      get
      {
        if (_expensiveLoad == null)
        {
          _expensiveLoad = new ExpensiveObject();
        }

        return _expensiveLoad;
      }
    }
      
    public void DoWork(int n)
    { 
      ExpensiveLoad.Move(n*10 - 100, n*10 + 100);
    }
  }
}