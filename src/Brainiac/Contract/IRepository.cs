using System.Collections.Generic;

namespace Brainiac.Contract
{
    public interface IRepository<T, TId> where T : IDataItem<TId>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetById(TId id);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        List<T> GetAll();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        int Count();

        /// <summary>
        /// 
        /// </summary>
        void Remove();

        /// <summary>
        /// 
        /// </summary>
        void RemoveAll();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        string Save(T entity);
    }
}
