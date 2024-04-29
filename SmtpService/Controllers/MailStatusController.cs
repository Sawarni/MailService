using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmtpService.DataAccess.Repository;

namespace SmtpService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailStatusController : ControllerBase
    {
        private readonly ILogger<MailStatusController> logger;
        private readonly IMailRepository mailRepository;

        public MailStatusController(ILogger<MailStatusController> logger, IMailRepository mailRepository)
        {
            this.logger = logger;
            this.mailRepository = mailRepository;
        }

        [HttpGet]
        [Route("mails")]
        public async Task<IActionResult> GetAll() { 
        
            var mails = await mailRepository.GetAll();
            return Ok(mails);
        }

        [HttpGet]
        [Route("mails/failed")]
        public async Task<IActionResult> GetFailedMails()
        {

            var mails = await mailRepository.GetByCondition(x=> !x.SendStatus);
            return Ok(mails);
        }


        [HttpGet]
        [Route("mails/success")]
        public async Task<IActionResult> GetSentMails()
        {

            var mails = await mailRepository.GetByCondition(x => x.SendStatus);
            return Ok(mails);
        }


    }
}
