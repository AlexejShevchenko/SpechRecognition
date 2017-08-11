using OneInc.Infrastructure;
using OneInc.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace OneInc.Services
{
    public class QuestionService : IQuestionService<Question>
    {
        private readonly AppContext _db;

        public QuestionService()
        {
            _db = new AppContext();
        }


        public async Task<Question> Get(int id)
        {
                return await _db.Question.Include(q => q.Options).FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<List<Question>> GetAll()
        {
            return await _db.Question.Include(q=>q.Options).ToListAsync();
        }

        public async Task Create(Question model)
        {
            Question question = await _db.Question.FirstOrDefaultAsync(q => string.Compare(q.Title, model.Title, true) == 0);
            if (question != null)
                throw new ValidationException("Такой вопрос уже добавлен", "Title");

            _db.Question.Add(model);
            await _db.SaveChangesAsync();
        }

        public async Task Update(Question model)
        {
            Question question = await _db.Question.FirstOrDefaultAsync(q => q.Id == model.Id);
            if (question == null)
                throw new ValidationException("Вопрос не найден", "");

            question.Title = model.Title;

            _db.Entry<Question>(question).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            Question question = await _db.Question.FirstOrDefaultAsync(q => q.Id == id);
            if (question == null)
                throw new ValidationException("Вопрос не найден", "");

            _db.Question.Remove(question);
            await _db.SaveChangesAsync();
        }
        
    }
}