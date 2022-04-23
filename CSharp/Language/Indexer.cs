// Copyright (c) 2021 Fabio Iotti
// The copyright holders license this file to you under the MIT license,
// available at https://github.com/bruce965/util/raw/master/LICENSE

using System;

namespace Utility
{
    public struct Indexer<TSource, TKey, TValue>
    {
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
}