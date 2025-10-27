using System.Threading;
using System.Threading.Tasks;

namespace SPSS.Repository.UnitOfWork;

public interface IUnitOfWork : IAsyncDisposable
{
	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

	Task BeginTransactionAsync(CancellationToken cancellationToken = default);

	Task CommitTransactionAsync(CancellationToken cancellationToken = default);

	Task RollbackTransactionAsync(CancellationToken cancellationToken = default);

	TRepository GetRepository<TRepository>() where TRepository : class;
}