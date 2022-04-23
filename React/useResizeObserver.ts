// Copyright (c) 2021 Fabio Iotti
// The copyright holders license this file to you under the MIT license,
// available at https://github.com/bruce965/util/raw/master/LICENSE

import { RefCallback, useMemo } from 'react';

export interface ResizeObserverEntryTyped<T extends Element> extends ResizeObserverEntry {
  readonly target: T
}

/**
 * Listen for changes to an Element's dimensions.
 * @see https://developer.mozilla.org/en-US/docs/Web/API/ResizeObserver
 */
export const useResizeObserver = <T extends Element>(
  callback: (entry: ResizeObserverEntryTyped<T>) => void,
  options?: ResizeObserverOptions,
): RefCallback<T> => {
  const refCallback = useMemo<RefCallback<T>>(() => {
    let resizeObserver: ResizeObserver | undefined;
    return target => {
      if (target == null) {
        resizeObserver?.disconnect();
      }
      else {
        resizeObserver ??= new ResizeObserver(entries => callback?.(entries[0] as ResizeObserverEntryTyped<T>));
        resizeObserver.observe(target, options);
      }
    }
  }, [callback, options?.box]);

  return refCallback;
};
