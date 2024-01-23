using static EquationSolver.EquationSolver;

namespace EquationSolver.test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");


            Console.WriteLine(string.Join(",", CreateEquation([1, 2]).Select(x => x.ToString())));
            Console.WriteLine(string.Join(",", CreateEquation([1, 2, 3]).Select(x => x.ToString())));
            Console.WriteLine(string.Join(",", CreateEquation([1, 2, 3, 4]).Select(x => x.ToString())));
            Console.WriteLine(string.Join(",", CreateEquation([1, 2, 3, 4, 5]).Select(x => x.ToString())));

            Console.WriteLine(string.Join(",", CreateEquation([-5, 8, -5, 3, 4]).Select(x => x.ToString())));

            Console.WriteLine(Equation2String(CreateEquation([-5, 8, -5, 3, 4])));

            //Console.WriteLine(ComplexArray2String(Equat2_Formula(1, 2, -15)));



        }
    }
}
