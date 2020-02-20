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

    public class BooksController : ODataController
    {
        BookLibDbContext db;
        public BooksController(BookLibDbContext db)
        {
            this.db = db;
        }

        [EnableQuery]
        public IQueryable<Book> Get()
        {
            return db.Books.AsQueryable<Book>();
        }

        [EnableQuery]
        public Book Get(int key)
        {
            return db.Books.Where(b => b.Id == key).FirstOrDefault();
        }

        [EnableQuery]
        public async Task<IActionResult> Post([FromBody] Book book)
        {
            try
            {
                db.Books.Add(book);
                await db.SaveChangesAsync();
                return Created(book);
            }
            catch
            {
                db.Books.Local.Remove(book);
                return BadRequest();
            }
        }

        [EnableQuery]
        public async Task<IActionResult> Patch([FromODataUri] int key, Delta<Book> delta)
        {
            var book = db.Books.FirstOrDefault(b => b.Id == key);
            if (book == null)
            {
                return NotFound();
            }
            delta.Patch(book);
            await db.SaveChangesAsync();
            return Updated(book);
        }

        [EnableQuery]
        public async Task<IActionResult> Put([FromODataUri] int key, [FromBody] Book book)
        {
            if (key != book.Id)
            {
                return BadRequest();
            }
            db.Entry(book).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return Updated(book);
        }

        [EnableQuery]
        public async Task<IActionResult> Delete(int key)
        {
            var book = db.Books.FirstOrDefault(b => b.Id == key);
            if (book == null)
            {
                return NotFound();
            }
            db.Books.Remove(book);
            await db.SaveChangesAsync();
            return Ok(book);
        }
    }
}