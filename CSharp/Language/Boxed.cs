// Copyright (c) 2021 Fabio Iotti
// The copyright holders license this file to you under the MIT license,
// available at https://github.com/bruce965/util/raw/master/LICENSE

namespace Utility;

/// <summary>
/// Boxed reference to a <typeparamref name="T"/>, resides on the heap.
/// </summary>
/// <typeparam name="T">Type of the boxed value.</typeparam>
public class Boxed<T>
{
    T _value;
    public ref T Value => ref _value;

    //public Boxed()
    //    => _value = default;

    public Boxed(T value)
        => _value = value;

    public static implicit operator T(Boxed<T> box)
        => box.Value;

    public static implicit operator Boxed<T>(T value)
        => new(value);
}
