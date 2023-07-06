using Books.Models;
using Books.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;

namespace Books.Controllers
{
    public class BookController : Controller
    {
        // GET: Book
        private readonly ApplicationDbContext _context;
        public BookController() 
        {
            _context = new ApplicationDbContext();
        }
        public ActionResult Index() 
        {
            var books = _context.Books.Include(m=>m.Category).ToList();
            
            return View(books);
        }
        public ActionResult Create()
        {
            var ModelView = new BookFormModelView
            {
                categories = _context.Categories.Where(m => m.IsActive).ToList()
            };
            return View("FormBook", ModelView);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var book = _context.Books.Include(m => m.Category).SingleOrDefault(m => m.Id == id);
            if (book == null)
                return HttpNotFound();

            return View(book);

        }
        public ActionResult Edit(int? id)
        {
            if(id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var book = _context.Books.Find(id);
            if (book == null)
                return HttpNotFound();

            var viewmodel = new BookFormModelView
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Description = book.Description,
                CategoryId = book.CategoryId,
                categories = _context.Categories.Where(m => m.IsActive).ToList()

            };
            return View("FormBook", viewmodel);
        }
        

        [HttpPost] 
        [ValidateAntiForgeryToken]
        public ActionResult Save(BookFormModelView model)
        {
            if(!ModelState.IsValid) 
            {
                model.categories = _context.Categories.Where(m => m.IsActive).ToList();
                return View("FormBook",model);
            }

            if(model.Id == 0)
            {
                var book = new Book
                {
                    Title = model.Title,
                    Author = model.Author,
                    CategoryId = model.CategoryId,
                    Description = model.Description
                };
                _context.Books.Add(book);
            }
            else
            {
                var book = _context.Books.Find(model.Id);
                if (book == null)
                    return HttpNotFound();

                book.Title = model.Title;
                book.Author = model.Author;
                book.CategoryId = model.CategoryId;
                book.Description = model.Description;
            }
            _context.SaveChanges();
            TempData["Message"] = "Saved Successfully";
            return RedirectToAction("Index");

        }
    }
}