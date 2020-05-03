using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Associa.Service.BAL.Interfaces;
using Associa.Service.DAL.Interfaces;
using DM = Associa.Service.DAL.Models;
using VM = Associa.Service.BAL.Models;

namespace Associa.Service.BAL.BusinessLogic
{
   public class InvoiceUpdateEventLogic : IInvoiceUpdateEventLogic
    {

        private IInvoiceUpdateEventRepository _invoiceUpdateEventRepository;
        private IMapper _mapper;

        public InvoiceUpdateEventLogic(IInvoiceUpdateEventRepository invoiceRepository, IMapper mapper)
        {
            _invoiceUpdateEventRepository = invoiceRepository;
            _mapper = mapper;

        }

        public async Task<bool> UpdateInvoiceStatus(List<VM.InvoiceStatusVM> invoiceStatusList)
        {
            var personId = new Guid();
            if (invoiceStatusList.Any())
            {
                foreach (var invoice in invoiceStatusList)
                {
                    var workFlowStatus = await _invoiceUpdateEventRepository.GetWorkFlowStatus(invoice.Id, personId);
                    var invoiceDetail = await _invoiceUpdateEventRepository.GetInvoiceDetail(invoice.Id);
                    var invoiceTrackerList = await _invoiceUpdateEventRepository.GetInvoiceTracker(invoice.Id, personId);
                    var invoiceTrackerIndex = 0;
                    if (invoiceTrackerList.Any())
                    {
                        invoiceTrackerIndex = invoiceTrackerList.Count - 1;
                        invoiceTrackerList[invoiceTrackerIndex].CurrentStatus = false;
                        invoiceTrackerList[invoiceTrackerIndex].CompleteTime = DateTime.UtcNow;
                    }
                    var newInvoiceTrackerList = new List<DM.InvoiceTracker>();
                    newInvoiceTrackerList.Add(new DM.InvoiceTracker()
                    {
                        InvoiceTrackerId = Guid.NewGuid(),
                        InvoiceId = invoice.Id,
                        PersonId = personId,
                        CurrentStatus = true,
                        InvoiceFlowStatus = invoice.Status,
                        StartTime = DateTime.UtcNow,
                        CompleteTime = DateTime.UtcNow.AddMinutes(1),
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = personId,
                        UpdatedDate = DateTime.UtcNow,
                        UpdatedBy = personId,
                        Sequence = invoiceTrackerList.Any() ? invoiceTrackerList.Count + 1 : 1
                    });
                    if (workFlowStatus.Any() && invoiceDetail != null)
                    {
                        if (workFlowStatus.Count == 1)
                        {
                            workFlowStatus[0].Status = invoice.Status;
                            invoiceDetail.Status = invoice.Status;
                            newInvoiceTrackerList[0].OutcomeStatus = invoice.Status;

                            await _invoiceUpdateEventRepository.UpdateWorkFlowStatus(workFlowStatus[0]);
                            await _invoiceUpdateEventRepository.UpdateInvoiceStatus(invoiceDetail);
                        }
                        else if (workFlowStatus.Count > 1 && workFlowStatus[0].Status == "Pending")
                        {
                            workFlowStatus[0].Status = invoice.Status;
                            newInvoiceTrackerList[0].OutcomeStatus = invoice.Status + " by CAM";

                            if (invoice.Status == "Approved")
                            {
                                newInvoiceTrackerList[0].InvoiceFlowStatus = "Awaiting Approver 2";
                                newInvoiceTrackerList[0].CurrentStatus = false;
                                newInvoiceTrackerList.Add(new DM.InvoiceTracker()
                                {
                                    InvoiceTrackerId = Guid.NewGuid(),
                                    InvoiceId = invoice.Id,
                                    PersonId = personId,
                                    CurrentStatus = true,
                                    InvoiceFlowStatus = "Awaiting Approver 2",
                                    OutcomeStatus = invoice.Status + " by CAM",
                                    StartTime = newInvoiceTrackerList[0].CompleteTime,
                                    CreatedDate = DateTime.UtcNow,
                                    CreatedBy = personId,
                                    UpdatedDate = DateTime.UtcNow,
                                    UpdatedBy = personId,
                                    Sequence = newInvoiceTrackerList[0].Sequence + 1
                                });
                                await _invoiceUpdateEventRepository.UpdateWorkFlowStatus(workFlowStatus[0]);
                            }
                            else if (invoice.Status == "Rejected")
                            {
                                workFlowStatus[1].Status = "Not Applicable";
                                await _invoiceUpdateEventRepository.UpdateWorkFlowStatusList(workFlowStatus);
                                await _invoiceUpdateEventRepository.UpdateInvoiceStatus(invoiceDetail);
                            }
                        }
                        else if (workFlowStatus.Count > 1 && workFlowStatus[0].Status == "Approved")
                        {
                            workFlowStatus[1].Status = invoice.Status;
                            invoiceDetail.Status = invoice.Status;
                            await _invoiceUpdateEventRepository.UpdateWorkFlowStatus(workFlowStatus[1]);
                            await _invoiceUpdateEventRepository.UpdateInvoiceStatus(invoiceDetail);

                            newInvoiceTrackerList[0].OutcomeStatus = invoice.Status + " by Board Member";
                        }

                        if (invoiceTrackerList.Any())
                        {
                            await _invoiceUpdateEventRepository.UpdateInvoiceTracker(invoiceTrackerList[invoiceTrackerIndex]);
                        }

                        await _invoiceUpdateEventRepository.AddInvoiceTracker(newInvoiceTrackerList);
                    }
                }

                return true;
            }
            else
                throw new Exception("User doesn't exists");
        }
    }
}
