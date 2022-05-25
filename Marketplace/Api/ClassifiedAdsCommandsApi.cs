using Serilog;
using Microsoft.AspNetCore.Mvc;
using static Marketplace.Contracts.ClassifiedAds;

namespace Marketplace.Api
{
    [Route("/ad")]
    public class ClassifiedAdsCommandsApi : Controller
    {
        private readonly ClassifiedAdsApplicationService _appService;

        public ClassifiedAdsCommandsApi(
            ClassifiedAdsApplicationService applicationService
        ) => _appService = applicationService;

        private async Task<IActionResult> HandleRequest<T>(T request, Func<T,Task> handler)
        {
            try
            {
                Log.Debug("Handling HTTP request of type {type}", typeof(T).Name);
                await handler(request);
                return Ok();
            }
            catch (Exception e)
            {
                Log.Error("Error handling the request", e);
                return new BadRequestObjectResult(new {
                    error = e.Message,
                    stackTrace = e.StackTrace
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(V1.Create request)
        {
            return await HandleRequest<V1.Create>(request, _appService.Handle);
        }

        [Route("name")]
        [HttpPut]
        public async Task<IActionResult> Put(V1.SetTitle request)
        {
            return await HandleRequest<V1.SetTitle>(request, _appService.Handle);
        }

        [Route("text")]
        [HttpPut]
        public async Task<IActionResult> Put(V1.UpdateText request)
        {
            return await HandleRequest<V1.UpdateText>(request, _appService.Handle);
        }

        [Route("price")]
        [HttpPut]
        public async Task<IActionResult> Put(V1.UpdatePrice request)
        {
            return await HandleRequest<V1.UpdatePrice>(request, _appService.Handle);
        }

        [Route("publish")]
        [HttpPut]
        public async Task<IActionResult> Put(V1.RequestToPublish request)
        {
            return await HandleRequest<V1.RequestToPublish>(request, _appService.Handle);
        }
    }
}