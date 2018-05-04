using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Student_WebAPI.Data;
using Student_WebAPI.Models;

namespace Student_WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Students")]
    public class StudentsController : Controller
    {
        StudentsDbContext studentsDbContext;
        public StudentsController(StudentsDbContext _studentDbContext)
        {
            studentsDbContext = _studentDbContext;
        }

        // GET: api/Students
        [HttpGet]
        public IEnumerable<Student> GetSorted(string sortFirstName)
        {
            IQueryable<Student> students;
            switch (sortFirstName)
            {
                case "desc":
                    students = studentsDbContext.Students.OrderByDescending(s => s.FirstName);
                    break;
                case "asc":
                    students = studentsDbContext.Students.OrderBy(s => s.FirstName);
                    break;
                default:
                    students = studentsDbContext.Students;
                    break;
            }
            return students;
        }

        // GET: api/Students/page
        [HttpGet("page")]
        public IEnumerable<Student> GetPage(int? pageNumber, int? pageSize)
        {
            var students = from s in studentsDbContext.Students.OrderBy(s => s.StudentId) select s;
            if (pageNumber < 1 || pageSize < 1)
            {
                return studentsDbContext.Students;
            }
            else
            {
                int currentPage = pageNumber ?? 1;
                int currentPageSize = pageSize ?? 5;
                var items = students.Skip((currentPage - 1) * currentPageSize).Take(currentPageSize).ToList();
                return items;
            }
            
        }

        // GET: api/Students/Search
        [HttpGet("Search")]
        public IEnumerable<Student> GetSearch(string searchStudent)
        {
            var students = studentsDbContext.Students.Where(s => s.FirstName.StartsWith(searchStudent));
            return students;
        }

        // GET: api/Students/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            var student = studentsDbContext.Students.SingleOrDefault(s => s.StudentId == id);
            if (student == null)
            {
                return NotFound("No student record found...");
            }
            return Ok(student);
        }
        
        // POST: api/Students
        [HttpPost]
        public IActionResult Post([FromBody]Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            studentsDbContext.Students.Add(student);
            studentsDbContext.SaveChanges(true);
            return StatusCode(StatusCodes.Status201Created);
        }
        
        // PUT: api/Students/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != student.StudentId)
            {
                return BadRequest();
            }
            try
            {
                studentsDbContext.Students.Update(student);
                studentsDbContext.SaveChanges(true);                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return NotFound("No Record found with this student id...");
            }
            return Ok("Student has been updated...");
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var student = studentsDbContext.Students.SingleOrDefault(s => s.StudentId == id);
            if (student == null)
            {
                return NotFound("No student record found...");
            }
            studentsDbContext.Students.Remove(student);
            studentsDbContext.SaveChanges(true);
            return Ok("The student has been deleted...");
        }
    }
}
