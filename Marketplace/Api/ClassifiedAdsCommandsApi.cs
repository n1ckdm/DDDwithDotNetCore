using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Api
{
    [Route("/ad")]
    public class ClassifiedAdsCommandsApi : Controller
    {
        private readonly ClassifiedAdsApplicationService _appService;

        public ClassifiedAdsCommandsApi(
            ClassifiedAdsApplicationService applicationService
        ) => _appService = applicationService;

        [HttpPost]
        public async Task<IActionResult> Post(
            Contracts.ClassifiedAds.V1.Create request
        )
        {
            _appService.Handle(request);
            return Ok();
        }
    }
}