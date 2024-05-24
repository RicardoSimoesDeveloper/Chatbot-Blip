using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using System.Net.Http;
using Newtonsoft.Json;

namespace TodoApi.Controllers;

[Route("api/TodoItems")]
[ApiController]
public class TodoItemsController : ControllerBase
{
    private readonly TodoContext _context;
    private readonly IHttpClientFactory _clientFactory;

    public TodoItemsController(TodoContext context, IHttpClientFactory clientFactory)
    {
        _context = context;
        _clientFactory = clientFactory;
    }

     // GET: api/TodoItems/GitHubRepos
    [HttpGet("GitHubRepos")]
    public async Task<ActionResult<IEnumerable<GitHubRepoDTO>>?> GetGitHubRepos()
    {
        var client = _clientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("User-Agent", "C# HttpClient");

        var response = await client.GetAsync("https://api.github.com/orgs/takenet/repos?sort=created&direction=asc");

        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            var gitHubRepos = JsonConvert.DeserializeObject<List<GitHubRepoDTO>>(jsonString);

            var csharpRepos = gitHubRepos.Where(repo => repo.Language == "C#").Take(5).ToList();

            return csharpRepos;
        }
        else
        {
            return NotFound();
        }
    }

}