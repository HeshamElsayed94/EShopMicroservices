using Ordering.Domain.Common.Errors;

namespace Ordering.Domain.Common.Results.Interfaces;

public interface IResult
{
    IReadOnlyList<Error>? Errors { get; }

    bool ISuccess { get; }
}

public interface IResult<out TValue> : IResult
{
    TValue Value { get; }
}