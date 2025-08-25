using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public class PermissionsDto
    {
        public int Id { get; set; }
        public string EmployeeName {get;set;}
        public string EmployeeSurname {get;set;}
        public int PermissionTypeId { get; set; }
    }
}
