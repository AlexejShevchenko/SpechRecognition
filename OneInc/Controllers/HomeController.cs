using OneInc.Models;
using OneInc.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Speech.AudioFormat;
using System.Speech.Recognition;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace OneInc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IQuestionService<Question> _questionService;

        public HomeController(IQuestionService<Question> questionService)
        {
            _questionService = questionService;
        }

        public async Task<ActionResult> Index(int page=1)
        {
            List<Question> questions = await _questionService.GetAll();
            page = CheckPageNum(page, questions.Count);

            ViewBag.Count = questions.Count;
            ViewBag.Current = page;

            return View();
        }


        public async Task<ActionResult> GetPage(int page = 1)
        {
            
            List<Question> questions = await _questionService.GetAll();
            page = CheckPageNum(page, questions.Count);

            Question question = questions.ElementAtOrDefault(page - 1);
            if (question == null)
                return HttpNotFound();

            return View(question);
        }

        private int CheckPageNum(int page, int maxPage)
        {
            if (page < 1)
                return 1;
            else if (page > maxPage)
                return maxPage;
            return page;
        }

    }
}