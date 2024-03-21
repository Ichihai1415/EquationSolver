using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EquationSolver.SymbolExpansion
{
    internal class Program
    {
        public static char[] DarkGrayText = new char[] { '+', '-', '(', ')', 'x', '^', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        static void Main()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("解をコンマ(,)区切りで入力してください。↓コピペ用\n" +
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
            var rootInput = Console.ReadLine().Split(',').Select(x => (object)x).ToArray();
            var equation = CreateEquation(rootInput);
            Console.WriteLine();
            var text = Equation2String(equation);

            string _text = text;
            Console.ForegroundColor = ConsoleColor.Gray;
            while (_text.Length > 0)
            {
                if (DarkGrayText.Contains(_text.First()))
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                else
                    Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write(_text.First());
                _text = _text.Remove(0, 1);
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("おまけ(足す回数(a+bだったら2、abは1))");
            for (int i = rootInput.Length; i >= 0; i--)
            {
                if (i > 1)
                    Console.WriteLine($"x^{i} : {NCR(rootInput.Length, i)}");
                else if (i == 1)
                    Console.WriteLine($"x : {NCR(rootInput.Length, i)}");
                else
                    Console.WriteLine($"0 : 1");
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("何かキーを押すと再実行します。");
            Console.ReadKey();
            Console.WriteLine();
            Main();
        }

        /// <summary>
        /// 方程式の係数の配列から文字列に変換します。
        /// </summary>
        /// <param name="coefficients">方程式の係数の配列</param>
        /// <returns>文字列(x^2-2x+1等)</returns>
        /// <exception cref="ArgumentException">引数が不正な場合</exception>
        public static string Equation2String(object[] coefficients)
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
            return eqStB.ToString();
        }


        /// <summary>
        /// 解から方程式の係数の配列を生成します。虚数解には対応してません。
        /// </summary>
        /// <param name="roots">解の配列(重解でもその分追加)</param>
        /// <returns>方程式の係数の配列(次数が大きい順)</returns>
        /// <exception cref="ArgumentException">引数が不正な場合</exception>
        public static string[] CreateEquation(object[] roots)
        {
            if (!roots.Any())
                throw new ArgumentException("引数がnullまたは個数が不足しています。", nameof(roots));

            var n = roots.Length;//解の数
            var coefficients = new string[n + 1];//係数
            coefficients[0] = "";

            for (int i = 1; i <= n; i++)//x^(n-i)の係数を求める
            {
                var coefComb = CreateCombinations(roots, i).Select(x => x.Select(y => y.ToString()));//係数ごとに定数部分の組み合わせを作る
                if (string.IsNullOrEmpty(coefficients[i]))
                    coefficients[i] += coefComb.ToArray()[0].Count() % 2 == 0 ? "\n+(+" : "\n-(+";
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
        /// objectの組み合わせを作ります。
        /// </summary>
        /// <param name="array">組み合わせを作る配列</param>
        /// <param name="r">選ぶ個数</param>
        /// <returns>objectの組み合わせ</returns>
        /// <exception cref="ArgumentException">引数が不正な場合</exception>
        public static List<List<object>> CreateCombinations(object[] array, int r)
        {
            if (r < 0 || array.Length < r)//num.Len>=r>=0
                throw new ArgumentException("引数が不正です。", nameof(r));
            var combinations = new List<List<object>>();
            CreateCombinations(ref combinations, array, r);
            return combinations;
        }

        /// <summary>
        /// objectの組み合わせを作成します。
        /// </summary>
        /// <remarks>内部参照用です。こちらを指定する必要はありません。</remarks>
        /// <param name="result">objectの組み合わせ(参照)</param>
        /// <param name="array">組み合わせを作る配列</param>
        /// <param name="r_remain">参照する残りの個数</param>
        /// <param name="start">参照する始点</param>
        /// <param name="combinationTmp">objectの組み合わせの内部一時保存</param>
        /// <exception cref="ArgumentException">引数が不正な場合</exception>
        /// <exception cref="ArgumentNullException">引数が不正な場合</exception>
        public static void CreateCombinations(ref List<List<object>> result, object[] array, int r_remain, int start = 0, List<object> combinationTmp = null)
        {
            if (result == null)
                throw new ArgumentNullException(nameof(result));
            if (!array.Any())
                throw new ArgumentException("引数がnullまたは個数が不足しています。", nameof(array));
            if (r_remain < 0)
                throw new ArgumentException("引数が不正です。", nameof(r_remain));
            if (start < 0)
                throw new ArgumentException("引数が不正です。", nameof(start));
            if (combinationTmp == null)
                combinationTmp = new List<object>();

            if (r_remain == 0)//r個の要素を選び終わった時
            {
                result.Add(new List<object>(combinationTmp));
                return;
            }

            for (int i = start; i <= array.Length - r_remain; i++)
            {
                combinationTmp.Add(array[i]);
                CreateCombinations(ref result, array, r_remain - 1, i + 1, combinationTmp);//残りの回数、インデックスを次のものになるようにして再帰的に呼び出し
                combinationTmp.RemoveAt(combinationTmp.Count - 1);
            }
        }


        /// <summary>
        /// nCr(組み合わせの総数)を計算します。
        /// </summary>
        /// <remarks>nPrがlong.MaxValueを超える場合計算不可です。</remarks>
        /// <param name="n">n個から選ぶ</param>
        /// <param name="r">r個選ぶ</param>
        /// <returns>組み合わせの総数</returns>
        /// <exception cref="ArgumentException">引数が不正な場合</exception>
        public static long NCR(int n, int r)
        {
            if (n < 1)
                throw new ArgumentException("引数が不正です。", nameof(n));
            if (r < 1 || n < r)
                throw new ArgumentException("引数が不正です。", nameof(r));
            if (n < r * r)
                r = n - r;
            long a = 1, b = 1;
            for (int i = 0; i < r; i++)
            {
                a *= n - i;
                b *= r - i;
            }
            return a / b;
        }

    }
}
