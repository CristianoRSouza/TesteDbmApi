using Microsoft.AspNetCore.Mvc;
using PresentationTest.Dtos;
using PresentationTest.Services;

namespace PresentationTest.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        public async Task<ActionResult> Index()
        {
            var products = await _productService.GetProductsAsync();

            if (products == null || !products.Any())
            {
                ViewBag.Message = "Nenhum produto encontrado.";
                return View(new List<ProductRequest>());
            }

            return View(products);
        }

        public async Task<ActionResult> Details(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProductRequest product)
        {
            if (ModelState.IsValid)
            {
                var success = await _productService.CreateProductAsync(product);
                if (success)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Erro ao adicionar o produto.");
            }
            return View(product);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, ProductRequest product)
        {
            if (ModelState.IsValid)
            {
                var success = await _productService.UpdateProductAsync(id, product);
                if (success)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Erro ao atualizar o produto.");
            }
            return View(product);
        }

        public async Task<ActionResult> Delete(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var success = await _productService.DeleteProductAsync(id);
            if (success)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Erro ao deletar o produto.");
            return View();
        }
    }
}
