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
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<StudentController> _logger;

        public StudentController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, ILogger<StudentController> logger)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                return View(await _context.Students.ToListAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the list of students.");
                return StatusCode(500, "Internal server error");
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var student = await _context.Students.FindAsync(id);
                if (student == null)
                {
                    return NotFound();
                }
                return View(student);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while editing the student with ID {id}.", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (student.ImageFile != null)
                    {
                        var fileName = Path.GetFileNameWithoutExtension(student.ImageFile.FileName);
                        var extension = Path.GetExtension(student.ImageFile.FileName);
                        student.ImagePath = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        var path = Path.Combine(_webHostEnvironment.WebRootPath, fileName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await student.ImageFile.CopyToAsync(fileStream);
                        }
                    }

                    _context.Add(student);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(student);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new student.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    if (student.ImageFile != null)
                    {
                        var fileName = Path.GetFileNameWithoutExtension(student.ImageFile.FileName);
                        var extension = Path.GetExtension(student.ImageFile.FileName);
                        student.ImagePath = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        var path = Path.Combine(_webHostEnvironment.WebRootPath, fileName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await student.ImageFile.CopyToAsync(fileStream);
                        }
                    }
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(student);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!StudentExists(student.Id))
                {
                    return NotFound();
                }
                else
                {
                    _logger.LogError(ex, "A concurrency error occurred while editing the student with ID {id}.", id);
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while editing the student with ID {id}.", id);
                return StatusCode(500, "Internal server error");
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var student = await _context.Students.FirstOrDefaultAsync(m => m.Id == id);
                if (student == null)
                {
                    return NotFound();
                }

                return View(student);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the student with ID {id} for deletion.", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var student = await _context.Students.FindAsync(id);
                if (student != null)
                {
                    if (!string.IsNullOrEmpty(student.ImagePath))
                    {
                        var path = Path.Combine(_webHostEnvironment.WebRootPath, student.ImagePath);
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                    }

                    _context.Students.Remove(student);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the student with ID {id}.", id);
                return StatusCode(500, "Internal server error");
            }
        }

        private bool StudentExists(int id)
        {
            try
            {
                return _context.Students.Any(e => e.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while checking if the student with ID {id} exists.", id);
                return false;
            }
        }
    }
}
