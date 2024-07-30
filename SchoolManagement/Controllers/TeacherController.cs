using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolManagement.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolManagement.Controllers
{
    public class TeacherController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<TeacherController> _logger;

        public TeacherController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, ILogger<TeacherController> logger)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                return View(await _context.Teachers.ToListAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the list of teachers.");
                return StatusCode(500, "Internal server error");
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Teacher teacher)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (teacher.ImageFile != null)
                    {
                        var fileName = Path.GetFileNameWithoutExtension(teacher.ImageFile.FileName);
                        var extension = Path.GetExtension(teacher.ImageFile.FileName);
                        teacher.ImagePath = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        var path = Path.Combine(_webHostEnvironment.WebRootPath, fileName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await teacher.ImageFile.CopyToAsync(fileStream);
                        }
                    }

                    _context.Add(teacher);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(teacher);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new teacher.");
                return StatusCode(500, "Internal server error");
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var teacher = await _context.Teachers.FindAsync(id);
                if (teacher == null)
                {
                    return NotFound();
                }
                return View(teacher);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while preparing the edit view for teacher with ID {id}.", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Teacher teacher)
        {
            if (id != teacher.Id)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    if (teacher.ImageFile != null)
                    {
                        var fileName = Path.GetFileNameWithoutExtension(teacher.ImageFile.FileName);
                        var extension = Path.GetExtension(teacher.ImageFile.FileName);
                        teacher.ImagePath = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        var path = Path.Combine(_webHostEnvironment.WebRootPath, fileName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await teacher.ImageFile.CopyToAsync(fileStream);
                        }
                    }

                    _context.Update(teacher);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(teacher);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!TeacherExists(teacher.Id))
                {
                    return NotFound();
                }
                else
                {
                    _logger.LogError(ex, "A concurrency error occurred while editing the teacher with ID {id}.", id);
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while editing the teacher with ID {id}.", id);
                return StatusCode(500, "Internal server error");
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var teacher = await _context.Teachers.FirstOrDefaultAsync(m => m.Id == id);
                if (teacher == null)
                {
                    return NotFound();
                }

                return View(teacher);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while preparing the delete view for teacher with ID {id}.", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var teacher = await _context.Teachers.FindAsync(id);
                if (teacher != null)
                {
                    // Optionally, delete the image file from the server
                    if (!string.IsNullOrEmpty(teacher.ImagePath))
                    {
                        var path = Path.Combine(_webHostEnvironment.WebRootPath, teacher.ImagePath);
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                    }

                    _context.Teachers.Remove(teacher);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the teacher with ID {id}.", id);
                return StatusCode(500, "Internal server error");
            }
        }

        private bool TeacherExists(int id)
        {
            try
            {
                return _context.Teachers.Any(e => e.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while checking if the teacher with ID {id} exists.", id);
                return false;
            }
        }
    }
}
