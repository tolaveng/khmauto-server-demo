﻿using Data.Api.Common;
using Data.Domain.Models;
using Data.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Domain.Repositories
{
    public class ServiceIndexRepository : IServiceIndexRepository
    {
        private readonly AppDataContext context;
        public ServiceIndexRepository(AppDataContext context)
        {
            this.context = context;
        }

        public async Task AddOrUpdateService(string serviceName, decimal price)
        {
            try
            {
                var serviceHash = serviceName.Trim().CleanWhiteSpace().GetShaHash();

                var service = await context.ServiceIndexs.SingleOrDefaultAsync(x => x.ServiceIndexHash == serviceHash);
                if (service != null)
                {
                    service.ServicePrice = price;
                    context.ServiceIndexs.Update(service);
                }
                else
                {
                    service = new ServiceIndex()
                    {
                        ServiceIndexHash = serviceHash,
                        ServiceName = serviceName.Trim(),
                        ServicePrice = price
                    };
                    await context.ServiceIndexs.AddAsync(service);
                }
                await context.SaveChangesAsync();

            } catch(Exception)
            {
                //ignored
            }
        }

        public async Task<IEnumerable<ServiceIndex>> FindByServiceName(string serviceName, int limit)
        {
            return await context.ServiceIndexs
                    .Where(z => z.ServiceName.Contains(serviceName.Trim(), StringComparison.OrdinalIgnoreCase))
                    .OrderBy(z => z.ServiceName)
                    .GroupBy(x => new { x.ServiceName, x.ServicePrice })
                    .Select(g => new ServiceIndex() { ServiceName = g.Key.ServiceName, ServicePrice = g.Key.ServicePrice })
                    .Take(limit)
                    .ToListAsync();

            //var serviceNames = serviceName.Split("\n");
            //if (serviceNames.Length == 1)
            //{
            //return await context.ServiceIndexs
            //    .Where(z => z.ServiceName.Contains(serviceNames[0].Trim(), StringComparison.OrdinalIgnoreCase))
            //    .GroupBy(x => new { x.ServiceName, x.ServicePrice })
            //    .Select(g => new ServiceIndex() { ServiceName = g.Key.ServiceName, ServicePrice = g.Key.ServicePrice })
            //    .Take(limit)
            //    .ToListAsync();
            //}

            //Expression<Func<ServiceIndex, bool>> predicate = ser =>
            //    ser.ServiceName.Contains(serviceNames[0].Trim(), StringComparison.OrdinalIgnoreCase);

            //for(var i = 1; i < serviceNames.Length; i++)
            //{
            //    if (string.IsNullOrWhiteSpace(serviceNames[i])) continue;
            //    var index = i;
            //    Expression<Func<ServiceIndex, bool>> exp = ser =>
            //      (ser.ServiceName.Contains(serviceNames[index].Trim(), StringComparison.OrdinalIgnoreCase));

            //    var invokedExpr = Expression.Invoke(exp, predicate.Parameters.Cast<Expression>());
            //    predicate = Expression.Lambda<Func<ServiceIndex, bool>>(
            //        Expression.OrElse(predicate.Body, invokedExpr), predicate.Parameters);
            //}

            //var query = context.ServiceIndexs.Where(predicate);
            //var result = await query.GroupBy(x => new { x.ServiceName, x.ServicePrice })
            //        .Select(g => new ServiceIndex() { ServiceName = g.Key.ServiceName, ServicePrice = g.Key.ServicePrice })
            //        .Take(limit)
            //        .ToListAsync();

            //return result;
        }

        public async Task<ServiceIndex> GetService(string serviceName)
        {
            return await context.ServiceIndexs.SingleOrDefaultAsync(z => z.ServiceName.Equals(serviceName.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        public async Task<IEnumerable<ServiceIndex>> GetAll(int limit)
        {
            try
            {
                // Distinct by using group by
                return await context.ServiceIndexs.Where(x => !string.IsNullOrWhiteSpace(x.ServiceName))
                    .OrderByDescending(z => z.ServiceIndexId).ThenBy(z => z.ServiceName)
                    .GroupBy(x => new { x.ServiceName, x.ServicePrice })
                    .Select(g => new ServiceIndex() { ServiceName = g.Key.ServiceName, ServicePrice = g.Key.ServicePrice })
                    .Take(limit)
                    .ToListAsync();
            }
            catch(Exception e)
            {
                return new List<ServiceIndex>();
            }
        }

        public async Task UpdateService(int id, string serviceName)
        {
            var service = await context.ServiceIndexs.SingleOrDefaultAsync(z => z.ServiceIndexId == id);
            if (service == null) return;
            var serviceHash = serviceName.Trim().CleanWhiteSpace().GetShaHash();
            service.ServiceName = serviceName;
            service.ServiceIndexHash = serviceHash;
            await context.SaveChangesAsync();
        }

        public async Task DeleteService(int id)
        {
            var service = await context.ServiceIndexs.SingleOrDefaultAsync(z => z.ServiceIndexId == id);
            if (service == null) return;
            context.ServiceIndexs.Remove(service);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ServiceIndex>> FindByServiceNamePaged(string serviceName, PaginationQuery pagination)
        {
            var skip = (pagination.PageNumber - 1) * pagination.PageSize;
            return await context.ServiceIndexs
                    .Where(z => z.ServiceName.Contains(serviceName.Trim(), StringComparison.OrdinalIgnoreCase))
                    .OrderBy(z => z.ServiceName)
                    .Skip(skip).Take(pagination.PageSize)
                    .ToListAsync();
        }

        public async Task<int> CountByServiceName(string serviceName)
        {
            return await context.ServiceIndexs
                    .Where(z => z.ServiceName.Contains(serviceName.Trim(), StringComparison.OrdinalIgnoreCase))
                    .CountAsync();
        }
    }
}
