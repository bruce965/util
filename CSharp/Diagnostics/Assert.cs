// Copyright (c) 2022 Fabio Iotti
// The copyright holders license this file to you under the MIT license,
// available at https://github.com/bruce965/util/raw/master/LICENSE

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace Utility;

public static class Assert
{
    static readonly Regex MayBeCompositeExpression = new Regex(@"[-+*/&|^~%?:<>=!]", RegexOptions.Compiled);

    /// <summary>
    /// Throws an <see cref="ArgumentNullException"/> if <paramref name="argument"/> is <c>null</c>.
    /// </summary>
    /// <param name="argument">The reference type argument to validate as non-null.</param>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="_paramName">The name of the parameter with which <paramref name="argument"/> corresponds (generated automatically from the compiler).</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="argument"/> is <c>null</c>.</exception>
    public static void ArgumentNotNull([NotNull] object? argument, string? message = null, [CallerArgumentExpression("argument")] string _paramName = null!)
    {
        if (argument is not null)
            return;

        throw new ArgumentNullException(_paramName, message);
    }

    /// <summary>
    /// Throws an <see cref="ArgumentOutOfRangeException"/> if a <paramref name="condition"/> is not met.
    /// </summary>
    /// <param name="argument">The reference type argument to check the condition on.</param>
    /// <param name="condition">The condition to be checked.</param>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="_paramName">The name of the parameter with which <paramref name="argument"/> corresponds (generated automatically from the compiler).</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="condition"/> is <c>false</c>.</exception>
    public static void ArgumentInRange(object? argument, [DoesNotReturnIf(false)] bool condition, string? message = null, [CallerArgumentExpression("argument")] string _paramName = null!)
    {
        if (condition)
            return;

        throw new ArgumentOutOfRangeException(_paramName, message);
    }

    /// <summary>
    /// Throws an <see cref="ArgumentException"/> if a <paramref name="condition"/> is not met.
    /// </summary>
    /// <param name="param">Reference to the parameter.</param>
    /// <param name="condition">Condition to be checked.</param>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="_paramName">The name of the parameter with which <paramref name="argument"/> corresponds (generated automatically from the compiler).</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="condition"/> is <c>false</c>.</exception>
    public static void Argument(object? param, [DoesNotReturnIf(false)] bool condition, string? message = null, [CallerArgumentExpression("param")] string _paramName = null!)
    {
        if (condition)
            return;

        throw new ArgumentException(message, _paramName);
    }

    /// <summary>
    /// Throws an <see cref="AssertionFailedException"/> if a <paramref name="condition"/> is not met.
    /// </summary>
    /// <param name="condition">Condition to be checked.</param>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="_expression">The condition expression (generated automatically from the compiler).</param>
    /// <exception cref="AssertionFailedException">Thrown if <paramref name="condition"/> is <c>false</c>.</exception>
    public static void That([DoesNotReturnIf(false)] bool condition, string? message = null, [CallerArgumentExpression("condition")] string _expression = null!)
    {
        if (condition)
            return;

        Fail(message, _expression);
    }

    /// <summary>
    /// Throws an <see cref="AssertionFailedException"/> if <paramref name="obj"/> is <c>null</c>.
    /// </summary>
    /// <param name="obj">Non-null object.</param>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="_expression">The non-null expression (generated automatically from the compiler).</param>
    /// <exception cref="AssertionFailedException">Thrown if <paramref name="obj"/> is <c>null</c>.</exception>
    public static void NotNull([NotNull] object? obj, string? message = null, [CallerArgumentExpression("obj")] string _expression = null!)
    {
        if (obj is not null)
            return;

        Fail(message, Wrap("{0} is not null", _expression));
    }

    /// <summary>
    /// Casts <paramref name="obj"/> to <typeparamref name="T"/>, otherwise throws an <see cref="AssertionFailedException"/>.
    /// </summary>
    /// <typeparam name="T">Type to be casted to.</typeparam>
    /// <param name="obj">Object to be casted to <typeparamref name="T"/>.</param>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="_expression">The expression the value of which to be casted to <typeparamref name="T"/> (generated automatically from the compiler).</param>
    /// <returns><paramref name="obj"/> casted to <typeparamref name="T"/>.</returns>
    /// <exception cref="AssertionFailedException">Thrown if <paramref name="obj"/> couldn't be casted to <typeparamref name="T"/>.</exception>
    public static T Cast<T>(object? obj, string? message = null, [CallerArgumentExpression("obj")] string _expression = null!)
    {
        if (obj is T casted)
            return casted;

        Fail(message, Wrap("{0} is {1}", _expression, typeof(T).FullName));
        return default;  // will never happen
    }

    /// <summary>
    /// Throws an <see cref="AssertionFailedException"/>.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <exception cref="AssertionFailedException">Always thrown.</exception>
    [DoesNotReturn]
    public static void Fail(string? message = null)
        => Fail(message, null);

    [DoesNotReturn]
    static void Fail(string? message, string? expression)
        => throw new AssertionFailedException(expression, message);

    [return: NotNullIfNotNull("expression")]
    static string? Wrap(string format, string? expression, params object?[] args)
    {
        if (expression == null)
            return null;

        if (MayBeCompositeExpression.IsMatch(expression))
            expression = $"({expression})";

        if (args == null || args.Length == 0)
            return string.Format(format, expression);

        return string.Format(format, new[] { expression }.Concat(args).ToArray());
    }
}

[Serializable]
public class AssertionFailedException : Exception
{
    public virtual string? Expression { get; }

    public AssertionFailedException() : this(null) { }

    public AssertionFailedException(string? expression) : this(expression, message: null) { }

    public AssertionFailedException(string? message, Exception? inner) : base(message ?? "Assertion failed.", inner) { }

    public AssertionFailedException(string? expression, string? message) : base((message ?? "Assertion failed.") + (expression == null ? "" : $"{Environment.NewLine}Expression: {expression}"))
    {
        Expression = expression;
    }

    protected AssertionFailedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
