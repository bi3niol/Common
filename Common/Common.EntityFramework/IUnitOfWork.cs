using System;
using System.Data.Entity;

namespace Common.EntityFramework
{
    public interface IUnitOfWork<TContext> : IDisposable where TContext : DbContext
    {
        TContext Context { get; }
        void Save();
    }
}
