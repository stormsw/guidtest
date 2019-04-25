using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Xunit.Performance;
using Microsoft.Xunit.Performance.Api;
using Xunit;

namespace guidtest
{
    public class UnitTest1
    {
        public static void Main(string[] args)
        {
            using (XunitPerformanceHarness p = new XunitPerformanceHarness(args))
            {
                string entryAssemblyPath = Assembly.GetEntryAssembly().Location;
                p.RunBenchmarks(entryAssemblyPath);
            }
        }

        [MeasureGCCounts]
        [Benchmark(InnerIterationCount = 10000)]
        public static void TestBoxingInputs()
        {
            var g = Guid.NewGuid();
            foreach (BenchmarkIteration iter in Benchmark.Iterations)
            {
                using (iter.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        generateKeyBoxingGuid(g);
                    }
                }
            }
        }

        [MeasureGCCounts]
        [Benchmark(InnerIterationCount = 10000)]
        public static void TestNotBoxingInputs()
        {
            var g = Guid.NewGuid();
            foreach (BenchmarkIteration iter in Benchmark.Iterations)
            {
                using (iter.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        generateKeyNonBoxingInterpolation(g);
                    }
                }
            }
        }

        [MeasureGCCounts]
        [Benchmark(InnerIterationCount = 10000)]
        public static void TestStringInputs()
        {
            var g = Guid.NewGuid().ToString();
            foreach (BenchmarkIteration iter in Benchmark.Iterations)
            {
                using (iter.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        generateKeyString(g);
                    }
                }
            }
        }

        [MeasureGCCounts]
        [Benchmark(InnerIterationCount = 10000)]
        public static void TestStringInterpolation()
        {
            var g = Guid.NewGuid().ToString();
            foreach (BenchmarkIteration iter in Benchmark.Iterations)
            {
                using (iter.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        generateKeyStringInterpolation(g);
                    }
                }
            }
        }
        // NoInlining prevents aggressive optimizations that
        // could render the benchmark meaningless
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string generateKeyBoxingGuid(Guid g)
        {
            return $"dal-key-{g}";
        }
        // NoInlining prevents aggressive optimizations that
        // could render the benchmark meaningless
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string generateKeyNonBoxingInterpolation(Guid g)
        {
            return "dal-key-{g.ToString()}";
        }
        // NoInlining prevents aggressive optimizations that
        // could render the benchmark meaningless
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string generateKeyString(string g)
        {
            return "dal-key-" + g;
        }
        // NoInlining prevents aggressive optimizations that
        // could render the benchmark meaningless
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string generateKeyStringInterpolation(string g)
        {
            return $"dal-key-{g}";
        }
    }
}
