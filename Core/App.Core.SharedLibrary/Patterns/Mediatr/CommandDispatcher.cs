using App.Core.SharedLibrary.Patterns.Result;
using App.Core.SharedLibrary.Errors;

namespace App.Core.SharedLibrary.Patterns.Mediatr.Abstractions;

public class CommandDispatcher : ICommandDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public CommandDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResult> ExecuteAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
        where TResult : ResultCommonLogic
    {
        Type handlerType = typeof(ICommandHandler<,>)
            .MakeGenericType(command.GetType(), typeof(TResult));

        dynamic? handler = _serviceProvider.GetService(handlerType);

        if (handler == null)
            throw new InvalidOperationException(string.Format(
                System.Globalization.CultureInfo.CurrentCulture,
                CommonErrorMessages.HandlerNotFound,
                "command",
                command.GetType().Name));

        return await handler.ExecuteAsync((dynamic)command, cancellationToken);
    }
}
