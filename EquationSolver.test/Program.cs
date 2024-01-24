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

            var a = CreateEquation([1, 2, 2]);
            Console.WriteLine(Equation2String(a));
            Console.WriteLine(ComplexArray2String(Equat3_Formula(a, true)));

            //var b = CreateEquation([1, 2, 3, 4]);

            var b = CreateEquation([1, 2, 3, 4]);
            foreach (var i in b)
            {
                Console.WriteLine(i);
            }
            Console.WriteLine(Equation2String(b));
            Console.WriteLine(ComplexArray2String(Equat4_Formula(b)));


            //return;


            int m = 10;
            for (int i = 1; i <= m; i++)
                for (int j = 0; j <= m; j++)
                    for (int k = 0; k <= m; k++)
                    {
                        Console.Write($"{i},{j},{k} -> ");
                        var equa = CreateEquation([i, j, k]);
                        Console.Write(Equation2String(equa));
                        Console.Write(" -> ");
                        Console.Write(ComplexArray2StringSimple(Equat3_Formula(equa), 10));
                        Console.WriteLine();
                        /*
                        Console.WriteLine();
                        Console.Write($"{i},{j},{k} -> ");
                        var equa = CreateEquation([i, j, k]);
                        Console.Write(Equation2String(equa));
                        Console.Write(" -> ");
                        Console.WriteLine();
                        Console.Write(ComplexArray2StringSimple(Equat3_Formula(equa, true), 10));
                        Console.WriteLine();
                        */
                    }
            /*
            for (int i = 1; i <= m; i++)
                for (int j = 0; j <= m; j++)
                    for (int k = 0; k <= m; k++)
                        for (int l = 0; l <= m; l++)
                        {
                            Console.Write($"{i},{j},{k},{l} -> ");
                            var equa = CreateEquation([i, j, k, l]);
                            Console.Write(Equation2String(equa));
                            Console.Write(" -> ");
                            Console.Write(ComplexArray2StringSimple(Equat4_Formula(equa), 10));
                            Console.WriteLine();
                            /*
                            Console.WriteLine();
                            Console.Write($"{i},{j},{k} -> ");
                            var equa = CreateEquation([i, j, k]);
                            Console.Write(Equation2String(equa));
                            Console.Write(" -> ");
                            Console.WriteLine();
                            Console.Write(ComplexArray2StringSimple(Equat3_Formula(equa, true), 10));
                            Console.WriteLine();
                            */
            //}

        }
    }
}
