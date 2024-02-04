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
        /// 1の立方根(+√3i)
        /// </summary>
        public static readonly Complex omegaP = new Complex(-1, sqrt3) / 2;
        /// <summary>
        /// 1の立方根(-√3i)
        /// </summary>
        public static readonly Complex omegaM = new Complex(-1, -sqrt3) / 2;

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
                var coefComb = CreateCombinations(roots, i);//係数ごとに定数部分の組み合わせを作る
                foreach (var combi_tmp in coefComb)
                {
                    var coefficientTmp = 1d;
                    foreach (var value in combi_tmp)//[1,2,3]内を掛ける//
                        coefficientTmp *= -value;
                    coefficients[i] += coefficientTmp;//掛けたものを足す
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

            var count = coefficients.Length;
            var eqStB = new StringBuilder();
            for (int i = 0; i < count; i++)//(count-i-1)次の項
            {
                var degree = count - i - 1;//次数
                var coefficient = coefficients[i];//x^(count-i-1)の係数
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
        /// doubleの組み合わせ(ここでは定数の組み合わせ([[1,2],[1,3],[2,3]]等))を作ります。
        /// </summary>
        /// <remarks>ここではx=1,2,3 => (x-1)(x-2)(x-3)で、[1,2]などとなるため注意してください</remarks>
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
                CreateCombinations(ref result, nums, r_remain - 1, i + 1, combinationTmp);//残りの回数、インデックスを次のものになるようにして再帰的に呼び出し
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
            if (r == -0)
                r = 0;
            if (i == -0)
                i = 0;
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
            var tmp12p = ComplexCbrt(tmp1 + tmp2);
            var tmp12m = ComplexCbrt(tmp1 - tmp2);
            var tmp3 = b / (3 * a);

            if (outputCalInfo)
            {
                Console.WriteLine("----------計算途中情報開始----------@Equat3_Formula");
                Console.WriteLine($"tmp1:{tmp1}");
                Console.WriteLine($"tmp2:{tmp2}");
                Console.WriteLine($"tmp12p:{tmp12p}");
                Console.WriteLine($"tmp12m:{tmp12m}");
                Console.WriteLine($"tmp3:{tmp3}");
                Console.WriteLine("----------計算途中情報終了----------@Equat3_Formula");
            }
            var x1 = tmp12p + tmp12m - tmp3;
            var x2 = omegaP * tmp12p + omegaM * tmp12m - tmp3;
            var x3 = omegaM * tmp12p + omegaP * tmp12m - tmp3;
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
            if (coefficients.Length != 5)
                throw new ArgumentException("引数の個数が不正です。", nameof(coefficients));
            double a = coefficients[0], b = coefficients[1], c = coefficients[2], d = coefficients[3], e = coefficients[4];
            if (a == 0)
                throw new ArgumentException("引数が不正です。", nameof(a), new Exception("最高次数の係数が0になっています。"));

            var tmp0 = (3 * b * b - 8 * a * c) / (12 * a * a);//12a^2で割る方
            var tmp1 = (2 * c * c * c - 72 * a * c * e + 27 * b * b * e + 27 * a * d * d - 9 * b * c * d) / (54 * a * a * a);
            var tmp2 = Complex.Pow(3 * (
                -256 * a * a * a * e * e * e + 192 * a * a * b * d * e * e + 128 * a * a * c * c * e * e - 144 * a * b * b * c * e * e
                + 27 * b * b * b * b * e * e - 144 * a * a * c * d * d * e + 6 * a * b * b * d * d * e + 80 * a * b * c * c * d * e
                - 18 * b * b * b * c * d * e - 16 * a * c * c * c * c * e + 4 * b * b * c * c * c * e + 27 * a * a * d * d * d * d
                - 18 * a * b * c * d * d * d + 4 * b * b * b * d * d * d + 4 * a * c * c * c * d * d - b * b * c * c * d * d
                ), 0.5) / (18 * a * a * a);
            var tmp12p = ComplexCbrt(tmp1 + tmp2);
            var tmp12m = ComplexCbrt(tmp1 - tmp2);
            var tmp1212p = tmp12p + tmp12m;
            var tmp01212p_sq = Complex.Pow(tmp0 + tmp1212p, 0.5);
            var tmp01212p_msq = Complex.Pow(tmp0 + tmp1212p, -0.5);
            var tmp3 = (8 * a * a * d - 4 * a * b * c + b * b * b) / (4 * a * a * a);
            var tmp4 = b / (2 * a);

            if (outputCalInfo)
            {
                Console.WriteLine("----------計算途中情報開始----------");
                Console.WriteLine($"tmp0:{tmp0}");
                Console.WriteLine($"tmp1:{tmp1}");
                Console.WriteLine($"tmp2:{tmp2}");
                Console.WriteLine($"tmp12p:{tmp12p}");
                Console.WriteLine($"tmp12m:{tmp12m}");
                Console.WriteLine($"tmp1212p:{tmp1212p}");
                Console.WriteLine($"tmp01212p_sq:{tmp01212p_sq}");
                Console.WriteLine($"tmp01212p_msq:{tmp01212p_msq}");
                Console.WriteLine($"tmp3:{tmp3}");
                Console.WriteLine($"tmp4:{tmp4}");
                Console.WriteLine("----------計算途中情報終了----------");
            }
            var x1 = 0.5 * (-tmp01212p_sq + Complex.Pow(tmp0 * 2 - tmp1212p + tmp3 * tmp01212p_msq, 0.5) - tmp4);
            var x2 = 0.5 * (-tmp01212p_sq - Complex.Pow(tmp0 * 2 - tmp1212p + tmp3 * tmp01212p_msq, 0.5) - tmp4);
            var x3 = 0.5 * (tmp01212p_sq + Complex.Pow(tmp0 * 2 - tmp1212p - tmp3 * tmp01212p_msq, 0.5) - tmp4);
            var x4 = 0.5 * (tmp01212p_sq - Complex.Pow(tmp0 * 2 - tmp1212p - tmp3 * tmp01212p_msq, 0.5) - tmp4);
            return new Complex[] { x1, x2, x3, x4 };
        }

        /// <summary>
        /// Complexの三乗根を求めます。
        /// </summary>
        /// <remarks>Complex.Powで複素数になる問題を回避するものです。</remarks>
        /// <param name="input">値</param>
        /// <returns>三乗根(分数化してできなければComplex.Powの方)</returns>
        public static Complex ComplexCbrt(Complex input)
        {
            //Console.WriteLine(input);
            if (input.Imaginary == 0)
            {
                var fra = new Fraction(input.Real, out double err);
                //Console.WriteLine($"{fra} err:{err}");
                bool convAble = fra.TryRdrt(out Fraction fraCbrt, 3);
                //Console.WriteLine($"cbrt:{convAble} val:{fraCbrt}");
                if (convAble)
                    return new Complex(fraCbrt.ToDouble(), 0);
            }
            return Complex.Pow(input, oneThird);
        }

        /// <summary>
        /// ユークリッドの互除法で最大公約数を求めます。
        /// </summary>
        /// <param name="a">値1</param>
        /// <param name="b">値2</param>
        /// <returns>最大公約数</returns>
        public static long GCD(long a, long b)
        {
            //Console.Write($"GCD  a:{a} b:{b} -> ");
            while (b != 0)
                (a, b) = (b, a % b);
            //Console.WriteLine(a);
            return a;
        }
    }

    /// <summary>
    /// 簡単な分数の構造体
    /// </summary>
    public struct Fraction
    {
        /// <summary>
        /// 分母
        /// </summary>
        public long denominator;
        /// <summary>
        /// 分子
        /// </summary>
        public long numerator;

        /// <summary>
        /// 分母と分子を指定して初期化します。自動で通分されます。
        /// </summary>
        /// <param name="denominator">分母</param>
        /// <param name="numerator">分子</param>
        /// <exception cref="ArgumentException">引数が不正な場合</exception>
        public Fraction(long denominator, long numerator)
        {
            if (denominator == 0)
                throw new ArgumentException("引数が不正です。分母を0にすることはできません。", nameof(denominator));
            this.denominator = denominator;
            this.numerator = numerator;
            //Reduce();
        }

        /// <summary>
        /// 小数から分数へ変換して初期化します。
        /// </summary>
        /// <remarks>失敗時-1/-1となります。</remarks>
        /// <param name="input">変換する小数</param>
        /// <param name="error">判定時の誤差(out)失敗時NaN</param>
        /// <param name="maxDenominator">分母の最大</param>
        /// <param name="toleranceLevel">計算した分数での小数と変換する小数の差の許容範囲</param>
        public Fraction(double input, out double error, int maxDenominator = 99999, double toleranceLevel = 0.00001)
        {
            for (int d = 2; d <= maxDenominator; d++)//input=1.6,d=2の例を以下に
            {
                var inpAbs = Math.Abs(input);
                var nearD = Math.Max(Math.Round(inpAbs / (1d / d)), 1);//inputを割ると余りが0に近い数字(0以上)//1.6/0.5=>3.2=>3
                //Console.WriteLine($"{input}/{d}->{nearD} {Math.Abs(inpAbs - (nearD / d))}");//動作確認用
                error = inpAbs - (nearD / d);
                if (Math.Abs(error) <= toleranceLevel)//1.6-3/2=>0.1=>false
                {
                    denominator = d;
                    if (input < 0)
                        numerator = -(int)nearD;
                    else
                        numerator = (int)nearD;
                    Reduce();
                    return;
                }
            }
            error = double.NaN;
            denominator = -1;
            numerator = -1;
        }

        /// <summary>
        /// 通分します。
        /// </summary>
        public void Reduce()
        {
            if (denominator == -1 && numerator == -1 || denominator == long.MinValue || numerator == long.MinValue)
                return;
            if (denominator < 0)
            {
                denominator = -denominator;
                numerator = -numerator;
            }
            var gcd = EquationSolver.GCD(denominator, Math.Abs(numerator));
            //Console.Write($"Reduce {this} / {gcd} -> ");
            denominator /= gcd;
            numerator /= gcd;
            //Console.WriteLine(this);
        }

        /// <summary>
        /// 値が不正でないか確認します。
        /// </summary>
        /// <returns>正常値の場合true、不正な値の場合false</returns>
        public bool IsNormalValue()
        {
            if (denominator > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 1/3,-2/5のような形式のstringに変換します
        /// </summary>
        /// <returns>stringに変換された分数　不正な場合NaN</returns>
        public override string ToString()
        {
            if (denominator > 0)
                return $"{numerator}/{denominator}";
            else
                return "NaN";
        }

        /// <summary>
        /// 小数に変換します。
        /// </summary>
        /// <returns>分数と同等の小数　不正な場合double.NaN</returns>
        public double ToDouble()
        {
            if (denominator > 0)
                return (double)numerator / denominator;
            else
                return double.NaN;
        }

        /// <summary>
        /// 累乗根を求めます。
        /// </summary>
        /// <param name="result">求めた分数、失敗した場合-1/-1</param>
        /// <param name="exp">累乗根の次数</param>
        /// <param name="maxTry">最大挑戦回数(分母と分子がこのexp乗と同じか判定します)</param>
        /// <returns>変換に成功した場合true、失敗した場合false</returns>
        public bool TryRdrt(out Fraction result, int exp, int maxTry = 20)
        {
            if (denominator < 0)
            {
                result = new Fraction(-1, -1);
                return false;
            }
            if (denominator == Math.Abs(numerator))
            {
                result = new Fraction(1, numerator / Math.Abs(numerator));//-2/2=-1
                return true;
            }

            long denTmp = long.MinValue, numTmp = long.MinValue;
            bool denUnOK = true, numUnOK = true;

            for (int i = 1; i <= maxTry && (denUnOK || numUnOK); i++)
            {
                long pow = (long)Math.Pow(i, exp);

                if (denUnOK)
                {
                    if (pow == denominator)
                    {
                        denTmp = i;
                        denUnOK = false;
                    }
                    else if (pow > denominator)
                        denUnOK = false;
                }
                if (numUnOK)
                {
                    if (pow == numerator)
                    {
                        numTmp = i;
                        numUnOK = false;
                    }
                    else if (pow == -numerator)
                    {
                        numTmp = -i;
                        numUnOK = false;
                    }
                    else if (pow > Math.Abs(numerator))
                        numUnOK = false;
                }
            }

            //Console.WriteLine($"{this} -> denTmp:{denTmp} numTmp:{numTmp}");
            if (denUnOK && numUnOK)
            {
                result = new Fraction(denTmp, numTmp);
                return true;
            }
            else
            {
                result = new Fraction(-1, -1);
                return false;
            }
        }
    }
}