using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjetEval.Models.TestUser;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace ProjetEval.Controllers
{
    public class GenreController : Controller
    {
        private readonly DbContextUserTest _context;

        public GenreController(DbContextUserTest context)
        {
            _context = context;
        }

        // GET: Genre
        public async Task<IActionResult> Index()
        {
              return _context.Genre != null ? 
                          View(await _context.Genre.ToListAsync()) :
                          Problem("Entity set 'DbContextUser.Genre'  is null.");
        }

        // GET: Genre/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Genre == null)
            {
                return NotFound();
            }

            var genre = await _context.Genre
                .FirstOrDefaultAsync(m => m.Id == id);
            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        // GET: Genre/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Genre/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nom")] Genre genre)
        {
            if (ModelState.IsValid)
            {
                _context.Add(genre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(genre);
        }

        // GET: Genre/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Genre == null)
            {
                return NotFound();
            }

            var genre = await _context.Genre.FindAsync(id);
            if (genre == null)
            {
                return NotFound();
            }
            return View(genre);
        }

        // POST: Genre/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom")] Genre genre)
        {
            if (id != genre.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(genre);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GenreExists(genre.Id))
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
            return View(genre);
        }

        // GET: Genre/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Genre == null)
            {
                return NotFound();
            }

            var genre = await _context.Genre
                .FirstOrDefaultAsync(m => m.Id == id);
            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        // POST: Genre/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Genre == null)
            {
                return Problem("Entity set 'DbContextUser.Genre'  is null.");
            }
            var genre = await _context.Genre.FindAsync(id);
            if (genre != null)
            {
                _context.Genre.Remove(genre);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GenreExists(int id)
        {
          return (_context.Genre?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpPost, ActionName("addCsv")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> addCsv([FromForm] IFormFile csvFile)
        {
            if (csvFile != null && csvFile.Length > 0)
            {
                try
                {
                    // Vérifier si l'extension du fichier est CSV
                    if (Path.GetExtension(csvFile.FileName).ToLower() == ".csv")
                    {
                        using (var reader = new StreamReader(csvFile.OpenReadStream()))
                        using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
                        {
                            var records = csv.GetRecords<Genre>().ToList();

                            // Insérer les données dans la base de données
                            // Exemple de code pour insérer dans la base de données ici
                            // Notez que vous devrez remplacer ce code par votre propre logique d'insertion dans la base de données
                            foreach (var record in records)
                            {
                                // Insérer record.Id et record.Nom dans la base de données
                                _context.Add(record);
                
                            }
                        }
                        await _context.SaveChangesAsync();

                        ViewBag.Message = "Le fichier a été téléchargé et les données ont été insérées avec succès.";
                    }
                    else
                    {
                        ViewBag.Message = "Veuillez télécharger un fichier CSV.";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Une erreur s'est produite lors du traitement du fichier : " + ex.Message;
                }
            }
            else
            {
                ViewBag.Message = "Veuillez sélectionner un fichier à télécharger.";
            }

            return RedirectToAction(nameof(Index));
        }
    }


}
