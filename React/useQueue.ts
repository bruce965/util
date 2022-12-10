// Copyright (c) 2022 Fabio Iotti
// The copyright holders license this file to you under the MIT license,
// available at https://github.com/bruce965/util/raw/master/LICENSE

import { useMemo, useState } from 'react';

export const useQueue = <T,>(initialValue?: T[]) => {
    const [values, setValues] = useState<T[]>(initialValue ?? []);

    const ops = useMemo(() => ({
        setQueue: setValues,
        /** Append an element to the end of the queue. */
        push(value: T) {
            setValues(q => [...q, value]);
        },
        /** Remove last element from the queue. */
        pop() {
            setValues(q => q.slice(0, q.length-1));
        },
        /** Add an element at the start of the queue. */
        shift(value: T) {
            setValues(q => [value, ...q]);
        },
        /** Remove an element from the start of the queue. */
        unshift() {
            setValues(q => q.slice(1));
        },
        /** Clear all values from the queue. */
        clear() {
            setValues([]);
        },
    }), [setValues]);

    return [values, ops] as [T[], typeof ops];
};
