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
        public Finances Expense { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            Expense = await _context.Finances
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

            if (id == null)
            {
                return NotFound();
            }

            var expense = await _context.Finances.FirstOrDefaultAsync(m => m.Id == id);

            if (expense == null)
            {
                return NotFound();
            }
            else
            {
                Expense = expense;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            Expense = await _context.Finances
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

            if (id == null)
            {
                return NotFound();
            }

            var expense = await _context.Finances.FindAsync(id);
            if (expense != null)
            {
                Expense = expense;
                _context.Finances.Remove(Expense);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
