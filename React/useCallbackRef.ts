// Copyright (c) 2023 Fabio Iotti
// The copyright holders license this file to you under the MIT license,
// available at https://github.com/bruce965/util/raw/master/LICENSE

import { DependencyList, EffectCallback, MutableRefObject, RefCallback, useEffect, useMemo, useRef } from 'react';

/**
 * Equivalent to {@link useRef}, but builds a {@link RefCallback} instead of a
 * {@link MutableRefObject}.
 *
 * The callback function may return a destructor like {@link useEffect}.
 */
export const useCallbackRef = <T,>(
    callback: (instance: T) => ReturnType<EffectCallback>,
    deps?: DependencyList
) => {
    const ref = useMemo<RefCallback<T>>(() => {
        let destructor: ReturnType<EffectCallback>;

        return el => {
            destructor?.();
            destructor = el == null ? undefined : callback(el);
        };
    }, deps ?? [callback]);

    return ref;
};
