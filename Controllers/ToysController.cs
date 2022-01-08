using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pascu_Serban_Proiect.Data;
using Pascu_Serban_Proiect.Models;
using Microsoft.AspNetCore.Authorization;

namespace Pascu_Serban_Proiect.Controllers
{
    [Authorize(Roles = "Employee")]
    public class ToysController : Controller
    {
        private readonly ToyShopContext _context;

        public ToysController(ToyShopContext context)
        {
            _context = context;
        }

        // GET: Toys
        [AllowAnonymous]
        public async Task<IActionResult> Index
            (string sortOrder,
             string currentFilter,
             string searchString,
             int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";

            if(searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;
            var toys = from b in _context.Toys 
                       select b;

            if (!string.IsNullOrEmpty(searchString))
            {
                toys = toys.Where(t => t.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    toys = toys.OrderByDescending(b => b.Name);
                    break;
                case "Price":
                    toys = toys.OrderBy(b => b.Price);
                    break;
                case "price_desc":
                    toys = toys.OrderByDescending(b => b.Price);
                    break;
                default:
                    toys = toys.OrderBy(b => b.Name);
                    break;
            }
            int pageSize = 2;
            return View(await PaginatedList<Toy>.CreateAsync(toys.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Toys/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toy = await _context.Toys
                .Include(t => t.Orders)
                    .ThenInclude(o => o.Customer)
                .Include(t => t.Orders)
                    .ThenInclude(o => o.Worker)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            
            if (toy == null)
            {
                return NotFound();
            }

            return View(toy);
        }

        // GET: Toys/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Toys/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name, Brand,Description,Price")] Toy toy)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(toy);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch(DbUpdateException /* ex */)
            {
                ModelState.AddModelError("", "Unable to save changes." +
                    "Try again, and if the problem persists");
            }
            return View(toy);
        }

        // GET: Toys/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toy = await _context.Toys.FindAsync(id);
            if (toy == null)
            {
                return NotFound();
            }
            return View(toy);
        }

        // POST: Toys/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kidToUpdate = await _context.Toys.FirstOrDefaultAsync(t => t.ID == id);
            if(await TryUpdateModelAsync<Toy>(
                kidToUpdate,
                "",
                t => t.Description, t => t.Name, t => t.Price))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch(DbUpdateException /* ex */)
                {
                    ModelState.AddModelError("", "Unable to save changes." +
                        "Try again, and if the problem persists");
                }
            }
            return View(kidToUpdate);
        }

        // GET: Toys/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toy = await _context.Toys
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (toy == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] = "Delete failed. Try again";
            }

            return View(toy);
        }

        // POST: Toys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var toy = await _context.Toys.FindAsync(id);
            if(toy == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Toys.Remove(toy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch(DbUpdateException /* ex */)
            {
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool ToyExists(int id)
        {
            return _context.Toys.Any(e => e.ID == id);
        }
    }
}
