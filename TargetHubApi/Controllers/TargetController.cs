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
        SubscriptionController subCtrl = new SubscriptionController();

        [HttpGet]
        public List<string> GetTargets(string Identifier, int ID, [FromUri] List<string> tags)
        {
            //var list = from t in db.Targets
            //           join ta in db.Tags on t.ID equals ta.TargetID
            //           where tags.Contains(ta.tag)
            //           select t.Name;
            return (Registered(Identifier, ID)) ?
                 (tags.Count != 0) ? db.Targets.Join(db.Tags, t => t.ID, ta => ta.TargetID, (t, ta) => new { t.Name, ta.tag }).
                 Where(t => tags.Contains(t.tag)).Select(o => o.Name).Distinct().ToList() : null : null;
        }

        [HttpGet]
        public HttpResponseMessage Download(string Identifier, int ID, string TargetName, string format)
        {
            //Check if the server is registered or not.
            if (Registered(Identifier, ID))
            {
                Target target = db.Targets.Where(t => t.Name == TargetName).FirstOrDefault();
                if(target == null)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Target does not exist!");
                HttpResponseMessage result;
                //TODO: check if the file exists even in database
                string filePath = format == "dat" ? 
                    target.DatFilePath : format == "xml" ? 
                    target.XmlFilePath : format=="chat"? target.ChatFilePath : "";
                if (filePath == "")
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "This File does not exist in the Hub!");
                }
                try
                {
                    var path = filePath;
                    result = new HttpResponseMessage(HttpStatusCode.OK);
                    var stream = new FileStream(path, FileMode.Open);
                    result.Content = new StreamContent(stream);
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");                    
                    if (target.Subscriptions.Where(x => x.ServerID == ID).Count() == 0)
                        target.Subscriptions.Add(new Subscription { TargetID = target.ID, ServerID = ID });
                    db.SaveChanges();
                }
                
                catch (Exception e)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
                }
                
                return result;
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Server is not registered!");
        }

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
                var provider = new CustomMultipartFormDataStreamProvider(root);
                try
                {
                    // Read the form data.
                    await Request.Content.ReadAsMultipartAsync(provider);

                    string format = "";
                    string filename = "";
                    // This illustrates how to get the file names.
                    foreach (MultipartFileData file in provider.FileData)
                    {
                        //Trace.WriteLine(file.Headers.ContentDisposition.FileName);
                        //Trace.WriteLine("Server file path: " + file.LocalFileName);
                        filename = file.Headers.ContentDisposition.FileName.ToLower().Replace("\"", "");
                        format = filename.LastIndexOf(".") == -1 ? "" : filename.Substring(filename.LastIndexOf(".") + 1);
                        if (format == "xml" || format == "dat")
                        {
                            Target myTarget = db.Targets.Where(t => t.Name == TargetName).FirstOrDefault();
                            if (myTarget == null)
                            {
                                myTarget = new Target()
                                {
                                    Name = TargetName,
                                    ChatFilePath = (format == "xml" && filename.Contains("_chat.xml")) ? root + "\\" + filename : "",
                                    XmlFilePath = (format == "xml" && !filename.Contains("_chat.xml")) ? root + "\\" + filename : "",
                                    DatFilePath = format == "dat" ? root + "\\" + filename : ""
                                };
                                db.Targets.Add(myTarget);
                                db.SaveChanges();
                                myTarget.Tags = new List<Tag>();
                                foreach (string s in Tags)
                                {
                                    myTarget.Tags.Add(new Tag { TargetID = myTarget.ID, tag = s });
                                }
                            }
                            else
                            {
                                if (format == "xml")
                                {
                                    if (filename.Contains("_chat.xml"))
                                        myTarget.ChatFilePath = root + "\\" + filename;
                                    else
                                        myTarget.XmlFilePath = root + "\\" + filename;
                                }
                                else
                                    myTarget.DatFilePath = root + "\\" + filename;
                                foreach (string s in Tags)
                                {

                                    if (!myTarget.Tags.Select(X => X.tag).Contains(s))
                                        myTarget.Tags.Add(new Tag { TargetID = myTarget.ID, tag = s });
                                }
                            }
                            if (myTarget.Subscriptions != null && myTarget.Subscriptions.Where(x => x.ServerID == ID).Count() == 0)
                                myTarget.Subscriptions.Add(new Subscription { TargetID = myTarget.ID, ServerID = ID });
                            db.SaveChanges();
                        }
                        else
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "only xml or dat types are valid!");
                        }
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, "Target: " + TargetName + " added");
                }
                catch (Exception e)
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
        [HttpGet]
        public string ForwardMessage(string Identifier, int ID, string TargetName, string UserName, string SentMessage)
        {
            if (Registered(Identifier, ID))
            {
                int TargetID = db.Targets.Where(t => t.Name == TargetName).FirstOrDefault().ID;
                string Uri = "/default.aspx" + "?Chat=" + TargetName + "&SentMessage=" +
                    SentMessage + "&User=" + UserName + "&Sender=Hub";
                List<Server> servers = db.Servers.
                    Join(db.Subscriptions, s => s.Id, su => su.ServerID, (s, su) => new { s, su.TargetID }).
                    Where(t => t.TargetID == TargetID).Select(o => o.s).ToList();
                foreach (Server s in db.Servers.
                    Join(db.Subscriptions, s => s.Id, su => su.ServerID, (s, su) => new { s, su.TargetID }).
                    Where(t => t.TargetID == TargetID).Select(o => o.s).ToList())
                {
                    if (s.Id != ID)
                    {
                        WebRequest.Create(new Uri(s.Address + Uri)).GetResponse();
                        return "Message forwarded";
                    }
                }
            }
            return "Server is not registered.";
        }
        public class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
        {
            public CustomMultipartFormDataStreamProvider(string path) : base(path) { }

            public override string GetLocalFileName(HttpContentHeaders headers)
            {
                return headers.ContentDisposition.FileName.Replace("\"", string.Empty);
            }
        }
    }
}