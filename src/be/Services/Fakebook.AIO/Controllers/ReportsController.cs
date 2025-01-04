using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using Fakebook.AIO.Models;
using Fakebook.AIO.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fakebook.AIO.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class ReportsController : ControllerBase
{

    private readonly IConfiguration _configuration;
    private readonly ICaseService _caseService;
    public ReportsController(IConfiguration configuration, ICaseService caseService)
    {
        _configuration = configuration;
        _caseService = caseService;
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


    [HttpGet("trigger-job/{caseId}")]
    public async Task<IActionResult> TriggerJob(string caseId)
    {
        var cas = await _caseService.GetByIdAsync(caseId);

        if (cas is null) return NotFound("The case id is invalid!");

        var caseJobName = cas.JobName;
        System.Console.WriteLine("JobName: " + caseJobName);
        // "JobName": "Trigger-Test-Agent",

        System.Console.WriteLine("Hit the api");
        var httpClient = new HttpClient();
        try
        {
            // Read Jenkins configuration
            var jenkinsHost = _configuration["AIOJenkins:HostName"];
            var jobName = _configuration["AIOJenkins:JobName"];
            var token = _configuration["AIOJenkins:Token"];
            var credentials = _configuration["AIOJenkins:Credentials"];

            if (string.IsNullOrEmpty(jenkinsHost) || string.IsNullOrEmpty(jobName) || string.IsNullOrEmpty(token) || string.IsNullOrEmpty(credentials))
            {
                return BadRequest(new { Message = "Missing Jenkins configuration settings." });
            }

            // Construct the Jenkins job URL
            var jenkinsUrl = $"{jenkinsHost}/job/{jobName}/buildWithParameters?token={token}&caseId={caseId}&jobName={caseJobName}";

            System.Console.WriteLine("My url");
            System.Console.WriteLine(jenkinsUrl);

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

    [HttpPost("upload/{caseId}")]
    public async Task<IActionResult> UploadAsync(string caseId, IFormFileCollection files)
    {
        var cas = await _caseService.GetByIdAsync(caseId);

        if (cas is null) return NotFound("The case id is invalid!");

        System.Console.WriteLine("Hit the api");
        var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedReports");
        Directory.CreateDirectory(uploadPath);

        System.Console.WriteLine("Count: " + files.Count());

        var uploadedFiles = new List<string>();
        var testResultsSummary = new List<CaseResultModel>();

        foreach (var file in files)
        {
            System.Console.WriteLine($"File Name: {file.FileName}, Length: {file.Length}");

            var filePath = Path.Combine(uploadPath, file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            uploadedFiles.Add(filePath);

            // If the file is a .trx file, process it
            if (Path.GetExtension(file.FileName).Equals(".trx", StringComparison.OrdinalIgnoreCase))
            {
                var resultSummary = ParseTrxFile(filePath);
                if (resultSummary != null)
                {
                    testResultsSummary.Add(resultSummary);
                }
            }
        }

        var firstReport = testResultsSummary[0];

        cas.NumberOfSuccess += firstReport.Passed;
        cas.NumberOfFailed += firstReport.Failed;

        await _caseService.UpdateAsync(caseId, cas);

        return Ok(new { Message = "Files uploaded successfully.", Files = uploadedFiles });
    }

    private CaseResultModel ParseTrxFile(string filePath)
    {
        try
        {
            var doc = XDocument.Load(filePath);

            // Namespace to match elements in the .trx file
            XNamespace ns = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010";

            // Extract counters from the ResultSummary
            var counters = doc.Descendants(ns + "Counters").FirstOrDefault();
            if (counters != null)
            {
                int total = int.Parse(counters.Attribute("total")?.Value ?? "0");
                int passed = int.Parse(counters.Attribute("passed")?.Value ?? "0");
                int failed = int.Parse(counters.Attribute("failed")?.Value ?? "0");

                return new CaseResultModel()
                {
                    File = Path.GetFileName(filePath),
                    Total = total,
                    Passed = passed,
                    Failed = failed
                };
            }

            throw new Exception("No test results found in the file.");
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"Error parsing .trx file {filePath}: {ex.Message}");
            throw new Exception("Failed to parse the file.");
        }
    }
}
