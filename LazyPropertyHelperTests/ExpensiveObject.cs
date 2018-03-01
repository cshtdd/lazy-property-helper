namespace LazyPropertyHelperTests
{
  public class ExpensiveObject
  {
    private static volatile int _createdCount;
    private static volatile int _destroyedCount;

    public static int CreatedCount => _createdCount;
    public static int DestroyedCount => _destroyedCount;

    public ExpensiveObject() => _createdCount++;

    public void Move(int source, int dest){}

    ~ExpensiveObject() => _destroyedCount++;
  }
}