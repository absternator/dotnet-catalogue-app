namespace Catalogue.Api.UnitOfWork;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly ILogger _logger;

    public IUnitOfWork Items { get; private set; }

    public UnitOfWork(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger("logs");
    }
}