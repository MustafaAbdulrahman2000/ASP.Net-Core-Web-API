using Microsoft.AspNetCore.Mvc;

namespace Module_01.Controllers;

public class ProductsController(ProductStore store): Controller
{

    [HttpGet]
    public IActionResult Index()
    {
        var products = store.GetAll();

        return View(model: products);
    }

    [HttpGet]
    public IActionResult Details(Guid id)
    {
        var product = store.GetById(id);

        if (product is null)
            return NotFound();

        return View(model: product);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Product product)
    {
        product.Id = Guid.NewGuid();

        store.Add(product);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Edit(Guid id)
    {
        var product = store.GetById(id);

        if (product is null)
            return NotFound();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult Edit(Product product)
    {
        var success = store.Update(product);

        if (!success)
            return NotFound();

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Delete(Guid id)
    {
        var product = store.GetById(id);

        if (product is null)
            return NotFound();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ActionName(name: "Delete")]
    public IActionResult DeleteConfirmed(Guid id)
    {
        var product = store.GetById(id);

        if (product is null)
            return NotFound();

        store.Delete(product);

        return RedirectToAction(nameof(Index));
    }
}