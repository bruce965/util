// Copyright (c) 2022 Fabio Iotti
// The copyright holders license this file to you under the MIT license,
// available at https://github.com/bruce965/snippets/raw/master/LICENSE

using System;
using System.Diagnostics;

namespace Utility;

/// <summary>
/// Generates an object on-demand and caches it, while still allowing
/// that object to be reclaimed by garbage collection.
/// </summary>
/// <typeparam name="T">The type of the object referenced.</typeparam>
[DebuggerDisplay("{ValueForDebugDisplay}")]
public class WeakLazy<T> where T : class
{
    readonly Func<T> build;
    readonly WeakReference<T?> weak;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public T Value
    {
        get
        {
            if (!weak.TryGetTarget(out var value))
            {
                value = build();
                weak.SetTarget(value);
            }

            return value;
        }
    }

    internal T? ValueForDebugDisplay
    {
        get
        {
            if (!weak.TryGetTarget(out var value))
                value = null;

            return value;
        }
    }

    public WeakLazy(Func<T> valueFactory)
    {
        build = valueFactory;
        weak = new WeakReference<T?>(null);
    }

    public static implicit operator T(WeakLazy<T> obj)
        => obj.Value;
}

public static class WeakLazy
{
    public static WeakLazy<T> FromDefaultConstructor<T>() where T : class, new()
        => new WeakLazy<T>(() => new T());
}
