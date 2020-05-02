using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Associa.Service.BAL.Interfaces;
using Associa.Service.BAL.Models;
using Associa.Service.Core.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace StatusUpdateEventAPI.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1")]
    public class InvoiceUpdateEventController : ControllerBase
    {
        private IInvoiceUpdateEventLogic _invoiceUpdateEventLogic;

        public InvoiceUpdateEventController(IInvoiceUpdateEventLogic invoiceUpdateEventLogic)
        {
            _invoiceUpdateEventLogic = invoiceUpdateEventLogic;
        }

        [NonAction]
        private IdentityToken InitializeToken()
        {
            var authorizationHeader = HttpContext.Request.Headers["Authorization"];
            return new IdentityToken(AuthenticationHeaderValue.Parse(authorizationHeader));
        }

        [HttpPost("updateInvoiceStatus")]
        [ActionName("updateInvoiceStatus")]
        public async Task<IActionResult> UpdateInvoiceStatus(List<InvoiceStatusVM> invoiceList)
        {
            var _idToken = InitializeToken();
            try
            {
                return Ok(await _invoiceUpdateEventLogic.UpdateInvoiceStatus(invoiceList, _idToken.UserId));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}