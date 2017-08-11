using OneInc.Models;
using OneInc.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace OneInc.Controllers
{
    public class ResultController : Controller
    {
        private readonly IResultService<Result> _resultService;
        private readonly IQuestionService<Question> _questionService;
        private readonly IOptionService<Option> _optionService;
        private readonly IRecognizeService _recognizeService;

        public ResultController(IResultService<Result> resultService, IQuestionService<Question> questionService, IRecognizeService recognizeService, IOptionService<Option> optionService)
        {
            _resultService = resultService;
            _questionService = questionService;
            _recognizeService = recognizeService;
            _optionService = optionService;
        }

        public async Task<ActionResult> Index()
        {
            List<Question> questions = await _questionService.GetAll();

            return View(questions);
        }


        [HttpPost]
        public async Task<JsonResult> Upload(int questionId, HttpPostedFileBase blob)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //  Запускаем разпозавание в новом потоке
                    Task<int> task1 = new Task<int>(() => _recognizeService.Recognize(blob.InputStream));
                    task1.Start();
                    int selectedOption = await task1;

                    //  если текст распознался, то сохраняем ответ
                    if (selectedOption > 0)
                    {
                        //  находим вопрос
                        Question question = await _questionService.Get(questionId);
                        if (question != null)
                        {
                            //  выбираем опцию по порядковому номеру
                            Option op = question.Options.ElementAtOrDefault(selectedOption - 1);
                            if (op != null)
                            {
                                //  переводим каретку на начало потока и приводим поток в массив байтов
                                blob.InputStream.Seek(0, SeekOrigin.Begin);
                                MemoryStream ms = new MemoryStream();
                                blob.InputStream.CopyTo(ms);

                                Result result = new Result()
                                {
                                    Voice = ms.ToArray(),
                                    OptionId = op.Id

                                };
                                await _resultService.Create(result);
                                return Json(new { success = true, selectedOption = selectedOption, isLastQuestion = await isLastQuestion(questionId) });
                            }
                        }

                    }
                }
                catch(Exception e) {
                    return Json(new { success = false, message=e.Message+" "+e.InnerException?.Message });
                }
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public async Task<ActionResult> AddResult(Result result)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _resultService.Create(result);
                    return Json(new { success = true, isLastQuestion = await isLastQuestionByOptionId(result.OptionId) });
                }
            }
            catch
            {
            }
            return Json(new { success = false });
        }

        private async Task<bool> isLastQuestion(int id)
        {
            List<Question> questions = await _questionService.GetAll();
            return questions.LastOrDefault()?.Id == id;
        }
        private async Task<bool> isLastQuestionByOptionId(int id)
        {
            List<Question> questions = await _questionService.GetAll();
            Option option = await _optionService.Get(id);
            return questions.LastOrDefault()?.Id == option.QuestionId;
        }

    }
}