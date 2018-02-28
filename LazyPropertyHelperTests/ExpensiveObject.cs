namespace LazyPropertyHelperTests
{
  public class ExpensiveObject
  {
    private static volatile int _instancesCount;

    public static int InstancesCount => _instancesCount;

    public ExpensiveObject() => _instancesCount++;

    public void Move(int source, int dest){}
  }
}