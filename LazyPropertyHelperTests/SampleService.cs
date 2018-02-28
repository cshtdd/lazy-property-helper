using System;
using System.Collections.Generic;

namespace LazyPropertyHelperTests
{
  public class SampleService
  {
    public static volatile List<string> Operations = new List<string>();

    public readonly string Id = Guid.NewGuid().ToString("N");

    public ExpensiveObject ExpensiveLoad => new ExpensiveObject();
      
    public void DoWork(int n)
    {
      Operations.Add($"{nameof(SampleService)}->DoWork; status:Start; {nameof(n)}:{n};");
        
      ExpensiveLoad.Move(n*10 - 100, n*10 + 100);
        
      Operations.Add($"{nameof(SampleService)}->DoWork; status:End; {nameof(n)}:{n};");
    }
  }
}