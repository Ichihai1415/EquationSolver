using static EquationSolver.EquationSolver;

namespace EquationSolver.test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");


            //Console.WriteLine(string.Join(",", CreateEquation([1, 2]).Select(x => x.ToString())));
            //Console.WriteLine(string.Join(",", CreateEquation([1, 2, 3]).Select(x => x.ToString())));
            //Console.WriteLine(string.Join(",", CreateEquation([1, 2, 3, 4]).Select(x => x.ToString())));
            //Console.WriteLine(string.Join(",", CreateEquation([1, 2, 3, 4, 5]).Select(x => x.ToString())));

            //Console.WriteLine(string.Join(",", CreateEquation([-5, 8, -5, 3, 4]).Select(x => x.ToString())));

            //Console.WriteLine(Equation2String(CreateEquation([-5, 8, -5, 3, 4])));

            //Console.WriteLine(ComplexArray2String(Equat2_Formula(1, 2, -15)));

            //Console.WriteLine(Equation2String([1]));
            //Console.WriteLine(Equation2String([2]));
            //Console.WriteLine(Equation2String([1, 2]));
            //Console.WriteLine(Equation2String([1, 2, 3]));
            //Console.WriteLine(Equation2String([1, 2, 3, 4]));
            //Console.WriteLine(Equation2String([1, 2, 3, 4, 5]));
            //Console.WriteLine(Equation2String([1, 2, 3, 4, 5, 6]));
            //Console.WriteLine(Equation2String([1, 1, 1, 1, 1, 1]));

            //Console.WriteLine(Equation2String());
            //Console.WriteLine(ComplexArray2String());
            /*
            var a = CreateEquation([1, 2, 3]);
            Console.WriteLine(Equation2String(a));
            Console.WriteLine(ComplexArray2String(Equat3_Formula(a)));
            */
            //var a = CreateEquation([1, 2, 3, 4]);
            var a = CreateEquation([0, 0, 0, 1]);
            foreach (var i in a)
            {
                Console.WriteLine(i);
            }
            Console.WriteLine(Equation2String(a));
            Console.WriteLine(ComplexArray2String(Equat4_Formula(a)));
        }
    }
}
