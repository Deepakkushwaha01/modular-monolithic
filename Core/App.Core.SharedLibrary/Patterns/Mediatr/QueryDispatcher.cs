using App.Core.SharedLibrary.Patterns.Result;
using App.Core.SharedLibrary.Errors;

namespace App.Core.SharedLibrary.Patterns.Mediatr.Abstractions;

public class QueryDispatcher : IQueryDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public QueryDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public async Task<Result<TResult>> ExecuteAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
    {
        if (query is null)
            throw new ArgumentNullException(nameof(query));

        Type handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));

        dynamic? handler = _serviceProvider.GetService(handlerType);
        if (handler == null)
            throw new InvalidOperationException(string.Format(
                System.Globalization.CultureInfo.CurrentCulture,
                CommonErrorMessages.HandlerNotFound,
                "query",
                query.GetType().Name));

        return await handler.ExecuteAsync((dynamic)query, cancellationToken);
    }
}
