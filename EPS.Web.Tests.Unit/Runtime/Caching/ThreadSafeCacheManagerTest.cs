using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EPS.Runtime.Caching;
using FakeItEasy;
using Xunit;

namespace EPS.Runtime.Caching.Tests.Unit
{
    public interface IThreadSafeCacheManagerTest
    {
        void DoSomething();
    }
    public class ThreadSafeCacheManagerTest : IThreadSafeCacheManagerTest
    {
        [Fact]
        public void DoSomething()
        {
            var cacheManager = ThreadSafeCacheManager<object>.Construct("myObjectCache", TimeSpan.MaxValue);            
            throw new NotImplementedException();
        }
    }
}
