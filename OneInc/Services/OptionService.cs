using OneInc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data.Entity;
using OneInc.Infrastructure;

namespace OneInc.Services
{
    public class OptionService : IOptionService<Option>
    {
        private readonly AppContext _db;

        public OptionService()
        {
            _db = new AppContext();
        }


        public async Task<Option> Get(int id)
        {
            return await _db.Options.Include(o=>o.Question).FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<List<Option>> GetAll()
        {
            return await _db.Options.Include(o => o.Question).ToListAsync();
        }

        public async Task Create(Option model)
        {
            Option option = await _db.Options.FirstOrDefaultAsync(q => string.Compare(q.Value, model.Value, true) == 0 && q.QuestionId == model.QuestionId);
            if (option != null)
                throw new ValidationException("Такой ответ уже добавлен", "Value");

            _db.Options.Add(model);
            await _db.SaveChangesAsync();
        }

        public async Task Update(Option model)
        {
            Option option = await _db.Options.FirstOrDefaultAsync(q => string.Compare(q.Value, model.Value, true) == 0 && q.QuestionId == model.QuestionId);
            if (option != null)
                throw new ValidationException("Такой ответ уже добавлен", "Value");

            option = await _db.Options.FirstOrDefaultAsync(q => q.Id == model.Id);
            if (option == null)
                throw new ValidationException("Ответ не найден", "");

            option.QuestionId = model.QuestionId;
            option.Value = model.Value;

            _db.Entry<Option>(option).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            Option option = await _db.Options.FirstOrDefaultAsync(q => q.Id == id);
            if (option == null)
                throw new ValidationException("Ответ не найден", "");

            _db.Options.Remove(option);
            await _db.SaveChangesAsync();
        }
    }
}