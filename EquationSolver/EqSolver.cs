using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace EquationSolver
{
    public static class EquationSolver
    {
        public static double sqrt3 = Math.Sqrt(3);
        public static double oneThird = 1d / 3d;

        /// <summary>
        /// 解から方程式の係数の配列を生成します。
        /// </summary>
        /// <param name="roots">解の配列(重解でもその分追加)</param>
        /// <returns>方程式の係数の配列(次数が大きい順)</returns>
        /// <exception cref="ArgumentException">引数が不正な場合</exception>
        public static double[] CreateEquation(double[] roots)
        {
            if (!roots.Any())
                throw new ArgumentException("引数がnullまたは個数が不足しています。", nameof(roots));

            var n = roots.Length;//解の数
            var coefficients = new double[n + 1];//係数
            coefficients[0] = 1;

            for (int i = 1; i <= n; i++)//x^(n-i)の係数を求める
            {
                var coefComb = CreateCombinations(roots, i);
                foreach (var combi_tmp in coefComb)
                {
                    var coefficientTmp = 1d;
                    foreach (var value in combi_tmp)
                        coefficientTmp *= -value;
                    coefficients[i] += coefficientTmp;
                }
            }
            return coefficients;
        }

        /// <summary>
        /// 方程式の係数の配列から文字列に変換します。
        /// </summary>
        /// <param name="coefficients">方程式の係数の配列</param>
        /// <returns>文字列(x^2-2x+1等)</returns>
        /// <exception cref="ArgumentException">引数が不正な場合</exception>
        public static string Equation2String(double[] coefficients)
        {
            if (!coefficients.Any())
                throw new ArgumentException("引数がnullまたは個数が不足しています。", nameof(coefficients));

            int count = coefficients.Length;
            var eqStB = new StringBuilder();
            for (int i = 0; i < count; i++)//(count-i-1)次の項
            {
                int degree = count - i - 1;//次数
                double coefficient = coefficients[i];//x^(count-i-1)の係数
                if (coefficient == 0)//項なし
                    continue;
                if (coefficient > 0 && eqStB.Length != 0)
                    eqStB.Append("+");
                if (degree == 0)//定数項
                {
                    eqStB.Append(coefficient);
                    continue;
                }
                if (coefficient != 1 && coefficient != -1)//1xとならないように
                    eqStB.Append(coefficient);
                if (degree == 1)//xの項
                    eqStB.Append("x");
                else
                {
                    eqStB.Append("x^");
                    eqStB.Append(degree);
                }
            }
            return eqStB.ToString();
        }

        /// <summary>
        /// doubleの組み合わせ(ここでは定数項の組み合わせ([[1,2],[1,3],[2,3]]等))を作ります。
        /// </summary>
        /// <remarks>ここでは(x-1)(x-2)(x-3)で、</remarks>
        /// <param name="nums">組み合わせを作る配列(ここでは解の配列)</param>
        /// <param name="r">選ぶ個数</param>
        /// <returns>doubleの組み合わせ</returns>
        /// <exception cref="ArgumentException">引数が不正な場合</exception>
        public static List<List<double>> CreateCombinations(double[] nums, int r)
        {
            if (r < 0 || nums.Length < r)//num.Len>=r>=0
                throw new ArgumentException("引数が不正です。", nameof(r));
            var combinations = new List<List<double>>();
            CreateCombinations(ref combinations, nums, r);
            return combinations;
        }

        /// <summary>
        /// doubleの組み合わせを作成します。
        /// </summary>
        /// <remarks>内部参照用です。こちらを指定する必要はありません。</remarks>
        /// <param name="result">doubleの組み合わせ(参照)</param>
        /// <param name="nums">組み合わせを作る配列</param>
        /// <param name="r_remain">参照する残りの個数</param>
        /// <param name="start">参照する始点</param>
        /// <param name="combinationTmp">doubleの組み合わせの内部一時保存([1,2]の部分)</param>
        /// <exception cref="ArgumentException">引数が不正な場合</exception>
        /// <exception cref="ArgumentNullException">引数が不正な場合</exception>
        public static void CreateCombinations(ref List<List<double>> result, double[] nums, int r_remain, int start = 0, List<double> combinationTmp = null)
        {
            if (result == null)
                throw new ArgumentNullException(nameof(result));
            if (!nums.Any())
                throw new ArgumentException("引数がnullまたは個数が不足しています。", nameof(nums));
            if (r_remain < 0)
                throw new ArgumentException("引数が不正です。", nameof(r_remain));
            if (start < 0)
                throw new ArgumentException("引数が不正です。", nameof(start));
            if (combinationTmp == null)
                combinationTmp = new List<double>();

            if (r_remain == 0)//r個の要素を選び終わった時
            {
                result.Add(new List<double>(combinationTmp));
                return;
            }

            for (int i = start; i <= nums.Length - r_remain; i++)
            {
                combinationTmp.Add(nums[i]);
                CreateCombinations(ref result, nums, r_remain - 1, i + 1, combinationTmp);
                combinationTmp.RemoveAt(combinationTmp.Count - 1);
            }
        }

        /// <summary>
        /// nCr(組み合わせの総数)を計算します。
        /// </summary>
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
            long a = 1, b = 1;
            for (int i = 0; i < r; i++)
            {
                a *= n - i;
                b *= r - i;
            }
            return a / b;
        }

        /// <summary>
        /// 複素数配列を文字列(1.2+3.4i,1.2+3.4i等)に変換します。
        /// </summary>
        /// <param name="input">複素数配列</param>
        /// <param name="digits">丸める桁数 -1で無効(既定)</param>
        /// <returns>変換された文字列</returns>
        public static string ComplexArray2StringSimple(Complex[] input, int digits = -1)
        {
            return string.Join(",", input.Select((x, i) => Complex2String(x, digits)));
        }

        /// <summary>
        /// 複素数配列を文字列(1.2+3.4i\n:1.2+3.4i等)に変換します。
        /// </summary>
        /// <param name="input">複素数配列</param>
        /// <param name="digits">丸める桁数 -1で無効(既定)</param>
        /// <returns>変換された文字列</returns>
        public static string ComplexArray2String(Complex[] input, int digits = -1)
        {
            return string.Join("\n", input.Select((x, i) => $"[{i}]:{Complex2String(x, digits)}"));
        }

        /// <summary>
        /// 複素数を文字列(1.2,1.2+3.4i,1.2-3.4i等)に変換します。
        /// </summary>
        /// <param name="input">複素数</param>
        /// <param name="digits">丸める桁数 -1で無効(既定)</param>
        /// <returns>変換された文字列</returns>
        public static string Complex2String(Complex input, int digits = -1)
        {
            double r = digits == -1 ? input.Real : Math.Round(input.Real, digits, MidpointRounding.AwayFromZero);
            double i = digits == -1 ? input.Imaginary : Math.Round(input.Imaginary, digits, MidpointRounding.AwayFromZero);
            if (i == 0)
                return r.ToString();
            else if (i > 0)
                return r.ToString() + "+" + i.ToString();
            else
                return r.ToString() + i.ToString();
        }

        /// <summary>
        /// 二次方程式の解を解の公式より求めます。
        /// </summary>
        /// <remarks>x={-b+-sqrt(b^2-4ac)}/(2a)</remarks>
        /// <param name="a">x^2の係数</param>
        /// <param name="b">xの係数</param>
        /// <param name="c">定数項</param>
        /// <returns>解の複素数配列</returns>
        /// <exception cref="ArgumentException">引数が不正な場合</exception>
        public static Complex[] Equat2_Formula(double a, double b = 0, double c = 0)
        {
            if (a == 0)
                throw new ArgumentException("引数が不正です。", nameof(a), new Exception("最高次数の係数が0になっています。"));
            var tmp1 = Complex.Sqrt(b * b - 4 * a * c);

            var x1 = (-b + tmp1) / (2 * a);
            var x2 = (-b - tmp1) / (2 * a);
            return new Complex[] { x1, x2 };
        }

        /// <summary>
        /// 三次方程式の解を解の公式より求めます。
        /// </summary>
        /// <remarks>ミスがある可能性があります。</remarks>
        /// <param name="coefficients">係数の配列</param>
        /// <returns>解の複素数配列</returns>
        /// <exception cref="ArgumentException">引数が不正な場合</exception>
        public static Complex[] Equat3_Formula(double[] coefficients)
        {
            if (coefficients.Length != 4)
                throw new ArgumentException("引数の個数が不正です。", nameof(coefficients));
            return Equat3_Formula(coefficients[0], coefficients[1], coefficients[2], coefficients[3]);
        }

        /// <summary>
        /// 三次方程式の解を解の公式より求めます。
        /// </summary>
        /// <remarks>ミスがある可能性があります。</remarks>
        /// <param name="a">x^3の係数</param>
        /// <param name="b">x^2の係数</param>
        /// <param name="c">xの係数</param>
        /// <param name="d">定数項</param>
        /// <returns>解の複素数配列</returns>
        /// <exception cref="ArgumentException">引数が不正な場合</exception>
        public static Complex[] Equat3_Formula(double a, double b = 0, double c = 0, double d = 0)
        {
            if (a == 0)
                throw new ArgumentException("引数が不正です。", nameof(a), new Exception("最高次数の係数が0になっています。"));
            var tmp1 = (-2 * b * b * b + 9 * a * b * c - 27 * a * a * d) / (54 * a * a * a);
            var tmp2 = Complex.Sqrt(3 * (27 * a * a * d * d - 18 * a * b * c * d + 4 * a * c * c * c + 4 * b * b * b * d - b * b * c * c)) / (18 * a * a);
            var tmp12p = Complex.Pow(tmp1 + tmp2, oneThird);//3乗根=1/3乗
            var tmp12m = Complex.Pow(tmp1 - tmp2, oneThird);
            var tmp3 = b / (3 * a);
            var tmp4 = new Complex(-1, sqrt3) / 2;
            var tmp5 = new Complex(-1, -sqrt3) / 2;

            var x1 = tmp12p + tmp12m - tmp3;
            var x2 = tmp4 * tmp12p + tmp5 * tmp12m - tmp3;
            var x3 = tmp5 * tmp12p + tmp4 * tmp12m - tmp3;
            Console.WriteLine("----------計算途中情報開始----------");
            Console.WriteLine($"tmp1:{tmp1}");
            Console.WriteLine($"tmp2:{tmp2}");
            Console.WriteLine($"tmp12p:{tmp12p}");
            Console.WriteLine($"tmp12m:{tmp12m}");
            Console.WriteLine($"tmp3:{tmp3}");
            Console.WriteLine($"tmp4:{tmp4}");
            Console.WriteLine($"tmp5:{tmp5}");
            Console.WriteLine("----------計算途中情報終了----------");
            return new Complex[] { x1, x2, x3 };
        }

    }
}