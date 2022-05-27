// Copyright (c) 2022 Fabio Iotti
// The copyright holders license this file to you under the MIT license,
// available at https://github.com/bruce965/util/raw/master/LICENSE

import React, { CSSProperties, DetailedHTMLProps, FC, HTMLAttributes, useCallback, useMemo, useState } from 'react';
import { ResizeObserverEntryTyped, useResizeObserver } from './useResizeObserver';

export interface ContentSizeProps {
	/** Minimum width in pixels. */
	minWidth?: number
	/** Maximum width in pixels. */
	maxWidth?: number
	/** Minimum height in pixels. */
	minHeight?: number
	/** Maximum height in pixels. */
	maxHeight?: number
	/**
	 * Vertical alignment.
	 *
	 * `0` = top, `0.5` = middle (default), `1` = bottom.\
	 * Any value in between or out of this range is also accepted.
	 */
	alignmentTop?: number
	/**
	 * Horizontal alignment.
	 *
	 * `0` = left, `0.5` = center (default), `1` = right.\
	 * Any value in between or out of this range is also accepted.
	 */
	alignmentLeft?: number
	///**
	// * Scaling/zooming direction bias.
	// * 
	// * `0` = zoom out (default), `.5` = balanced, `1` = zoom in.\
	// * Any value in between is also accepted.
	// */
	//zoomDirection?: number
}

const INITIAL_SIZE = { width: 0, height: 0, scale: 1, x: 1, y: 1 };

const CONTAINER_STYLE: CSSProperties = {
	position: 'relative',
};

const CONTENT_STYLE: CSSProperties = {
	position: 'absolute',
	transformOrigin: '0 0',
};

/**
 * Scales and aligns its content according to the provided parameters.
 */
export const ContentSize: FC<ContentSizeProps & DetailedHTMLProps<HTMLAttributes<HTMLDivElement>, HTMLDivElement>> = ({
	minWidth = 0,
	maxWidth = Infinity,
	minHeight = 0,
	maxHeight = Infinity,
	alignmentTop = .5,
	alignmentLeft = .5,
	//zoomDirection = 0,
	...props
}) => {
	const [position, setPosition] = useState(INITIAL_SIZE);

	const containerStyle = useMemo<CSSProperties>(() => ({
		...props.style,
		CONTAINER_STYLE,
	}), [props.style]);

	const contentStyle = useMemo<CSSProperties>(() => ({
		...CONTENT_STYLE,
		width: position.width,
		height: position.height,
		transform: position.scale === 1 ? undefined : `scale(${position.scale})`,
		left: position.x,
		top: position.y,
	}), [position]);

	const containerResizeCallback = useCallback((e: ResizeObserverEntryTyped<HTMLDivElement>) => {
		const size = adjustSize(
			e.contentRect,
			{ min: minWidth, max: maxWidth },
			{ min: minHeight, max: maxHeight }
		);

		const offset = adjustOffset(
			e.contentRect,
			{
				width: size.width * size.scale,
				height: size.height * size.scale,
			},
			{
				top: alignmentTop,
				left: alignmentLeft,
			}
		);

		//const widthScaled = size.width * size.scale;
		//const heightScaled = size.height * size.scale;

		//const scaleAdjustment = zoomDirection * Math.max(
		//	(e.contentRect.width - widthScaled) / widthScaled,
		//	(e.contentRect.height - heightScaled) / heightScaled
		//);
		
		setPosition({
			width: size.width,
			height: size.height,
			scale: size.scale,// * (1 + scaleAdjustment),
			x: offset.x,// * (1 - zoomDirection),
			y: offset.y,// * (1 - zoomDirection),
		});
	}, [minWidth, maxWidth, minHeight, maxHeight, alignmentTop, alignmentLeft/*, zoomDirection*/]);

	const containerRef = useResizeObserver(containerResizeCallback);

	return (
		<div {...props} ref={containerRef} style={containerStyle}>
			<div style={contentStyle}>
				{props.children}
			</div>
		</div>
	);
};

const adjustSize = (
	available: { width: number, height: number },
	width: { min: number, max: number },
	height: { min: number, max: number },
) => {
	let size = { width: available.width, height: available.height, scale: 1 };

	// adjust scale

	if (size.width > width.max) {
		const ratio = width.max / size.width;
		size = { width: size.width * ratio, height: size.height * ratio, scale: size.scale / ratio };
	}

	if (size.height > height.max) {
		const ratio = height.max / size.height;
		size = { width: size.width * ratio, height: size.height * ratio, scale: size.scale / ratio };
	}

	if (size.width < width.min) {
		const ratio = width.min / size.width;
		size = { width: size.width * ratio, height: size.height * ratio, scale: size.scale / ratio };
	}

	if (size.height < height.min) {
		const ratio = height.min / size.height;
		size = { width: size.width * ratio, height: size.height * ratio, scale: size.scale / ratio };
	}

	// constrain limits

	if (size.width > width.max) {
		size.width = width.max;
	}

	if (size.height > height.max) {
		size.height = height.max;
	}
	
	return size;
};

const adjustOffset = (
	available: { width: number, height: number },
	size: { width: number, height: number },
	alignment: { top: number, left: number },
) => {
	return {
		x: (available.width - size.width) * alignment.left,
		y: (available.height - size.height) * alignment.top,
	}
};
