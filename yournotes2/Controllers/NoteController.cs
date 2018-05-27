using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using yournotes.Models;

namespace yournotes.Controllers {
    public class NoteController : Controller {
        
        
        private readonly Db _context;
 
        public NoteController(Db context) {
            _context = context;
        }
        
        [HttpPost("/getnotes")]
        public async Task<IActionResult> GetNotes([FromBody] User user) {
            var users = await _context.Users.Include(u => u.Notes).ToArrayAsync();
            
            foreach (var u in users) {
                if (u.Id.Equals(user.Id)) {
                    var res = new {
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

        [HttpPost("/notes")]
        public async Task<IActionResult> CreateNote([FromBody] Note note, User user) {
            Console.WriteLine(note.Text);
            Console.WriteLine(user.Email);
//            Note newNote = new Note();
//            newNote.Title = note.Title;
//            note.User = user;
//            user.Notes.Add(note);
//            await _context.Notes.AddAsync(note);
//            _context.Users.Update(user);
            return Ok();
        }
        
        [HttpPut("/notes")]
        public IActionResult UpdateNote([FromBody] Note note)
        {
            Console.WriteLine("looooooooooooooooooooooooooooooooooooooooooooooool");
            Console.WriteLine(note.Text);
            return Json(new JsonResult(note));
        }
        
        [HttpDelete("/notes")]
        public IActionResult DeleteNote([FromBody] Note note)
        {
            Console.WriteLine("looooooooooooooooooooooooooooooooooooooooooooooool");
            Console.WriteLine(note.Text);
            return Json(new JsonResult(note));
        }
    }
}