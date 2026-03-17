namespace Unitz.Core;

using System.Numerics;
using System.Runtime.CompilerServices;

/// <summary>
/// Рациональное число вида <c>(Numerator / Denominator) * 10^Exponent10</c>.
/// </summary>
/// <remarks>
/// Назначение: точная арифметика для коэффициентов преобразования единиц, где двоичная
/// и десятичная плавающие точки дают погрешности (например, дроби 5/9 невозможно
/// представить точно в десятичном виде).
/// <para>
/// Нормальная форма хранения:
/// <list type="bullet">
/// <item><description><see cref="Denominator"/> всегда &gt; 0.</description></item>
/// <item><description>Дробь сокращена по НОД.</description></item>
/// <item><description>Все множители 2 и 5 из знаменателя вынесены в <see cref="Exponent10"/>,
/// а соответствующая компенсация перенесена в числитель. Это гарантирует,
/// что эквивалентные представления (например, <c>1/500</c> и <c>2 * 10^-3</c>)
/// приводятся к одному каноническому виду и дают одинаковый хеш.</description></item>
/// </list>
/// Примеры нормализации:
/// <code>
/// 1/500 * 10^0  →  2/1 * 10^-3
/// 1000/1 * 10^0 →  1/1 * 10^3
/// </code>
/// </para>
/// </remarks>
public readonly struct Rational : IEquatable<Rational>
{
    /// <summary>
    /// Аддитивная нейтральная величина: <c>0/1 * 10^0</c>.
    /// </summary>
    public static readonly Rational Zero = new(0, 1);

    /// <summary>
    /// Мультипликативная единица: <c>1/1 * 10^0</c>.
    /// </summary>
    public static readonly Rational One = new(1, 1);

    private const int MaxShift = 18;
    private const int MaxBitWidth = 63;

    private static readonly long[] PrecomputedPow10 =
    [
        1L,
        10L,
        100L,
        1_000L,
        10_000L,
        100_000L,
        1_000_000L,
        10_000_000L,
        100_000_000L,
        1_000_000_000L,
        10_000_000_000L,
        100_000_000_000L,
        1_000_000_000_000L,
        10_000_000_000_000L,
        100_000_000_000_000L,
        1_000_000_000_000_000L,
        10_000_000_000_000_000L,
        100_000_000_000_000_000L,
        1_000_000_000_000_000_000L
    ];

    /// <summary>
    /// Создает рациональное число и приводит его к канонической форме.
    /// </summary>
    /// <param name="numerator">Числитель.</param>
    /// <param name="denominator">Знаменатель.</param>
    /// <param name="exponent10">Десятичная экспонента.</param>
    /// <exception cref="DivideByZeroException">Если <paramref name="denominator"/> равен нулю.</exception>
    /// <exception cref="OverflowException">
    /// При внутренних компенсациях множителей 2 и 5 может произойти переполнение числителя.
    /// </exception>
    public Rational(long numerator, long denominator = 1, int exponent10 = 0)
    {
        if (denominator == 0)
        {
            throw new DivideByZeroException("Знаменатель не может быть равен нулю.");
        }

        if (numerator == 0)
        {
            denominator = 1;
            exponent10 = 0;
        }

        if (denominator < 0)
        {
            numerator = -numerator;
            denominator = -denominator;
        }

        var gcd = GreatestCommonDivisorFast(numerator, denominator);
        if (gcd > 1)
        {
            numerator /= gcd;
            denominator /= gcd;
        }

        int resultExponent10 = exponent10;

        while (numerator != 0 && numerator % 10 == 0)
        {
            numerator /= 10;
            resultExponent10++;
        }

        while (denominator % 10 == 0)
        {
            denominator /= 10;
            resultExponent10--;
        }

        while ((denominator & 1) == 0)
        {
            denominator >>= 1;
            numerator = checked(numerator * 5);
            resultExponent10--;
        }

        while (denominator % 5 == 0)
        {
            denominator /= 5;
            numerator = checked(numerator * 2);
            resultExponent10--;
        }

        this.Numerator = numerator;
        this.Denominator = denominator;
        this.Exponent10 = resultExponent10;
    }

    /// <summary>
    /// Создает рациональное число из <see cref="decimal"/>, извлекая десятичную шкалу в экспоненту.
    /// </summary>
    /// <param name="value">Десятичное число.</param>
    /// <exception cref="OverflowException">Если мантисса не умещается в <see cref="long"/>.</exception>
    public Rational(decimal value)
    {
        var bits = decimal.GetBits(value);
        var scale = (bits[3] >> 16) & 0xFF;
        var isNegative = (bits[3] & unchecked((int)0x80000000)) != 0;

        var low = (uint)bits[0];
        var mid = (uint)bits[1];
        var high = (uint)bits[2];

        BigInteger mantissa = low;
        mantissa |= (BigInteger)mid << 32;
        mantissa |= (BigInteger)high << 64;

        while (mantissa > 0 && mantissa % 10 == 0)
        {
            mantissa /= 10;
            scale--;
        }

        if (mantissa > long.MaxValue)
        {
            throw new OverflowException($"Число {value} слишком велико для {nameof(Rational)}.");
        }

        if (isNegative)
        {
            mantissa = -mantissa;
        }

        this = new Rational((long)mantissa, 1, -scale);
    }

    /// <summary>
    /// Числитель канонической дроби.
    /// </summary>
    public long Numerator { get; }

    /// <summary>
    /// Знаменатель канонической дроби. Всегда положителен и взаимно прост с 10.
    /// </summary>
    public long Denominator { get; }

    /// <summary>
    /// Десятичная экспонента, на которую умножается базовая дробь.
    /// </summary>
    public int Exponent10 { get; }

    /// <summary>
    /// Явное преобразование из <see cref="decimal"/> в <see cref="Rational"/>.
    /// </summary>
    /// <param name="value">Число, которое преобразуется в рациональное.</param>
    public static explicit operator Rational(decimal value)
    {
        return new Rational(value);
    }

    /// <summary>
    /// Явное преобразование из <see cref="long"/> в <see cref="Rational"/>.
    /// </summary>
    /// <param name="value">Число, которое преобразуется в рациональное.</param>
    public static explicit operator Rational(long value)
    {
        return new Rational(value);
    }

    /// <summary>
    /// Явное преобразование из <see cref="Rational"/> в <see cref="decimal"/>.
    /// </summary>
    /// <param name="value">Рациональное число которое преобразуется в <c>decimal</c>.</param>
    public static explicit operator decimal(Rational value)
    {
        var baseDecimal = value.Numerator / (decimal)value.Denominator;
        var scaleDecimal = Pow10Decimal(value.Exponent10);
        return baseDecimal * scaleDecimal;
    }

    /// <summary>
    /// Явное преобразование из <see cref="Rational"/> в <see cref="double"/>.
    /// </summary>
    /// <param name="value">Рациональное число которое преобразуется в <c>double</c>.</param>
    public static explicit operator double(Rational value)
    {
        var baseDouble = value.Numerator / (double)value.Denominator;
        var scaleDouble = Pow10Double(value.Exponent10);
        return baseDouble * scaleDouble;
    }

    /// <summary>
    /// Проверяет два рациональных числа на равенство.
    /// </summary>
    /// <param name="left">Первое число.</param>
    /// <param name="right">Второе число.</param>
    /// <returns>
    /// <c>true</c>, если числа равны, <c>false</c> в противном случае.
    /// </returns>
    public static bool operator ==(Rational left, Rational right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Проверяет два рациональных числа на равенство.
    /// </summary>
    /// <param name="left">Первое число.</param>
    /// <param name="right">Второе число.</param>
    /// <returns>
    /// <c>true</c>, если числа не равны, <c>false</c> в противном случае.
    /// </returns>
    public static bool operator !=(Rational left, Rational right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Складывает два рациональных числа.
    /// </summary>
    /// <param name="left">Первое число.</param>
    /// <param name="right">Второе число.</param>
    /// <returns>
    /// Новое рациональное число являющеся резульатом сложения приведенное к нормальной форме и выровненной экспонентой.
    /// </returns>
    public static Rational operator +(Rational left, Rational right)
    {
        if (left.Numerator == 0)
        {
            return right;
        }

        if (right.Numerator == 0)
        {
            return left;
        }

        AlignExponentsForAdd(left, right, out int targetExponent10, out int leftDeltaExp, out int rightDeltaExp);

        if (leftDeltaExp == 0 && rightDeltaExp == 0 && left.Denominator == right.Denominator)
        {
            var sum = (Int128)left.Numerator + right.Numerator;
            if (sum >= long.MinValue && sum <= long.MaxValue)
            {
                return new Rational((long)sum, left.Denominator, targetExponent10);
            }
        }

        if (leftDeltaExp > MaxShift || rightDeltaExp > MaxShift)
        {
            return BigAdd(left, right);
        }

        var leftScale = Pow10UpTo18(leftDeltaExp);
        var rightScale = Pow10UpTo18(rightDeltaExp);

        if (PossiblyOverflowsOnMultiply(left.Numerator, leftScale) ||
            PossiblyOverflowsOnMultiply(right.Numerator, rightScale))
        {
            return BigAdd(left, right);
        }

        var denominatorsGcd = GreatestCommonDivisorFast(left.Denominator, right.Denominator);
        var leftDenPart = left.Denominator / denominatorsGcd;
        var rightDenPart = right.Denominator / denominatorsGcd;

        var leftCrossGcd = GreatestCommonDivisorFast(left.Numerator, rightDenPart);
        var rightCrossGcd = GreatestCommonDivisorFast(right.Numerator, leftDenPart);

        var reducedLeftNumerator = left.Numerator / leftCrossGcd;
        var reducedRightNumerator = right.Numerator / rightCrossGcd;
        leftDenPart /= rightCrossGcd;
        rightDenPart /= leftCrossGcd;

        var scaledLeftNumerator = (Int128)reducedLeftNumerator * leftScale;
        var scaledRightNumerator = (Int128)reducedRightNumerator * rightScale;

        var leftTerm = scaledLeftNumerator * rightDenPart;
        var rightTerm = scaledRightNumerator * leftDenPart;

        var resultNumerator = leftTerm + rightTerm;
        var resultDenominator = (Int128)leftDenPart * right.Denominator;

        if (!FitsInt64(resultNumerator) || !FitsPositiveInt64(resultDenominator))
        {
            return BigAdd(left, right);
        }

        return new Rational((long)resultNumerator, (long)resultDenominator, targetExponent10);
    }

    /// <summary>
    /// Вычитает два рациональных числа.
    /// </summary>
    /// <param name="left">Первое число.</param>
    /// <param name="right">Второе число.</param>
    /// <returns>
    /// Новое рациональное число являющееся разностью двух чисел, приведенное к нормальной форме и выровненной экспонентой.
    /// </returns>
    public static Rational operator -(Rational left, Rational right)
    {
        return left + new Rational(-right.Numerator, right.Denominator, right.Exponent10);
    }

    /// <summary>
    /// Находит произведение  дву рациональных чисел.
    /// </summary>
    /// <param name="left">Первое число.</param>
    /// <param name="right">Второе число.</param>
    /// <returns>
    /// Новое рациональное число являющеюся произведением двух чисел, приведенное к нормальной форме и выровненной экспонентой.
    /// </returns>
    public static Rational operator *(Rational left, Rational right)
    {
        if (left.Numerator == 0 || right.Numerator == 0)
        {
            return Zero;
        }

        if (!TryAddInt32(left.Exponent10, right.Exponent10, out int resultExponent10))
        {
            return BigMul(left, right);
        }

        if (PossiblyOverflowsOnMultiply(left.Numerator, right.Numerator) ||
            PossiblyOverflowsOnMultiply(left.Denominator, right.Denominator))
        {
            return BigMul(left, right);
        }

        var rawNumerator = (Int128)left.Numerator * right.Numerator;
        var rawDenominator = (Int128)left.Denominator * right.Denominator;

        if (!FitsInt64(rawNumerator) || !FitsPositiveInt64(rawDenominator))
        {
            return BigMul(left, right);
        }

        return new Rational((long)rawNumerator, (long)rawDenominator, resultExponent10);
    }

    /// <summary>
    /// Деление рациональных чисел.
    /// </summary>
    /// <param name="left">Делимое.</param>
    /// <param name="right">Делитель.</param>
    /// <returns>
    /// Новое рациональное число являющеюся частным двух чисел, приведенное к нормальной форме и выровненной экспонентой.
    /// </returns>
    public static Rational operator /(Rational left, Rational right)
    {
        if (right.Numerator == 0)
        {
            throw new DivideByZeroException();
        }

        if (left.Numerator == 0)
        {
            return Zero;
        }
        
        // "переворачиваем" делитель
        var divisor = new Rational(right.Denominator, right.Numerator, -right.Exponent10);
        return left * divisor;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Rational other)
    {
        return this.Numerator == other.Numerator
               && this.Denominator == other.Denominator
               && this.Exponent10 == other.Exponent10;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is Rational r && this.Equals(r);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(this.Numerator, this.Denominator, this.Exponent10);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{this.Numerator}/{this.Denominator} * 10^{this.Exponent10}";
    }

    private static void AlignExponentsForAdd(
        Rational left,
        Rational right,
        out int targetExponent10,
        out int leftDeltaExp,
        out int rightDeltaExp)
    {
        targetExponent10 = Math.Min(left.Exponent10, right.Exponent10);
        leftDeltaExp = left.Exponent10 - targetExponent10;
        rightDeltaExp = right.Exponent10 - targetExponent10;

        int maxShift = Math.Max(leftDeltaExp, rightDeltaExp);
        if (maxShift > MaxShift)
        {
            int reduce = maxShift - MaxShift;
            targetExponent10 += reduce;
            leftDeltaExp -= reduce;
            rightDeltaExp -= reduce;
        }
    }

    private static Rational BigAdd(Rational left, Rational right)
    {
        AlignExponentsForAdd(left, right, out int targetExponent10, out int leftDeltaExp, out int rightDeltaExp);

        (BigInteger leftNum, BigInteger leftDen) = ToBigScaled(left, leftDeltaExp);
        (BigInteger rightNum, BigInteger rightDen) = ToBigScaled(right, rightDeltaExp);

        BigInteger denGcd = BigInteger.GreatestCommonDivisor(leftDen, rightDen);
        BigInteger leftDenPart = leftDen / denGcd;
        BigInteger rightDenPart = rightDen / denGcd;

        BigInteger resultNum = (leftNum * rightDenPart) + (rightNum * leftDenPart);
        BigInteger resultDen = leftDenPart * rightDen;

        return FromBig(resultNum, resultDen, targetExponent10);
    }

    private static Rational BigMul(Rational left, Rational right)
    {
        BigInteger leftNum = new(left.Numerator);
        BigInteger leftDen = new(left.Denominator);
        BigInteger rightNum = new(right.Numerator);
        BigInteger rightDen = new(right.Denominator);

        BigInteger g1 = BigInteger.GreatestCommonDivisor(BigInteger.Abs(leftNum), rightDen);
        BigInteger g2 = BigInteger.GreatestCommonDivisor(BigInteger.Abs(rightNum), leftDen);

        leftNum /= g1;
        rightDen /= g1;
        rightNum /= g2;
        leftDen /= g2;

        BigInteger resultNum = leftNum * rightNum;
        BigInteger resultDen = leftDen * rightDen;

        int resultExponent10 = checked(left.Exponent10 + right.Exponent10);

        return FromBig(resultNum, resultDen, resultExponent10);
    }

    private static (BigInteger Num, BigInteger Den) ToBigScaled(Rational value, int exponentDelta)
    {
        if (exponentDelta == 0)
        {
            return (new BigInteger(value.Numerator), new BigInteger(value.Denominator));
        }

        BigInteger scale = BigInteger.Pow(10, exponentDelta);
        return (new BigInteger(value.Numerator) * scale, new BigInteger(value.Denominator));
    }

    private static Rational FromBig(BigInteger inputNumerator, BigInteger inputDenominator, int exponent10)
    {
        if (inputDenominator.Sign == 0)
        {
            throw new DivideByZeroException();
        }

        if (inputDenominator.Sign < 0)
        {
            inputNumerator = BigInteger.Negate(inputNumerator);
            inputDenominator = BigInteger.Negate(inputDenominator);
        }

        BigInteger finalGcd = BigInteger.GreatestCommonDivisor(BigInteger.Abs(inputNumerator), inputDenominator);
        if (finalGcd > 1)
        {
            inputNumerator /= finalGcd;
            inputDenominator /= finalGcd;
        }

        while (inputDenominator % 10 == 0)
        {
            inputDenominator /= 10;
            exponent10--;
        }

        while (inputNumerator % 10 == 0 && inputNumerator != 0)
        {
            inputNumerator /= 10;
            exponent10++;
        }

        while ((inputDenominator & 1) == 0)
        {
            inputDenominator >>= 1;
            inputNumerator *= 5;
            exponent10--;
        }

        while (inputDenominator % 5 == 0)
        {
            inputDenominator /= 5;
            inputNumerator *= 2;
            exponent10--;
        }

        if (inputNumerator >= long.MinValue
            && inputNumerator <= long.MaxValue
            && inputDenominator >= 1
            && inputDenominator <= long.MaxValue)
        {
            return new Rational((long)inputNumerator, (long)inputDenominator, exponent10);
        }

        throw new OverflowException(
            $"Число {inputNumerator}/{inputDenominator} слишком велико для {nameof(Rational)}.");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static long Pow10UpTo18(int exponent)
    {
        if (exponent < 0 || exponent > MaxShift)
        {
            throw new ArgumentOutOfRangeException(nameof(exponent));
        }

        return PrecomputedPow10[exponent];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static decimal Pow10Decimal(int exponent)
    {
        if (exponent == 0)
        {
            return 1m;
        }

        decimal power = 1m;

        if (exponent > 0)
        {
            for (int i = 0; i < exponent; i++)
            {
                power *= 10m;
            }
        }
        else
        {
            for (int i = 0; i < -exponent; i++)
            {
                power /= 10m;
            }
        }

        return power;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static double Pow10Double(int exponent)
    {
        if (exponent == 0)
        {
            return 1.0;
        }

        var power = 1.0;

        if (exponent > 0)
        {
            for (var i = 0; i < exponent; i++)
            {
                power *= 10.0;
            }
        }
        else
        {
            for (var i = 0; i < -exponent; i++)
            {
                power /= 10.0;
            }
        }

        return power;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static long GreatestCommonDivisorFast(long left, long right)
    {
        ulong leftAbs = Abs64(left);
        ulong rightAbs = Abs64(right);

        if (leftAbs == 0)
        {
            return (long)rightAbs;
        }

        if (rightAbs == 0)
        {
            return (long)leftAbs;
        }

        int commonShift = BitOperations.TrailingZeroCount(leftAbs | rightAbs);

        leftAbs >>= BitOperations.TrailingZeroCount(leftAbs);

        do
        {
            rightAbs >>= BitOperations.TrailingZeroCount(rightAbs);

            if (leftAbs > rightAbs)
            {
                (leftAbs, rightAbs) = (rightAbs, leftAbs);
            }

            rightAbs -= leftAbs;
        }
        while (rightAbs != 0);

        return (long)(leftAbs << commonShift);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool FitsInt64(Int128 value)
    {
        return value >= long.MinValue && value <= long.MaxValue;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool FitsPositiveInt64(Int128 value)
    {
        return value >= 1 && value <= long.MaxValue;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool TryAddInt32(int left, int right, out int sum)
    {
        long temp = (long)left + right;

        if (temp > int.MaxValue || temp < int.MinValue)
        {
            sum = 0;
            return false;
        }

        sum = (int)temp;
        return true;
    }

    /// <summary>
    /// Проверяет, может ли умножение <paramref name="a"/> на <paramref name="b"/> вести к переполнению <see cref="long"/>.
    /// </summary>
    /// <remarks>
    /// Возможны ложно-положительные срабатывания на границе диапазона: в этом случае
    /// происходит переход на <see cref="BigInteger"/>, что безопасно.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool PossiblyOverflowsOnMultiply(long a, long b)
    {
        if (a == 0 || b == 0)
        {
            return false;
        }

        if (a == 1 || b == 1)
        {
            return false;
        }

        if (a == -1)
        {
            return b == long.MinValue;
        }

        if (b == -1)
        {
            return a == long.MinValue;
        }

        if (a == long.MinValue || b == long.MinValue)
        {
            return true;
        }

        var absA = (ulong)(a < 0 ? -a : a);
        var absB = (ulong)(b < 0 ? -b : b);

        var bitsA = 63 - BitOperations.LeadingZeroCount(absA);
        var bitsB = 63 - BitOperations.LeadingZeroCount(absB);

        return bitsA + bitsB >= MaxBitWidth;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong Abs64(long value)
    {
        if (value >= 0)
        {
            return (ulong)value;
        }

        return (ulong)(~value + 1);
    }
}