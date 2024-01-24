using System;
using System.Linq;
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
                    Console.WriteLine("解をコンマ(,)区切りで入力してください。");
                    var rootInput = Console.ReadLine().Split(',').Select(x => double.Parse(x)).ToArray();

                    var creEqSt = DateTime.Now;
                    var equation = CreateEquation(rootInput);
                    var creEqEn = DateTime.Now;

                    Console.WriteLine($"式を展開すると、 {Equation2String(equation)} になります。");

                    Console.WriteLine("四捨五入する桁(小数点第x位)を入力してください。無効にする場合-1にしてください。0または変換に失敗した場合10となります。");
                    int.TryParse(Console.ReadLine(), out int rDigits);
                    if (rDigits == 0)
                        rDigits = 10;

                    var creSolSt = DateTime.Now;
                    var rootSolved = new System.Numerics.Complex[0];
                    if (rootInput.Length == 2)
                        rootSolved = Equat2_Formula(equation);
                    else if (rootInput.Length == 3)
                        rootSolved = Equat3_Formula(equation);
                    else if (rootInput.Length == 4)
                        rootSolved = Equat4_Formula(equation);
                    else
                        throw new Exception($"値が不正です。{rootInput.Length}次方程式は対応していません。");
                    var creSolEn = DateTime.Now;

                    Console.WriteLine($"式を解くと、 {ComplexArray2StringSimple(rootSolved, rDigits).Replace("\n", " ")} になります。(計算の都合上誤差が発生します)\n");
                    Console.WriteLine($"方程式生成:{(creEqEn - creEqSt).TotalMilliseconds}ms  解計算:{(creSolEn - creSolSt).TotalMilliseconds}ms でした。\n\n\n\n");
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
