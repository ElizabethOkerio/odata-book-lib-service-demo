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

    public class UsersController : ODataController
    {
        BookLibDbContext db;
        public UsersController(BookLibDbContext db)
        {
            this.db = db;
        }

        [EnableQuery]
        public IQueryable<User> Get()
        {
            return db.Users.AsQueryable<User>();
        }

        [EnableQuery]
        public User Get(int key)
        {
            return db.Users.Where(u => u.Id == key).FirstOrDefault();
        }

        [EnableQuery]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            try
            {
                db.Users.Add(user);
                await db.SaveChangesAsync();
                return Created(user);
            }
            catch
            {
                db.Users.Local.Remove(user);
                return BadRequest();
            }
        }

        [EnableQuery]
        public async Task<IActionResult> Patch([FromODataUri] int key, Delta<User> delta)
        {
            var user = db.Users.FirstOrDefault(c => c.Id == key);
            if (user == null)
            {
                return NotFound();
            }
            delta.Patch(user);
            await db.SaveChangesAsync();
            return Updated(user);
        }

        [EnableQuery]
        public async Task<IActionResult> Put([FromODataUri] int key, [FromBody] User user)
        {
            if (key != user.Id)
            {
                return BadRequest();
            }
            db.Entry(user).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return Updated(user);
        }

        [EnableQuery]
        public async Task<IActionResult> Delete(int key)
        {
            var user = db.Users.FirstOrDefault(c => c.Id == key);
            if (user == null)
            {
                return NotFound();
            }
            db.Users.Remove(user);
            await db.SaveChangesAsync();
            return Ok(user);
        }
    }
}