using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private const string IN_REVIEW_L2 = "In Review L2";
        private const string L1 = " L1";
        private const string L2 = " L2";
        private const string NOT_APPLICABLE ="Not Applicable";
        private const string AWAITING_APPROVER_2 = "Awaiting Approver 2";
        private const string PENDING = "Pending";
        private const string APPROVED = "Approved";
        private const string REJECTED = "Rejected";
        private const string BY_BOARD_MEMBER = " by Board Member";
        private const string BY_CAM = " by CAM";
        private const string BOARD_MEMBER = "board member";
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
                    var user = await _invoiceUpdateEventRepository.GetUserDetails(personId);
                    var invoiceDetail = await _invoiceUpdateEventRepository.GetInvoiceDetail(invoice.Id);
                    var workFlowStatus = invoiceDetail.WorkFlowStatus?.ToList();
                    var newInvoiceTrackerList = new List<DM.InvoiceTracker>
                    {
                        new DM.InvoiceTracker()
                    };
                    var isL2ApprovalRequired = false;
                    if (workFlowStatus == null) continue;
                    for (var i = 0; i < workFlowStatus.Count; i++)
                    {
                        if (workFlowStatus[i].Status != PENDING ||
                            workFlowStatus[i].TemplateStep.RoleId != user.Role.RoleId) continue;
                        var currentApprover = i + 1;
                        workFlowStatus[i].Status = invoice.Status;
                        workFlowStatus[i].UpdatedBy = personId;
                        workFlowStatus[i].UpdatedDate = DateTime.UtcNow;
                        newInvoiceTrackerList[0].OutcomeStatus = invoice.Status + " By " + user.Role.RoleName;
                        newInvoiceTrackerList[0].Status = invoice.Status + " L" + currentApprover;

                        switch (invoice.Status)
                        {
                            case APPROVED:
                                isL2ApprovalRequired = i < workFlowStatus.Count - 1;
                                break;
                            case REJECTED when i < workFlowStatus.Count - 1:
                            {
                                for (var j = i + 1; j < workFlowStatus.Count; j++)
                                {
                                    workFlowStatus[j].Status = NOT_APPLICABLE;
                                    workFlowStatus[j].UpdatedBy = personId;
                                    workFlowStatus[j].UpdatedDate = DateTime.UtcNow;
                                }

                                await UpdateInvoiceStatus(invoiceDetail, invoice, personId);
                                break;
                            }
                        }

                        var isWorkFlowUpdated =
                            await _invoiceUpdateEventRepository.UpdateWorkFlowStatusList(workFlowStatus);
                        if (!isWorkFlowUpdated) continue;
                        await InsertUpdateInvoiceTracker(newInvoiceTrackerList, invoice,
                            user, currentApprover, isL2ApprovalRequired);
                        if (i == workFlowStatus.Count - 1)
                        {
                            await UpdateInvoiceStatus(invoiceDetail, invoice, personId);
                        }
                    }
                }

                return true;
            }
            
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

        public async Task<bool> InsertUpdateInvoiceTracker(List<DM.InvoiceTracker> newInvoiceTrackerList, VM.InvoiceStatusVM invoice, DM.Person user, int currentApprover, bool isL2ApprovalRequired = false)
        {
            var invoiceTrackerList = await _invoiceUpdateEventRepository.GetInvoiceTracker(invoice.Id);
            var invoiceTrackerIndex = 0;
            if (invoiceTrackerList.Any())
            {
                invoiceTrackerIndex = invoiceTrackerList.Count - 1;
                invoiceTrackerList[invoiceTrackerIndex].CurrentStatus = false;
                invoiceTrackerList[invoiceTrackerIndex].CompleteTime = invoiceTrackerList[invoiceTrackerIndex].StartTime > DateTime.UtcNow 
                    ? invoiceTrackerList[invoiceTrackerIndex].StartTime : DateTime.UtcNow;
                invoiceTrackerList[invoiceTrackerIndex].UpdatedBy = user.PersonId;
                invoiceTrackerList[invoiceTrackerIndex].UpdatedDate = DateTime.UtcNow;
            }

            newInvoiceTrackerList[0].InvoiceTrackerId = Guid.NewGuid();
            newInvoiceTrackerList[0].InvoiceId = invoice.Id;
            newInvoiceTrackerList[0].PersonId = user.PersonId;
            newInvoiceTrackerList[0].CurrentStatus = true;
            newInvoiceTrackerList[0].InvoiceFlowStatus = invoice.Status;
            newInvoiceTrackerList[0].StartTime = invoiceTrackerList[invoiceTrackerIndex]?.CompleteTime > DateTime.UtcNow 
                ? invoiceTrackerList[invoiceTrackerIndex].CompleteTime : DateTime.UtcNow;
            newInvoiceTrackerList[0].CompleteTime = newInvoiceTrackerList[0].StartTime?.AddMinutes(1);
            newInvoiceTrackerList[0].CreatedDate = DateTime.UtcNow;
            newInvoiceTrackerList[0].CreatedBy = user.PersonId;
            newInvoiceTrackerList[0].UpdatedDate = DateTime.UtcNow;
            newInvoiceTrackerList[0].UpdatedBy = user.PersonId;
            newInvoiceTrackerList[0].Sequence = invoiceTrackerList.Any() ? invoiceTrackerList.Count + 1 : 1;

            if (isL2ApprovalRequired)
            {
                currentApprover += 1;
                newInvoiceTrackerList[0].InvoiceFlowStatus = "Awaiting Approver " + currentApprover;
                newInvoiceTrackerList[0].CurrentStatus = false;
                newInvoiceTrackerList.Add(new DM.InvoiceTracker()
                {
                    InvoiceTrackerId = Guid.NewGuid(),
                    InvoiceId = invoice.Id,
                    PersonId = user.PersonId,
                    CurrentStatus = true,
                    Status = "In Review L" + currentApprover,
                    InvoiceFlowStatus = "Awaiting Approver " + currentApprover,
                    OutcomeStatus = invoice.Status + " By " + user.Role.RoleName,
                    StartTime = newInvoiceTrackerList[0].CompleteTime,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = user.PersonId,
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedBy = user.PersonId,
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
