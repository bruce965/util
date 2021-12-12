// Copyright (c) 2021 Fabio Iotti
// The copyright holders license this file to you under the MIT license,
// available at https://github.com/bruce965/snippets/raw/master/LICENSE

const enum Status {
  Pending,
  Building,
  Built,
  Error,
}

/**
 * Function that computes and caches a value on first invocation.
 */
export interface Lazy<T> {
  (): T
}

/**
 * Wrap a function called on first invocation to compute a value,
 * and returns the cached value on subsequent invocations.
 * @param factory Value factory.
 * @returns Value returned on the first invocation of {@link factory}.
 * @throws Error thrown on the first invocation of {@link factory}.
 */
export function lazy<T>(factory: () => T): Lazy<T> {
  let status = Status.Pending;
  let value: T | unknown | undefined;
  const lazy: Lazy<T> = () => {
    if (status === Status.Built)
      return value as T;

    if (status === Status.Pending) {
      status = Status.Building;

      try {
        value = factory();
        status = Status.Built;

        return value as T;
      }
      catch (e) {
        value = e;
        status = Status.Error;
      }
    }
    else if (status === Status.Building) {
      value = new Error("Lazy factory recursive invocation.");
      status = Status.Error;
    }

    throw value;
  };

  return lazy;
}
