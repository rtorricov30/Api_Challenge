using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Permission:Entity
    {
      
        public string EmployeeName { get; set; } = string.Empty;
        public string EmployeeSurname { get; set; } = string.Empty;
        public int PermissionTypeId { get; set; }
        public DateTime PermissionDate { get; set; } = DateTime.Now;

        [ForeignKey("PermissionTypeId")]
        public virtual PermissionType? PermissionType { get; set; }
    }
}
