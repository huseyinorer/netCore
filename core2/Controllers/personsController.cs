using core2.Entities;
using core2.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace core2.Controllers
{
    public class personsController : Controller
    {
        private readonly SchoolContext _context;

        public personsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: persons
        public async Task<IActionResult> Index()
        {
            return View(await _context.persons.ToListAsync());
        }

        // GET: persons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persons = await _context.persons
                .SingleOrDefaultAsync(m => m.personid == id);
            if (persons == null)
            {
                return NotFound();
            }

            return View(persons);
        }

        // GET: persons/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: persons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("personid,lastname,firstname,address,city")] persons persons)
        {
            if (ModelState.IsValid)
            {
                persons.personid = persons.personid != 0 ? _context.persons.Max(p => p.personid) + 1 : 0;
                _context.Add(persons);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(persons);
        }

        // GET: persons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persons = await _context.persons.SingleOrDefaultAsync(m => m.personid == id);
            if (persons == null)
            {
                return NotFound();
            }
            return View(persons);
        }

        // POST: persons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("personid,lastname,firstname,address,city")] persons persons)
        {
            if (id != persons.personid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(persons);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!personsExists(persons.personid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(persons);
        }

        // GET: persons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persons = await _context.persons
                .SingleOrDefaultAsync(m => m.personid == id);
            if (persons == null)
            {
                return NotFound();
            }

            return View(persons);
        }

        // POST: persons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var persons = await _context.persons.SingleOrDefaultAsync(m => m.personid == id);
            _context.persons.Remove(persons);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool personsExists(int id)
        {
            return _context.persons.Any(e => e.personid == id);
        }
    }
}
