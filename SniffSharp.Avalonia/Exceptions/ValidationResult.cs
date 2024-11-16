using System;
using SniffSharp.Avalonia.Utility;

namespace SniffSharp.Avalonia.Exceptions;

public partial class ValidationResult
{
    public static ValidationResult<TValue> Create<TValue>(ValidationResultType type = ValidationResultType.Success, string? message = null)
    {
        return new(type, message);
    }
    
    public static ValidationResult<TValue> Create<TValue>(ValidationResultType type, string? message, TValue value)
    {
        return new(type, message, value);
    }
    
    public static ValidationResult<TValue> Create<TValue>(TValue value, ValidationResultType type)
    {
        return new(type, null, value);
    }
}

public partial class ValidationResult<THolder>(ValidationResultType type = ValidationResultType.Success, string? message = default, THolder? value = default)
{
    public THolder? Value { get; set; } = value;

    public string? Message { get; set; } = message;

    private ValidationResultType Type { get; set; } = type;

    public bool IsValid => Type == ValidationResultType.Success;
}

public enum ValidationResultType
{
    Success,
    Error
}