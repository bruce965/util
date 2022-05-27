// Copyright (c) 2022 Fabio Iotti
// The copyright holders license this file to you under the MIT license,
// available at https://github.com/bruce965/util/raw/master/LICENSE

using System.Text;

namespace Utility;

static class StringBuilderExtensions
{
	/// <param name="sb"></param>
	/// <returns></returns>
	/// <inheritdoc cref="string.Trim(char)"/>
	public static StringBuilder Trim(this StringBuilder sb, char trimChar)
		=> sb.TrimEnd(trimChar).TrimStart(trimChar);

	/// <param name="sb"></param>
	/// <returns></returns>
	/// <inheritdoc cref="string.TrimStart(char)"/>
	public static StringBuilder TrimStart(this StringBuilder sb, char trimChar)
	{
		var len = 0;
		while (len < sb.Length && sb[len] == trimChar)
			len++;

		if (len != 0)
			sb.Length -= len;

		return sb;
	}

	/// <param name="sb"></param>
	/// <returns></returns>
	/// <inheritdoc cref="string.TrimEnd(char)"/>
	public static StringBuilder TrimEnd(this StringBuilder sb, char trimChar)
	{
		var len = sb.Length;
		while (len > 0 && sb[len - 1] == trimChar)
			len--;

		if (len != sb.Length)
			sb.Length = len;

		return sb;
	}
}
