using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Task.DBC;
using Task.DTO;
using Task.Models;

public class HomeController : Controller
{
    private readonly AppDBContext _db;

    public HomeController(AppDBContext db)
    {
        _db = db;
    }

    // CREATE
    public IActionResult Create()
    {
        ViewBag.Departments = new SelectList(_db.Departments, "DepartmentId", "DepartementName");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(JoinDTO joinDTO)
    {
        if (ModelState.IsValid)
        {
            var employee = new Employee
            {
                EmployeeName = joinDTO.EmployeeName,
                DepartmentId = joinDTO.DepartmentId,
            };

            _db.Employees.Add(employee);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Departments = new SelectList(_db.Departments, "DepartmentId", "DepartementName", joinDTO.DepartmentId);
        return View(joinDTO);
    }

    // READ
    public async Task<IActionResult> Index()
    {
        var data = await _db.Employees
            .Include(e => e.Department)
            .ToListAsync();
        return View(data);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var employee = await _db.Employees
            .Include(e => e.Department)
            .FirstOrDefaultAsync(m => m.EmployeeId == id);

        if (employee == null) return NotFound();

        return View(employee);
    }

    // UPDATE
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var employee = await _db.Employees.FindAsync(id);
        if (employee == null) return NotFound();

        ViewBag.Departments = new SelectList(_db.Departments, "DepartmentId", "DepartementName", employee.DepartmentId);
        return View(employee);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Employee employee)
    {
        if (id != employee.EmployeeId) return NotFound();

        if (ModelState.IsValid)
        {
            var existing = await _db.Employees.FindAsync(id);
            if (existing == null) return NotFound();

            existing.EmployeeName = employee.EmployeeName;
            existing.DepartmentId = employee.DepartmentId;

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Departments = new SelectList(_db.Departments, "DepartmentId", "DepartementName", employee.DepartmentId);
        return View(employee);
    }

    // DELETE
    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var employee = await _db.Employees
            .Include(e => e.Department)
            .FirstOrDefaultAsync(m => m.EmployeeId == id);

        if (employee == null) return NotFound();

        return View(employee);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var employee = await _db.Employees.FindAsync(id);
        if (employee != null)
        {
            _db.Employees.Remove(employee);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Privacy() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() =>
        View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}
