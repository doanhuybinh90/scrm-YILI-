using Abp.Domain.Entities;
using Abp.EntityFrameworkCore;
using Abp.EntityFrameworkCore.Repositories;

namespace Pb.Wechat.EntityFrameworkCore.Repositories
{
    public abstract class KfRepositoryBase<TEntity, TPrimaryKey> : EfCoreRepositoryBase<KfDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected KfRepositoryBase(IDbContextProvider<KfDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //add your common methods for all repositories
    }

    /// <summary>
    /// Base class for custom repositories of the application.
    /// This is a shortcut of <see cref=KfRepositoryBase{TEntity,TPrimaryKey}"/> for <see cref="int"/> primary key.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public abstract class KfRepositoryBase<TEntity> : KfRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected KfRepositoryBase(IDbContextProvider<KfDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)!!!
    }
}
