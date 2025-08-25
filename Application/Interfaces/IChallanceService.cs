using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dto;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IChallanceService
    {
        List<Permission> GetAll();
        Task<Permission> Create(PermissionsDto input);
        Task<Permission> Update(PermissionsDto input);
    }
}
