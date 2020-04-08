using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace WebAPIDemo.Models
{
    public class WorkOrder
    {
        [Key]
        public long WorkOrderId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [ForeignKey("User")]
        public long? AssignedUserId { get; set; }
        public int? FacilityId { get; set; }
        public int? UnitId { get; set; }
        public User User { get; set; }
    }

    public class AddWorkOrder
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public long? AssignedUserId { get; set; }
        public int? FacilityId { get; set; }
        public int? UnitId { get; set; }
    }

    public class UpdateWorkOrder
    {
        [Required]
        public long? WorkOrderId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public long? AssignedUserId { get; set; }
        public int? FacilityId { get; set; }
        public int? UnitId { get; set; }
    }

    public class GetWorkOrder
    {
        public long? WorkOrderId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public long? AssignedUserId { get; set; }
        public int? FacilityId { get; set; }
        public int? UnitId { get; set; }
    }
}
