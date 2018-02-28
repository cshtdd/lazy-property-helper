namespace LazyPropertyHelperTests
{
  public class ExpensiveObject
  {
    public static volatile int InstancesCount;
 
    public ExpensiveObject() => InstancesCount++;

    public void Move(int source, int dest){}
  }
}