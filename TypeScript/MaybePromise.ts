// Copyright (c) 2021 Fabio Iotti
// The copyright holders license this file to you under the MIT license,
// available at https://github.com/bruce965/snippets/raw/master/LICENSE

/**
 * Either an asynchronous Promise or directly a value.
 * 
 * MaybePromise values are await-able when used in asynchronous contexts.
 * Unlike with Promises, their values may be resolved synchronously, in which case
 * it's possible to execute synchronously with {@link MaybePromise} helper methods.
 */
export type MaybePromise<T> = T | Promise<T>;

/**
 * Either an asynchronous promise-like or directly a value.
 * 
 * MaybePromise values are await-able when used in asynchronous contexts.
 * Unlike with Promises, their values may be resolved synchronously, in which case
 * it's possible to execute synchronously with {@link MaybePromise} helper methods.
 */
export type MaybePromiseLike<T> = T | PromiseLike<T>;

export const MaybePromise: {
  /**
   * Attaches callbacks for the resolution and/or rejection of the MaybePromise.
   * The callback may be invoked synchronously and immediately.
   * @param onfulfilled The callback to execute when the MaybePromise is resolved.
   * @param onrejected The callback to execute when the MaybePromise is rejected.
   * @returns A MaybePromise for the completion of which ever callback is executed.
   */
  then<T, TResult1 = T, TResult2 = never>(promise: MaybePromiseLike<T>, onfulfilled: (value: T) => MaybePromiseLike<TResult1>, onrejected?: ((reason: unknown) => MaybePromiseLike<TResult2>) | undefined | null): MaybePromise<TResult1 | TResult2>;
  then<T, TResult1 = T, TResult2 = never>(promise: MaybePromiseLike<T>, onfulfilled?: ((value: T) => MaybePromiseLike<TResult1>) | undefined | null, onrejected?: ((reason: unknown) => MaybePromiseLike<TResult2>) | undefined | null): MaybePromise<T | TResult1 | TResult2>;

  /**
   * Attaches a callback for only the rejection of the MaybePromise.
   * @param onrejected The callback to execute when the MaybePromise is rejected.
   * @returns A MaybePromise for the completion of the callback.
   */
  catch<T, TResult = never>(promise: MaybePromiseLike<T>, onrejected?: ((reason: unknown) => MaybePromiseLike<TResult>) | undefined | null): MaybePromise<T | TResult>;

  /**
   * Attaches a callback that is invoked when the MaybePromise is settled (fulfilled or rejected).
   * The callback may be invoked synchronously and immediately.
   * @param onfinally The callback to execute when the MaybePromise is settled (fulfilled or rejected).
   * @returns A MaybePromise for the completion of the callback.
   */
  finally<T>(promise: MaybePromiseLike<T>, onfinally?: (() => void) | undefined | null): MaybePromise<T>

  /**
   * Creates a MaybePromise that is resolved with an array of results when all of
   * the provided MaybePromises resolve, or rejected when any MaybePromise is rejected.
   * @param values An array of values or promise-like values.
   * @returns A Promise if any of the values is promise-like, otherwise directly the values.
   */
  // https://github.com/microsoft/TypeScript/blob/main/lib/lib.es2015.promise.d.ts
  all<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(values: readonly [MaybePromiseLike<T1>, MaybePromiseLike<T2>, MaybePromiseLike<T3>, MaybePromiseLike<T4>, MaybePromiseLike<T5>, MaybePromiseLike<T6>, MaybePromiseLike<T7>, MaybePromiseLike<T8>, MaybePromiseLike<T9>, MaybePromiseLike<T10>]): MaybePromise<[T1, T2, T3, T4, T5, T6, T7, T8, T9, T10]>;
  all<T1, T2, T3, T4, T5, T6, T7, T8, T9>(values: readonly [MaybePromiseLike<T1>, MaybePromiseLike<T2>, MaybePromiseLike<T3>, MaybePromiseLike<T4>, MaybePromiseLike<T5>, MaybePromiseLike<T6>, MaybePromiseLike<T7>, MaybePromiseLike<T8>, MaybePromiseLike<T9>]): MaybePromise<[T1, T2, T3, T4, T5, T6, T7, T8, T9]>;
  all<T1, T2, T3, T4, T5, T6, T7, T8>(values: readonly [MaybePromiseLike<T1>, MaybePromiseLike<T2>, MaybePromiseLike<T3>, MaybePromiseLike<T4>, MaybePromiseLike<T5>, MaybePromiseLike<T6>, MaybePromiseLike<T7>, MaybePromiseLike<T8>]): MaybePromise<[T1, T2, T3, T4, T5, T6, T7, T8]>;
  all<T1, T2, T3, T4, T5, T6, T7>(values: readonly [MaybePromiseLike<T1>, MaybePromiseLike<T2>, MaybePromiseLike<T3>, MaybePromiseLike<T4>, MaybePromiseLike<T5>, MaybePromiseLike<T6>, MaybePromiseLike<T7>]): MaybePromise<[T1, T2, T3, T4, T5, T6, T7]>;
  all<T1, T2, T3, T4, T5, T6>(values: readonly [MaybePromiseLike<T1>, MaybePromiseLike<T2>, MaybePromiseLike<T3>, MaybePromiseLike<T4>, MaybePromiseLike<T5>, MaybePromiseLike<T6>]): MaybePromise<[T1, T2, T3, T4, T5, T6]>;
  all<T1, T2, T3, T4, T5>(values: readonly [MaybePromiseLike<T1>, MaybePromiseLike<T2>, MaybePromiseLike<T3>, MaybePromiseLike<T4>, MaybePromiseLike<T5>]): MaybePromise<[T1, T2, T3, T4, T5]>;
  all<T1, T2, T3, T4>(values: readonly [MaybePromiseLike<T1>, MaybePromiseLike<T2>, MaybePromiseLike<T3>, MaybePromiseLike<T4>]): MaybePromise<[T1, T2, T3, T4]>;
  all<T1, T2, T3>(values: readonly [MaybePromiseLike<T1>, MaybePromiseLike<T2>, MaybePromiseLike<T3>]): MaybePromise<[T1, T2, T3]>;
  all<T1, T2>(values: readonly [MaybePromiseLike<T1>, MaybePromiseLike<T2>]): MaybePromise<[T1, T2]>;
  all<T>(values: readonly MaybePromiseLike<T>[]): MaybePromise<T[]>;

  /**
   * Creates a MaybePromise that is resolved or rejected when any of the provided
   * MaybePromise are resolved or rejected.
   * @param values An array of MaybePromises.
   * @returns The first non promise-like value, otherwise a Promise for the first promise-like value that resolves.
   */
  race<T>(values: readonly T[]): MaybePromise<T extends MaybePromiseLike<infer U> ? U : T>;

  /**
   * Equivalent to throwing. Provided just for completeness.
   * @param reason The reason the MaybePromise was rejected.
   */
  reject(reason?: unknown): never;

  /**
   * Creates a new resolved MaybePromise for the provided value.
   * @param value A value, or promise-like value.
   * @returns The value, or a Promise whose internal state matches the provided promise-like value.
   */
  resolve(): void;
  resolve<T>(value: MaybePromiseLike<T>): MaybePromise<T>;
} = {
  then(value, onfulfilled, onrejected) {
    if (isPromiseLike(value))
      return value.then(onfulfilled, onrejected);

    if (onfulfilled)
      return onfulfilled(value);

    return value;
  },

  catch(value, onrejected) {
    if (isPromiseLike(value))
      return Promise.prototype.catch.call(value, onrejected);

    return value;
  },

  finally(value, onfinally) {
    if (isPromiseLike(value))
      return Promise.prototype.finally.call(value, onfinally);

    onfinally();
    return value;
  },

  all<T>(values: readonly MaybePromiseLike<T>[]): MaybePromise<any> {
    if (Array.prototype.some.call(values, isPromiseLike))
      return Promise.all(values);

    return [...values] as T[];
  },

  race<T>(values: readonly T[]) {
    for (const value of values)
      if (!isPromiseLike(value))
        return value as any;

    return Promise.race(values) as Promise<any>;
  },

  reject(reason) {
    throw reason;
  },

  resolve<T>(value?: MaybePromiseLike<T>): MaybePromise<T> | undefined {
    if (isPromiseLike(value))
      return Promise.resolve(value);

    return value;
  },
};

function isPromiseLike(candidate: unknown): candidate is PromiseLike<unknown> {
  return candidate != null && typeof (candidate as PromiseLike<unknown>).then === 'function';
}
