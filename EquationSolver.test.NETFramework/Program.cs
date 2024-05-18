using System;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using static EquationSolver.EquationSolver;

namespace EquationSolver.test.NETFramework
{
    /// <summary>
    /// .NET Frameworkで実行
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// コンソール既定色
        /// </summary>
        public static readonly ConsoleColor c = Console.ForegroundColor;
        /// <summary>
        /// メニューテキスト
        /// </summary>
        public const string menuText = "" +
            "1.コマンド\n" +
            "2.解から方程式を作り計算\n" +
            "3.連続実行\n" +
            "4.方程式の係数を直接指定して計算";

        /// <summary>
        /// メインエントリポイント
        /// </summary>
        static void Main(/*string[] args*/)//RequestInput()
        {
            //Console.WriteLine("This library is released under the **MIT License**. The creator assumes no liability for any reason whatsoever.\nMore details can be found in the command menu.\n");
            //Thread.Sleep(1000);
            ConWriteLineC("\n" +
                "          ///////////////////////////////////////////////////////////\n" +
                "         ////   EquationSolver.test.NETFramework.exe     - ロ X ////\n" +
                "        ///////////////////////////////////////////////////////////\n" +
                "       ////                                                   ////\n" +
                "      ////    -- debugging software in .NET Framework --     ////\n" +
                "     ////   https://github.com/Ichihai1415/EquationSolver   ////\n" +
                "    ////                              update: 2024/05/18   ////\n" +
                "   ////                                                   ////\n" +
                "  ///////////////////////////////////////////////////////////\n\n", c);
            while (true)
                try
                {
                start:
                    ConWriteLineC("モードを入力してください。", c);
                    double[] equation;
                    ConWriteLineC(menuText, ConsoleColor.Yellow);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    switch (Console.ReadLine())
                    {
                        case "1":
                            ConWriteLineC("コマンドを入力してください。()が付いたものまたは-が付いたものを指定してください。", c);
                            ConWriteLineC("" +
                            "ShowInfoAnime() / -sia : 情報アニメーションを表示します。\n" +
                            "ShowLicense()   / -sli : ライセンス全文を表示します。\n" +
                            "Clear()         / -cle : コンソールをクリアします。\n" +
                            "ClearShowAni()  / -csa : コンソールをクリアし情報アニメーションを表示します。", ConsoleColor.DarkYellow);
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            switch (Console.ReadLine())
                            {
                                case "ShowInfoAnime()":
                                case "-sia":
                                    Console.ForegroundColor = c;
                                    Command.ShowInfoAnime();
                                    break;
                                case "ShowLicense()":
                                case "-sli":
                                    Console.ForegroundColor = c;
                                    Command.ShowLicense();
                                    break;
                                case "Clear()":
                                case "-cle":
                                    Command.Clear();
                                    break;
                                case "ClearShowAni()":
                                case "-csa":
                                    Command.Clear();
                                    Console.ForegroundColor = c;
                                    Command.ShowInfoAnime();
                                    break;
                                default:
                                    throw new Exception("コマンドが見つかりませんでした。");
                            }
                            goto start;
                        case "2":
                            var rootInput = RequestInput("解をコンマ(,)区切りで入力してください。").Split(',').Select(x => double.Parse(x)).ToArray();
                            var creEqSt = DateTime.Now;
                            equation = CreateEquation(rootInput);
                            var creEqEn = DateTime.Now;

                            ConWriteC("式を展開すると、 ", c);
                            ConWriteC(Equation2String(equation), ConsoleColor.Green);
                            ConWriteLineC($" になります。 方程式生成:{(creEqEn - creEqSt).TotalMilliseconds}ms", c);
                            break;
                        case "3":
                            ContinuousExecution();
                            goto start;
                        case "4":
                            equation = RequestInput("係数をコンマ(,)区切りで入力してください。(降べきの順・0も含める)").Split(',').Select(x => double.Parse(x)).ToArray();
                            ConWriteC("式は、 ", c);
                            ConWriteC(Equation2String(equation), ConsoleColor.Green);
                            ConWriteLineC(" になります。", c);
                            break;
                        default:
                            throw new Exception("数字のみで選択肢のものを入力してください。");
                    }

                    int.TryParse(RequestInput("四捨五入する桁(小数点第x位)を入力してください。無効にする場合-1にしてください。0または変換に失敗した場合10となります。"), out int rDigits);
                    rDigits = rDigits == 0 ? 10 : rDigits;
                    var isSave = RequestInput("結果を出力しますか？(y/n)(三次・四次のみ)") == "y";

                    Complex[] rootSolved;
                    var creSolSt = DateTime.Now;
                    var infoText = "";
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    switch (equation.Length)//(equation.Length-1)次式
                    {
                        case 2:
                            rootSolved = new Complex[] { new Complex(-equation[1] / equation[0], 0) };
                            break;
                        case 3:
                            rootSolved = Equat2_Formula(equation, true);
                            break;
                        case 4:
                            if (isSave)
                                rootSolved = Equat3_Formula(equation, true, out infoText);
                            else
                                rootSolved = Equat3_Formula(equation, true);
                            break;
                        case 5:
                            if (isSave)
                                rootSolved = Equat4_Formula(equation, true, out infoText);
                            else
                                rootSolved = Equat4_Formula(equation, true);
                            break;
                        default:
                            throw new Exception($"値が不正です。{equation.Length - 1}次方程式は対応していません。");
                    }
                    var creSolEn = DateTime.Now;
                    rootSolved = rootSolved.OrderBy(x => x.Magnitude).ToArray();

                    ConWriteC("解は ", c);
                    ConWriteC(ComplexArray2StringSimple(rootSolved, rDigits).Replace("\n", " "), ConsoleColor.Green);
                    ConWriteLineC($" になります。解計算+表示等処理:{(creSolEn - creSolSt).TotalMilliseconds}ms", c);
                    if (isSave)
                    {
                        SaveFile(infoText, out string path);
                        Console.WriteLine();
                        ConWriteC(Path.GetFullPath(path), ConsoleColor.Green);
                        ConWriteLineC(" に保存しました。", c);
                    }
                    ConWriteLineC("\n--------------------------------------------------\n", c);
                }
                catch (Exception ex)
                {
                    ConWriteLineC("\n**********エラーが発生しました。内容を確認してください。**********", ConsoleColor.Red);
                    ConWriteLineC(ex.ToString(), ConsoleColor.Red);
                    ConWriteLineC("******************************************************************\n", ConsoleColor.Red);
                }
        }

        public static void ContinuousExecution()//長いので移した
        {
            var degree = int.Parse(RequestInput("注意:解や解の差は小数も可能ですが丸め誤差が累積されます。\n次数を入力してください。"));
            var minRoot = double.Parse(RequestInput("最小解を入力してください。"));
            var maxRoot = double.Parse(RequestInput("最大解を入力してください。"));
            var difRoot = double.Parse(RequestInput("解の差を入力してください。"));
            int.TryParse(RequestInput("四捨五入する桁(小数点第x位)を入力してください。無効にする場合-1にしてください。0または変換に失敗した場合5となります。"), out int rDigits);
            rDigits = rDigits == 0 ? 5 : rDigits;
            var isSave = RequestInput("結果を出力しますか？(y/n)(すべて同じファイルに保存します)") == "y";
            var stb = new StringBuilder();
            var stt = DateTime.Now;
            var count = 0;
            switch (degree)
            {
                case 1:
                    for (double i = minRoot; i <= maxRoot; i += difRoot)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        ConWriteArray(new object[] { i }, false);
                        ConWriteC(" -> ", c);
                        var equa = CreateEquation(new double[] { i });
                        var equast = Equation2String(equa);
                        ConWriteC(equast, ConsoleColor.Blue);
                        ConWriteC(" -> ", c);
                        var rts = ComplexArray2StringSimple(new Complex[] { new Complex(-equa[1] / equa[0], 0) }, 5);
                        ConWriteC(rts, ConsoleColor.Green);
                        if (isSave)
                            StBAppendArray(ref stb, new object[] { i, " -> ", equast, " -> ", rts }, true);
                        Console.WriteLine();
                        count++;
                    }
                    break;
                case 2:
                    for (double i = minRoot; i <= maxRoot; i += difRoot)
                        for (double j = i; j <= maxRoot; j += difRoot)
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            ConWriteArray(new object[] { i, ",", j }, false);
                            ConWriteC(" -> ", c);
                            var equa = CreateEquation(new double[] { i, j });
                            var equast = Equation2String(equa);
                            ConWriteC(equast, ConsoleColor.Blue);
                            ConWriteC(" -> ", c);
                            var rts = ComplexArray2StringSimple(Equat2_Formula(equa).OrderBy(x => x.Magnitude).ToArray(), rDigits);
                            ConWriteC(rts, ConsoleColor.Green);
                            if (isSave)
                                StBAppendArray(ref stb, new object[] { i, ",", j, " -> ", equast, " -> ", rts }, true);
                            Console.WriteLine();
                            count++;
                        }
                    break;
                case 3:
                    for (double i = minRoot; i <= maxRoot; i += difRoot)
                        for (double j = i; j <= maxRoot; j += difRoot)
                            for (double k = j; k <= maxRoot; k += difRoot)
                            {
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                ConWriteArray(new object[] { i, ",", j, ",", k }, false);
                                ConWriteC(" -> ", c);
                                var equa = CreateEquation(new double[] { i, j, k });
                                var equast = Equation2String(equa);
                                ConWriteC(equast, ConsoleColor.Blue);
                                ConWriteC(" -> ", c);
                                var rts = ComplexArray2StringSimple(Equat3_Formula(equa).OrderBy(x => x.Magnitude).ToArray(), rDigits);
                                ConWriteC(rts, ConsoleColor.Green);
                                if (isSave)
                                    StBAppendArray(ref stb, new object[] { i, ",", j, ",", k, " -> ", equast, " -> ", rts }, true);
                                Console.WriteLine();
                                count++;
                            }
                    break;
                case 4:
                    for (double i = minRoot; i <= maxRoot; i += difRoot)
                        for (double j = i; j <= maxRoot; j += difRoot)
                            for (double k = j; k <= maxRoot; k += difRoot)
                                for (double l = k; l <= maxRoot; l += difRoot)
                                {
                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                    ConWriteArray(new object[] { i, ",", j, ",", k, ",", l }, false);
                                    ConWriteC(" -> ", c);
                                    var equa = CreateEquation(new double[] { i, j, k, l });
                                    var equast = Equation2String(equa);
                                    ConWriteC(equast, ConsoleColor.Blue);
                                    ConWriteC(" -> ", c);
                                    var rts = ComplexArray2StringSimple(Equat4_Formula(equa).OrderBy(x => x.Magnitude).ToArray(), rDigits);
                                    ConWriteC(rts, ConsoleColor.Green);
                                    if (isSave)
                                        StBAppendArray(ref stb, new object[] { i, ",", j, ",", k, ",", l, " -> ", equast, " -> ", rts }, true);
                                    Console.WriteLine();
                                    count++;
                                }
                    break;
                default:
                    throw new Exception($"値が不正です。{degree}次方程式は対応していません。");
            }
            ConWriteLineC($"計算数:{count} 総計算時間:{(DateTime.Now - stt).TotalSeconds}s", c);
            if (isSave)
            {
                SaveFile(stb.ToString(), out string path);
                Console.WriteLine();
                ConWriteC(Path.GetFullPath(path), ConsoleColor.Green);
                ConWriteLineC(" に保存しました。\n", c);
                stb.Clear();
            }
            ConWriteLineC("\n--------------------------------------------------\n", c);
        }

        /// <summary>
        /// output\yyyyMMdd-HHmmss.txtに保存します。
        /// </summary>
        /// <param name="text">保存するテキスト</param>
        /// <param name="path">保存したパス</param>
        public static void SaveFile(string text, out string path)
        {
            path = @"output\" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".txt";
            Directory.CreateDirectory("output");
            File.WriteAllText(path, text);
        }

        /// <summary>
        /// 入力を求めます。
        /// </summary>
        /// <remarks>表示するテキストはコンソール既定色、入力はシアン</remarks>
        /// <param name="showText"></param>
        /// <returns>入力されたテキスト(空文字入力可)</returns>
        public static string RequestInput(string showText)
        {
            Console.ForegroundColor = c;
            Console.WriteLine(showText);
            Console.ForegroundColor = ConsoleColor.Cyan;
            return Console.ReadLine();
        }

        /// <summary>
        /// 色付きでコンソール出力します。
        /// </summary>
        /// <param name="text">表示するテキスト</param>
        /// <param name="color">表示する色</param>
        public static void ConWriteC(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
        }

        /// <summary>
        /// 色付きでコンソール出力します。
        /// </summary>
        /// <param name="text">表示するテキスト</param>
        /// <param name="color">表示する色</param>
        public static void ConWriteLineC(string text, ConsoleColor color)
        {
            ConWriteC(text, color);
            Console.WriteLine();
        }

        /// <summary>
        /// 配列を一つずつコンソール出力します。改行は最後以外のみされます(<paramref name="addLineLast"/>で指定可能)
        /// </summary>
        /// <remarks>いくつかの内容を出力する際にConsole.Writeを大量に書くかFormat等で連結するかではない方法(効果不明)</remarks>
        /// <param name="srcArray">表示する配列</param>
        /// <param name="addLineLast">最後に改行するか</param>
        public static void ConWriteArray(object[] srcArray, bool addLineLast = true)
        {
            foreach (var obj in srcArray)
                Console.Write(obj);
            if (addLineLast)
                Console.WriteLine();
        }

        /// <summary>
        /// 配列を一つずつ<see cref="StringBuilder"/>に追加します。改行は最後以外のみされます(<paramref name="addLineLast"/>で指定可能)
        /// </summary>
        /// <param name="stb">追加する<see cref="StringBuilder"/></param>
        /// <param name="srcArray">追加する配列</param>
        /// <param name="addLineLast">最後に改行するか</param>
        public static void StBAppendArray(ref StringBuilder stb, object[] srcArray, bool addLineLast = false)
        {
            foreach (var obj in srcArray)
                stb.Append(obj);
            if (addLineLast)
                stb.AppendLine();
        }
    }
}
