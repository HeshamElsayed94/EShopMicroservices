using BuildingBlocks.Common.Results.Errors;

namespace BuildingBlocks.Common.Results.Interfaces;

public interface IResult
{
	List<Error>? Errors { get; }

	bool ISuccess { get; }
}

public interface IResult<out TValue> : IResult
{
	TValue Value { get; }
}