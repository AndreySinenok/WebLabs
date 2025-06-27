using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Sinenok.Domain.Entities;
using Sinenok.UI.Data;
using Sinenok.UI.Services;

namespace Sinenok.UI.Areas.Admin.Pages
{
    public class DetailsModel(IProductService productService) : PageModel
    {
        public Gadget gadget { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await productService.GetProductByIdAsync(id.Value);
            if (!result.Success || result.Data == null)
            {
                return NotFound();
            }

            gadget = result.Data;
            return Page();
        }
    }
}