using System;
using System.Linq;
using System.Numerics;
using System.Threading;
using static EquationSolver.EquationSolver;
using static EquationSolver.EquationSolver.Command;

namespace EquationSolver.test.NETFramework
{
    internal class Program
    {
        public static readonly ConsoleColor c = Console.ForegroundColor;
        public const string menuText = "" +
            "1.コマンド\n" +
            "2.解から方程式を作り計算\n" +
            "3.連続実行\n" +
            "4.方程式の係数を直接指定して計算";

        static void Main(string[] args)
        {
            Console.WriteLine("This library is released under the **MIT License**. The creator assumes no liability for any reason whatsoever.\nMore details can be found in the command menu.\n");
            Thread.Sleep(1000);
            while (true)
                try
                {
                start:
                    Console.ForegroundColor = c;
                    Console.WriteLine("モードを入力してください。");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(menuText);
                    Console.ForegroundColor = ConsoleColor.Blue;

                    double[] equation;
                    switch (Console.ReadLine())
                    {
                        case "1":
                            Console.ForegroundColor = c;
                            Console.WriteLine("コマンドを入力してください。()が付いたものまたは-が付いたものを指定してください。\n");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("" +
                            "ShowInfoAnime() / -sia : 情報アニメーションを表示します。\n" +
                            "ShowLicense()   / -sli : ライセンス全文を表示します。\n" +
                            "Clear()         / -cle : コンソールをクリアします。\n" +
                            "ClearShowAni()  / -csa : コンソールをクリアし情報アニメーションを表示します。");
                            Console.ForegroundColor = ConsoleColor.Cyan;

                            switch (Console.ReadLine())
                            {
                                case "ShowInfoAnime()":
                                case "-sia":
                                    Console.ForegroundColor = c;
                                    ShowInfoAnime();
                                    break;
                                case "ShowLicense()":
                                case "-sli":
                                    Console.ForegroundColor = c;
                                    ShowLicense();
                                    break;
                                case "Clear()":
                                case "-cle":
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Clear();
                                    break;
                                case "ClearShowAni()":
                                case "-csa":
                                    Clear();
                                    Console.ForegroundColor = c;
                                    ShowInfoAnime();
                                    break;
                                default:
                                    throw new Exception("コマンドが見つかりませんでした。");
                            }
                            goto start;
                        case "2":
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
                        case "3":
                            CC();
                            goto start;
                        case "4":
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

        public static void CC()
        {
            Console.ForegroundColor = c;
            Console.WriteLine("次数を入力してください。");
            Console.ForegroundColor = ConsoleColor.Cyan;
            var degree = int.Parse(Console.ReadLine());
            Console.ForegroundColor = c;
            Console.WriteLine("最小解を入力してください。");
            Console.ForegroundColor = ConsoleColor.Cyan;
            var minRoot = double.Parse(Console.ReadLine());
            Console.ForegroundColor = c;
            Console.WriteLine("最大解を入力してください。");
            Console.ForegroundColor = ConsoleColor.Cyan;
            var maxRoot = double.Parse(Console.ReadLine());
            Console.ForegroundColor = c;
            Console.WriteLine("解の差を入力してください。");
            Console.ForegroundColor = ConsoleColor.Cyan;
            var difRoot = double.Parse(Console.ReadLine());

            switch (degree)
            {
                case 1:
                    for (double i = minRoot; i <= maxRoot; i += difRoot)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write($"{i}");
                        Console.ForegroundColor = c;
                        Console.Write(" -> ");
                        var equa = CreateEquation(new double[] { i });
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write(Equation2String(equa));
                        Console.ForegroundColor = c;
                        Console.Write(" -> ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(ComplexArray2StringSimple(new Complex[] { new Complex(-equa[1] / equa[0], 0) }, 5));
                        Console.WriteLine();
                    }
                    break;
                case 2:
                    for (double i = minRoot; i <= maxRoot; i += difRoot)
                        for (double j = i; j <= maxRoot; j += difRoot)
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write($"{i},{j}");
                            Console.ForegroundColor = c;
                            Console.Write(" -> ");
                            var equa = CreateEquation(new double[] { i, j });
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.Write(Equation2String(equa));
                            Console.ForegroundColor = c;
                            Console.Write(" -> ");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(ComplexArray2StringSimple(Equat2_Formula(equa).OrderBy(x => x.Magnitude).ToArray(), 5));
                            Console.WriteLine();
                        }
                    break;
                case 3:
                    for (double i = minRoot; i <= maxRoot; i += difRoot)
                        for (double j = i; j <= maxRoot; j += difRoot)
                            for (double k = j; k <= maxRoot; k += difRoot)
                            {
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.Write($"{i},{j},{k}");
                                Console.ForegroundColor = c;
                                Console.Write(" -> ");
                                var equa = CreateEquation(new double[] { i, j, k });
                                Console.ForegroundColor = ConsoleColor.DarkGreen;
                                Console.Write(Equation2String(equa));
                                Console.ForegroundColor = c;
                                Console.Write(" -> ");
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write(ComplexArray2StringSimple(Equat3_Formula(equa).OrderBy(x => x.Magnitude).ToArray(), 5));
                                Console.WriteLine();
                            }
                    break;
                case 4:
                    for (double i = minRoot; i <= maxRoot; i += difRoot)
                        for (double j = i; j <= maxRoot; j += difRoot)
                            for (double k = j; k <= maxRoot; k += difRoot)
                                for (double l = k; l <= maxRoot; l += difRoot)
                                {
                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                    Console.Write($"{i},{j},{k},{l}");
                                    Console.ForegroundColor = c;
                                    Console.Write(" -> ");
                                    var equa = CreateEquation(new double[] { i, j, k, l });
                                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                                    Console.Write(Equation2String(equa));
                                    Console.ForegroundColor = c;
                                    Console.Write(" -> ");
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.Write(ComplexArray2StringSimple(Equat4_Formula(equa).OrderBy(x => x.Magnitude).ToArray(), 5));
                                    Console.WriteLine();
                                }
                    break;
                default:
                    throw new Exception($"値が不正です。{degree}次方程式は対応していません。");
            }


        }
    }
}
