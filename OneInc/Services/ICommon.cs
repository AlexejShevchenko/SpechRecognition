using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace OneInc.Services
{
    public interface ICommon<T>
    {
        Task<List<T>> GetAll();
        Task<T> Get(int id);
        Task Create(T model);
        Task Update(T model);
        Task Delete(int id);
    }
}