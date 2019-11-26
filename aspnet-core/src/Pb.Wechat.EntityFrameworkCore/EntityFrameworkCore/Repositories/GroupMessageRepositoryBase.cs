using Abp.Domain.Entities;
using Abp.EntityFrameworkCore;
using Abp.EntityFrameworkCore.Repositories;

namespace Pb.Wechat.EntityFrameworkCore.Repositories
{
    public abstract class GroupMessageRepositoryBase<TEntity, TPrimaryKey> : EfCoreRepositoryBase<GroupMessageDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected GroupMessageRepositoryBase(IDbContextProvider<GroupMessageDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //add your common methods for all repositories
    }

    /// <summary>
    /// Base class for custom repositories of the application.
    /// This is a shortcut of <see cref="CYRepositoryBase{TEntity,TPrimaryKey}"/> for <see cref="int"/> primary key.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public abstract class GroupMessageRepositoryBase<TEntity> : GroupMessageRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected GroupMessageRepositoryBase(IDbContextProvider<GroupMessageDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)!!!
    }
}
