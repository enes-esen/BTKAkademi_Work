﻿using System;
using System.Linq.Expressions;

namespace Repositories.Contracts
{
	public interface IRepositoryBase<T>
	{
		IQueryable<T> FindAll(bool trackChanges);
		IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges);

		void Create();
		void Update();
		void Delete();
	}
}

