using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using SPSS.Repository.UnitOfWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SPSS.BusinessObject.Context;

namespace SPSS.Repository.UnitOfWork.Implementations;

public class UnitOfWork : IUnitOfWork
{
    private readonly ProductOrderDBContext _context;
    private readonly IServiceProvider _serviceProvider;
    private IDbContextTransaction? _transaction;
    private bool _disposed = false;

    private readonly Dictionary<Type, object> _repositories;

    public UnitOfWork(
        ProductOrderDBContext context,
        IServiceProvider serviceProvider)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _repositories = new Dictionary<Type, object>();
    }

    public TRepository GetRepository<TRepository>() where TRepository : class
    {
        var type = typeof(TRepository);

        if (_repositories.ContainsKey(type))
        {
            return (TRepository)_repositories[type];
        }

        var repository = _serviceProvider.GetRequiredService(type);

        _repositories.Add(type, repository);
        return (TRepository)repository;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            throw new InvalidOperationException("A transaction is already in progress.");
        }
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
        {
            throw new InvalidOperationException("No transaction to commit.");
        }

        try
        {
            await SaveChangesAsync(cancellationToken);
            await _transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
        {
            throw new InvalidOperationException("No transaction to rollback.");
        }

        try
        {
            await _transaction.RollbackAsync(cancellationToken);
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed) return;

        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
        }

        await _context.DisposeAsync();

        _repositories.Clear();
        _disposed = true;
    }
}