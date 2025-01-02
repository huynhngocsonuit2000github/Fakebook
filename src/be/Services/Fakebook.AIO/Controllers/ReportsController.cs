using System.Net.Http.Headers;
using System.Text;
using Fakebook.AIO.Models;
using Microsoft.AspNetCore.Mvc;

namespace Fakebook.AIO.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class ReportsController : ControllerBase
{

    private readonly IConfiguration _configuration;
    public ReportsController(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    [HttpPost("create-job")]
    public async Task<IActionResult> CreateJobAsync()
    {
        string _jobscriptFilesPath = "./Jobs/Jobscript.xml"; // Path to Jenkinsfile
        string _jenkinsFilesPath = "./Jobs/Jenkinsfile"; // Path to Jenkinsfile
        string _jenkinsBaseUrl = _configuration["AIOJenkins:HostName"]; // Jenkins base URL
        string _jenkinsCredentials = _configuration["AIOJenkins:Credentials"]; // Jenkins credentials in the form "username:password"

        try
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(_jenkinsCredentials)));

            // Check if the job already exists
            var jobExistsResponse = await client.GetAsync($"{_jenkinsBaseUrl}/job/TestJobName/api/json");
            if (jobExistsResponse.IsSuccessStatusCode)
            {
                return Conflict(new { message = "Jenkins job already exists." });
            }

            // Read the Jenkinsfile content
            if (!System.IO.File.Exists(_jobscriptFilesPath) || !System.IO.File.Exists(_jenkinsFilesPath))
            {
                return BadRequest(new { message = "Jenkinsfile not found." });
            }

            var jenkinsScriptContent = await System.IO.File.ReadAllTextAsync(_jobscriptFilesPath);
            var jenkinsFileContent = await System.IO.File.ReadAllTextAsync(_jenkinsFilesPath);
            jenkinsScriptContent = jenkinsScriptContent.Replace("{{pipeline}}", jenkinsFileContent);
            var jobName = "TestJobName"; // Specify the job name here

            // Fetch the CSRF crumb
            var crumbResponse = await client.GetAsync($"{_jenkinsBaseUrl}/crumbIssuer/api/json");
            var crumbData = await crumbResponse.Content.ReadAsStringAsync();
            var crumb = Newtonsoft.Json.JsonConvert.DeserializeObject<Crumb>(crumbData).crumb;

            // Jenkins API URL to create a new job
            var jenkinsUrl = $"{_jenkinsBaseUrl}/createItem?name={jobName}"; // POST request to create job
            var content = new StringContent(jenkinsScriptContent, Encoding.UTF8, "application/xml");
            content.Headers.Add("Jenkins-Crumb", crumb);

            var response = await client.PostAsync(jenkinsUrl, content);

            if (response.IsSuccessStatusCode)
            {
                return Ok(new { message = "Jenkins job created successfully." });
            }
            else
            {
                return StatusCode((int)response.StatusCode, new { message = "Failed to create Jenkins job." });
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while creating the Jenkins job.", details = ex.Message });
        }
    }

    [HttpPost("update-job")]
    public async Task<IActionResult> UpdateJobAsync()
    {
        string _jobscriptFilesPath = "./Jobs/Jobscript.xml"; // Path to Jenkinsfile
        string _jenkinsFilesPath = "./Jobs/Jenkinsfile"; // Path to Jenkinsfile
        string _jenkinsBaseUrl = _configuration["AIOJenkins:HostName"]; // Jenkins base URL
        string _jenkinsCredentials = _configuration["AIOJenkins:Credentials"]; // Jenkins credentials in the form "username:password"

        try
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(_jenkinsCredentials)));

            // Check if the job already exists
            var jobExistsResponse = await client.GetAsync($"{_jenkinsBaseUrl}/job/TestJobName/api/json");
            if (!jobExistsResponse.IsSuccessStatusCode)
            {
                return NotFound(new { message = "Jenkins job does not exist." });
            }

            // Read the Jenkinsfile content
            if (!System.IO.File.Exists(_jobscriptFilesPath) || !System.IO.File.Exists(_jenkinsFilesPath))
            {
                return BadRequest(new { message = "Jenkinsfile not found." });
            }

            var jenkinsScriptContent = await System.IO.File.ReadAllTextAsync(_jobscriptFilesPath);
            var jenkinsFileContent = await System.IO.File.ReadAllTextAsync(_jenkinsFilesPath);
            jenkinsScriptContent = jenkinsScriptContent.Replace("{{pipeline}}", jenkinsFileContent);

            // Fetch the CSRF crumb
            var crumbResponse = await client.GetAsync($"{_jenkinsBaseUrl}/crumbIssuer/api/json");
            var crumbData = await crumbResponse.Content.ReadAsStringAsync();
            var crumb = Newtonsoft.Json.JsonConvert.DeserializeObject<Crumb>(crumbData).crumb;

            // Jenkins API URL to update the job
            var jenkinsUrl = $"{_jenkinsBaseUrl}/job/TestJobName/config.xml"; // POST request to update job
            var content = new StringContent(jenkinsScriptContent, Encoding.UTF8, "application/xml");
            content.Headers.Add("Jenkins-Crumb", crumb);

            var response = await client.PostAsync(jenkinsUrl, content);

            if (response.IsSuccessStatusCode)
            {
                return Ok(new { message = "Jenkins job updated successfully." });
            }
            else
            {
                return StatusCode((int)response.StatusCode, new { message = "Failed to update Jenkins job." });
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while updating the Jenkins job.", details = ex.Message });
        }
    }


    [HttpPost("trigger-job")]
    public async Task<IActionResult> TriggerJob()
    {
        System.Console.WriteLine("Hit the api");
        var httpClient = new HttpClient();
        try
        {
            // Read Jenkins configuration
            var jenkinsHost = _configuration["AIOJenkins:HostName"];
            var jobName = _configuration["AIOJenkins:JobName"];
            var token = _configuration["AIOJenkins:Token"];
            var credentials = _configuration["AIOJenkins:Credentials"];

            System.Console.WriteLine(jenkinsHost);
            System.Console.WriteLine(jobName);
            System.Console.WriteLine(token);
            System.Console.WriteLine(credentials);

            if (string.IsNullOrEmpty(jenkinsHost) || string.IsNullOrEmpty(jobName) || string.IsNullOrEmpty(token) || string.IsNullOrEmpty(credentials))
            {
                return BadRequest(new { Message = "Missing Jenkins configuration settings." });
            }

            // Construct the Jenkins job URL
            var jenkinsUrl = $"{jenkinsHost}/job/{jobName}/build?token={token}";

            // Encode credentials in Base64
            var base64Credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));

            // Create request
            var request = new HttpRequestMessage(HttpMethod.Post, jenkinsUrl);
            request.Headers.Add("Authorization", $"Basic {base64Credentials}");

            // Send the request
            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return Ok(new { Message = "Job triggered successfully." });
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, new { Message = "Failed to trigger job.", Details = responseContent });
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while triggering the job.", Details = ex.Message });
        }
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadAsync(IFormFileCollection files)
    {
        System.Console.WriteLine("Hit the api");
        var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedReports");
        Directory.CreateDirectory(uploadPath);

        System.Console.WriteLine("Count: " + files.Count());

        var uploadedFiles = new List<string>();
        foreach (var file in files)
        {
            System.Console.WriteLine($"File Name: {file.FileName}, Length: {file.Length}");

            var filePath = Path.Combine(uploadPath, file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            uploadedFiles.Add(filePath);
        }

        return Ok(new { Message = "Files uploaded successfully.", Files = uploadedFiles });
    }

}
