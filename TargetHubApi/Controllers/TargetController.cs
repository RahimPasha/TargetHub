using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Threading.Tasks;
using System.Web.Http;
using TargetHubApi.Infrastructure;
using TargetHubApi.Models;
using System.Diagnostics;
using System.Collections.Generic;

namespace TargetHubApi.Controllers
{
    public class TargetController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        public List<string> GetTargets(string Identifier,int ID,[FromUri] List<string>tags)
        {
            List<string> TargetNameList = new List<string>();
            if (tags.Count != 0)
            {
                //var list = from t in db.Targets
                //           join ta in db.Tags on t.ID equals ta.TargetID
                //           where tags.Contains(ta.tag)
                //           select t.Name;
                TargetNameList = db.Targets.Join(db.Tags, t => t.ID, ta => ta.TargetID, (t, ta) => new { t.Name, ta.tag }).
                    Where(t => tags.Contains(t.tag)).
                    Select(o => o.Name).Distinct().ToList();
            }
            return TargetNameList;
        }

        [HttpGet]
        public HttpResponseMessage Download(string Identifier, int ID, string TargetName, string format)
        {
            //Check if the server is registered or not.
            if (Registered(Identifier, ID))
            {
                HttpResponseMessage result;
                string filePath = "";
                if (format == "dat")
                {
                    //TODO: check if the file exists even in database
                    filePath = db.Targets.Where(t => t.Name == TargetName).FirstOrDefault().DatFilePath;
                }
                else if (format == "xml")
                {
                    //TODO: check if the file exists even in database
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
                catch (Exception e)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
                }
                return result;
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Server is not registered!");
        }

        //Overloading didn't work
        //[HttpPost]
        //public async Task<HttpResponseMessage> Upload(string Identifier, int ID, string TargetName)
        //{
        //    return await Upload(Identifier, ID, TargetName, new List<string>());
        //}
        [HttpPost]
        public async Task<HttpResponseMessage> Upload(string Identifier, int ID, string TargetName, [FromUri] List<string> Tags)
        {
            List<Tag> TagList = new List<Tag>();
            
            //Check if the server is registered or not.
            if (Registered(Identifier, ID))
            {
                // Check if the request contains multipart/form-data.
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                string root = HttpContext.Current.Server.MapPath("~/files");
                var provider = new MultipartFormDataStreamProvider(root);
                try
                {
                    // Read the form data.
                    await Request.Content.ReadAsMultipartAsync(provider);

                    string format = "";
                    string filename = "";
                    // This illustrates how to get the file names.
                    foreach (MultipartFileData file in provider.FileData)
                    {
                        Trace.WriteLine(file.Headers.ContentDisposition.FileName);
                        Trace.WriteLine("Server file path: " + file.LocalFileName);
                        filename = file.Headers.ContentDisposition.FileName.ToLower().Replace("\"", "");
                        format = filename.LastIndexOf(".") == -1 ? "" : filename.Substring(filename.LastIndexOf(".") + 1);
                        if (format == "xml" || format == "dat")
                        {
                            if (db.Targets.Where(t => t.Name == TargetName).Count() == 0)
                            {
                                var myTarget = new Target()
                                {
                                    Name = TargetName,
                                    XmlFilePath = format == "xml" ? root + "\\" + filename : "",
                                    DatFilePath = format == "dat" ? root + "\\" + filename : ""
                                };
                                db.Targets.Add(myTarget);
                                db.SaveChanges();
                                myTarget.Tags = new List<Tag>();
                                foreach(string s in Tags)
                                {
                                    myTarget.Tags.Add(new Tag { TargetID = myTarget.ID, tag = s });
                                }
                            }
                            else
                            {
                                if (format == "xml")
                                    db.Targets.Where(t => t.Name == TargetName).FirstOrDefault().XmlFilePath = root + "\\" + filename;
                                else
                                    db.Targets.Where(t => t.Name == TargetName).FirstOrDefault().DatFilePath = root + "\\" + filename;
                                var myTarget = db.Targets.Where(t => t.Name == TargetName).FirstOrDefault();
                                foreach (string s in Tags)
                                {
                                    
                                    if (!myTarget.Tags.Select(X => X.tag).Contains(s))
                                        myTarget.Tags.Add(new Tag { TargetID = myTarget.ID, tag = s });
                                }
                            }
                            db.SaveChanges();
                        }
                        else
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "only xml or dat types are valid!");
                        }
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, "Target: " + TargetName + " added");
                }
                catch (System.Exception e)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
                }
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Server is not registered!");
        }
        private bool Registered(string identifier, int ID)
        {
            return db.Servers.Where(s => s.Id == ID && s.Identifier == identifier).Count() == 0 ? false : true;

        }
    }
}