using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Data;
using SalesWebMvc.Models;
using SalesWebMvc.Models.Enums;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services;

namespace SalesWebMvc.Controllers
{
    public class SalesRecordsController : Controller
    {
        private readonly SalesRecordService _salesRecordService;
        private readonly SellerService _sellerService;
        private readonly SalesWebMvcContext _context;

        public SalesRecordsController(SalesRecordService salesRecordService, SellerService sellerService, SalesWebMvcContext context)
        {
            _salesRecordService = salesRecordService;
            _sellerService = sellerService;
            _context = context;
        }
        public IActionResult Index()
        {
            var list = _salesRecordService.FindLast();
            return View(list);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SalesRecord saleRecord = await _salesRecordService.FindByIdAsync(id.Value);
            if (saleRecord == null)
            {
                return NotFound();
            }

            List<Seller> list = await _sellerService.FindAllAsync();
            SaleRecordViewModel viewModel = new SaleRecordViewModel{ Seller = list, SalesRecord = saleRecord};
            return View(viewModel);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(int id, SalesRecord sr)
        {
            if (!ModelState.IsValid)
            {
                var list = await _sellerService.FindAllAsync();
                var viewModel = new SaleRecordViewModel { Seller = list, SalesRecord = sr};
                return View(list);
            }
            if (id != sr.Id)
            {
                return NotFound();
            }

            try
            {
                await _salesRecordService.UpdateAsync(sr);
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException)
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obj = await _salesRecordService.FindByIdAsync(id.Value);
            if (obj == null)
            {
                return BadRequest();
            }
            return View(obj);
        }

        public async Task<IActionResult> SimpleSearch(DateTime? minDate, DateTime? maxDate)
        {
            if (!minDate.HasValue)
            {
                minDate = new DateTime(DateTime.Now.Year, 1, 1);
            }
            if (!maxDate.HasValue)
            {
                maxDate = DateTime.Now;
            }
            ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd");
            ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");
            var result = await _salesRecordService.FindByDateAsync(minDate, maxDate);
            return View(result);
        }

        public async Task<IActionResult> GroupingSearch(DateTime? minDate, DateTime? maxDate)
        {
            if (!minDate.HasValue)
            {
                minDate = new DateTime(DateTime.Now.Year, 1, 1);
            }
            if (!maxDate.HasValue)
            {
                maxDate = DateTime.Now;
            }
            ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd");
            ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");
            var result = await _salesRecordService.FindByDateGroupingAsync(minDate, maxDate);
            return View(result);
        }

        public async Task<IActionResult> InsertSale()
        {
            var sellers = await _sellerService.FindAllAsync();
            var viewModel = new SaleRecordViewModel { Seller = sellers };            
            return View(viewModel);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> InsertSale(SalesRecord saleRecord)
        {

            if (!ModelState.IsValid)
            {
                var sellers = await _sellerService.FindAllAsync();
                var viewModel = new SaleRecordViewModel { Seller = sellers };
                return View(viewModel);
            }
            _salesRecordService.InsertSale(saleRecord);
            return RedirectToAction(nameof(Index));
        }
    }
}
