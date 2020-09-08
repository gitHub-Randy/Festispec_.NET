using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FestiSpec.Entities.Offline
{
    public class OfflineDbSet<TEntity> : IDbSet<TEntity> where TEntity : class
    {
        public List<TEntity> Set { get; set; }

        public OfflineDbSet(List<TEntity> set)
        {
            Set = set;
        }

        public ObservableCollection<TEntity> Local => throw new NotImplementedException();

        public Expression Expression => throw new NotImplementedException();

        public Type ElementType => throw new NotImplementedException();

        public IQueryProvider Provider => throw new NotImplementedException();

        public TEntity Add(TEntity entity)
        {
            Set.Add(entity);
            return entity;
        }

        public TEntity Attach(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public TEntity Create()
        {
            throw new NotImplementedException();
        }

        public TEntity Find(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return Set.GetEnumerator();
        }

        public TEntity Remove(TEntity entity)
        {
            Set.Remove(entity);
            return null;
        }

        TDerivedEntity IDbSet<TEntity>.Create<TDerivedEntity>()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Set.GetEnumerator();
        }
    }
}
