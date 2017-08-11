using OneInc.Models;
using OneInc.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DevExtreme.AspNet.Data;
using OneInc.Infrastructure;
using System.Net.Http.Headers;

namespace OneInc.Areas.Admin.Controllers
{
    public class ResultController : Controller
    {
        public IResultService<Result> _service;
        public IQuestionService<Question> _questionService;

        public ResultController(IResultService<Result> service, IQuestionService<Question> questionService)
        {
            _service = service;
            _questionService = questionService;
        }

        // GET: Admin/Result
        public async Task<ActionResult> Index()
        {
            List<Result> results = await _service.GetAll();
            return View(results);
        }

        // GET: Admin/Result/Details/5
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                Result result = await _service.Get(id);
                if (result == null)
                    return HttpNotFound();

                return View(result);
            }
            catch(ValidationException e)
            {
                return HttpNotFound(e.Message);
            }
            
        }
        
        // GET: Admin/Result/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                Result question = await _service.Get(id);
                if (question == null)
                    return HttpNotFound();

                return View(question);
            }
            catch (ValidationException e)
            {
                return HttpNotFound(e.Message);
            }
        }

        // POST: Admin/Result/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirm(int id)
        {
            try
            {
                await _service.Delete(id);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
