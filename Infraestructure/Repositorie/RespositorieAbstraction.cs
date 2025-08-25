using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain;
using Infraestructure.DataBaseContext;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Repositorie
{
    public class RespositorieAbstraction<TEntity> (AppContextSql context): IRepositorieAbstraction<TEntity> where TEntity :  Entity
    {
        private AppContextSql _context { get; } = context;
        public IQueryable<TEntity> GetAll()
        {
            try
            {
                var foreignProperties = typeof(TEntity).GetProperties()
                    .Where(x => !x.GetAccessors()[0].IsFinal && x.GetAccessors()[0].IsVirtual).Where(x =>
                        x.GetCustomAttributes(true).Any(o => o is ForeignKeyAttribute));
                var propertyInfos = foreignProperties.Concat(typeof(TEntity).GetProperties()
                    .Where(x => !x.GetAccessors()[0].IsFinal && x.GetAccessors()[0].IsVirtual)
                    .Where(x => x.PropertyType.GetInterfaces().Contains(typeof(IEnumerable))));
                IQueryable<TEntity> query = _context.Set<TEntity>() ;
                query = propertyInfos.Aggregate(query, (o, propertyInfo) => o.Include(propertyInfo.Name));
                return query;
            }
            catch (Exception sql)
            {
                throw;
            }
        }
        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            try
            {
                await _context.Set<TEntity>().AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                // Considera registrar la excepción o manejarla de alguna manera
                throw;
            }
        }
        public async Task<TEntity> UpdateAsync(int entityId, TEntity entity)
        {
            try
            {
                var toUpdate = await _context.Set<TEntity>().SingleOrDefaultAsync(o => o.Id == entityId);
                if (toUpdate == null)
                {
                    throw new KeyNotFoundException($"Entity with ID {entityId} not found.");
                }

                // Mapear manualmente las propiedades, excluyendo Id
                var properties = typeof(TEntity).GetProperties()
                    .Where(p => p.Name != "Id" && p.CanWrite && p.GetSetMethod() != null);

                foreach (var prop in properties)
                {
                    var newValue = prop.GetValue(entity);
                    prop.SetValue(toUpdate, newValue);
                }

                await _context.SaveChangesAsync();
                return toUpdate;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while updating the entity.", ex);
            }
        }
    }
}
