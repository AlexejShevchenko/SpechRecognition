using Ninject;
using OneInc.Models;
using OneInc.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneInc.Utils
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;
        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }
        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
        private void AddBindings()
        {
            kernel.Bind<IQuestionService<Question>>().To<QuestionService>();
            kernel.Bind<IOptionService<Option>>().To<OptionService>();
            kernel.Bind<IResultService<Result>>().To<ResultService>();
            kernel.Bind<IRecognizeService>().To<RecognizeService>();
        }
    }
}