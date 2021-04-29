﻿using BenchmarkDotNet.Attributes;
using JM.LinqFaster;
using LinqFasterer;
using Nessos.LinqOptimizer.CSharp;
using Nessos.Streams.CSharp;
using NetFabric.Hyperlinq;
using StructLinq;

namespace LinqBenchmarks.Array.ValueType
{
    public class ArrayValueTypeSelect: ValueTypeArrayBenchmarkBase
    {
        [Benchmark(Baseline = true)]
        public FatValueType ForLoop()
        {
            var sum = default(FatValueType);
            var array = source;
            for (var index = 0; index < array.Length; index++)
            {
                ref readonly var item = ref array[index];
                sum += item * 3;
            }
            return sum;
        }

        [Benchmark]
        public FatValueType ForeachLoop()
        {
            var sum = default(FatValueType);
            foreach (var item in source)
                sum += item * 3;
            return sum;
        }

        [Benchmark]
        public FatValueType Linq()
        {
            var items = System.Linq.Enumerable.Select(source, item => item * 3);
            var sum = default(FatValueType);
            foreach (var item in items)
                sum += item;
            return sum;
        }

        [Benchmark]
        public FatValueType LinqFaster()
        {
            var items = source.SelectF(item => item * 3);
            var sum = default(FatValueType);
            foreach (var item in items)
                sum += item;
            return sum;
        }

        [Benchmark]
        public FatValueType LinqFasterer()
        {
            var items = EnumerableF.SelectF(source, item => item * 3);
            var sum = default(FatValueType);
            foreach (var item in items)
                sum += item;
            return sum;
        }

        [Benchmark]
        public FatValueType LinqAF()
        {
            var items = global::LinqAF.ArrayExtensionMethods.Select(source, item => item * 3);
            var sum = default(FatValueType);
            foreach (var item in items)
                sum += item;
            return sum;
        }

        [Benchmark]
        public FatValueType LinqOptimizer()
        {
            var items = source
                .AsQueryExpr()
                .Select(item => item * 3)
                .Run();
            var sum = default(FatValueType);
            foreach (var item in items)
                sum += item;
            return sum;
        }

        [Benchmark]
        public FatValueType Streams()
        {
            var items = source
                .AsStream()
                .Select(item => item * 3)
                .ToEnumerable();
            var sum = default(FatValueType);
            foreach (var item in items)
                sum += item;
            return sum;
        }

        [Benchmark]
        public FatValueType StructLinq()
        {
            var items = source
                .ToRefStructEnumerable()
                .Select((in FatValueType item) => item * 3);
            var sum = default(FatValueType);
            foreach (var item in items)
                sum += item;
            return sum;
        }

        [Benchmark]
        public FatValueType StructLinq_ValueDelegate()
        {
            var selector = new TripleOfFatValueType();
            var items = source
                .ToRefStructEnumerable()
                .Select(ref selector, x => x, x => x);
            var sum = default(FatValueType);
            foreach (var item in items)
                sum += item;
            return sum;
        }

        [Benchmark]
        public FatValueType Hyperlinq()
        {
            var items = source.AsValueEnumerable()
                .Select(item => item * 3);
            var sum = default(FatValueType);
            foreach (var item in items)
                sum += item;
            return sum;
        }

        [Benchmark]
        public FatValueType Hyperlinq_ValueDelegate()
        {
            var items = source.AsValueEnumerable()
                .Select<FatValueType, TripleOfFatValueType>();
            var sum = default(FatValueType);
            foreach (var item in items)
                sum += item;
            return sum;
        }
    }
}
