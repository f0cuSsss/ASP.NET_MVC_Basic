using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASP_MVC_Basic_14_04_2020.Controllers
{
    public class BookController : Controller
    {
        // GET: Book
        public ActionResult Index()
        {
            ViewBag.Books = Models.Book.GetList();

            return View();
        }

        public ViewResult Add()
        {
            if(Session["AddBookMessage"] != null)
            {
                ViewBag.Message = Session["AddBookMessage"];
                Session["AddBookMessage"] = null;
            }
            return View();
        }

        public RedirectToRouteResult AddBook(Models.Book book)
        {
            Session["InvalidControl"] = null;
            Session["AddBookMessage"] = null;

            if (String.IsNullOrEmpty(book.Title))
            {
                Session["AddBookMessage"] = "Title could not be empty";
                Session["InvalidControl"] = "1";
            }
            else if(Request.Files[0].ContentLength == 0)
            {
                Session["AddBookMessage"] = "Select a cover for the book";
                Session["InvalidControl"] = "2";
            }
            else if(book.Id_Author == 0)
            {
                Session["AddBookMessage"] = "Select the author from dropdown list";
                Session["InvalidControl"] = "3";
            }
            else if (book.Id_Genre == 0)
            {
                Session["AddBookMessage"] = "Select the genre from dropdown list";
                Session["InvalidControl"] = "4";
            }

            // File transfer
            // Files[0].ContentLength - размер файла (в байтах)
            // Files[0].ContentType - тип ресурса (MIME)
            // Request.Files[0].InputStream - файловый поток (для копирования)

            // System -> IIS Express
            // Server("/") -> Project
            String SitePath = Server.MapPath("/");
            String CoverPath = @"/Images/Cover/";

            String FileName = (new Random().Next()) + Request.Files[0].FileName;

            using (var file = new System.IO.FileStream(SitePath + CoverPath + FileName, System.IO.FileMode.Create))
            {
                try
                {
                    Request.Files[0].InputStream.CopyTo(file);
                    //Session["AddBookMessage"] = "File transfer OK";
                }
                catch (Exception ex)
                {
                    Session["AddBookMessage"] = ex.Message;
                }
            }

            if(Session["AddBookMessage"] == null)
            {
                // Все проверки пройдены - добавляем в БД
                book.Cover_file = FileName;

                try
                {
                    book.Price = Convert.ToDecimal(Request.Params["Price"]);
                    Models.Util.InsertInTable("Book", book);
                }
                catch (Exception ex)
                {
                    Session["AddBookMessage"] = ex.Message;
                }
            }

            // HW
            // Поместить данные о книге в БД
            // * При ошибке внесения (или проверок) оставить данные в полях ввода


            if (Session["AddBookMessage"] == null)
            {
                Session["AddBookMessage"] = "Add OK";

                Session["AddBookTitle"] =
                Session["AddBookAnnotation"] =
                Session["AddBookYear"] =
                Session["AddBookPages"] =
                Session["AddBookPrice"] =
                Session["AddBookAuthorId"] =
                Session["AddBookGenreId"] = null;
            }
            else
            {
                Session["AddBookTitle"] = book.Title;
                Session["AddBookAnnotation"] = book.Annotation;
                Session["AddBookYear"] = book.Year;
                Session["AddBookPages"] = book.Pages;
                Session["AddBookPrice"] = Request.Params["Price"];
                Session["AddBookAuthorId"] = book.Id_Author;
                Session["AddBookGenreId"] = book.Id_Genre;
                
            }


            return RedirectToRoute(new { action = "Add" });
        }

        public EmptyResult ResetAddBook()
        {
            Session["AddBookTitle"] =
            Session["AddBookAnnotation"] =
            Session["AddBookYear"] =
            Session["AddBookPages"] =
            Session["AddBookPrice"] =
            Session["AddBookAuthorId"] =
            Session["AddBookGenreId"] = null;

            return new EmptyResult();
        }

        //============== Genre ==============

        public ViewResult Genre()
        {
            if (Session["AddGenreError"] != null)
            {
                ViewBag.ErrMessage = Session["AddGenreError"];
                Session["AddGenreError"] = null;
            }

            try
            {
                ViewBag.GenreList = Models.Genre.GetGenres();
            }
            catch (Exception ex)
            {
                ViewBag.ErrMessage = ex.Message;
            }

            return View();
        }

        public RedirectToRouteResult AddGenre(Models.Genre genre)
        {
            if (String.IsNullOrEmpty(genre.Name))
            {
                Session["AddGenreError"] = "Name could not be empty";
            }
            else if (genre.Description?.Length > 256)
            {
                Session["AddGenreError"] = "Description is too long";
            }
            else
            {
                try
                {
                    var con = Models.Util.GetConnection();

                    // Add mode
                    if (genre.Id == 0)
                    {
                        if (Models.Util.IsValueInDBField("Genre", "Name", genre.Name))
                        {
                            throw new Exception($"Genre {genre.Name} already exists");
                        }
                        Models.Util.InsertInTable("Genre", genre);
                        Session["AddGenreError"] = "Add seccessful";
                    }
                    else // Update mode
                    {
                        // Проверка, не существует ли другой жанр с этим именем
                        using (
                            var cmd = new SqlCommand(
                                $"SELECT Id FROM Genre " +
                                $"WHERE UPPER(Name) LIKE UPPER(N'{genre.Name}') AND Id <> {genre.Id}", con))
                        {
                            if (cmd.ExecuteScalar() != null)
                            {
                                throw new Exception($"Name {genre.Name} matches other in DB");
                            }
                        }

                        Models.Util.UpdateInTable("Genre", genre);
                        Session["AddGenreError"] = "Edit seccessful";
                    }
                }
                catch (Exception ex)
                {
                    Session["AddGenreError"] = ex.Message;
                }
            }

            return RedirectToRoute(
                new
                {
                    action = "Genre"
                });
        }

        public RedirectToRouteResult DelGenre(Models.Genre genre)
        {
            if (genre.Id == 0)
            {
                Session["AddGenreError"] = "Invalid data format";
            }
            else
            {
                try
                {
                    Models.Util.DeleteFromTable("Genre", genre.Id);
                    Session["AddGenreError"] = "Delete successful";
                }
                catch (Exception ex)
                {
                    Session["AddGenreError"] = ex.Message;
                }
            }



            return RedirectToRoute(
                new
                {
                    action = "Genre"
                });
        }

        //============== City ==============

        public ViewResult City()
        {

            if (Session["CityMessage"] != null)
            {
                ViewBag.Message = Session["CityMessage"];
                Session["CityMessage"] = null;
            }

            try
            {
                ViewBag.CityList = Models.City.GetCities();
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
            }

            return View();
        }

        public RedirectToRouteResult AddCity(Models.City city)
        {
            if (String.IsNullOrEmpty(city.Name))
            {
                Session["CityMessage"] = "Name could not be empty";
            }
            else
            {
                try
                {
                    var con = Models.Util.GetConnection();

                    // Add mode
                    if (city.Id == 0)
                    {
                        if (Models.Util.IsValueInDBField("City", "Name", city.Name))
                        {
                            throw new Exception($"City {city.Name} already exists");
                        }
                        Models.Util.InsertInTable("City", city);
                        Session["CityMessage"] = "Add seccessful";
                    }
                    else // Update mode
                    {
                        // Проверка, не существует ли другой город с этим именем
                        using (
                            var cmd = new SqlCommand(
                                $"SELECT Id FROM City " +
                                $"WHERE UPPER(Name) LIKE UPPER(N'{city.Name}') AND Id <> {city.Id}", con))
                        {
                            if (cmd.ExecuteScalar() != null)
                            {
                                throw new Exception($"Name {city.Name} matches other in DB");
                            }
                        }

                        Models.Util.UpdateInTable("City", city);
                        Session["CityMessage"] = "Edit seccessful";
                    }
                }
                catch (Exception ex)
                {
                    Session["CityMessage"] = ex.Message;
                }
            }

            return RedirectToRoute(
                new
                {
                    action = "City"
                });
        }

        public RedirectToRouteResult DelCity(Models.City city)
        {
            if (city.Id == 0)
            {
                Session["CityMessage"] = "Invalid data format";
            }
            else
            {
                try
                {
                    Models.Util.DeleteFromTable("City", city.Id);
                    Session["CityMessage"] = "Delete successful";
                }
                catch (Exception ex)
                {
                    Session["CityMessage"] = ex.Message;
                }
            }

            return RedirectToRoute(
                new
                {
                    action = "City"
                });
        }

        //============== Author ==============

        public ViewResult Author()
        {
            try
            {
                ViewBag.AuthorsList = Models.Author.GetAuthors();
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
            }

            return View();
        }

        public string AddAuthorStr(Models.Author author)
        {
            if (author == null || String.IsNullOrEmpty(author.Name))
            {
                return "{\"status\":1, \"msg\":\"Not enought data\"}";
            }


            try
            {
                if (Models.Util.IsValueInDBField("Author", "Name", author.Name))
                {
                    return $"{{\"status\":2, \"msg\":\"Author '{author.Name}' already in DB\"}}";
                }
                Models.Util.InsertInTable("Author", author);
            }
            catch (Exception ex)
            {
                return $"{{\"status\":3, \"msg\":\"{ex.Message.Replace('"', '\'').Replace('\n', ' ').Replace('\r', ' ')}\"}}"; 
            }


            return "{\"status\":0, \"msg\":\"OK\"}"; ;
        }

        public JsonResult AddAuthor(Models.Author author)
        {
            if (author == null || String.IsNullOrEmpty(author.Name))
            {
                return Json(new
                {
                    status = 1,
                    msg = "Not enought data"
                });
            }


            try
            {
                if (Models.Util.IsValueInDBField("Author", "Name", author.Name))
                {
                    return Json(new
                    {
                        status = 2,
                        msg = $"Author '{author.Name}' already in DB"
                    });
                }

                Models.Util.InsertInTable("Author", author);

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = 3,
                    msg = ex.Message.Replace('"', '\'').Replace('\n', ' ').Replace('\r', ' ')
                });
            }

            return Json(new
            {
                status = 0
            });
        }

        public JsonResult AuthorList()
        {
            List<Models.Author> authors = Models.Author.GetAuthors();

            return Json(authors, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EditAuthor(Models.Author author)
        {
            int status = 0;
            string message = "";

            if (String.IsNullOrEmpty(author.Name))
            { // Удаление (пустое имя - было стёрто)
                try
                {
                    Models.Util.DeleteFromTable("Author", author.Id);
                    status = 0;
                    message = "Delete successful";
                }
                catch (Exception ex)
                {
                    status = 2;
                    message = ex.Message;
                }
            }
            else
            { // Обновление - замена имени
                String AuthorName = author.Name.Trim();
                AuthorName = AuthorName.Replace("  ", " ");

                var con = Models.Util.GetConnection();
                var cmd = new SqlCommand(
                    $"SELECT COUNT(Id) FROM Author WHERE UPPER(Name) LIKE UPPER(N'{AuthorName}') AND Id <> {author.Id}", con);

                int n = 0;

                try
                {
                    n = Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    status = 3;
                    message = ex.Message;
                }
                finally
                {
                    cmd.Dispose();
                }

                if(n > 0)
                {
                    status = 4;
                    message = "Name already in DB";
                }

                if (status == 0)
                {
                    try
                    {
                        Models.Util.UpdateInTable("Author", new { Name = author.Name, Id = author.Id });
                        status = 0;
                        message = "Update successful";
                    }
                    catch (Exception ex)
                    {
                        status = 1;
                        message = ex.Message;
                    } 
                }
            }
            return Json(new
            {
                status = status,
                message = message
            });
        }

        [HttpGet]
        public String EditAuthor()
        {
            // TODO:
            // Узнать ип и переслать куда надо
            // IP - Request.ServerVariables["REMOTE_ADDR"]

            return "Delete OK";
        }


        //[HttpPost]
        public JsonResult EditAuthorPseudo(Models.Author author)
        {
            int status = 0;
            string message = "";

            try
            {
                Models.Util.UpdateInTable("Author", new { Pseudo = author.Pseudo, Id = author.Id });
                status = 0;
                message = "Update successful";
            }
            catch (Exception ex)
            {
                status = 1;
                message = ex.Message;
            }

            return Json(new
            {
                status = status,
                message = message
            });
        }

        //============== Publisher ==============
    }
}
