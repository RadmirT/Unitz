namespace Unitz.Core;

/// <summary>
/// Вектор описывающий  вектор размерности единицы измерения.
/// </summary>
public readonly record struct Dimension 
{
    /// <summary>
    /// Безразмерная величина (все экспоненты равны нулю).
    /// </summary>
    public static readonly Dimension Dimensionless = new(0, 0, 0, 0, 0, 0, 0);
    
    private readonly int[] _e; // Вектор размерности: L,M,T,I,Th,N,J;

    /// <summary>Инициализирует новый объект <see cref="Dimension"/> с заданными значениями экспонент.</summary>
    /// <param name="l">Длина (L).</param>
    /// <param name="m">Масса (M).</param>
    /// <param name="t">Время (T).</param>
    /// <param name="i">Электрический ток (I).</param>
    /// <param name="th">Экспонента для термодинамической температуры (Th).</param>
    /// <param name="n">Экспонента для количества вещества (N).</param>
    /// <param name="j">Экспонента для силы света (J).</param>
    public Dimension(int l = 0, int m = 0, int t = 0, int i = 0, int th = 0, int n = 0, int j = 0)
    {
        this._e = [l, m, t, i, th, n, j];
    }

    /// <summary>Оператор сложения двух размерностей <see cref="Dimension"/>.</summary>
    /// <param name="a">Первый объект <see cref="Dimension"/>.</param>
    /// <param name="b">Второй объект <see cref="Dimension"/>.</param>
    /// <returns>Новый объект <see cref="Dimension"/>.</returns>
    /// <remarks>Это алгебра  размерностей. Т.е.  L(расстояние) + L(расстояние)= 2L (расстояние * расстояние = площадь) или I(ток) + T(время) = IT(ток * время =  заряд).</remarks>
    public static Dimension operator +(Dimension a, Dimension b)
    {
        return new Dimension(
            a._e[0] + b._e[0],
            a._e[1] + b._e[1],
            a._e[2] + b._e[2],
            a._e[3] + b._e[3],
            a._e[4] + b._e[4],
            a._e[5] + b._e[5],
            a._e[6] + b._e[6]);
    }
}