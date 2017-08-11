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
    public class OptionController : Controller
    {
        public IOptionService<Option> _service;
        public IQuestionService<Question> _questionService;

        public OptionController(IOptionService<Option> service, IQuestionService<Question> questionService)
        {
            _service = service;
            _questionService = questionService;
        }

        // GET: Admin/Option
        public async Task<ActionResult> Index()
        {
            List<Option> questions = await _service.GetAll();
            return View(questions);
        }

        // GET: Admin/Option/Details/5
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                Option question = await _service.Get(id);
                if (question == null)
                    return HttpNotFound();

                return View(question);
            }
            catch(ValidationException e)
            {
                return HttpNotFound(e.Message);
            }
            
        }

        // GET: Admin/Option/Create
        public async Task<ActionResult> Create()
        {
            List<Question> questions = await _questionService.GetAll();
            ViewBag.Questions = new SelectList(questions.Select(q => new { id=q.Id, name=q.Title }), "id", "name");
            return View();
        }

        // POST: Admin/Option/Create
        [HttpPost]
        public async Task<ActionResult> Create(Option model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _service.Create(model);
                    return RedirectToAction("Index");
                }     
            }
            catch (ValidationException e)
            {
                ModelState.AddModelError(e.Property, e.Message);
            }

            List<Question> questions = await _questionService.GetAll();
            ViewBag.Questions = new SelectList(questions.Select(q => new { id = q.Id, name = q.Title }), "id", "name");
            return View(model);
        }

        // GET: Admin/Option/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                Option question = await _service.Get(id);
                if (question == null)
                    return HttpNotFound();

                List<Question> questions = await _questionService.GetAll();
                ViewBag.Questions = new SelectList(questions.Select(q => new { id = q.Id, name = q.Title }), "id", "name", question.QuestionId);

                return View(question);
            }
            catch (ValidationException e)
            {
                return HttpNotFound(e.Message);
            }
        }

        // POST: Admin/Option/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(Option model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _service.Update(model);
                    return RedirectToAction("Index");
                }
            }
            catch (ValidationException e)
            {
                ModelState.AddModelError(e.Property, e.Message);
            }

            List<Question> questions = await _questionService.GetAll();
            ViewBag.Questions = new SelectList(questions.Select(q => new { id = q.Id, name = q.Title }), "id", "name", model.QuestionId);
            return View(model);
        }

        // GET: Admin/Option/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                Option question = await _service.Get(id);
                if (question == null)
                    return HttpNotFound();

                return View(question);
            }
            catch (ValidationException e)
            {
                return HttpNotFound(e.Message);
            }
        }

        // POST: Admin/Option/Delete/5
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
