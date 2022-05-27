// Copyright (c) 2022 Fabio Iotti
// The copyright holders license this file to you under the MIT license,
// available at https://github.com/bruce965/util/raw/master/LICENSE

using System;
using System.Diagnostics;

namespace Utility;

/// <summary>
/// Generates an object on-demand and caches it, while still allowing
/// that object to be reclaimed by garbage collection.
/// </summary>
/// <typeparam name="T">The type of the object referenced.</typeparam>
[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}()}}")]
public class WeakLazy<T> where T : class
{
    readonly Func<T> _build;
    readonly WeakReference<T?> _weak;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public T Value
    {
        get
        {
            if (!_weak.TryGetTarget(out var value))
            {
                value = _build();
                _weak.SetTarget(value);
            }

            return value;
        }
    }

    public WeakLazy(Func<T> valueFactory)
    {
        _build = valueFactory;
        _weak = new WeakReference<T?>(null);
    }

    T? GetDebuggerDisplay()
    {
        if (!_weak.TryGetTarget(out var value))
            value = null;

        return value;
    }

    public static implicit operator T(WeakLazy<T> lazy)
        => lazy.Value;

    public static implicit operator WeakLazy<T>(T value)
        => new(() => value);
}

public static class WeakLazy
{
    public static WeakLazy<T> FromDefaultConstructor<T>() where T : class, new()
        => new(() => new T());
}
