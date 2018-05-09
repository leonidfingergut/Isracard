using Isracard.Classes;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Isracard.Controllers
{
    public class GithubController : ApiController
    {
        private static string githubURL = "https://api.github.com/search/repositories?q=";
        const string REPOSITORY_BOOKMARK = "repository_bookmark";
        // GET: api/Github
        //get/search repository
        public async Task<List<Repository>> Get(string rname)
        {
            List<Repository> listRepository = new List<Repository>();
            do
            {
                if (string.IsNullOrEmpty(rname))
                {
                    break;
                }
                string url = githubURL + rname;
                using (HttpClient client = new HttpClient())
                {
                    // ServicePointManager.Expect100Continue = false;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    try
                    {
                        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla");
                        var response = await client.GetAsync(url);

                        if (response.IsSuccessStatusCode)
                        {
                            var responseContent = response.Content;
                            string responseString = await responseContent.ReadAsStringAsync();

                            JObject jsonRepositories = JObject.Parse(responseString);
                            var list =
                                    from item in jsonRepositories["items"]
                                    select new Repository() { ID = (int)item["id"], Name = (string)item["name"], Avatar = (string)item["owner"]["avatar_url"] };
                            listRepository = list.ToList<Repository>();
                        }
                    }
                    catch (Exception ex)
                    {
                        //log
                    }
                }

            } while (false);
            return listRepository;
        }

        //get repository from session
        public async Task<List<Repository>> Get()
        {
            List<Repository> listRepository = new List<Repository>();
            do
            {
                try
                {
                    var session = HttpContext.Current.Session;

                    if (session != null && session[REPOSITORY_BOOKMARK] != null)
                    {
                        listRepository = ((Dictionary<int, Repository>)session[REPOSITORY_BOOKMARK]).Select(kvp => kvp.Value).ToList(); ;
                    }
                    else
                    {
                        listRepository = new List<Repository>();
                    }
                }
                catch (Exception ex)
                {
                    //log
                }
            } while (false);
            return listRepository;
        }


        // POST: api/Github
        //set repository to session
        public bool Post([FromBody]Repository repository)
        {
            bool ret = false;

            do
            {
                if (repository == null)
                {
                    break;
                }
                var session = HttpContext.Current.Session;

                if (session != null && session[REPOSITORY_BOOKMARK] != null)
                {
                    var list = (Dictionary<int, Repository>)session[REPOSITORY_BOOKMARK];
                    if (list.Keys.Contains(repository.ID))
                    {
                        list.Remove(repository.ID);
                    }
                    else
                    {
                        repository.bookmark = true;
                        list.Add(repository.ID, repository);
                    }
                }
                else
                {
                    Dictionary<int, Repository> list = new Dictionary<int, Repository>();
                    repository.bookmark = true;
                    list.Add(repository.ID, repository);
                    HttpContext.Current.Session.Add(REPOSITORY_BOOKMARK, list);
                }

                ret = true;
            } while (false);

            return ret;
        }

    }
}
