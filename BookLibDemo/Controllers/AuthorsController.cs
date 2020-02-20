using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BookLibService.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookLibService.Controllers
{

    public class AuthorsController : ODataController
    {
        BookLibDbContext db;
        public AuthorsController(BookLibDbContext db)
        {
            this.db = db;
        }

        [EnableQuery]
        public IQueryable<Author> Get()
        {
            return db.Authors.AsQueryable<Author>();
        }

        [EnableQuery]
        public Author Get(int key)
        {
            return db.Authors.Where(a => a.Id == key).FirstOrDefault();
        }

        [EnableQuery]
        public async Task<IActionResult> Post([FromBody] Author author)
        {
            try
            {
                db.Authors.Add(author);
                await db.SaveChangesAsync();
                return Created(author);
            }
            catch
            {
                db.Authors.Local.Remove(author);
                return BadRequest();
            }
        }

        [EnableQuery]
        public async Task<IActionResult> Patch([FromODataUri] int key, Delta<Author> delta)
        {
            var author = db.Authors.FirstOrDefault(a => a.Id == key);
            if (author == null)
            {
                return NotFound();
            }
            delta.Patch(author);
            await db.SaveChangesAsync();
            return Updated(author);
        }

        [EnableQuery]
        public async Task<IActionResult> Put([FromODataUri] int key, [FromBody] Author author)
        {
            if (key != author.Id)
            {
                return BadRequest();
            }
            db.Entry(author).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return Updated(author);
        }

        [EnableQuery]
        public async Task<IActionResult> Delete(int key)
        {
            var author = db.Authors.FirstOrDefault(a => a.Id == key);
            if (author == null)
            {
                return NotFound();
            }
            db.Authors.Remove(author);
            await db.SaveChangesAsync();
            return Ok(author);
        }
    }
}