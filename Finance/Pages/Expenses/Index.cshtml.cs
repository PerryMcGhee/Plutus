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

namespace Finance.Pages.Expenses
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly Finance.Data.ApplicationDbContext _context;

        public IndexModel(Finance.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Expense> Expense { get;set; } = default!;

        public async Task OnGetAsync()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            Expense = await _context.Expenses
                .Where(e => e.UserId == userId)
                .Include(e => e.User)
                .ToListAsync();
        }
    }
}
