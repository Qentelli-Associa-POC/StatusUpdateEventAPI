using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Associa.Service.BAL.Interfaces;
using Associa.Service.BAL.Models;

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
        
        [HttpPost("updateInvoiceStatus")]
        [ActionName("updateInvoiceStatus")]
        public async Task<IActionResult> UpdateInvoiceStatus(List<InvoiceStatusVM> invoiceList)
        {
            try
            {
                return Ok(await _invoiceUpdateEventLogic.UpdateInvoiceStatus(invoiceList));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}