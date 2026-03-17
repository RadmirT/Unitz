namespace Unitz.Core;

using System.Text;

/// <summary>
/// Представляет единицу измерения физической величины.
/// </summary>
/// <remarks>
/// Единица измерения определяется размерностью, множителем к базовой единице и смещением относительно нуля базовой шкалы.
/// Единицы с ненулевым смещением (например, градусы Цельсия или Фаренгейта) называются аффинными.
/// Для таких единиц арифметические операции над значениями имеют смысл только после приведения к абсолютной шкале (без смещения),
/// иначе результаты будут физически некорректны.
/// <para>
/// Например, 20 C соответствуют 68 F. Если просто умножить эти значения на 2, получим 40 C и 136 F — но эти температуры уже
/// не эквивалентны. Корректное преобразование должно выполняться через абсолютные значения (в данном случае в Кельвинах).
/// </para>
/// </remarks>
public class Unit : IEquatable<Unit>
{
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="Unit"/> с указанными параметрами.
    /// </summary>
    /// <param name="name">Инвариантное имя единицы.</param>
    /// <param name="symbol">Символ единицы.</param>
    /// <param name="dimension">Размерность единицы.</param>
    /// <param name="scale">Множитель к базовой единице измерения.</param>
    /// <param name="offset">Смещение относительно нуля базовой единицы измерения.</param>
    public Unit(string? name, string? symbol, Dimension dimension, decimal scale = 1, decimal offset = 0)
        : this(name, symbol, dimension, new Rational(scale), new Rational(offset))
    {
    }

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="Unit"/> для указанной размерности.
    /// </summary>
    /// <param name="dimension">Размерность единицы измерения.</param>
    /// <param name="scale">Множитель к базовой единице измерения.</param>
    /// <param name="offset">Смещение относительно нуля базовой единицы измерения.</param>
    public Unit(Dimension dimension, decimal scale = 1, decimal offset = 0)
        : this(null, null, dimension, new Rational(scale), new Rational(offset))
    {
    }

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="Unit"/> с рациональными параметрами.
    /// </summary>
    /// <param name="dimension">Размерность единицы измерения.</param>
    /// <param name="scale">Множитель к базовой единице измерения.</param>
    /// <param name="offset">Смещение относительно нуля базовой единицы измерения.</param>
    public Unit(Dimension dimension, Rational scale, Rational offset)
        : this(null, null, dimension, scale, offset)
    {
    }

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="Unit"/>.
    /// </summary>
    /// <param name="name">Инвариантное имя единицы.</param>
    /// <param name="symbol">Символ единицы.</param>
    /// <param name="dimension">Размерность единицы.</param>
    /// <param name="scale">Множитель к базовой единице измерения.</param>
    /// <param name="offset">Смещение относительно нуля базовой единицы измерения.</param>
    public Unit(string? name, string? symbol, Dimension dimension, Rational? scale, Rational? offset)
    {
        var s = scale ?? Rational.One;
        var o = offset ?? Rational.Zero;
        this.InvariantName = name ?? GenerateName(dimension, s, o);
        this.InvariantSymbol = symbol ?? GenerateName(dimension, s, o);
        this.Dimension = dimension;
        this.Scale = s;
        this.Offset = o;
    }

    /// <summary>
    /// Инвариантное (стабильное) имя единицы измерения.
    /// </summary>
    public string InvariantName { get; }

    /// <summary>
    /// Символ единицы измерения (например, "m", "s", "kg").
    /// </summary>
    public string InvariantSymbol { get; }

    /// <summary>
    /// Размерность единицы измерения.
    /// </summary>
    public Dimension Dimension { get; }

    /// <summary>
    /// Множитель относительно базовой единицы измерения.
    /// </summary>
    public Rational Scale { get; }

    /// <summary>
    /// Смещение нуля относительно базовой единицы (используется для аффинных единиц).
    /// </summary>
    public Rational Offset { get; }

    /// <summary>
    /// Возвращает признак того, является ли единица измерения аффинной.
    /// </summary>
    public bool IsAffine => this.Offset != Rational.Zero;

    /// <summary>
    /// Проверяет равенство двух единиц измерения.
    /// </summary>
    /// <param name="left">Левая единица измерения.</param>
    /// <param name="right">Правая единица измерения.</param>
    /// <returns><c>true</c>, если единицы равны; иначе <c>false</c>.</returns>
    public static bool operator ==(Unit? left, Unit? right)
    {
        return left?.Equals(right) ?? right is null;
    }

    /// <summary>
    /// Проверяет неравенство двух единиц измерения.
    /// </summary>
    /// <param name="left">Левая единица измерения.</param>
    /// <param name="right">Правая единица измерения.</param>
    /// <returns><c>true</c>, если единицы различаются; иначе <c>false</c>.</returns>
    public static bool operator !=(Unit? left, Unit? right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Проверяет, имеют ли две указанные единицы измерения одинаковую размерность.
    /// </summary>
    /// <param name="unitA">Первая единица измерения для сравнения.</param>
    /// <param name="unitB">Вторая единица измерения для сравнения.</param>
    /// <returns>
    /// <c>true</c>, если обе единицы принадлежат одной и той же физической размерности 
    /// (например, метры и футы — обе представляют длину); 
    /// иначе — <c>false</c>.
    /// </returns>
    public static bool IsSameDimension(Unit unitA, Unit unitB) => unitA.Dimension.Equals(unitB.Dimension); 

    /// <summary>
    /// Определяет, эквивалентна ли текущая единица измерения другой.
    /// </summary>
    /// <param name="other">Единица измерения для сравнения.</param>
    /// <returns><c>true</c>, если единицы эквивалентны; иначе <c>false</c>.</returns>
    public bool Equals(Unit? other)
    {
        if (other is null)
        {
            return false;
        }

        return this.Dimension.Equals(other.Dimension)
               && this.Scale.Equals(other.Scale)
               && this.Offset.Equals(other.Offset);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is Unit u && this.Equals(u);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(this.Dimension, this.Scale, this.Offset);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{this.InvariantName} [{this.InvariantSymbol}] {{{this.Dimension}}}";
    }

    /// <summary>
    /// Преобразует значение из текущей единицы измерения в базовые единицы СИ.
    /// </summary>
    /// <param name="value">Значение в текущей единице измерения.</param>
    /// <returns>Значение в базовых единицах измерения.</returns>
    public double ToSiValue(double value)
    {
        return (value * (double)this.Scale) + (double)this.Offset;
    }
        
    /// <summary>
    /// Проверяет, имеют ли две единицы измерения одинаковую размерность.
    /// </summary>
    /// <param name="other">Единица измерения, с которой выполняется сравнение.</param>
    /// <returns>
    /// <c>true</c>, если обе единицы относятся к одной и той же физической размерности 
    /// (например, метры и футы — обе имеют размерность длины); 
    /// иначе — <c>false</c>.
    /// </returns>
    public bool IsSameDimension(Unit other) => IsSameDimension(this, other); 
    
    /// <summary>
    /// Преобразует значение из базовых единиц СИ в текущую единицу измерения.
    /// </summary>
    /// <param name="value">Значение в базовых единицах измерения.</param>
    /// <returns>Значение в текущей единице измерения.</returns>
    public double FromSiValue(double value)
    {
        return (value - (double)this.Offset) / (double)this.Scale;
    }

    /// <summary>
    /// Генерирует инвариантное имя единицы измерения на основе её размерности, множителя и смещения.
    /// </summary>
    /// <param name="dimension">Размерность единицы измерения.</param>
    /// <param name="scale">Множитель относительно базовой единицы.</param>
    /// <param name="offset">Смещение относительно нуля базовой единицы.</param>
    /// <returns>Сформированное инвариантное имя.</returns>
    private static string GenerateName(Dimension dimension, Rational scale, Rational offset)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append($"{dimension}");
        if (scale != Rational.One)
        {
            stringBuilder.Append($" {scale}");
        }

        if (offset != Rational.Zero)
        {
            stringBuilder.Append($" {offset}");
        }

        return stringBuilder.ToString();
    }
}