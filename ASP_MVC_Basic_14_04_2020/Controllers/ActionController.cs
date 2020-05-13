using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASP_MVC_Basic_14_04_2020.Controllers
{
    public class ActionController : Controller
    {
        // General type - ActionResult
        public ActionResult Index()
        {
            return View();
        }

        // PartialViewResult - without Layout page
        public PartialViewResult Partial()
        {
            //return PartialView("Index");
            return PartialView();
        }

        // ContentResult - Local content
        public ContentResult ContentRes()
        {
            return Content("<h1>Content Result</h1>");
        }

        // EmptyResult - Valid answer (status 200), but empty (no body)
        public EmptyResult EmptyRes()
        {
            return new EmptyResult();
        }

        // Upload:   client->server;
        // Download: server->client;
        // FileResult - for file downloading
        // ! For method using
        // 1. create Files/ folder
        // 2. copy a64f.jpg to it
        public FileResult FileRes()
        {
            // ~ корневой каталог сайта
            return File("~/Files/a64f.jpg", "image/jpg");

            // Save file as .docx with name FileRes
            //return File("~/Files/КурсовойПроект.docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");

            // Save file as .docx with name Курсовой.docx
            //return File("~/Files/КурсовойПроект.docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "Курсовой.docx");

            // Save file as .doc
            //return File("~/Files/КурсовойПроект.docx", "application/msword");
        }

        // redirect (301) - change URL in browser
        public RedirectResult Redirect()
        {
            return Redirect("https://itstep.org");
        }

        public RedirectToRouteResult Route()
        {
            return RedirectToRoute(
                    new
                    {
                        controller = "Action",
                        action = "Partial"
                    }
                );
        }

        public JsonResult JsonRes()
        {
            return Json(
                new
                {
                    Name = "Petrovich",
                    Birth = "2000-01-01",
                    Code = 1231
                }, JsonRequestBehavior.AllowGet // By default JSON restricted in GET mode
            );
        }

        public JavaScriptResult JS(String id)
        {
            String msg;
            switch (id)
            {
                case "en": msg = "Hello"; break;
                case "ru": msg = "Привет"; break;
                case "ua": msg = "Вітання"; break;
                default: msg = "Lang not supported"; break;
            }

            return JavaScript(String.Format("alert('{0}')", msg));
        }

        // Status Code in HTTP Response
        public HttpStatusCodeResult Restricted()
        {
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
        }

        public HttpStatusCodeResult Restricted2()
        {
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden, "Restricted!!!");
        }

        public NewResult newResult()
        {
            return new NewResult();
        }

        public NewJSONResult newJSONRes()
        {
            return new NewJSONResult();
        }

        // for ../xml ; ../json
        public CurrencyResult GetCurrency(String id)
        {
            return new CurrencyResult(id);
        }


        public DifferentResult GetCurrency2()
        {
            return new DifferentResult();
        }

    }

    public class NewResult : ActionResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            String HtmlContent = @"<!doctype html>
                                <html>
                                <head>
                                    <title>New Result</title>
                                </head>
                                <body>
                                    <h2>Action result override demo</h2>
                                    <p>
                                       public override void ExecuteResult(ControllerContext context)
                                    </p>
                                </body>
                                </html>";
            // HTTP - headers part
            context.HttpContext.Response.Headers.Add("Content-Type", "text/html");
            context.HttpContext.Response.Headers.Add("Content-Length", HtmlContent.Length.ToString());
            // HTTP - body part
            context.HttpContext.Response.Write(HtmlContent);
        }
    }

    public class NewJSONResult : ActionResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            String HtmlContent = @"{
                                  'name': 'John',
                                  'age': 30,
                                  'isAdmin': false,
                                  'courses': ['html', 'css', 'js'],
                                  'wife': null
                                }";
            // HTTP - headers part
            context.HttpContext.Response.Headers.Add("Content-Type", "text/html");
            context.HttpContext.Response.Headers.Add("Content-Length", HtmlContent.Length.ToString());
            // HTTP - body part
            context.HttpContext.Response.Write(HtmlContent);
        }
    }

    public class NewJSONResult2 : JsonResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            String Content = "{ \"name\": \"John\", \"age\": 30 }";

            context.HttpContext.Response.Headers.Add("Content-Type", "application/json");
            context.HttpContext.Response.Headers.Add("Content-Length", Content.Length.ToString());

            context.HttpContext.Response.Write(Content);
        }
    }

    // for ../xml ; ../json
    public class CurrencyResult : ActionResult
    {
        String type = String.Empty;
        public CurrencyResult(String type)
        {
            this.type = type;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            String Content = String.Empty;

            switch (type)
            {
                case "xml":
                    Content = "&lt;currencies&gt;<br/> &lt;name&gt;USD&lt;/name&gt; <br/>&lt;sale&gt;28.8&lt;/sale&gt; <br/>&lt;/currencies&gt;";
                    //Content = "<exchangerates> &lt; row &gt; &lt; exchangerate ccy=\"USD\" base_ccy=\"UAH\" buy=\"26.90000\" sale=\"27.40000\"/&gt; &lt;/ row &gt;" +
                    //"&lt; row &gt;"+
                    //"&lt; exchangerate ccy=\"EUR\" base_ccy=\"UAH\" buy=\"29.00000\" sale=\"29.77000\"/&gt;"+
                    //"&lt;/ row &gt;" +
                    //"&lt; row &gt;" +
                    //"&lt; exchangerate ccy=\"RUR\" base_ccy=\"UAH\" buy=\"0.32000\" sale=\"0.37000\"/ &gt;" +
                    //"&lt;/ row &gt;" +
                    //"&lt; row &gt;" +
                    //"&lt; exchangerate ccy=\"BTC\" base_ccy=\"USD\" buy=\"6677.3584\" sale=\"7380.2382\"/ &gt;" +
                    //"&lt;/ row &gt;" +
                    //"&lt;/ exchangerates &gt; ";
                    context.HttpContext.Response.Headers.Add("Content-Type", "application/xml");
                    break;
                default:
                case "json":
                    Content = "{ \"name\": \"USD\", \"sale\": 28.8 }";
                    //Content = "[{\"ccy\":\"USD\",\"base_ccy\":\"UAH\",\"buy\":\"26.90000\",\"sale\":\"27.40000\"},{\"ccy\":\"EUR\",\"base_ccy\":\"UAH\",\"buy\"" +
                    //    ":\"29.00000\",\"sale\":\"29.77000\"},{\"ccy\":\"RUR\",\"base_ccy\":\"UAH\",\"buy\":\"0.32000\",\"sale\":\"0.37000\"},{\"ccy\":\"BTC\",\"base_ccy\":\"USD\",\"buy\"" +
                    //    ":\"6682.2066\",\"sale\":\"7385.5968\"}]";
                    context.HttpContext.Response.Headers.Add("Content-Type", "application/json");
                    break;
            }

            context.HttpContext.Response.Headers.Add("Content-Length", Content.Length.ToString());

            context.HttpContext.Response.Write(Content);
        }
    }


    public class DifferentResult : ActionResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            // Возможность работы с неполными запросами <<?json>>
            // Params --
            // QueryString -- 
            // RawUrl +
            var response = context.HttpContext.Response;
            var request = context.HttpContext.Request;
            String Content = String.Empty;

            if (request.RawUrl.Contains("?json"))
            {
                Content = "{ \"name\": \"USD\", \"sale\": 28.8 }";
                //response.Headers.Add("Content-Type", "application/json");
                response.ContentType = "application/json";
            }
            else
            {
                //Content = "&lt;currencies&gt;<br/> &lt;name&gt;USD&lt;/name&gt; <br/>&lt;sale&gt;28.8&lt;/sale&gt; <br/>&lt;/currencies&gt;";
                Content = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n" +
                    "<root>" +
                    "<name>USD</name>\n" +
                    "<sale>28.8</sale>" +
                    "</root>";
                //response.Headers.Add("Content-Type", "application/xml");

                if (request.RawUrl.Contains("?file"))
                {
                    response.ContentType = "application/octet-stream";
                    response.Headers.Add("Content-Disposition", "attachment; filename=\"currencies.xml\"");
                }
                else
                    response.ContentType = "application/xml";
            }

            response.Headers.Add("Content-Length", Content.Length.ToString());

            response.Write(Content);
        }
    }


}