// Copyright (c) 2023 Fabio Iotti
// The copyright holders license this file to you under the MIT license,
// available at https://github.com/bruce965/util/raw/master/LICENSE

import { DependencyList, useCallback, useEffect, useRef } from 'react';

export const useTimer = (callback: () => void, deps?: DependencyList) => {
    const handle = useRef<number>();

    const set = useCallback((timeout: number) => {
        clearTimeout(handle.current);
        handle.current = setTimeout(callback, timeout);
    }, deps ?? [callback]);

    const clear = useCallback(() => {
        clearTimeout(handle.current);
    }, []);

    useEffect(() => {
        return () => {
            clearTimeout(handle.current);
        };
    }, []);

    return {
        set,
        clear,
    };
};
