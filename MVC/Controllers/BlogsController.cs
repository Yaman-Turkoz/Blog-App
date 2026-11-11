#nullable disable
using APP.Models;
using CORE.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

// Adapted from Custom MVC Template.

namespace MVC.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IService<BlogRequest, BlogResponse> _blogService;
        private readonly IService<TagRequest, TagResponse> _tagService;

        public BlogsController(
            IService<BlogRequest, BlogResponse> blogService,
            IService<TagRequest, TagResponse> tagService
        )
        {
            _blogService = blogService;
            _tagService = tagService;
        }

        private void SetViewData()
        {
            ViewBag.TagIds = new MultiSelectList(_tagService.List(), "Id", "Name");
        }

        private void SetTempData(string message, string key = "Message")
        {
            TempData[key] = message;
        }

        public IActionResult Index()
        {
            var list = _blogService.List();
            return View(list); // return response collection as model to the Index view
        }

        public IActionResult Details(int id)
        {
            var item = _blogService.Single(id);
            if (item is null)
                return NotFound();
            return View(item); // return response item as model to the Details view
        }

        public IActionResult Create()
        {
            SetViewData(); // set ViewData dictionary to carry extra data other than the model to the view
            return View(); // return Create view with no model
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(BlogRequest blog)
        {
            if (ModelState.IsValid) // check data annotation validation errors in the request
            {
                var response = _blogService.Add(blog);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message); // set TempData dictionary to carry the message to the redirected action's view
                    return RedirectToAction(nameof(Details), new { id = response.Id }); // redirect to Details action with id parameter as response.Id route value
                }
                ModelState.AddModelError("", response.Message); // to display service error message in the validation summary of the view
            }
            SetViewData(); // set ViewData dictionary to carry extra data other than the model to the view
            return View(blog); // return request as model to the Create view
        }

        public IActionResult Edit(int id)
        {
            var item = _blogService.Edit(id);
            SetViewData(); // set ViewData dictionary to carry extra data other than the model to the view
            if (item is null)
                return NotFound();
            return View(item); // return request as model to the Edit view
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(BlogRequest blog)
        {
            if (ModelState.IsValid) // check data annotation validation errors in the request
            {
                var response = _blogService.Update(blog);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message); // set TempData dictionary to carry the message to the redirected action's view
                    return RedirectToAction(nameof(Details), new { id = response.Id }); // redirect to Details action with id parameter as response.Id route value
                }
                ModelState.AddModelError("", response.Message); // to display service error message in the validation summary of the view
            }
            SetViewData(); // set ViewData dictionary to carry extra data other than the model to the view
            return View(blog); // return request as model to the Edit view
        }

        public IActionResult Delete(int id)
        {
            var item = _blogService.Single(id);
            if (item is null)
                return NotFound();
            return View(item); // return response item as model to the Delete view
        }

        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var response = _blogService.Remove(id);
            SetTempData(response.Message); // set TempData dictionary to carry the message to the redirected action's view
            return RedirectToAction(nameof(Index)); // redirect to the Index action
        }
    }
}