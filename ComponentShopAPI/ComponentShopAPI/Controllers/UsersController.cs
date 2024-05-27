﻿using ComponentShopAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComponentShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ComponentShopDBContext _context;

        public UsersController(ComponentShopDBContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // POST: api/Users/register
        [HttpPost("register")]
        public async Task<ActionResult<User>> RegisterUser([FromQuery] string username, [FromQuery] string password)
        {
            if (_context.Users.Any(user => user.Username == username))
            {
                return Ok(new { userExists = true });
            }

            _context.Users.Add(new User(username, password));
            await _context.SaveChangesAsync();

            return Created("GetUsers", new { userExists = false });
        }

        // POST: api/Users/login
        [HttpPost("login")]
        public ActionResult<User> LoginUser([FromQuery] string username, [FromQuery] string password)
        {
            User? currentUser = _context.Users.Where(user => user.Username == username && user.Password == password).FirstOrDefault();

            if (currentUser == null)
            {
                return Ok(new { loginSuccessful = false });
            }

            return Ok(new { currentUser, loginSuccessful = true });
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}