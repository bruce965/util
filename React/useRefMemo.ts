// Copyright (c) 2023 Fabio Iotti
// The copyright holders license this file to you under the MIT license,
// available at https://github.com/bruce965/util/raw/master/LICENSE

import { Ref, useMemo, useRef } from 'react';

/**
 * Equivalent to {@link useMemo}, but backed by a {@link Ref}.
 *
 * According to React's documentation, {@link useMemo} may throw away cached
 * values in the future. If such behavior is undesired, this hook is a safe
 * drop-in replacement.
 */
export const useRefMemo = <T,>(factory: () => T) => {
    const ref = useRef<T>();
    const memoized = ref.current ??= factory();
    return memoized;
};
