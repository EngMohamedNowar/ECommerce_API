using System;

namespace ECommerce.Domain.Common
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error Error { get; }

        protected Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None)
                throw new InvalidOperationException("لا يمكن أن يكون النجاح مصحوبًا بـ Error.");

            if (!isSuccess && error == Error.None)
                throw new InvalidOperationException("الفشل لازم يكون له Error.");

            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success() => new(true, Error.None);

        public static Result Failure(Error error) => new(false, error);

        public static Result<TValue> Success<TValue>(TValue value) =>
            new(value, true, Error.None);

        public static Result<TValue> Failure<TValue>(Error error) =>
            new(default, false, error);

        // Helper بيرجع Result مناسب حسب لو فيه Value ولا لأ (مفيد مع validation)
        public static Result Create(bool condition, Error error) =>
            condition ? Success() : Failure(error);
    }

    public class Result<TValue> : Result
    {
        private readonly TValue? _value;

        protected internal Result(TValue? value, bool isSuccess, Error error)
            : base(isSuccess, error)
        {
            _value = value;
        }

        public TValue Value => IsSuccess
            ? _value!
            : throw new InvalidOperationException("مينفعش تجيب Value من Result فاشل.");

        public static implicit operator Result<TValue>(TValue value) =>
            Create(value);

        public static Result<TValue> Create(TValue? value) =>
            value is not null ? Success(value) : Failure<TValue>(Error.NotFound("Value.Null", "القيمة غير موجودة."));
    }
}