// Copyright (c) 2021 Fabio Iotti
// The copyright holders license this file to you under the MIT license,
// available at https://github.com/bruce965/util/raw/master/LICENSE

using UnityEngine;

namespace Utility
{
    /// <summary>
    /// Boxed reference to a value, resides on the heap.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Boxed<T>
    {
        T _value;
        public ref T Value => ref _value;

        public Boxed()
            => Value = default;

        public Boxed(T value)
            => Value = value;

        public static implicit operator T(Boxed<T> box)
            => box.Value;

        public static explicit operator Boxed<T>(T value)
            => new Boxed<T>(value);
    }
}
