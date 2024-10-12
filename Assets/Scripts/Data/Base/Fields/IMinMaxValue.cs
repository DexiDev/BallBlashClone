using System;

namespace DexiDev.Game.Data.Fields
{
    public interface IMinMaxValue<T> : IDataField<T> where T : struct, IComparable, IFormattable, IConvertible, IEquatable<T>
    {
        public T MinValue { get; }
        public T MaxValue { get; }

        public T ClampValue(T value);
    }
}