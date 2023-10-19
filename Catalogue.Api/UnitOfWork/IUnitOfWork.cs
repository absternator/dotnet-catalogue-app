using Catalogue.Api.Repositories;

namespace Catalogue.Api.UnitOfWork
{
    public interface IUnitOfWork
    {
        IItemRepository Items { get; }

        Task CompleteAsync();
    }
}