// Copyright (c) 2021-2022 Fabio Iotti
// The copyright holders license this file to you under the MIT license,
// available at https://github.com/bruce965/util/raw/master/LICENSE

namespace Utilities;

/// <summary>
/// Indexer to be used as a property.
///
/// <code>
/// record Fruit(string Name, float Weight);
///
/// class Basket
/// {
///     public List&lt;Fruit&gt; Fruits { get; } = new();
///
///     public Indexer&lt;Basket, int, string&gt; FruitNames =&gt; new(this, (s, i) =&gt; s.Fruits[i].Name, (s, i, v) =&gt; s.Fruits[i].Name = v);
///     public ReadOnlyIndexer&lt;Basket, int, float&gt; FruitWeights =&gt; new(this, (s, i) =&gt; s.Fruits[i].Weight);
/// }
///
/// var basket = new Basket();
///
/// basket.Fruits.Add(new Fruit() { Name = "Banana", Weight = 120 });
/// Console.WriteLine(basket.FruitNames[0]);  // Banana
/// Console.WriteLine(basket.FruitWeights[0]);  // 120
///
/// basket.FruitNames[0] = "Kiwi";
/// Console.WriteLine(basket.FruitNames[0]);  // Kiwi
/// Console.WriteLine(basket.FruitWeights[0]);  // 120
/// </code>
/// </summary>
public class Indexer<TSource, TKey, TValue>
{
    // TODO: in C# 10 the non-readonly indexer can no longer be a struct, find a way to avoid creating a new instance every time it's accessed.

    readonly TSource _source;
    readonly Func<TSource, TKey, TValue> _getter;
    readonly Action<TSource, TKey, TValue> _setter;

    public TValue this[TKey key]
    {
        get => _getter(_source, key);
        set => _setter(_source, key, value);
    }

    public Indexer(TSource source, Func<TSource, TKey, TValue> getter, Action<TSource, TKey, TValue> setter)
    {
        _source = source;
        _getter = getter;
        _setter = setter;
    }
}

/// <inheritdoc cref="Indexer{TSource, TKey, TValue}" />
public struct ReadOnlyIndexer<TSource, TKey, TValue>
{
    readonly TSource _source;
    readonly Func<TSource, TKey, TValue> _getter;

    public TValue this[TKey key]
        => _getter(_source, key);

    public ReadOnlyIndexer(TSource source, Func<TSource, TKey, TValue> getter)
    {
        _source = source;
        _getter = getter;
    }
}
