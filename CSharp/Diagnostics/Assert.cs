// Copyright (c) 2022 Fabio Iotti
// The copyright holders license this file to you under the MIT license,
// available at https://github.com/bruce965/util/raw/master/LICENSE

using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace Utility;

/// <summary>
/// Utilities to make assertions evaluated during execution.
/// <para>
/// Unlike <see cref="Debug.Assert"/>, these assertions are not ignored in release builds.
/// </para>
/// </summary>
public static class Assert
{
    static readonly Regex MayBeCompositeExpression = new(@"[-+*/&|^~%?:<>=!]", RegexOptions.Compiled);

    /// <summary>
    /// Throws an <see cref="ArgumentNullException"/> if <paramref name="argument"/> is <c>null</c>.
    /// </summary>
    /// <param name="argument">The reference type argument to validate as non-null.</param>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds (added automatically by the compiler).</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="argument"/> is <c>null</c>.</exception>
    public static T ArgumentNotNull<T>([NotNull] T? argument, string? message = null, [CallerArgumentExpression("argument")] string? paramName = null)
    {
        if (argument is not null)
            return argument;

        throw new ArgumentNullException(paramName, message);
    }

    /// <summary>
    /// Throws an <see cref="ArgumentOutOfRangeException"/> if a <paramref name="condition"/> is not met.
    /// </summary>
    /// <param name="argument">The reference type argument to check the condition on.</param>
    /// <param name="condition">The condition to be checked.</param>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds (added automatically by the compiler).</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="condition"/> is <c>false</c>.</exception>
    public static T ArgumentInRange<T>(T argument, [DoesNotReturnIf(false)] bool condition, string? message = null, [CallerArgumentExpression("argument")] string? paramName = null)
    {
        ArgumentInRange(condition, message, paramName);
        return argument;
    }

    /// <inheritdoc cref="ArgumentInRange{T}(T, bool, string?, string)"/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Span<T> ArgumentInRange<T>(Span<T> argument, [DoesNotReturnIf(false)] bool condition, string? message = null!, [CallerArgumentExpression("argument")] string? paramName = null)
    {
        ArgumentInRange(condition, message, paramName);
        return argument;
    }

    /// <inheritdoc cref="ArgumentInRange{T}(T, bool, string?, string)"/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static ReadOnlySpan<T> ArgumentInRange<T>(ReadOnlySpan<T> argument, [DoesNotReturnIf(false)] bool condition, string? message = null, [CallerArgumentExpression("argument")] string? paramName = null)
    {
        ArgumentInRange(condition, message, paramName);
        return argument;
    }

    [StackTraceHidden]
    static void ArgumentInRange([DoesNotReturnIf(false)] bool condition, string? message, string? paramName)
    {
        if (condition)
            return;

        throw new ArgumentOutOfRangeException(paramName, message);
    }

    /// <summary>
    /// Throws an <see cref="ArgumentException"/> if a <paramref name="condition"/> is not met.
    /// </summary>
    /// <param name="param">Reference to the parameter.</param>
    /// <param name="condition">Condition to be checked.</param>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds (added automatically by the compiler).</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="condition"/> is <c>false</c>.</exception>
    public static T Argument<T>(T param, [DoesNotReturnIf(false)] bool condition, string? message = null, [CallerArgumentExpression("param")] string? paramName = null)
    {
        Argument(condition, message, paramName);
        return param;
    }

    /// <inheritdoc cref="Argument{T}(T, bool, string?, string)"/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Span<T> Argument<T>(Span<T> param, [DoesNotReturnIf(false)] bool condition, string? message = null, [CallerArgumentExpression("param")] string? paramName = null)
    {
        Argument(condition, message, paramName);
        return param;
    }

    /// <inheritdoc cref="Argument{T}(T, bool, string?, string)"/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static ReadOnlySpan<T> Argument<T>(ReadOnlySpan<T> param, [DoesNotReturnIf(false)] bool condition, string? message = null, [CallerArgumentExpression("param")] string? paramName = null)
    {
        Argument(condition, message, paramName);
        return param;
    }

    [StackTraceHidden]
    static void Argument([DoesNotReturnIf(false)] bool condition, string? message, string? paramName)
    {
        if (condition)
            return;

        throw new ArgumentException(message, paramName);
    }

    /// <summary>
    /// Throws an <see cref="AssertionFailedException"/> if a <paramref name="condition"/> is not met.
    /// </summary>
    /// <param name="condition">Condition to be checked.</param>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="expression">The condition expression (added automatically by the compiler).</param>
    /// <exception cref="AssertionFailedException">Thrown if <paramref name="condition"/> is <c>false</c>.</exception>
    public static void That([DoesNotReturnIf(false)] bool condition, string? message = null, [CallerArgumentExpression("condition")] string? expression = null)
    {
        if (condition)
            return;

        Fail(message, expression);
        throw null; // will never happen
    }

    /// <summary>
    /// Throws an <see cref="AssertionFailedException"/> if <paramref name="obj"/> is not <c>null</c>.
    /// </summary>
    /// <param name="obj">Null object.</param>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="expression">The null expression (added automatically by the compiler).</param>
    /// <exception cref="AssertionFailedException">Thrown if <paramref name="obj"/> is not <c>null</c>.</exception>
    public static void Null<T>(T obj, string? message = null, [CallerArgumentExpression("obj")] string? expression = null)
    {
        if (obj is null)
            return;

        Fail(message, WrapExpression("{0} is null", expression));
        throw null; // will never happen
    }

    /// <summary>
    /// Throws an <see cref="AssertionFailedException"/> if <paramref name="obj"/> is <c>null</c>.
    /// </summary>
    /// <param name="obj">Non-null object.</param>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="expression">The non-null expression (added automatically by the compiler).</param>
    /// <exception cref="AssertionFailedException">Thrown if <paramref name="obj"/> is <c>null</c>.</exception>
    public static T NotNull<T>([NotNull] T obj, string? message = null, [CallerArgumentExpression("obj")] string? expression = null)
    {
        if (obj is not null)
            return obj;

        Fail(message, WrapExpression("{0} is not null", expression));
        throw null; // will never happen
    }

    /// <summary>
    /// Casts <paramref name="obj"/> to <typeparamref name="T"/>, otherwise throws an <see cref="AssertionFailedException"/>.
    /// </summary>
    /// <typeparam name="T">Type to be casted to.</typeparam>
    /// <param name="obj">Object to be casted to <typeparamref name="T"/>.</param>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="expression">The expression the value of which to be casted to <typeparamref name="T"/> (added automatically by the compiler).</param>
    /// <returns><paramref name="obj"/> casted to <typeparamref name="T"/>.</returns>
    /// <exception cref="AssertionFailedException">Thrown if <paramref name="obj"/> couldn't be casted to <typeparamref name="T"/>.</exception>
    public static T Cast<T>(object? obj, string? message = null, [CallerArgumentExpression("obj")] string? expression = null)
    {
        if (obj is T casted)
            return casted;

        Fail(message, WrapExpression("{0} is {1}", expression, typeof(T).FullName));
        throw null; // will never happen
    }

    /// <summary>
    /// Throws an <see cref="AssertionFailedException"/> if the specified <see cref="object"/> instances are not considered equal.
    /// </summary>
    /// <param name="objA">The first object to compare.</param>
    /// <param name="objB">The second object to compare.</param>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="expressionA">The expression of the first argument (added automatically by the compiler).</param>
    /// <param name="expressionB">The expression of the second argument (added automatically by the compiler).</param>
    /// <returns><paramref name="objB"/>.</returns>
    /// <exception cref="AssertionFailedException">Thrown if <paramref name="objA"/> and <paramref name="objB"/> are not considered equal, not thrown if both are <see langword="null"/>.</exception>
    [return: NotNullIfNotNull(nameof(objB))]
    public static T Equal<T>([NotNullIfNotNull("objB")] T objA, [NotNullIfNotNull("objA")] T objB, string? message = null, [CallerArgumentExpression("objA")] string? expressionA = null, [CallerArgumentExpression("objB")] string? expressionB = null)
    {
        if (object.Equals(objA, objB))
            return objB;

        Fail(message, WrapExpression("Equals({0}, {1})", expressionA, expressionB));
        throw null; // will never happen
    }

    /// <summary>
    /// Throws an <see cref="AssertionFailedException"/> if the specified instances are not the same instance.
    /// </summary>
    /// <param name="objA">The first object to compare.</param>
    /// <param name="objB">The second object to compare.</param>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="expressionA">The expression of the first argument (added automatically by the compiler).</param>
    /// <param name="expressionB">The expression of the second argument (added automatically by the compiler).</param>
    /// <returns><param name="objA"> and <param name="objB">.</returns>
    /// <exception cref="AssertionFailedException">Thrown if <paramref name="objA"/> is not the same instance as <paramref name="objB"/>, not thrown if both are <see langword="null"/>.</exception>
    [return: NotNullIfNotNull(nameof(objA)), NotNullIfNotNull(nameof(objB))]
    public static T ReferenceEqual<T>([NotNullIfNotNull("objB")] T objA, [NotNullIfNotNull("objA")] T objB, string? message = null, [CallerArgumentExpression("objA")] string? expressionA = null, [CallerArgumentExpression("objB")] string? expressionB = null)
    {
        if (object.ReferenceEquals(objA, objB))
            return objA;

        Fail(message, WrapExpression("ReferenceEquals({0}, {1})", expressionA, expressionB));
        throw null; // will never happen
    }

    /// <summary>
    /// Throws an <see cref="AssertionFailedException"/>.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <returns>This method never returns; the return type enables usage of the <see langword="throw"/> keyword.</returns>
    /// <exception cref="AssertionFailedException">Always thrown.</exception>
    [DoesNotReturn]
    public static Exception Fail(string? message = null)
    {
        Fail(message, null);
        throw null; // will never happen
    }

    [StackTraceHidden]
    [DoesNotReturn]
    static void Fail(string? message, string? expression)
        => throw new AssertionFailedException(message, expression);

    [return: NotNullIfNotNull(nameof(expression))]
    static string? WrapExpression(string format, string? expression, params object?[] args)
    {
        if (expression == null)
            return null;

        if (MayBeCompositeExpression.IsMatch(expression))
            expression = $"({expression})";

        if (args == null || args.Length == 0)
            return string.Format(format, expression);

        return string.Format(format, new[] { expression }.Concat(args).ToArray());
    }

    /// <inheritdoc cref="object.Equals"/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static new bool Equals(object? objA, object? objB)
        => object.Equals(objA, objB);

    /// <inheritdoc cref="object.ReferenceEquals"/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static new bool ReferenceEquals(object? objA, object? objB)
        => object.ReferenceEquals(objA, objB);
}

/// <summary>
/// Represents assertions that failed during execution.
/// </summary>
[Serializable]
public class AssertionFailedException : Exception
{
    public AssertionFailedException() : this(null, null, null) { }

    public AssertionFailedException(string? message) : this(message, null, null) { }

    public AssertionFailedException(string? message, Exception? innerException) : this(message, null, innerException) { }

    public AssertionFailedException(string? message, string? expression) : this(message, expression, null) { }

    public AssertionFailedException(string? message, string? expression, Exception? innerException) : base(message ?? "Assertion failed.", innerException)
    {
        Expression = expression;
    }

    protected AssertionFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        Expression = info.GetString(nameof(Expression));
    }

    /// <inheritdoc/>
    public override string Message
        => Expression == null ? base.Message : $"{base.Message} (Expression '{Expression}')";

    /// <summary>
    /// Gets a string representation of the expression for which the assertion failed.
    /// </summary>
    /// <value>String representation of the expression for which the assertion failed, or <see langword="null"/> if not available.</value>
    public virtual string? Expression { get; }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(Expression), Expression, typeof(string));
    }
}
