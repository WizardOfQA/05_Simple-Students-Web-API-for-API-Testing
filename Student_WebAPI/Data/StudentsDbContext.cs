using Microsoft.EntityFrameworkCore;
using Student_WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Student_WebAPI.Data
{
    public class StudentsDbContext: DbContext
    {
        public StudentsDbContext(DbContextOptions<StudentsDbContext>options):base(options)
        {

        }
        public DbSet<Student> Students { get; set; }
    }
}
