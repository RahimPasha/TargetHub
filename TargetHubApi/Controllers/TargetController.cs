using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using TargetHubApi.Infrastructure;
using TargetHubApi.Models;

namespace TargetHubApi.Controllers
{
    public class TargetController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();
        
        [HttpGet]
        public HttpResponseMessage Download(string Identifier, int ID,  string TargetName, string format)
        {
            if (Registered(Identifier, ID))
            {
                HttpResponseMessage result;
                string filePath = "";
                if (format == "dat")
                {
                    filePath = db.Targets.Where(t => t.Name == TargetName).FirstOrDefault().DatFilePath;
                }
                else if (format == "xml")
                {
                    filePath = db.Targets.Where(t => t.Name == TargetName).FirstOrDefault().XmlFilePath;
                }
                try
                {
                    var path = filePath;
                    result = new HttpResponseMessage(HttpStatusCode.OK);
                    var stream = new FileStream(path, FileMode.Open);
                    result.Content = new StreamContent(stream);
                    result.Content.Headers.ContentType =
                        new MediaTypeHeaderValue("application/octet-stream");
                }
                catch(Exception e)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }
                return result;
            }
            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        private bool Registered(string identifier, int ID)
        {
            return db.Servers.Where(s => s.Id == ID && s.Identifier == identifier).Count() == 0 ? false : true;
              
        }
    }
}