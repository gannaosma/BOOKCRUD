using Books.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Books.Controllers.API
{
    public class BookController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public BookController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            var book = _context.Books.Find(id);
            if (book == null) 
                return NotFound();

            _context.Books.Remove(book);
            _context.SaveChanges();

            return Ok();
        }
    }
}
