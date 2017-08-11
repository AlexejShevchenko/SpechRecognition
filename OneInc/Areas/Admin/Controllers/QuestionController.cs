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
    public class QuestionController : Controller
    {
        public IQuestionService<Question> _service;

        public QuestionController(IQuestionService<Question> service)
        {
            _service = service;
        }

        // GET: Admin/Question
        public async Task<ActionResult> Index()
        {
            List<Question> questions = await _service.GetAll();
            return View(questions);
        }

        // GET: Admin/Question/Details/5
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                Question question = await _service.Get(id);
                if (question == null)
                    return HttpNotFound();

                return View(question);
            }
            catch(ValidationException e)
            {
                return HttpNotFound(e.Message);
            }
            
        }

        // GET: Admin/Question/Create
        public async Task<ActionResult> Create()
        {
            return View();
        }

        // POST: Admin/Question/Create
        [HttpPost]
        public async Task<ActionResult> Create(Question model)
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

            return View(model);
        }

        // GET: Admin/Question/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                Question question = await _service.Get(id);
                if (question == null)
                    return HttpNotFound();

                return View(question);
            }
            catch (ValidationException e)
            {
                return HttpNotFound(e.Message);
            }
        }

        // POST: Admin/Question/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(Question model)
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

            return View(model);
        }

        // GET: Admin/Question/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                Question question = await _service.Get(id);
                if (question == null)
                    return HttpNotFound();

                return View(question);
            }
            catch (ValidationException e)
            {
                return HttpNotFound(e.Message);
            }
        }

        // POST: Admin/Question/Delete/5
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
