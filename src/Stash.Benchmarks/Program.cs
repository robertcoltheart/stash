using System.Diagnostics;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using Stash.Tests.Performance;

namespace Stash.Benchmarks
{
    public class Program
    {
        public static void Main(string[] args)
        {
#if DEBUG
            var config = Debugger.IsAttached
                ? (IConfig) new DebugInProcessConfig()
                : new DebugBuildConfig();
#else
            var config = DefaultConfig.Instance;
#endif

            BenchmarkRunner.Run<SerializeDeserializeBenchmarks>(config);
        }
    }
}
