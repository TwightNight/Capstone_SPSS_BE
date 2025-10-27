using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Shared.Base
{
	public interface IGenericRepository<T, TKey> where T : class
	{
		IQueryable<T> Entities { get; }

		Task<T?> GetByIdAsync(TKey id);

		Task<T?> GetSingleAsync(
			Expression<Func<T, bool>> predicate,
			Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);

		Task<T?> GetFirstOrDefaultAsync(
			Expression<Func<T, bool>> predicate,
			Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);

		Task<IEnumerable<T>> GetAsync(
			Expression<Func<T, bool>>? filter = null,
			Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
			Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);

		Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(
			int pageNumber,
			int pageSize,
			Expression<Func<T, bool>>? filter = null,
			Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
			Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);

		Task<int> CountAsync(Expression<Func<T, bool>>? filter = null);

		Task<bool> ExistsAsync(Expression<Func<T, bool>>? filter = null);

		void Add(T entity);

		void AddRange(IEnumerable<T> entities);

		void Update(T entity);

		void Delete(T entity);

		Task DeleteAsync(TKey id);

		void RemoveRange(IEnumerable<T> entities);

		void Attach(T entity);

		void Detach(T entity);
	}
}
