namespace LazyPropertyHelperTests
{
  public class ExpensiveObject
  {
    private static volatile int _instancesCount;
    private static volatile int _destroyedCount;

    public static int InstancesCount => _instancesCount;
    public static int DestroyedCount => _destroyedCount;

    public ExpensiveObject() => _instancesCount++;

    public void Move(int source, int dest){}

    ~ExpensiveObject() => _destroyedCount++;
  }
}