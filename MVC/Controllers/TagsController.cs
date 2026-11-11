#nullable disable
using APP.Models;
using CORE.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

// Adapted from Custom MVC Template.

namespace MVC.Controllers
{
    public class TagsController : Controller
    {
        private readonly IService<TagRequest, TagResponse> _tagService;

        public TagsController(
            IService<TagRequest, TagResponse> tagService
        )
        {
            _tagService = tagService;
        }

        private void SetViewData()
        {

        }

        private void SetTempData(string message, string key = "Message")
        {
            TempData[key] = message;
        }

        public IActionResult Index()
        {
            var list = _tagService.List();
            return View(list); // return response collection as model to the Index view
        }

        public IActionResult Details(int id)
        {
            var item = _tagService.Single(id);
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
        public IActionResult Create(TagRequest tag)
        {
            if (ModelState.IsValid) // check data annotation validation errors in the request
            {
                var response = _tagService.Add(tag);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message); // set TempData dictionary to carry the message to the redirected action's view
                    return RedirectToAction(nameof(Details), new { id = response.Id }); // redirect to Details action with id parameter as response.Id route value
                }
                ModelState.AddModelError("", response.Message); // to display service error message in the validation summary of the view
            }
            SetViewData(); // set ViewData dictionary to carry extra data other than the model to the view
            return View(tag); // return request as model to the Create view
        }

        public IActionResult Edit(int id)
        {
            var item = _tagService.Edit(id);
            SetViewData(); // set ViewData dictionary to carry extra data other than the model to the view
            if (item is null)
                return NotFound();
            return View(item); // return request as model to the Edit view
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(TagRequest tag)
        {
            if (ModelState.IsValid) // check data annotation validation errors in the request
            {
                var response = _tagService.Update(tag);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message); // set TempData dictionary to carry the message to the redirected action's view
                    return RedirectToAction(nameof(Details), new { id = response.Id }); // redirect to Details action with id parameter as response.Id route value
                }
                ModelState.AddModelError("", response.Message); // to display service error message in the validation summary of the view
            }
            SetViewData(); // set ViewData dictionary to carry extra data other than the model to the view
            return View(tag); // return request as model to the Edit view
        }

        public IActionResult Delete(int id)
        {
            var item = _tagService.Single(id);
            if (item is null)
                return NotFound();
            return View(item); // return response item as model to the Delete view
        }

        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var response = _tagService.Remove(id);
            SetTempData(response.Message); // set TempData dictionary to carry the message to the redirected action's view
            return RedirectToAction(nameof(Index)); // redirect to the Index action
        }
    }
}