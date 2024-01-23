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
                else if (degree == 1)//xの項
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
        /// 複素数配列を文字列([1]:1.2+3.4i\n[1]:1.2+3.4i等)に変換します。
        /// </summary>
        /// <param name="input">複素数配列</param>
        /// <returns>変換された文字列</returns>
        public static string ComplexArray2String(Complex[] input)
        {
            return string.Join(Environment.NewLine, input.Select((x, i) => $"[{i}]:{Complex2String(x)}"));
        }

        /// <summary>
        /// 複素数を文字列(1.2,1.2+3.4i,1.2-3.4i等)に変換します。
        /// </summary>
        /// <param name="input">複素数</param>
        /// <returns>変換された文字列</returns>
        public static string Complex2String(Complex input)
        {
            if (input.Imaginary == 0)
                return input.Real.ToString();
            else if (input.Imaginary > 0)
                return input.Real.ToString() + "+" + input.Imaginary.ToString();
            else
                return input.Real.ToString() + input.Imaginary.ToString();
        }

        /// <summary>
        /// 二次方程式の解を解の公式より求めます。
        /// </summary>
        /// <remarks>x={-b+-sqrt(b^2-4ac)}/(2a)</remarks>
        /// <param name="c1">x^2の係数</param>
        /// <param name="c2">xの係数</param>
        /// <param name="c3">定数項</param>
        /// <returns>解の複素数配列</returns>
        public static Complex[] Equat2_Formula(double c1, double c2 = 0, double c3 = 0)
        {
            var root = Complex.Sqrt(c2 * c2 - 4 * c1 * c3);//ルート部分
            var x1 = (-c2 + root) / (2 * c1);
            var x2 = (-c2 - root) / (2 * c1);
            return new Complex[] { x1, x2 };
        }
    }
}