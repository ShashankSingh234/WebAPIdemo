using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebAPIDemo.Context;
using WebAPIDemo.Models;

namespace WebAPIDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WorkOrderController : ControllerBase
    {
        private readonly DatabaseContext _context;
        public WorkOrderController(DatabaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get detail of specific Work Order.
        /// </summary>
        /// <param name="workOrderId">Id of work order.</param>
        /// <returns></returns>
        /// <response code="200">Returns the work order detail.</response>
        /// <response code="404">Work order not found.</response>  
        [HttpGet("GetWorkOrder")]
        public IActionResult GetWorkOrderDetail(int workOrderId)
        {
            WorkOrder wo = _context.WorkOrder.SingleOrDefault(wo => wo.WorkOrderId.Equals(workOrderId));

            if (wo == null)
            {
                return NotFound($"WorkOrder {workOrderId} not found.");
            }

            GetWorkOrder getWorkOrder = new GetWorkOrder()
            {
                WorkOrderId = wo.WorkOrderId,
                Title = wo.Title,
                Description = wo.Description,
                AssignedUserId = wo.AssignedUserId,
                FacilityId = wo.FacilityId,
                UnitId = wo.UnitId
            };

            return Ok(getWorkOrder);
        }

        /// <summary>
        /// List of work order added.
        /// </summary>
        /// <param name="pageNumber">Page number.</param>
        /// <param name="pageSize">Page size.</param>
        /// <returns></returns>
        [HttpGet("GetWorkOrderList")]
        public IActionResult GetWorkOrderList(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0)
            {
                return BadRequest("Page number should be greater than 0.");
            }

            List<WorkOrder> woList = _context.WorkOrder.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return Ok(woList.Select(x => new GetWorkOrder()
            {
                WorkOrderId = x.WorkOrderId,
                Title = x.Title,
                Description = x.Description,
                AssignedUserId = x.AssignedUserId,
                FacilityId = x.FacilityId,
                UnitId = x.UnitId
            }).ToList());
        }

        /// <summary>
        /// Add new work order.
        /// </summary>
        /// <param name="addWorkOrder"></param>
        /// <returns></returns>
        [HttpPost("Add")]
        public async Task<IActionResult> AddWorkOrderAsync([FromQuery]AddWorkOrder addWorkOrder)
        {
            try
            {
                WorkOrder workOrder = new WorkOrder()
                {
                    Title = addWorkOrder.Title,
                    Description = addWorkOrder.Description,
                    AssignedUserId = addWorkOrder.AssignedUserId,
                    FacilityId = addWorkOrder.FacilityId,
                    UnitId = addWorkOrder.UnitId,
                };

                _context.WorkOrder.Add(workOrder);
                await _context.SaveChangesAsync();
                return Ok($"WorkOrder {workOrder.WorkOrderId} added successfully.");
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.InnerException.Message);
            }
        }

        /// <summary>
        /// Update already added work order details.
        /// </summary>
        /// <param name="updateWorkOrder"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        public async Task<IActionResult> EditWorkOrderAsync([FromQuery]UpdateWorkOrder updateWorkOrder)
        {
            WorkOrder wo = _context.WorkOrder.SingleOrDefault(wo => wo.WorkOrderId.Equals(updateWorkOrder.WorkOrderId));

            if (wo == null)
            {
                return NotFound($"WorkOrder {updateWorkOrder.WorkOrderId} not found.");
            }

            wo.Title = updateWorkOrder.Title;
            wo.Description = updateWorkOrder.Description;
            wo.AssignedUserId = updateWorkOrder.AssignedUserId;
            wo.FacilityId = updateWorkOrder.FacilityId;
            wo.UnitId = updateWorkOrder.UnitId;
            try
            {
                await _context.SaveChangesAsync();

                return Ok($"WorkOrder {updateWorkOrder.WorkOrderId} updated successfully.");
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.InnerException.Message);
            }
        }

        /// <summary>
        /// Deletes the specified work order.
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteWorkOrder(int workOrderId)
        {
            WorkOrder wo = _context.WorkOrder.SingleOrDefault(wo => wo.WorkOrderId.Equals(workOrderId));

            if (wo == null)
            {
                return NotFound($"WorkOrder {workOrderId} not found.");
            }

            _context.WorkOrder.Remove(wo);
            await _context.SaveChangesAsync();

            return Ok($"WorkOrder {workOrderId} deleted successfully.");
        }

    }
}