// Copyright (c) 2021 Fabio Iotti
// The copyright holders license this file to you under the MIT license,
// available at https://github.com/bruce965/util/raw/master/LICENSE

using System.Text;

namespace Utility
{
    public static class StringCasing
    {
        public static string ToCSharpName(string text)
        {
            var sb = new StringBuilder(text.Length);

            var isFirstChar = true;
            var previousCharWasUppercase = false;
            var previousCharWasSeparator = true;
            var lastUpperWordStartIndex = 0;
            var resultIndex = 0;
            for (var i = 0; i < text.Length; i++)
            {
                var chr = text[i];

                if (!char.IsLetterOrDigit(chr))
                {
                    previousCharWasUppercase = false;
                    previousCharWasSeparator = true;
                    lastUpperWordStartIndex = resultIndex + 1;
                    continue;
                }

                var isLower = !isFirstChar && !previousCharWasSeparator && (previousCharWasUppercase || char.IsLower(chr));
                sb.Append(isLower ? char.ToLowerInvariant(chr) : char.ToUpperInvariant(chr));

                // upper-case sequences of characters up to 4 chars can stay upper-case
                if (i - lastUpperWordStartIndex <= 4 && previousCharWasUppercase && (char.IsLower(chr) || i == text.Length - 1))
                    for (var j = lastUpperWordStartIndex; j < ((i == text.Length - 1) ? sb.Length : resultIndex); j++)
                        sb.Replace(sb[j], char.ToUpperInvariant(sb[j]), j, 1);

                isFirstChar = false;
                previousCharWasUppercase = char.IsUpper(chr);
                previousCharWasSeparator = char.IsDigit(chr);
                lastUpperWordStartIndex = previousCharWasUppercase ? lastUpperWordStartIndex : (resultIndex + 1);

                resultIndex++;
            }

            return sb.ToString();
        }
    }
}
