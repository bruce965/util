// Copyright (c) 2021 Fabio Iotti
// The copyright holders license this file to you under the MIT license,
// available at https://github.com/bruce965/snippets/raw/master/LICENSE

interface WeakNode<T> {
  children?: WeakMap<object, WeakNode<T>> | undefined
  value?: { v: T } | undefined
};

/**
 * Equivalent to {@link WeakMap}, but taking an array of keys instead of a single object.
 */
export class WeakTree<K extends object[], V> {
  readonly #root: WeakNode<V> = {};

  delete(key: K): boolean {
    const node = getNode(this.#root, key);
    if (node == null)
      return false;

    if (node.value == null)
      return false;

    node.value = undefined;
    return true;
  }

  get(key: K): V | undefined {
    const node = getNode(this.#root, key);
    return node?.value?.v;
  }

  has(key: K): boolean {
    const node = getNode(this.#root, key);
    return node?.value != null;
  }

  set(key: K, value: V): this {
    const node = getNode(this.#root, key, true);
    node.value = { v: value };
    return this;
  }
}

function getNode<T>(root: WeakNode<T>, keys: object[], addMissing: true): WeakNode<T>;
function getNode<T>(root: WeakNode<T>, keys: object[], addMissing?: boolean): WeakNode<T> | undefined;
function getNode<T>(root: WeakNode<T>, keys: object[], addMissing = false): WeakNode<T> | undefined {
  let current = root;
  for (const key of keys) {
    if (!current.children) {
      if (!addMissing)
        return undefined;

      current.children = new WeakMap();
    }

    let childNode = current.children.get(key);
    if (!childNode) {
      if (!addMissing)
        return undefined;

      childNode = {};
      current.children.set(key, childNode);
    }

    current = childNode;
  }

  return current;
}
