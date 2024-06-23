using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using static EquationSolver.EquationSolver;

namespace EquationSolver.Benchmark
{
    //リリースビルドで実行すること
    internal class Program
    {
        static void Main(string[] args)
        {
            var bench = BenchmarkRunner.Run<EqSolverBenchmark>();
        }
    }

    [HtmlExporter]
    //[MemoryDiagnoser]
    //[ShortRunJob]
    [RPlotExporter]
    [MinColumn, MaxColumn]
    public class EqSolverBenchmark
    {
        public readonly Random r = new();

        /// <summary>
        /// 乱数生成
        /// </summary>
        /// <returns></returns>
        [Benchmark]
        public double GenRandom()
        {
            return r.Next(-1000, 999) + r.NextDouble();
        }


        [Benchmark]
        public void Create3()
        {
            var eq = CreateEquation([GenRandom(), GenRandom(), GenRandom()]);
        }

        [Benchmark]
        public void Create4()
        {
            var eq = CreateEquation([GenRandom(), GenRandom(), GenRandom(), GenRandom()]);
        }

        [Benchmark]
        public void Solve3()
        {
            var x = Equat3_Formula([GenRandom(), GenRandom(), GenRandom(), GenRandom()]);
        }

        [Benchmark]
        public void Solve4()
        {
            var x = Equat4_Formula([GenRandom(), GenRandom(), GenRandom(), GenRandom(), GenRandom()]);
        }
    }
}
