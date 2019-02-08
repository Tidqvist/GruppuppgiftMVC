using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mvc02.Services;
using MVC02.Data;
using MVC02.Models;
using MVC02.Models.ViewModels;


namespace MVC02.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthService _auth;

        public ProductsController(ApplicationDbContext context, AuthService auth)
        {
            _context = context;
            _auth = auth;

        }

        // GET: Products
        public async Task<IActionResult> Index(int? id)
        {
            if (id.HasValue)
                return View(await _context.Product.Include(x => x.Category).Where(x => x.Category.Id == id).ToListAsync());

            else
            {
                var r = User.IsInRole("apa");
                var productList = _context.Product.Include(x => x.Category).Include(x => x.ProductsRoles).ToList();
                if (User.IsInRole("admin"))
                {
                    return View(productList);
                }
                var roleList = _auth.GetAllRoles().ToList();
                var userRoles = roleList;
                var userProductList = productList;
                foreach (var role in roleList)
                {
                    if (!User.IsInRole(role.Text))
                        userRoles = userRoles.Where(x => x != role).ToList();
                }

               foreach (var product in productList)
                {
                        bool ProductRoleMatchUserRole = false;
                    foreach (var role in userRoles)
                    {
                        foreach (var productRole in product.ProductsRoles)
                        {
                            if (productRole.RoleId == role.Value)
                                ProductRoleMatchUserRole = true;
                        }
                    }
                    if (!ProductRoleMatchUserRole)
                        userProductList = userProductList.Where(userProduct => userProduct != product).ToList();
                }
               
                return View(userProductList);
                //return View(await _context.Product.Include(x => x.Category).Include(x => x.ProductsRoles).Where(product => product.ProductsRoles.Any(role =>  User.IsInRole(role.RoleId.ToString()))).ToListAsync());
            }
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.Include(x => x.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            var viewModel = new CreateProductVm();
            viewModel.AllCategories = _context.Category.Select(category => new SelectListItem() { Text = category.Name, Value = category.Id.ToString() });
            viewModel.AllRoles = _auth.GetAllRoles();

            return View(viewModel);
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductVm vm) // [Bind("Id,Name,Price,CategoryId,ForSale,ProductsRoles")] 
        {


            if (ModelState.IsValid)
            {
                vm.Product.ProductsRoles = new List<ProductsRoles>();
                //List<string> xs = new List<string>();
                foreach (var role in vm.SelectedRoles)
                {
                    //xs.Add(role);
                    vm.Product.ProductsRoles.Add(new ProductsRoles { RoleId = role });
                }
                vm.SelectedRoles = new List<string>();
                _context.Add(vm.Product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(null);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var viewModel = new CreateProductVm();
            viewModel.AllCategories = _context.Category.Select(category => new SelectListItem() { Text = category.Name, Value = category.Id.ToString() });
            viewModel.Product = product;

            return View(viewModel);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,CategoryId,ForSale")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.Include(x => x.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
