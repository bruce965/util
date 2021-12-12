// Copyright (c) 2021 Fabio Iotti
// The copyright holders license this file to you under the MIT license,
// available at https://github.com/bruce965/snippets/raw/master/LICENSE

import { MutableRefObject, RefCallback } from 'react';
import { WeakTree } from '../TypeScript/WeakTree';

const refsCache = new WeakTree<(MutableRefObject<unknown> | RefCallback<unknown>)[], RefCallback<unknown>>();

/**
 * Merge multiple {@link MutableRefObject} or {@link RefCallback} references into
 * a single {@link RefCallback} reference.
 */
export const mergeRefs = <T>(...refs: (MutableRefObject<T> | RefCallback<T>)[]): RefCallback<T> => {
  let cachedRef = refsCache.get(refs);
  if (cachedRef == null) {
    cachedRef = (instance: T) => {
      for (const ref of refs) {
        if (typeof ref === 'function')
          ref(instance);
        else
          ref.current = instance;
      }
    };

    refsCache.set(refs, cachedRef);
  }

  return cachedRef;
};
