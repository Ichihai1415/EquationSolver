using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EquationSolver.SymbolExpansion
{
    /// <summary>
    /// 文字での方程式生成用ソフト
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// 結果表示時にグレー色にするもの
        /// </summary>
        public static char[] DarkGrayChars = new char[] { '+', '-', '(', ')', '=' };
        /// <summary>
        /// 結果表示時に黄色にするもののうちxの次数部分
        /// </summary>
        public static char[] YellowChars_num = new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        static void Main()
        {
            /*
            for (int i = 0; i < 100; i++)
                Console.WriteLine($"{i},{i / 2}  {NCR_long(i, i / 2)}");
            */
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("解をコンマ(,)区切りで入力してください。x(かける)は省略されるため、数字のみや2文字以上は注意してください。↓コピペ用\n" +
                " (1) a\n" +
                " (2) a,b\n" +
                " (3) a,b,c\n" +
                " (4) a,b,c,d\n" +
                " (5) a,b,c,d,e\n" +
                " (6) a,b,c,d,e,f\n" +
                " (7) a,b,c,d,e,f,g\n" +
                " (8) a,b,c,d,e,f,g,h\n" +
                " (9) a,b,c,d,e,f,g,h,i\n" +
                "(10) a,b,c,d,e,f,g,h,i,j\n" +
                "(11) a,b,c,d,e,f,g,h,i,j,k\n" +
                "(12) a,b,c,d,e,f,g,h,i,j,k,l\n" +
                "(13) a,b,c,d,e,f,g,h,i,j,k,l,m\n" +
                "(14) a,b,c,d,e,f,g,h,i,j,k,l,m,n\n" +
                "(15) a,b,c,d,e,f,g,h,i,j,k,l,m,n,o\n" +
                "(16) a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p\n" +
                "(17) a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q\n" +
                "(18) a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r\n" +
                "(19) a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s\n" +
                "(20) a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t\n" +
                "");
            Console.ForegroundColor = ConsoleColor.Cyan;
            var rootInput = Console.ReadLine().Split(',');

            Console.WriteLine("方程式生成中...");
            var equation = CreateEquation_st(rootInput).Select(x => "\n" + x).ToArray();
            Console.WriteLine();
            var text = Equation2String(equation).Replace("=0", "\n=0");
            var lastChar = ' ';//1つ前の文字
            var lastBefChar = ' ';//2つ前の文字

            string _text = text;
            Console.ForegroundColor = ConsoleColor.Gray;
            while (_text.Length > 0)
            {
                var first = _text.First();
                if (DarkGrayChars.Contains(first))
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                else if ((lastChar == '^' || lastBefChar == '^') && YellowChars_num.Contains(first))//x^2,x^12のように99次までの数字部分
                    Console.ForegroundColor = ConsoleColor.Yellow;
                else if (first == '^' || first == '0')
                    Console.ForegroundColor = ConsoleColor.Yellow;
                else if ((lastChar == ')' || lastChar == '\n') && first == 'x')//解にxが含まれても(x1,x2のように)色を分ける
                    Console.ForegroundColor = ConsoleColor.Yellow;
                else
                    Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write(first);
                lastBefChar = lastChar;
                lastChar = first;
                _text = _text.Remove(0, 1);
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("おまけ(足す回数(組み合わせの数)(a+bだったら2、abは1))");
            for (int i = rootInput.Length; i >= 0; i--)
            {
                if (i > 1)
                    Console.WriteLine($"x^{i} : {rootInput.Length}C{i} = {NCR_long(rootInput.Length, i)}");
                else if (i == 1)
                    Console.WriteLine($"x : {rootInput.Length}C{i} = {NCR_long(rootInput.Length, i)}");
                else
                    Console.WriteLine($"0 : {rootInput.Length}C{i} = {NCR_long(rootInput.Length, i)}");
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine();
            Console.WriteLine("何かキーを押すと再実行します。");
            Console.ReadKey();
            Console.WriteLine();
            Main();
        }

        /// <summary>
        /// 方程式の係数(文字列)の配列から文字列に変換します。
        /// </summary>
        /// <param name="coefficients">方程式の係数の配列(例:[,-(a+b),+(ab)])</param>
        /// <returns>文字列(x^2-2x+1等)</returns>
        /// <exception cref="ArgumentException">引数が不正な場合</exception>
        public static string Equation2String(string[] coefficients)
        {
            if (!coefficients.Any())
                throw new ArgumentException("引数がnullまたは個数が不足しています。", nameof(coefficients));

            var count = coefficients.Length;
            var eqStB = new StringBuilder();
            for (int i = 0; i < count; i++)//(count-i-1)次の項
            {
                var degree = count - i - 1;//次数
                var coefficient = coefficients[i];//x^(count-i-1)の係数
                eqStB.Append(coefficient);
                if (degree > 1)
                {
                    eqStB.Append("x^");
                    eqStB.Append(degree);
                }
                else if (degree == 1)
                    eqStB.Append("x");
            }
            eqStB.Append("=0");
            return eqStB.ToString();
        }

        /// <summary>
        /// 解から方程式の係数(文字列のまま)の配列を生成します。object[]で指定できます(内部でToString()されます)。
        /// </summary>
        /// <param name="roots">解の配列(重解でもその分追加)</param>
        /// <returns>方程式の係数の配列(次数が大きい順)</returns>
        /// <exception cref="ArgumentException">引数が不正な場合</exception>
        public static string[] CreateEquation_st(object[] roots)
        {
            if (!roots.Any())
                throw new ArgumentException("引数がnullまたは個数が不足しています。", nameof(roots));

            var n = roots.Length;//解の数
            var coefficients = new string[n + 1];//係数(最高次数~定数項)
            coefficients[0] = "";

            for (int i = 1; i <= n; i++)//x^(n-i)の係数を求める
            {
                var coefComb = CreateCombinations(roots, i).Select(x => x.Select(y => y.ToString()));//係数ごとに定数部分の組み合わせを作る
                if (string.IsNullOrEmpty(coefficients[i]))
                    coefficients[i] += coefComb.ToArray()[0].Count() % 2 == 0 ? "+(+" : "-(+";
                foreach (var combi_tmp in coefComb)
                {
                    coefficients[i] += "+";
                    foreach (var value in combi_tmp)
                        coefficients[i] += value;
                }
                coefficients[i] += ")";
                coefficients[i] = coefficients[i].Replace("++", "");
            }
            return coefficients;
        }

        /// <summary>
        /// 組み合わせを作ります。
        /// </summary>
        /// <example><code>
        /// //a,b,cの中から2つ選ぶ
        /// CreateCombinations(new string[]{ "a", "b", "c" }, 2);
        /// //[[a,b],[a,c],[b,c]]
        /// </code></example>
        /// <param name="srcArray">組み合わせを作る配列</param>
        /// <param name="r">選ぶ個数</param>
        /// <returns>組み合わせの2次元配列(0:すべての組み合わせの配列,1:1つの組み合わせの配列)</returns>
        /// <exception cref="ArgumentException">引数が不正な場合</exception>
        public static List<List<T>> CreateCombinations<T>(T[] srcArray, int r)
        {
            if (r < 0 || srcArray.Length < r)//num.Len>=r>=0
                throw new ArgumentException("引数が不正です。", nameof(r));
            var combinations = new List<List<T>>();
            CreateCombinations(ref combinations, srcArray, r);
            return combinations;
        }

        /// <summary>
        /// 組み合わせを作成します。
        /// </summary>
        /// <remarks>内部参照用です。こちらは指定しないでください。</remarks>
        /// <param name="result">組み合わせ(参照)</param>
        /// <param name="srcArray">組み合わせを作る配列</param>
        /// <param name="r_remain">初回呼び出し時選ぶ個数、再帰呼び出し時参照する残りの個数</param>
        /// <param name="start">参照する始点(再帰呼び出し時のみ)</param>
        /// <param name="combinationTmp">組み合わせの内部一時保存(再帰呼び出し時のみ)</param>
        /// <exception cref="ArgumentException">引数が不正な場合</exception>
        /// <exception cref="ArgumentNullException">引数が不正な場合</exception>
        public static void CreateCombinations<T>(ref List<List<T>> result, T[] srcArray, int r_remain, int start = 0, List<T> combinationTmp = null)
        {
            if (result == null)
                throw new ArgumentNullException(nameof(result));
            if (!srcArray.Any())
                throw new ArgumentException("引数がnullまたは個数が不足しています。", nameof(srcArray));
            if (r_remain < 0)
                throw new ArgumentException("引数が不正です。", nameof(r_remain));
            if (start < 0)
                throw new ArgumentException("引数が不正です。", nameof(start));
            if (combinationTmp == null)//初回呼び出し時
                combinationTmp = new List<T>();

            if (r_remain == 0)//選び終わった時
            {
                result.Add(new List<T>(combinationTmp));
                return;
            }

            for (int i = start; i <= srcArray.Length - r_remain; i++)
            {
                combinationTmp.Add(srcArray[i]);//最初のやつ([a,b,c],2のとき[a,b])
                CreateCombinations(ref result, srcArray, r_remain - 1, i + 1, combinationTmp);//残りの回数、インデックスを次のものになるようにして再帰的に呼び出し
                combinationTmp.RemoveAt(combinationTmp.Count - 1);//余計なものも付くから削除
            }
        }

        /// <summary>
        /// nCr(組み合わせの総数)を計算します。
        /// </summary>
        /// <remarks>nPr((n < r * 2)のときnP(n-r))がlong.MaxValueを超える場合、値は正常値ではなくなります。29C14までは可能です。</remarks>
        /// <param name="n">n個から選ぶ</param>
        /// <param name="r">r個選ぶ</param>
        /// <returns>組み合わせの総数</returns>
        /// <exception cref="ArgumentException">引数が不正な場合</exception>
        public static long NCR_long(int n, int r)//ulongにすればちょっと増える　.NET7以降ならInt128/UInt128使える(そこまでは普通不要)
        {
            if (n < 0)
                throw new ArgumentException("引数が不正です。", nameof(n));
            if (r < 0 || n < r)
                throw new ArgumentException("引数が不正です。", nameof(r));
            if (n == 0 || r == 0)
                return 1;
            if (n < r * 2)
                r = n - r;//nCr = nC(n-r)
            long a = 1, b = 1;
            for (int i = 0; i < r; i++)
            {
                a *= n - i;
                b *= r - i;
            }
            return a / b;
        }

        /// <summary>
        /// nCr(組み合わせの総数)を計算します。
        /// </summary>
        /// <remarks>nPr((n < r * 2)のときnP(n-r))がdouble.MaxValueを超える場合、値は正常値ではなくなります。大きい値の場合誤差が発生する可能性があります。</remarks>
        /// <param name="n">n個から選ぶ</param>
        /// <param name="r">r個選ぶ</param>
        /// <returns>組み合わせの総数</returns>
        /// <exception cref="ArgumentException">引数が不正な場合</exception>
        public static double NCR_double(int n, int r)
        {
            if (n < 0)
                throw new ArgumentException("引数が不正です。", nameof(n));
            if (r < 0 || n < r)
                throw new ArgumentException("引数が不正です。", nameof(r));
            if (n == 0 || r == 0)
                return 1;
            if (n < r * 2)
                r = n - r;//nCr = nC(n-r)
            double a = 1, b = 1;
            for (int i = 0; i < r; i++)
            {
                a *= n - i;
                b *= r - i;
            }
            return a / b;
        }
    }
}
