using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using ProjetEval.Models.TestUser;

namespace ProjetEval.Controllers
{
    public class ProfilsController : Controller
    {
        private readonly DbContextUserTest _context;

        public ProfilsController(DbContextUserTest context)
        {
            _context = context;
        }

        // GET: Profils
        public async Task<IActionResult> Index()
        {
              return _context.Profil != null ? 
                          View(await _context.Profil.Include(p => p.IdgenreNavigation).ToListAsync()) :
                          Problem("Entity set 'DbContextUserTest.Profil'  is null.");
        }

        // GET: Profils/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Profil == null)
            {
                return NotFound();
            }

            var profil = await _context.Profil
                .FirstOrDefaultAsync(m => m.Id == id);
            if (profil == null)
            {
                return NotFound();
            }

            return View(profil);
        }

        // GET: Profils/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Profils/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nom,Prenom,Naissance,Idgenre,Max,Min")] Profil profil)
        {
            if (ModelState.IsValid)
            {
                _context.Add(profil);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(profil);
        }

        // GET: Profils/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Profil == null)
            {
                return NotFound();
            }

            var profil = await _context.Profil.FindAsync(id);
            if (profil == null)
            {
                return NotFound();
            }
            return View(profil);
        }

        // POST: Profils/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,Prenom,Naissance,Idgenre,Max,Min")] Profil profil)
        {
            if (id != profil.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(profil);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfilExists(profil.Id))
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
            return View(profil);
        }

        // GET: Profils/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Profil == null)
            {
                return NotFound();
            }

            var profil = await _context.Profil
                .FirstOrDefaultAsync(m => m.Id == id);
            if (profil == null)
            {
                return NotFound();
            }

            return View(profil);
        }

        // POST: Profils/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Profil == null)
            {
                return Problem("Entity set 'DbContextUserTest.Profil'  is null.");
            }
            var profil = await _context.Profil.FindAsync(id);
            if (profil != null)
            {
                _context.Profil.Remove(profil);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProfilExists(int id)
        {
          return (_context.Profil?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpPost, ActionName("ExportPdf")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExportToPdfAsync(string? filePath)
        {
            var profils = await _context.Profil.Include(p => p.IdgenreNavigation).ToListAsync();

            var chemin = "";
            if(filePath!=null){
                chemin = Path.Combine(filePath, "profils.pdf");
            }

            Console.WriteLine(chemin);

            using (var writer = new PdfWriter(chemin))
            {
                using (var pdf = new PdfDocument(writer))
                {
                    var document = new Document(pdf);
                    var table = new Table(2);
                    table.AddHeaderCell("Nom"); // Nom de la colonne pour le nom du profil
                    table.AddHeaderCell("Genre"); // Nom de la colonne pour le genre

                    foreach (var profil in profils)
                    {
                        table.AddCell(profil.Nom);
                        if(profil.IdgenreNavigation!=null){
                            table.AddCell(profil.IdgenreNavigation.Nom);
                        }else{
                            table.AddCell("null");
                        } // Suppose que le nom du genre est dans une propriété "Nom"
                    }

                    document.Add(table);
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
