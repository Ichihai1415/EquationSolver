﻿using static EquationSolver.EquationSolver;

namespace EquationSolver.test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");


            //Console.WriteLine($"125,25:{GCD(125, 25)}");
            //Console.WriteLine($"25,125:{GCD(25, 125)}");
            //Console.WriteLine($"-125,25:{GCD(-125, 25)}");
            //Console.WriteLine($"-25,125:{GCD(-25, 125)}");
            //Console.WriteLine($"125,-25:{GCD(125, -25)}");
            //Console.WriteLine($"25,-125:{GCD(25, -125)}");
            //Console.WriteLine($"-125,-25:{GCD(-125, -25)}");
            //Console.WriteLine($"-25,-125:{GCD(-25, -125)}");


            //Console.WriteLine(Math.Pow(0.037037037037037035, 1d/3));
            //Console.WriteLine(Complex.Pow(0.037037037037037035, 1d/3));
            //Console.WriteLine(Complex.Pow(-0.037037037037037035, 1d/3));
            //Console.WriteLine(Complex.Pow(-1d/27, 1d/3));

            //Complex result = Complex.Pow(-0.037037037037037035, 1d / 3);
            //Console.WriteLine($"Result: {result}");
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
            Console.WriteLine();
            Console.WriteLine(ComplexArray2String(Equat3_Formula([1, 2, 3, 0], true)));

            //var b = CreateEquation([1, 2, 3, 4]);


            //var b = CreateEquation([5, 0, 1, 3]);
            //foreach (var i in b)
            //{
            //    Console.Write(i);
            //    Console.Write(" ");
            //}
            //Console.WriteLine();
            //Console.WriteLine(Equation2String(b));
            //Console.WriteLine(ComplexArray2String(Equat4_Formula(b, true)));


            //return;


            int m = 8;


            //for (int i = 1; i <= m; i++)
            //    for (int j = 0; j <= m; j++)
            //        for (int k = 0; k <= m; k++)
            //        {

            //            Console.Write($"{i},{j},{k} -> ");
            //            var equa = CreateEquation([i, j, k]);
            //            Console.Write(Equation2String(equa));
            //            Console.Write(" -> ");
            //            Console.Write(ComplexArray2StringSimple([.. Equat3_Formula(equa).OrderBy(x => x.Magnitude)], 10));
            //            Console.WriteLine();

            //            /*
            //            Console.WriteLine();
            //            Console.Write($"{i},{j},{k} -> ");
            //            var equa = CreateEquation([i, j, k]);
            //            Console.Write(Equation2String(equa));
            //            Console.Write(" -> ");
            //            Console.WriteLine();
            //            Console.Write(ComplexArray2StringSimple(Equat3_Formula(equa, true), 10));
            //            Console.WriteLine();
            //            */
            //        }

            /*
            Console.Write("0.5773502692 -> " + string.Join("/", Decimal2Fraction(0.5773502692, out double error, int.MaxValue, 1e-15)).Replace("-1/-1", "(変換失敗)"));
            Console.WriteLine($" error:{error}");
            Console.Write("0.2886751346 -> " + string.Join("/", Decimal2Fraction(0.2886751346, out error, int.MaxValue, 1e-15)).Replace("-1/-1", "(変換失敗)"));
            Console.WriteLine($" error:{error}");
            Console.Write("1.1547005384 -> " + string.Join("/", Decimal2Fraction(1.1547005384, out error, int.MaxValue, 1e-15)).Replace("-1/-1", "(変換失敗)"));
            Console.WriteLine($" error:{error}");
            Console.Write("1.7320508076 -> " + string.Join("/", Decimal2Fraction(1.7320508076, out error, int.MaxValue, 1e-15)).Replace("-1/-1", "(変換失敗)"));
            Console.WriteLine($" error:{error}");
            Console.Write("2.3094010768 -> " + string.Join("/", Decimal2Fraction(2.3094010768, out error, int.MaxValue, 1e-15)).Replace("-1/-1", "(変換失敗)"));
            Console.WriteLine($" error:{error}");
            Console.Write("0.8660254038 -> " + string.Join("/", Decimal2Fraction(0.8660254038, out error, int.MaxValue, 1e-15)).Replace("-1/-1", "(変換失敗)"));
            Console.WriteLine($" error:{error}");
            */


            for (int i = 1; i <= m; i++)
                for (int j = i; j <= m; j++)
                    for (int k = j; k <= m; k++)
                        for (int l = k; l <= m; l++)
                        {
                            Console.Write($"{i},{j},{k},{l} -> ");
                            var equa = CreateEquation([i, j, k, l]);
                            Console.Write(Equation2String(equa));
                            Console.Write(" -> ");
                            Console.Write(ComplexArray2StringSimple([.. Equat4_Formula(equa).OrderBy(x => x.Magnitude)], 5));
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


            var b = CreateEquation([5, 4, 4, 3]);
            Console.WriteLine();
            Console.WriteLine(Equation2String(b));
            Console.WriteLine(ComplexArray2String(Equat4_Formula(b, true)));
        }
    }
}
