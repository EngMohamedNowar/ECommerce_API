using System;

namespace ECommerce.Domain.Common
{
    public sealed class Error
    {
        public string Code { get; }
        public string Message { get; }
        public ErrorType Type { get; }

        private Error(string code, string message, ErrorType type)
        {
            Code = code;
            Message = message;
            Type = type;
        }

        // Represents "no error" (used with Result pattern)
        public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure);

        public static Error Validation(string code, string message) =>
            new(code, message, ErrorType.Validation);

        public static Error NotFound(string code, string message) =>
            new(code, message, ErrorType.NotFound);

        public static Error Conflict(string code, string message) =>
            new(code, message, ErrorType.Conflict);

        public static Error Unauthorized(string code, string message) =>
            new(code, message, ErrorType.UnAuthorized);

        public static Error Forbidden(string code, string message) =>
            new(code, message, ErrorType.Forbidden);

        public static Error Failure(string code, string message) =>
            new(code, message, ErrorType.Failure);

        public override string ToString() => $"{Code}: {Message}";

        public static bool operator ==(Error? left, Error? right)
        {
            if (left is null && right is null) return true;
            if (left is null || right is null) return false;
            return left.Equals(right);
        }

        public static bool operator !=(Error? left, Error? right) => !(left == right);

        public override bool Equals(object? obj)
        {
            if (obj is not Error other) return false;
            return Code == other.Code && Message == other.Message && Type == other.Type;
        }

        public override int GetHashCode() => HashCode.Combine(Code, Message, Type);
    }
}