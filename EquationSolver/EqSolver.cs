using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace EquationSolver
{
    public static class EquationSolver
    {
        /// <summary>
        /// ルート3
        /// </summary>
        public static readonly double sqrt3 = Math.Sqrt(3);
        /// <summary>
        /// 三分の一
        /// </summary>
        public static readonly double oneThird = 1d / 3d;

        /// <summary>
        /// 解から方程式の係数の配列を生成します。虚数解には対応してません。
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
                else if (coefficient == -1)
                    eqStB.Append("-");
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
            long a = 1, b = 1;
            for (int i = 0; i < r; i++)
            {
                a *= n - i;
                b *= r - i;
            }
            return a / b;
        }

        /// <summary>
        /// 複素数配列を単純文字列(1.2+3.4i,1.2+3.4i等)に変換します。
        /// </summary>
        /// <param name="input">複素数配列</param>
        /// <param name="digits">丸める桁数 -1で無効(既定)</param>
        /// <returns>変換された文字列</returns>
        public static string ComplexArray2StringSimple(Complex[] input, int digits = -1)
        {
            return string.Join(",", input.Select((x, i) => Complex2String(x, digits)));
        }

        /// <summary>
        /// 複素数配列を改行あり文字列([0]:1.2+3.4i\n[1]:1.2+3.4i等)に変換します。
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
            var r = digits == -1 ? input.Real : Math.Round(input.Real, digits, MidpointRounding.AwayFromZero);
            var i = digits == -1 ? input.Imaginary : Math.Round(input.Imaginary, digits, MidpointRounding.AwayFromZero);
            if (i == 0)
                return r.ToString();
            else if (i > 0)
                return r.ToString() + "+" + i.ToString() + "i";
            else
                return r.ToString() + i.ToString() + "i";
        }

        /// <summary>
        /// 二次方程式の解を解の公式より求めます。
        /// </summary>
        /// <remarks>x={-b+-sqrt(b^2-4ac)}/(2a)</remarks>
        /// <param name="coefficients">係数の配列</param>
        /// <param name="outputCalInfo">計算途中情報を出力するか</param>
        /// <returns>解の複素数配列</returns>
        /// <exception cref="ArgumentException">引数が不正な場合</exception>
        public static Complex[] Equat2_Formula(double[] coefficients, bool outputCalInfo = false)
        {
            if (coefficients.Length != 3)
                throw new ArgumentException("引数の個数が不正です。", nameof(coefficients));
            double a = coefficients[0], b = coefficients[1], c = coefficients[2];
            if (a == 0)
                throw new ArgumentException("引数が不正です。", nameof(a), new Exception("最高次数の係数が0になっています。"));
            var tmp1 = Complex.Pow(b * b - 4 * a * c, 0.5);

            if (outputCalInfo)
            {
                Console.WriteLine("----------計算途中情報開始----------");
                Console.WriteLine($"tmp1:{tmp1}");
                Console.WriteLine("----------計算途中情報終了----------");
            }
            var x1 = (-b + tmp1) / (2 * a);
            var x2 = (-b - tmp1) / (2 * a);
            return new Complex[] { x1, x2 };
        }

        /// <summary>
        /// 三次方程式の解を解の公式(独自算出)より求めます。
        /// </summary>
        /// <remarks>ミスがある可能性があります。</remarks>
        /// <param name="coefficients">係数の配列</param>
        /// <param name="outputCalInfo">計算途中情報を出力するか</param>
        /// <returns>解の複素数配列</returns>
        /// <exception cref="ArgumentException">引数が不正な場合</exception>
        public static Complex[] Equat3_Formula(double[] coefficients, bool outputCalInfo = false)
        {
            if (coefficients.Length != 4)
                throw new ArgumentException("引数の個数が不正です。", nameof(coefficients));
            double a = coefficients[0], b = coefficients[1], c = coefficients[2], d = coefficients[3];
            if (a == 0)
                throw new ArgumentException("引数が不正です。", nameof(a), new Exception("最高次数の係数が0になっています。"));

            var tmp1 = (-2 * b * b * b + 9 * a * b * c - 27 * a * a * d) / (54 * a * a * a);
            var tmp2 = Complex.Pow(3 * (27 * a * a * d * d - 18 * a * b * c * d + 4 * a * c * c * c + 4 * b * b * b * d - b * b * c * c), 0.5) / (18 * a * a);
            var tmp12p = Complex.Pow(tmp1 + tmp2, oneThird);
            var tmp12m = Complex.Pow(tmp1 - tmp2, oneThird);
            var tmp3 = b / (3 * a);
            var tmp4 = new Complex(-1, sqrt3) / 2;
            var tmp5 = new Complex(-1, -sqrt3) / 2;

            if (outputCalInfo)
            {
                Console.WriteLine("----------計算途中情報開始----------");
                Console.WriteLine($"tmp1:{tmp1}");
                Console.WriteLine($"tmp2:{tmp2}");
                Console.WriteLine($"tmp12p:{tmp12p}");
                Console.WriteLine($"tmp12m:{tmp12m}");
                Console.WriteLine($"tmp3:{tmp3}");
                Console.WriteLine($"tmp4:{tmp4}");
                Console.WriteLine($"tmp5:{tmp5}");
                Console.WriteLine("----------計算途中情報終了----------");
            }
            var x1 = tmp12p + tmp12m - tmp3;
            var x2 = tmp4 * tmp12p + tmp5 * tmp12m - tmp3;
            var x3 = tmp5 * tmp12p + tmp4 * tmp12m - tmp3;
            return new Complex[] { x1, x2, x3 };
        }

        /// <summary>
        /// 四次方程式の解を解の公式(独自算出)より求めます。
        /// </summary>
        /// <remarks>ミスがある可能性があります。</remarks>
        /// <param name="coefficients">係数の配列</param>
        /// <param name="outputCalInfo">計算途中情報を出力するか</param>
        /// <returns>解の複素数配列</returns>
        /// <exception cref="ArgumentException">引数が不正な場合</exception>
        public static Complex[] Equat4_Formula(double[] coefficients, bool outputCalInfo = false)
        {
            Console.WriteLine("[警告]未完成です。");
            if (coefficients.Length != 5)
                throw new ArgumentException("引数の個数が不正です。", nameof(coefficients));
            double a = coefficients[0], b = coefficients[1], c = coefficients[2], d = coefficients[3], e = coefficients[4];
            if (a == 0)
                throw new ArgumentException("引数が不正です。", nameof(a), new Exception("最高次数の係数が0になっています。"));

            var tmp0 = (3 * b * b - 8 * a * c) / (12 * a * a);//12a^2で割る方
            var tmp1 = (2 * c * c * c - 72 * a * c * e + 27 * b * b * e + 27 * a * d * d - 9 * b * c * d) / (54 * a * a * a);
            var tmp2 = Complex.Sqrt(3 * (
                -256 * a * a * a * e * e * e + 192 * a * a * b * d * e * e + 128 * a * a * c * c * e * e - 144 * a * b * b * c * e * e
                + 27 * b * b * b * b * e * e - 144 * a * a * c * d * d * e + 6 * a * b * b * d * d * e + 80 * a * b * c * c * d * e
                - 18 * b * b * b * c * d * e - 16 * a * c * c * c * c * e + 4 * b * b * c * c * c * e + 27 * a * a * d * d * d * d
                - 18 * a * b * c * d * d * d + 4 * b * b * b * d * d * d + 4 * a * c * c * c * d * d - b * b * c * c * d * d)) / (18 * a * a * a);
            var tmp12p = Complex.Pow(tmp1 + tmp2, oneThird);
            var tmp12m = Complex.Pow(tmp1 - tmp2, oneThird);
            var tmp1212p = tmp12p + tmp12m;
            var tmp01212p_sq = Complex.Pow(tmp0 + tmp1212p, 0.5);
            var tmp01212p_msq = Complex.Pow(tmp0 + tmp1212p, -0.5);
            var tmp3 = b / (4 * a);
            var tmp_unknown = 1*0;//2gの部分(未確認)//0にすると1,2,3,4で合う

            if (outputCalInfo)
            {
                Console.WriteLine("----------計算途中情報開始----------");
                Console.WriteLine($"tmp0:{tmp0}");
                Console.WriteLine($"tmp1:{tmp1}");
                Console.WriteLine($"tmp2:{tmp2}");
                Console.WriteLine($"tmp12p:{tmp12p}");
                Console.WriteLine($"tmp12m:{tmp12m}");
                Console.WriteLine($"tmp1212p:{tmp1212p}");
                Console.WriteLine($"tmp1212p_sq:{tmp01212p_sq}");
                Console.WriteLine($"tmp1212p_msq:{tmp01212p_msq}");
                Console.WriteLine("----------計算途中情報終了----------");
            }
            var x1 = 0.5 * (-tmp01212p_sq + Complex.Pow(tmp0 * 2 - tmp1212p + tmp_unknown * tmp01212p_msq, 0.5));// - tmp3?
            var x2 = 0.5 * (-tmp01212p_sq - Complex.Pow(tmp0 * 2 - tmp1212p + tmp_unknown * tmp01212p_msq, 0.5));
            var x3 = 0.5 * (tmp01212p_sq + Complex.Pow(tmp0 * 2 - tmp1212p - tmp_unknown * tmp01212p_msq, 0.5));
            var x4 = 0.5 * (tmp01212p_sq - Complex.Pow(tmp0 * 2 - tmp1212p - tmp_unknown * tmp01212p_msq, 0.5));
            return new Complex[] { x1, x2, x3, x4 };
        }
    }
}