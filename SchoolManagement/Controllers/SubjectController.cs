using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolManagement.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolManagement.Controllers
{
    public class SubjectController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SubjectController> _logger;

        public SubjectController(ApplicationDbContext context, ILogger<SubjectController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var subjects = await _context.Subjects
                    .Include(s => s.Teachers)
                    .Include(s => s.Students)
                    .ToListAsync();
                return View(subjects);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the list of subjects.");
                return StatusCode(500, "Internal server error");
            }
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                ViewData["TeacherIds"] = new MultiSelectList(await _context.Teachers.ToListAsync(), "Id", "Name");
                ViewData["StudentIds"] = new MultiSelectList(await _context.Students.ToListAsync(), "Id", "Name");
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while preparing the create subject view.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Class,Language,TeacherIds,StudentIds")] SubjectViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var subject = new Subject
                    {
                        Name = model.Name,
                        Class = model.Class,
                        Language = model.Language,
                        Teachers = _context.Teachers.Where(t => model.TeacherIds.Contains(t.Id)).ToList(),
                        Students = _context.Students.Where(s => model.StudentIds.Contains(s.Id)).ToList()
                    };

                    _context.Add(subject);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["TeacherIds"] = new MultiSelectList(await _context.Teachers.ToListAsync(), "Id", "Name", model.TeacherIds);
                ViewData["StudentIds"] = new MultiSelectList(await _context.Students.ToListAsync(), "Id", "Name", model.StudentIds);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new subject.");
                return StatusCode(500, "Internal server error");
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var subject = await _context.Subjects
                    .Include(s => s.Teachers)
                    .Include(s => s.Students)
                    .FirstOrDefaultAsync(s => s.Id == id);

                if (subject == null)
                {
                    return NotFound();
                }

                var model = new SubjectViewModel
                {
                    Id = subject.Id,
                    Name = subject.Name,
                    Class = subject.Class,
                    Language = subject.Language,
                    TeacherIds = subject.Teachers.Select(t => t.Id).ToList(),
                    StudentIds = subject.Students.Select(s => s.Id).ToList()
                };

                ViewData["TeacherIds"] = new MultiSelectList(await _context.Teachers.ToListAsync(), "Id", "Name", model.TeacherIds);
                ViewData["StudentIds"] = new MultiSelectList(await _context.Students.ToListAsync(), "Id", "Name", model.StudentIds);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while editing the subject with ID {id}.", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Class,Language,TeacherIds,StudentIds")] SubjectViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    var subject = await _context.Subjects
                        .Include(s => s.Teachers)
                        .Include(s => s.Students)
                        .FirstOrDefaultAsync(s => s.Id == id);

                    if (subject == null)
                    {
                        return NotFound();
                    }

                    subject.Name = model.Name;
                    subject.Class = model.Class;
                    subject.Language = model.Language;
                    subject.Teachers = _context.Teachers.Where(t => model.TeacherIds.Contains(t.Id)).ToList();
                    subject.Students = _context.Students.Where(s => model.StudentIds.Contains(s.Id)).ToList();

                    _context.Update(subject);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["TeacherIds"] = new MultiSelectList(await _context.Teachers.ToListAsync(), "Id", "Name", model.TeacherIds);
                ViewData["StudentIds"] = new MultiSelectList(await _context.Students.ToListAsync(), "Id", "Name", model.StudentIds);
                return View(model);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!SubjectExists(model.Id))
                {
                    return NotFound();
                }
                else
                {
                    _logger.LogError(ex, "A concurrency error occurred while editing the subject with ID {id}.", id);
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while editing the subject with ID {id}.", id);
                return StatusCode(500, "Internal server error");
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var subject = await _context.Subjects
                    .Include(s => s.Teachers)
                    .Include(s => s.Students)
                    .FirstOrDefaultAsync(s => s.Id == id);

                if (subject == null)
                {
                    return NotFound();
                }

                return View(subject);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the subject with ID {id} for deletion.", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var subject = await _context.Subjects.FindAsync(id);
                if (subject != null)
                {
                    _context.Subjects.Remove(subject);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the subject with ID {id}.", id);
                return StatusCode(500, "Internal server error");
            }
        }

        private bool SubjectExists(int id)
        {
            try
            {
                return _context.Subjects.Any(e => e.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while checking if the subject with ID {id} exists.", id);
                return false;
            }
        }
    }
}
