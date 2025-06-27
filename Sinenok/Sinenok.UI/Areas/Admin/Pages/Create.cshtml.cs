using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sinenok.Domain.Entities;
using Sinenok.UI.Data;
using Sinenok.UI.Services;

namespace Sinenok.UI.Areas.Admin.Pages
{
    public class CreateModel(ICategoryService categoryService, IProductService
productService) : PageModel
    {
        public async Task<IActionResult> OnGet()
        {
            var categoryListData = await categoryService.GetCategoryListAsync();
            ViewData["CategoryId"] = new SelectList(categoryListData.Data, "Id",
    "Name");
            return Page();
        }

        [BindProperty]
        public Gadget Gadget { get; set; } = default!;
        [BindProperty]
        public IFormFile? Image { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD 
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            await productService.CreateProductAsync(Gadget,Image);
        return RedirectToPage("./Index");
        }
    }
}
