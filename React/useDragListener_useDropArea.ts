// Copyright (c) 2023 Fabio Iotti
// The copyright holders license this file to you under the MIT license,
// available at https://github.com/bruce965/util/raw/master/LICENSE

import { RefCallback, RefObject, useCallbackRef, useEffect, useMemo, useState } from 'react';

// WARNING: some of the source code in this file is experimental and may have
// not been properly tested! Discretion advised.

export interface DragListenerOptions {
    target?: RefObject<unknown>
    //onDragOver?(event: DragEvent): void
    onDrop?(event: DragEvent): void
}

export const useDragListener = (options?: DragListenerOptions) => {
    const [dragInProgress, setDragInProgress] = useState(false);
    const [items, setItems] = useState<DataTransferItemList | null>(null);

    const target = options?.target;
    //const dragCallback = options?.onDragOver;
    const dropCallback = options?.onDrop;

    // track drag state
    useEffect(() => {
        const dragTargets: EventTarget[] = [];
        //let dragOverAlreadyCalled = false;

        const enterHandler = (e: DragEvent) => {
            if (e.target) {
                dragTargets.push(e.target);

                if (dragTargets.length === 1) {
                    //console.log('useDragListener enter');
                    setDragInProgress(true);
                    setItems(null); // will be replaced when handling `dragover`
                }
            }
        };

        const leaveHandler = (e: DragEvent) => {
            if (e.target) {
                const index = dragTargets.indexOf(e.target);
                if (index >= 0)
                    dragTargets.splice(index, 1);

                for (let i = 0; i < dragTargets.length; i++)
                    if (!document.contains(dragTargets[i] as Node))
                        dragTargets.splice(i--, 1);
            }

            if (dragTargets.length === 0) {
                //console.log('useDragListener leave');
                setDragInProgress(false);
                setItems(null);
                //dragOverAlreadyCalled = false;
            }
        };

        const overHandler = (e: DragEvent) => {
            //console.log('useDragListener over');
            const newItems = e.dataTransfer?.items ?? null;
            setItems(current => {
                return isProbablySameItemList(current, newItems) ? current : newItems;
            });

            //if (!dragOverAlreadyCalled) {
            //    if (target?.current == null || (e.target as HTMLElement).contains(target.current as HTMLElement)) {
            //        dragOverAlreadyCalled = true;
            //        dragCallback?.(e);
            //    }
            //}
        };

        const dropHandler = (e: DragEvent) => {
            leaveHandler(e);

            dropCallback?.(e);
        };

        window.addEventListener('dragenter', enterHandler);
        window.addEventListener('dragleave', leaveHandler);
        window.addEventListener('dragover', overHandler, false);
        window.addEventListener('drop', dropHandler);

        return () => {
            window.removeEventListener('dragenter', enterHandler);
            window.removeEventListener('dragleave', leaveHandler);
            window.removeEventListener('dragover', overHandler, false);
            window.removeEventListener('drop', dropHandler);
        };
    }, [target, /*dragCallback, */dropCallback]);

    return {
        dragInProgress,
        items: dragInProgress ? items : null,
    };
};

/**
 * While dragging, browsers change this list once per-frame (tested on Windows
 * 10), this function checks if two item lists are probably the same drag
 * operation.
 */
const isProbablySameItemList = (a: DataTransferItemList | null, b: DataTransferItemList | null) => {
    if (a == null || b == null)
        return a === b;

    if (a.length != b.length)
        return false;

    for (let i = 0; i < a.length; i++) {
        const itemA = a[i]!;
        const itemB = b[i]!;

        if (itemA.kind !== itemB.kind)
            return false;

        if (itemA.type !== itemB.type)
            return false;
    }

    return true;
}

export interface DropAreaOptions {
    onDrop(items: DataTransferItemList, dropped: boolean): boolean
}

export const useDropArea = <T extends HTMLElement>(options: DropAreaOptions) => {
    const { items } = useDragListener();

    const onDrop = options.onDrop;

    const ref: RefCallback<T> = useCallbackRef(el => {
        let lastTestedItemsList: DataTransferItemList | null = null;
        let lastTestedItemsListResult: boolean = false;

        const dragOverHandler = (e: DragEvent) => {
            const items = e.dataTransfer?.items;
            if (items != null) {
                if (isProbablySameItemList(lastTestedItemsList, items)) {
                    if (lastTestedItemsListResult)
                        e.preventDefault();
                }
                else {
                    lastTestedItemsList = items;
                    lastTestedItemsListResult = onDrop(items, false);

                    if (lastTestedItemsListResult)
                        e.preventDefault();
                }
            }
        };

        const dropHandler = (e: DragEvent) => {
            const items = e.dataTransfer?.items;
            if (items != null && onDrop(items, true))
                e.preventDefault();
        };

        el.addEventListener('dragover', dragOverHandler);
        el.addEventListener('drop', dropHandler);

        return () => {
            el.removeEventListener('dragover', dragOverHandler);
            el.removeEventListener('drop', dropHandler);
        };
    }, [onDrop]);

    const isDroppable = useMemo(() => {
        //console.log('useDropArea isDroppable', items);
        return items != null && onDrop(items, false);
    }, [onDrop, items]);

    return {
        ref,
        isDroppable,
    };
};
