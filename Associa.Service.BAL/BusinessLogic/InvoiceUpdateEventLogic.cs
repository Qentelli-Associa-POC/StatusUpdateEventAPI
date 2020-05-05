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

        #region Constants
        private readonly string IN_REVIEW_L2 = "In Review L2";
        private readonly string L1 = " L1";
        private readonly string L2 = " L2";
        private readonly string NOT_APPLICABLE ="Not Applicable";
        private readonly string AWAITING_APPROVER_2 = "Awaiting Approver 2";
        private readonly string PENDING = "Pending";
        private readonly string APPROVED = "Approved";
        private readonly string REJECTED = "Rejected";
        private readonly string BY_BOARD_MEMBER = " by Board Member";
        private readonly string BY_CAM = " by CAM";

        #endregion

        public InvoiceUpdateEventLogic(IInvoiceUpdateEventRepository invoiceRepository, IMapper mapper)
        {
            _invoiceUpdateEventRepository = invoiceRepository;
            _mapper = mapper;

        }

        public async Task<bool> UpdateInvoiceStatus(Guid personId, List<VM.InvoiceStatusVM> invoiceStatusList)
        {
            if (invoiceStatusList.Any())
            {
                foreach (var invoice in invoiceStatusList)
                {
                    var workFlowStatus = await _invoiceUpdateEventRepository.GetWorkFlowStatus(invoice.Id);
                    var invoiceDetail = await _invoiceUpdateEventRepository.GetInvoiceDetail(invoice.Id);

                    var newInvoiceTrackerList = new List<DM.InvoiceTracker>();
                    newInvoiceTrackerList.Add(new DM.InvoiceTracker());
                    var isL2ApprovalRequired = false;
                    var isWorkFlowUpdated = false;
                    if (workFlowStatus.Any() && invoiceDetail != null)
                    {
                        if (workFlowStatus.Count == 1)
                        {
                            workFlowStatus[0].Status = invoice.Status;
                            newInvoiceTrackerList[0].OutcomeStatus = invoice.Status;
                            newInvoiceTrackerList[0].Status = invoice.Status;

                            isWorkFlowUpdated = await UpdateWorkFlowStatus(workFlowStatus[0], personId);
                            if (isWorkFlowUpdated)
                            {
                                await UpdateInvoiceStatus(invoiceDetail, invoice, personId);
                            }
                        }
                        else if (workFlowStatus.Count > 1 && workFlowStatus[0].Status == PENDING)
                        {
                            workFlowStatus[0].Status = invoice.Status;
                            newInvoiceTrackerList[0].OutcomeStatus = string.Concat(invoice.Status, BY_CAM);
                            newInvoiceTrackerList[0].Status = string.Concat(invoice.Status, L1);

                            if (invoice.Status == APPROVED)
                            {
                                isL2ApprovalRequired = true;
                                isWorkFlowUpdated = await UpdateWorkFlowStatus(workFlowStatus[0], personId);
                            }
                            else if (invoice.Status == REJECTED)
                            {
                                workFlowStatus[1].Status = NOT_APPLICABLE;
                                workFlowStatus[1].UpdatedBy = personId;
                                workFlowStatus[1].UpdatedDate = DateTime.UtcNow;
                                isWorkFlowUpdated = await _invoiceUpdateEventRepository.UpdateWorkFlowStatusList(workFlowStatus);
                                if (isWorkFlowUpdated)
                                {
                                    await UpdateInvoiceStatus(invoiceDetail, invoice, personId);
                                }
                            }
                        }
                        else if (workFlowStatus.Count > 1 && workFlowStatus[0].Status == APPROVED)
                        {
                            workFlowStatus[1].Status = invoice.Status;
                            newInvoiceTrackerList[0].Status = string.Concat(invoice.Status, L2);
                            isWorkFlowUpdated = await UpdateWorkFlowStatus(workFlowStatus[1], personId);
                            if (isWorkFlowUpdated)
                            {
                                await UpdateInvoiceStatus(invoiceDetail, invoice, personId);
                            }

                            newInvoiceTrackerList[0].OutcomeStatus = string.Concat(invoice.Status, BY_BOARD_MEMBER);
                        }

                        if (isWorkFlowUpdated)
                        {
                            await InsertUpdateInvoiceTracker(newInvoiceTrackerList, invoice, personId, isL2ApprovalRequired);
                        }
                    }
                }

                return true;
            }
            else
                throw new Exception("User doesn't exists");
        }

        public async Task<bool> UpdateWorkFlowStatus(DM.WorkFlowStatus workFlowStatus, Guid personId)
        {
            workFlowStatus.UpdatedBy = personId;
            workFlowStatus.UpdatedDate = DateTime.UtcNow;
            return await _invoiceUpdateEventRepository.UpdateWorkFlowStatus(workFlowStatus);
        }

        public async Task<bool> UpdateInvoiceStatus(DM.Invoice oldInvoice, VM.InvoiceStatusVM invoiceStatus, Guid personId)
        {
            oldInvoice.UpdatedBy = personId;
            oldInvoice.UpdatedDate = DateTime.UtcNow;
            oldInvoice.Status = invoiceStatus.Status;
            return await _invoiceUpdateEventRepository.UpdateInvoiceStatus(oldInvoice);
        }

        public async Task<bool> InsertUpdateInvoiceTracker(List<DM.InvoiceTracker> newInvoiceTrackerList, VM.InvoiceStatusVM invoice, Guid personId, bool isL2ApprovalRequired = false)
        {
            var invoiceTrackerList = await _invoiceUpdateEventRepository.GetInvoiceTracker(invoice.Id);
            var invoiceTrackerIndex = 0;
            if (invoiceTrackerList.Any())
            {
                invoiceTrackerIndex = invoiceTrackerList.Count - 1;
                invoiceTrackerList[invoiceTrackerIndex].CurrentStatus = false;
                invoiceTrackerList[invoiceTrackerIndex].CompleteTime = DateTime.UtcNow;
                invoiceTrackerList[invoiceTrackerIndex].UpdatedBy = personId;
                invoiceTrackerList[invoiceTrackerIndex].UpdatedDate = DateTime.UtcNow;
            }

            newInvoiceTrackerList[0].InvoiceTrackerId = Guid.NewGuid();
            newInvoiceTrackerList[0].InvoiceId = invoice.Id;
            newInvoiceTrackerList[0].PersonId = personId;
            newInvoiceTrackerList[0].CurrentStatus = true;
            newInvoiceTrackerList[0].InvoiceFlowStatus = invoice.Status;
            newInvoiceTrackerList[0].StartTime = DateTime.UtcNow;
            newInvoiceTrackerList[0].CompleteTime = DateTime.UtcNow.AddMinutes(1);
            newInvoiceTrackerList[0].CreatedDate = DateTime.UtcNow;
            newInvoiceTrackerList[0].CreatedBy = personId;
            newInvoiceTrackerList[0].UpdatedDate = DateTime.UtcNow;
            newInvoiceTrackerList[0].UpdatedBy = personId;
            newInvoiceTrackerList[0].Sequence = invoiceTrackerList.Any() ? invoiceTrackerList.Count + 1 : 1;

            if (isL2ApprovalRequired)
            {
                newInvoiceTrackerList[0].InvoiceFlowStatus = AWAITING_APPROVER_2;
                newInvoiceTrackerList[0].CurrentStatus = false;
                newInvoiceTrackerList.Add(new DM.InvoiceTracker()
                {
                    InvoiceTrackerId = Guid.NewGuid(),
                    InvoiceId = invoice.Id,
                    PersonId = personId,
                    CurrentStatus = true,
                    Status = IN_REVIEW_L2,
                    InvoiceFlowStatus = AWAITING_APPROVER_2,
                    OutcomeStatus = string.Concat(invoice.Status, BY_CAM),
                    StartTime = newInvoiceTrackerList[0].CompleteTime,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = personId,
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedBy = personId,
                    Sequence = newInvoiceTrackerList[0].Sequence + 1
                });
            }

            if (invoiceTrackerList.Any())
            {
                await _invoiceUpdateEventRepository.UpdateInvoiceTracker(invoiceTrackerList[invoiceTrackerIndex]);
            }
            return await _invoiceUpdateEventRepository.AddInvoiceTracker(newInvoiceTrackerList);
        }
    }
}
