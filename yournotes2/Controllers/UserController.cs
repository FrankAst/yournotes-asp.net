using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using yournotes.Models;

namespace yournotes.Controllers
{
    public class UserController : Controller
    {
        
        private readonly Db _context;
 
        public UserController(Db context) {
            _context = context;
        }
        
        [HttpGet("/users")]
        public async Task<IActionResult> Get(int id) {
            
            var users = await _context.Users.Include(u => u.Notes).ToArrayAsync();
            
            foreach (var u in users) {
                if (u.Id.Equals(id)) {
                    var res = new {
                        id = u.Id,
                        email = u.Email,
                        notes = u.Notes.Select(n => {
                            return new {
                                id = n.Id,
                                title = n.Title,
                                text = n.Text
                            };
                        })
                    };
                    return Ok(res);
                }
            }

            return BadRequest("No user found!");
        }
        
        [HttpPost("/login")]
        public async Task<IActionResult> Login([FromBody] User user) {
            var users = await _context.Users.Include(u => u.Notes).ToArrayAsync();
            
            foreach (var u in users) {
                if (u.Email.Equals(user.Email) && u.Password.Equals(user.Password)) {
                    var res = new {
                        id = u.Id,
                        email = u.Email,
                        notes = u.Notes.Select(n => {
                            return new {
                                id = n.Id,
                                title = n.Title,
                                text = n.Text
                            };
                        })
                    };
                    return Ok(res);
                }
            }

            return BadRequest("Wrong credentials!");
        }
        
        [HttpPost("/signup")]
        public async Task<IActionResult> Signup([FromBody] User user)
        {
            var users = await _context.Users.ToArrayAsync();

            foreach (var u in users)
            {
                if (u.Email.Equals(user.Email))
                {
                    return BadRequest("User already exists");
                }
            }
          
            User newUser = new User();
            newUser.Email = user.Email;
            newUser.Password = user.Password;
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return Ok(newUser);
        }
    }
}