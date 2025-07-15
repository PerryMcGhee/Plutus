using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Finance.Data;
using Finance.Models;
using Microsoft.AspNetCore.Authorization;



namespace Finance.Pages.FinanceCrudPages
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly Finance.Data.ApplicationDbContext _context;

        public DeleteModel(Finance.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Finances Finance { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("./Index");
            }

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            Finance = await _context.Finances
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

            if (Finance == null)
            {
                return RedirectToPage("./Index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("./Index");
            }
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            Finance = await _context.Finances
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);
            if (Finance == null)
            {
                return RedirectToPage("./Index");

            }

            _context.Finances.Remove(Finance);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
