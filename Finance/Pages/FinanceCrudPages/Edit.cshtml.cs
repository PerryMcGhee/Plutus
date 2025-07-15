using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Finance.Data;
using Finance.Models;
using Microsoft.AspNetCore.Authorization;

namespace Finance.Pages.FinanceCrudPages
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly Finance.Data.ApplicationDbContext _context;

        public EditModel(Finance.Data.ApplicationDbContext context)
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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Finance).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FinanceExists(Finance.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool FinanceExists(int id)
        {
            return _context.Finances.Any(e => e.Id == id);
        }
    }
}
