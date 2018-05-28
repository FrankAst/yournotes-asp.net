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
        public async Task<IActionResult> CreateNote([FromBody] CreateNoteRequest req) {
            var users = await _context.Users.Include(u => u.Notes).ToArrayAsync();
            foreach (var u in users) {
                if (u.Id.Equals(req.user.Id)) {
                    u.Notes.Add(req.note);
                    await _context.SaveChangesAsync();
                    return Ok("note creaed successfuly");
                }
            }

            return BadRequest("User not found at creating new note");
        }
        
        [HttpPut("/notes")]
        public async Task<IActionResult> UpdateNote([FromBody] Note note) {
            var notes = await _context.Notes.ToArrayAsync();
            foreach (var n in notes) {
                if (n.Id.Equals(note.Id)) {
                    if(note.Text != null) n.Title = note.Title;
                    if(note.Title != null) n.Text = note.Text;
                    await _context.SaveChangesAsync();
                    return Ok("note updated successfuly");
                }
            }
            return BadRequest("Note not found at updating note");
        }
        
        [HttpDelete("/notes")]
        public async Task<IActionResult> DeleteNote([FromBody] Note note) {
            var notes = await _context.Notes.ToArrayAsync();
            foreach (var n in notes) {
                if (n.Id.Equals(note.Id)) {
                    _context.Notes.Remove(n);
                    await _context.SaveChangesAsync();
                    return Ok("note deleted successfuly");
                }
            }
            
            return BadRequest("Note not found at deleting note");
        }
        
        
        public class CreateNoteRequest {
            public Note note { get; set; }
            public User user { get; set; }
        }
    }
}