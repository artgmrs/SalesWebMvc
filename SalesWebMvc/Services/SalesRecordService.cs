using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc.Services
{
    public class SalesRecordService
    {
        private readonly SalesWebMvcContext _context;

        public SalesRecordService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public IList<SalesRecord> FindLast()
        {
            //var result = from obj in _context.SalesRecord  orderby obj.Date ascending select obj ;
            //var listCount = _context.SalesRecord.Count();

            //result
            //    .AsNoTracking()
            //    .Include(x => x.Seller)
            //    .Skip(listCount - 5)
            //    .Take(5)
            //    .AsEnumerable()
            //    .ToList();

            return _context.SalesRecord
                .AsNoTracking()
                .Include(sales => sales.Seller)
                .OrderByDescending(o => o.Date).Take(5).ToList();
        }

        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            var result = from obj in _context.SalesRecord select obj;
            if (minDate.HasValue)
            {
                result = result.Where(x => x.Date >= minDate.Value);
            }
            if (maxDate.HasValue)
            {
                result = result.Where(x => x.Date <= maxDate.Value);
            }

            return await result
                .Include(x => x.Seller)
                .Include(x => x.Seller.Department)
                .OrderByDescending(x => x.Date)
                .ToListAsync();
        }

        public async Task<List<IGrouping<Department,SalesRecord>>> FindByDateGroupingAsync(DateTime? minDate, DateTime? maxDate)
        {
            var result = from obj in _context.SalesRecord select obj;
            if (minDate.HasValue)
            {
                result = result.Where(x => x.Date >= minDate.Value);
            }
            if (maxDate.HasValue)
            {
                result = result.Where(x => x.Date <= maxDate.Value);
            }

            return await result
                .Include(x => x.Seller)
                .Include(x => x.Seller.Department)
                .OrderByDescending(x => x.Date)
                .GroupBy(x => x.Seller.Department)
                .ToListAsync();
        }

        public void InsertSale(SalesRecord salesRecord)
        {
            _context.SalesRecord.Add(salesRecord);
            _context.SaveChanges();
        }
    }
}
