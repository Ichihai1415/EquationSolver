using System;
using System.Linq;
using System.Numerics;
using static EquationSolver.EquationSolver;

namespace EquationSolver.test.NETFramework
{
    internal class Program
    {
        public static readonly ConsoleColor c = Console.ForegroundColor;
        static void Main(string[] args)
        {
            while (true)
                try
                {
                    Console.ForegroundColor = c;
                    Console.Write("モードを入力してください。");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\n" +
                        "1.解から方程式を作り計算\n" +
                        "2.方程式の係数を直接指定して計算");
                    Console.ForegroundColor = ConsoleColor.Blue;

                    double[] equation;
                    switch (Console.ReadLine())
                    {
                        case "1":
                            Console.ForegroundColor = c;
                            Console.WriteLine("解をコンマ(,)区切りで入力してください。");
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            var rootInput = Console.ReadLine().Split(',').Select(x => double.Parse(x)).ToArray();

                            var creEqSt = DateTime.Now;
                            equation = CreateEquation(rootInput);
                            var creEqEn = DateTime.Now;

                            Console.ForegroundColor = c;
                            Console.Write("式を展開すると、 ");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(Equation2String(equation));
                            Console.ForegroundColor = c;
                            Console.WriteLine($" になります。 方程式生成:{(creEqEn - creEqSt).TotalMilliseconds}ms");
                            break;
                        case "2":
                            Console.ForegroundColor = c;
                            Console.WriteLine("係数をコンマ(,)区切りで入力してください。(降べきの順・0も含める)");
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            equation = Console.ReadLine().Split(',').Select(x => double.Parse(x)).ToArray();
                            Console.ForegroundColor = c;
                            Console.Write("式は、 ");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(Equation2String(equation));
                            Console.ForegroundColor = c;
                            Console.WriteLine(" になります。");
                            break;
                        default:
                            throw new Exception("数字のみで選択肢のものを入力してください。");
                    }

                    Console.ForegroundColor = c;
                    Console.WriteLine("四捨五入する桁(小数点第x位)を入力してください。無効にする場合-1にしてください。0または変換に失敗した場合10となります。");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    int.TryParse(Console.ReadLine(), out int rDigits);
                    Console.ForegroundColor = c;
                    if (rDigits == 0)
                        rDigits = 10;

                    Complex[] rootSolved;
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    var creSolSt = DateTime.Now;
                    switch (equation.Length)//(equation.Length-1)次式
                    {
                        case 2:
                            rootSolved = new Complex[] { new Complex(-equation[1] / equation[0], 0) };
                            break;
                        case 3:
                            rootSolved = Equat2_Formula(equation, true);
                            break;
                        case 4:
                            rootSolved = Equat3_Formula(equation, true);
                            break;
                        case 5:
                            rootSolved = Equat4_Formula(equation, true);
                            break;
                        default:
                            throw new Exception($"値が不正です。{equation.Length - 1}次方程式は対応していません。");
                    }
                    var creSolEn = DateTime.Now;
                    Console.ForegroundColor = c;
                    rootSolved = rootSolved.OrderBy(x => x.Magnitude).ToArray();

                    Console.ForegroundColor = c;
                    Console.Write("解は、 ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(ComplexArray2StringSimple(rootSolved, rDigits).Replace("\n", " "));
                    Console.ForegroundColor = c;
                    Console.Write($" になります。※計算の都合上誤差が発生します。解計算:{(creSolEn - creSolSt).TotalMilliseconds}ms\n\n--------------------------------------------------\n\n");
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.ToString());
                    Console.ForegroundColor = c;
                }
        }
    }
}
