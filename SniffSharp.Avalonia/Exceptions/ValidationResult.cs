using System;
using SniffSharp.Avalonia.Utility;

namespace SniffSharp.Avalonia.Exceptions;

public class ValidationResult(ValidationResultType type = ValidationResultType.Success, string? message = default)
{
    public bool IsValid => type == ValidationResultType.Success;
    
    public static ValidationResult Success() => new ValidationResult();
    
    public static ValidationResult Error(string message) => new ValidationResult(ValidationResultType.Error, message);
}

public enum ValidationResultType
{
    Success,
    Error
}