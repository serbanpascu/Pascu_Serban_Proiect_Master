using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pascu_Serban_Proiect.Data;
using Pascu_Serban_Proiect.Models;
using Pascu_Serban_Proiect.Models.ToyShopViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Pascu_Serban_Proiect.Controllers
{
    [Authorize(Policy = "OnlySales")]
    public class BrandsController : Controller
    {
        private readonly ToyShopContext _context;

        public BrandsController(ToyShopContext context)
        {
            _context = context;
        }

        // GET: Brands
        public async Task<IActionResult> Index(int? id, int? toyID)
        {
            var viewModel = new BrandIndexData();
            viewModel.Brands = await _context.Brands
                .Include(i => i.BrandedToys)
                    .ThenInclude(i => i.Toy)
                        .ThenInclude(i => i.Orders)
                            .ThenInclude(i => i.Customer)
                .AsNoTracking()
                .OrderBy(i => i.BrandName)
                .ToListAsync();

            if (id != null)
            {
                ViewData["BrandID"] = id.Value;
                Brand brand = viewModel.Brands.Where(
                    i => i.ID == id.Value).Single();
                viewModel.Toys = brand.BrandedToys.Select(t => t.Toy);
            }

            if (toyID != null)
            {
                ViewData["ToyID"] = toyID.Value;
                viewModel.Orders = viewModel.Toys.Where(
                    x => x.ID == toyID).Single().Orders;
            }

            return View(viewModel);
        }

        // GET: Brands/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands
                .FirstOrDefaultAsync(m => m.ID == id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        // GET: Brands/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Brands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,BrandName,Adress")] Brand brand)
        {
            if (ModelState.IsValid)
            {
                _context.Add(brand);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(brand);
        }

        // GET: Brands/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands
                .Include(i => i.BrandedToys).ThenInclude(i => i.Toy)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (brand == null)
            {
                return NotFound();
            }
            PopulateBrandedToyData(brand);
            return View(brand);
        }

        private void PopulateBrandedToyData(Brand brand)
        {
            var allToys = _context.Toys;
            var brandedToys = new HashSet<int>(brand.BrandedToys.Select(c => c.ToyID));
            var viewModel = new List<BrandedToyData>();
            foreach (var toy in allToys)
            {
                viewModel.Add(new BrandedToyData
                {
                    ToyID = toy.ID,
                    Name = toy.Name,
                    IsBranded = brandedToys.Contains(toy.ID)
                });
            }
            ViewData["Toys"] = viewModel;
        }

        // POST: Brands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedToys)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brandToUpdate = await _context.Brands
                .Include(i => i.BrandedToys)
                .ThenInclude(i => i.Toy)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (await TryUpdateModelAsync<Brand>(
                brandToUpdate,
                "",
                i => i.BrandName, i => i.Adress))
            {
                UpdateBrandedToys(selectedToys, brandToUpdate);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {
                    ModelState.AddModelError("", "Unable to save changes." + "Try again, and if the problem persists,");
                }
                return RedirectToAction(nameof(Index));
            }
            UpdateBrandedToys(selectedToys, brandToUpdate);
            PopulateBrandedToyData(brandToUpdate);
            return View(brandToUpdate);
        }

        private void UpdateBrandedToys(string[] selectedToys, Brand brandToUpdate)
        {
            if (selectedToys == null)
            {
                brandToUpdate.BrandedToys = new List<BrandedToy>();
                return;
            }

            var selectedToysHS = new HashSet<string>(selectedToys);
            var brandedToys = new HashSet<int>(brandToUpdate.BrandedToys.Select(c => c.Toy.ID));

            foreach (var toy in _context.Toys)
            {
                if (selectedToysHS.Contains(toy.ID.ToString()))
                {
                    if (!brandedToys.Contains(toy.ID))
                    {
                        brandToUpdate.BrandedToys.Add(new BrandedToy { BrandID = brandToUpdate.ID, ToyID = toy.ID });
                    }
                }
                else
                {
                    if (brandedToys.Contains(toy.ID))
                    {
                        BrandedToy toyToRemove = brandToUpdate.BrandedToys.FirstOrDefault(i => i.ToyID == toy.ID);
                        _context.Remove(toyToRemove);
                    }
                }
            }
        }

        // GET: Brands/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands
                .FirstOrDefaultAsync(m => m.ID == id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        // POST: Brands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var brand = await _context.Brands.FindAsync(id);
            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BrandExists(int id)
        {
            return _context.Brands.Any(e => e.ID == id);
        }
    }
}
