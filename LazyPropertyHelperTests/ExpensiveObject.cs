using System;
using System.Collections.Generic;

namespace LazyPropertyHelperTests
{
  public class ExpensiveObject
  {
    public static volatile int InstancesCount = 0;
    public static volatile List<string> Movements = new List<string>();

    public readonly string Id = Guid.NewGuid().ToString("N");
 
    public ExpensiveObject() => InstancesCount++;

    public void Move(int source, int dest)
    {
      Movements.Add($"{nameof(ExpensiveObject)}->Move; {nameof(source)}:{source}; {nameof(dest)}:{dest};");
    }
  }
}