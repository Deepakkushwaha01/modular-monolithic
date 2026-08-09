using App.Core.SharedLibrary.Patterns.Result;

namespace App.Core.SharedLibrary.Patterns.Mediatr.Abstractions;

public interface IQuery<TResult>
{
}


public interface IQueryHandler<in TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    Task<Result<TResult>> ExecuteAsync(TQuery query, CancellationToken cancellationToken = default(CancellationToken));
}

public interface IQueryDispatcher
{
    Task<Result<TResult>> ExecuteAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default(CancellationToken));
}