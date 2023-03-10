// Copyright (c) 2023 Fabio Iotti
// The copyright holders license this file to you under the MIT license,
// available at https://github.com/bruce965/util/raw/master/LICENSE

import { DependencyList, useCallback, useEffect, useRef, useState } from 'react';

interface PromiseResult<T> {
  run<TArgs extends any[]>(factory: (...args: TArgs) => T | PromiseLike<T>, ...args: TArgs): Promise<Awaited<T>>
  invoked: boolean
  pending: boolean
  resolved: boolean
  rejected: boolean
  result?: Awaited<T>
  error?: any
}

export function usePromise<T>(): PromiseResult<T> {
  const [, refresh] = useState<unknown>();
  const lastInvocation = useRef<{ pending: boolean, result?: Awaited<T>, error?: any, rejected?: boolean }>();

  const run = useCallback<PromiseResult<T>['run']>(async (factory, ...args): Promise<Awaited<T>> => {
    const thisInvocation = lastInvocation.current = { pending: true };
    refresh({});

    try {
      const result = await factory(...args);

      if (lastInvocation.current === thisInvocation) {
        lastInvocation.current.result = result;
        lastInvocation.current.pending = false;
        refresh({});
      }

      return result;
    }
    catch (error) {
      if (lastInvocation.current === thisInvocation) {
        lastInvocation.current.error = error;
        lastInvocation.current.rejected = true;
        lastInvocation.current.pending = false;
        refresh({});
      }

      throw error;
    }
  }, []);

  return {
    run,
    invoked: lastInvocation.current != null,
    pending: lastInvocation.current?.pending === true,
    resolved: !lastInvocation.current?.pending && !lastInvocation.current?.rejected,
    rejected: lastInvocation.current?.rejected === true,
    result: lastInvocation.current?.result,
    error: lastInvocation.current?.error,
  };
}

interface PromiseFactoryResult<T> {
  pending: boolean
  resolved: boolean
  rejected: boolean
  result?: Awaited<T>
  error?: any
}

export function usePromiseFactory<T>(factory: () => T | PromiseLike<T>, deps?: DependencyList): PromiseFactoryResult<T> {
  const { run, invoked, ...rest } = usePromise<T>();
  useEffect(() => { run(factory); }, deps);
  return rest;
};
