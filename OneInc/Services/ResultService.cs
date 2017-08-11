using OneInc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data.Entity;
using OneInc.Infrastructure;
using System.IO;

namespace OneInc.Services
{
    public class ResultService : IResultService<Result>
    {
        private readonly AppContext _db;

        public ResultService()
        {
            _db = new AppContext();
        }


        public async Task<Result> Get(int id)
        {
            return await _db.Results.Include(o=>o.Option).Include(o=>o.Option.Question).FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<List<Result>> GetAll()
        {
            return await _db.Results.Include(o => o.Option).Include(o => o.Option.Question).ToListAsync();
        }

        public async Task Create(Result model)
        {
            Option option = await _db.Options.FirstOrDefaultAsync(o => o.Id == model.OptionId);
            if(option == null)
                throw new ValidationException("Ответ не найден", "");

            _db.Results.Add(model);
            await _db.SaveChangesAsync();
        }

        public async Task Update(Result model)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(int id)
        {
            Result result = await _db.Results.FirstOrDefaultAsync(q => q.Id == id);
            if (result == null)
                throw new ValidationException("Ответ не найден", "");

            _db.Results.Remove(result);
            await _db.SaveChangesAsync();
        }

        
    }
}