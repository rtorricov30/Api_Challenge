using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dto;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class ChallanceService(IRepositorieAbstraction<Permission> service,ILogger<ChallanceService> logger) : IChallanceService
    {
        private IRepositorieAbstraction<Permission> _service {  get; } = service;
        private ILogger<ChallanceService> _logger { get; } = logger ;
        
        public List<Permission> GetAll()
        {
            try
            {
                var result = _service.GetAll();
                return result.ToList();
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error al obtener permisos");
                throw;
            }
        }
        public async Task<Permission> Create(PermissionsDto input)
        {
            // Validate input
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            try
            {
                var result = await _service.CreateAsync(new Permission
                {
                    EmployeeName = input.EmployeeName,
                    EmployeeSurname = input.EmployeeSurname,
                    PermissionTypeId = input.PermissionTypeId,
                });

                if (result == null)
                    throw new InvalidOperationException("Failed to create permission ");

                return result;
            }
            catch (Exception ex)
            {
              
                _logger.LogError(ex, "Error al crear permisos");
                throw;  
            }
        }
        public async Task<Permission> Update(PermissionsDto input)
        {
            // Validate input
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            try
            {
                var result = await _service.UpdateAsync(input.Id,new Permission
                {
                    EmployeeName = input.EmployeeName,
                    EmployeeSurname = input.EmployeeSurname,
                    PermissionTypeId = input.PermissionTypeId,
                });

                if (result == null)
                    throw new InvalidOperationException("Failed to create permission ");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al modificar");

                throw;
            }
        }
    }
}
