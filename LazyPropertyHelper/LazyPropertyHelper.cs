using System;

namespace LazyPropertyHelper
{
    public static class LazyPropertyHelper
    {
        public static Func<T> Create<T>(Func<T> expensiveComputation) => new LazyPropertyHelperInternal<T>(expensiveComputation).Read;

        private class LazyPropertyHelperInternal<T>
        {
            private readonly object _criticalSection = new object();
            private Func<T> _expensiveComputationReader;

            private readonly Func<T> _expensiveComputation;
    
            public T Read() => ExpensiveComputation;
            private T ExpensiveComputation => _expensiveComputationReader();

            public LazyPropertyHelperInternal(Func<T> expensiveComputation)
            {
                _expensiveComputation = expensiveComputation;
                _expensiveComputationReader = CalculateAndCacheExpensiveComputation;
            }     

            private class CacheHolder
            {
                private readonly T _cachedResult;

                public CacheHolder(Func<T> expensiveComputation) => _cachedResult = expensiveComputation();

                public T Read() => _cachedResult;
            }
      
            private T CalculateAndCacheExpensiveComputation()
            {
                lock (_criticalSection)
                {
                    if (_expensiveComputationReader == CalculateAndCacheExpensiveComputation)
                    {
                        _expensiveComputationReader = new CacheHolder(_expensiveComputation).Read;
                    }
                }
      
                return _expensiveComputationReader();
            }
        }
    }
}
