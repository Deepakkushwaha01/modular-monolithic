using App.Core.SharedLibrary.Patterns.Result;

namespace App.Core.SharedLibrary.Patterns.Mediatr.Abstractions;

public interface ICommand<TResult> where TResult : ResultCommonLogic
{
}


public interface ICommandHandler<in TCommand, TResult>
    where TCommand : ICommand<TResult>
    where TResult : ResultCommonLogic
{
    Task<TResult> ExecuteAsync(TCommand command, CancellationToken cancellationToken = default(CancellationToken));
}


public interface ICommandDispatcher
{
    Task<TResult> ExecuteAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default(CancellationToken)) where TResult : ResultCommonLogic;
}