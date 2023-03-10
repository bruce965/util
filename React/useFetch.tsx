// Copyright (c) 2023 Fabio Iotti
// The copyright holders license this file to you under the MIT license,
// available at https://github.com/bruce965/util/raw/master/LICENSE

import { useCallback, useRef, useState } from 'react';

interface FetchResult<T> {
  fetch: typeof fetch
  called: boolean
  loading: boolean
  response?: Response
  data?: T
  error?: unknown
}

export function useFetch(format: 'arrayBuffer'): FetchResult<ArrayBuffer>;
export function useFetch(format: 'blob'): FetchResult<Blob>;
export function useFetch(format: 'json'): FetchResult<any>;
export function useFetch(format: 'text'): FetchResult<string>;
export function useFetch(format?: null): FetchResult<void>;
export function useFetch(format?: 'arrayBuffer'|'blob'|'json'|'text'|null): FetchResult<any> {
  const [, refresh] = useState<unknown>();
  const lastCall = useRef<{ loading: boolean, response?: Response, data?: any, error?: unknown }>();

  const customFetch = useCallback<typeof fetch>(async (...args) => {
    const thisCall = lastCall.current = { loading: true };
    refresh({});

    try {
      const response = await fetch(...args);

      if (lastCall.current === thisCall) {
        lastCall.current.response = response;
        refresh({});

        (async () => {
          let data: any;
          switch (format) {
            case 'arrayBuffer':
              data = await response.arrayBuffer();
              break;
            case 'blob':
              data = await response.blob();
              break;
            case 'json':
              data = await response.json();
              break;
            case 'text':
              data = await response.text();
              break;
          }

          if (lastCall.current === thisCall) {
            lastCall.current.data = data;
            lastCall.current.loading = false;
            refresh({});
          }
        })();
      }

      return response;
    }
    catch (error) {
      if (lastCall.current === thisCall) {
        lastCall.current.error = error;
        lastCall.current.loading = false;
        refresh({});
      }

      throw error;
    }
  }, []);

  return {
    fetch: customFetch,
    called: lastCall.current != null,
    loading: lastCall.current?.loading === true,
    response: lastCall.current?.response,
    data: lastCall.current?.data,
    error: lastCall.current?.error,
  };
}
