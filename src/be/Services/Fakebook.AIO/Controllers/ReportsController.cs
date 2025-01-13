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
    private readonly IPipelineService _pipelineService;
    public ReportsController(IConfiguration configuration, ICaseService caseService, IPipelineService pipelineService)
    {
        _configuration = configuration;
        _caseService = caseService;
        _pipelineService = pipelineService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var data = await _pipelineService.GetAllAsync();

        return Ok(data);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(string id)
    {
        var cas = await _pipelineService.GetByIdAsync(id);

        if (cas is null)
            return NotFound("Pipeline not found");

        return Ok(cas);
    }


    [HttpPost("create-job")]
    public async Task<IActionResult> CreateJobAsync(PipelineCreateModel input)
    {
        string _jobscriptFilesPath = "./Jobs/Jobscript.xml"; // Path to Jenkinsfile
        string _jenkinsBaseUrl = _configuration["AIOJenkins:HostName"]; // Jenkins base URL
        string _jenkinsCredentials = _configuration["AIOJenkins:Credentials"]; // Jenkins credentials in the form "username:password"

        try
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(_jenkinsCredentials)));

            // Check if the job already exists
            var jobExistsResponse = await client.GetAsync($"{_jenkinsBaseUrl}/job/{input.JobName}/api/json");
            if (jobExistsResponse.IsSuccessStatusCode)
            {
                return Conflict(new { message = "Jenkins job already exists." });
            }

            // Read the Jenkinsfile content
            if (!System.IO.File.Exists(_jobscriptFilesPath))
            {
                return BadRequest(new { message = "Jenkinsfile not found." });
            }

            var jenkinsScriptContent = await System.IO.File.ReadAllTextAsync(_jobscriptFilesPath);
            jenkinsScriptContent = jenkinsScriptContent
                                                    .Replace("{{pipeline}}", input.PipelineContent)
                                                    .Replace("{{description}}", input.JobDescription)
                                                    .Replace("{{authToken}}", input.AuthToken);


            // Fetch the CSRF crumb
            var crumbResponse = await client.GetAsync($"{_jenkinsBaseUrl}/crumbIssuer/api/json");
            var crumbData = await crumbResponse.Content.ReadAsStringAsync();
            var crumb = Newtonsoft.Json.JsonConvert.DeserializeObject<Crumb>(crumbData).crumb;

            // Jenkins API URL to create a new job
            var jenkinsUrl = $"{_jenkinsBaseUrl}/createItem?name={input.JobName}"; // POST request to create job
            var content = new StringContent(jenkinsScriptContent, Encoding.UTF8, "application/xml");
            content.Headers.Add("Jenkins-Crumb", crumb);

            var response = await client.PostAsync(jenkinsUrl, content);

            if (response.IsSuccessStatusCode)
            {
                await _pipelineService.CreateAsync(input.ToPipeline());

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

    [HttpPost("update-job/{id}")]
    public async Task<IActionResult> UpdateJobAsync(string id, PipelineCreateModel input)
    {
        string _jobscriptFilesPath = "./Jobs/Jobscript.xml"; // Path to Jenkinsfile
        string _jenkinsBaseUrl = _configuration["AIOJenkins:HostName"]; // Jenkins base URL
        string _jenkinsCredentials = _configuration["AIOJenkins:Credentials"]; // Jenkins credentials in the form "username:password"

        try
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(_jenkinsCredentials)));

            // Check if the job already exists
            var jobExistsResponse = await client.GetAsync($"{_jenkinsBaseUrl}/job/{input.JobName}/api/json");
            if (!jobExistsResponse.IsSuccessStatusCode)
            {
                return NotFound(new { message = "Jenkins job does not exist." });
            }

            // Read the Jenkinsfile content
            if (!System.IO.File.Exists(_jobscriptFilesPath))
            {
                return BadRequest(new { message = "Jenkinsfile not found." });
            }

            var jenkinsScriptContent = await System.IO.File.ReadAllTextAsync(_jobscriptFilesPath);
            jenkinsScriptContent = jenkinsScriptContent
                                                    .Replace("{{pipeline}}", input.PipelineContent)
                                                    .Replace("{{description}}", input.JobDescription)
                                                    .Replace("{{authToken}}", input.AuthToken);

            // Fetch the CSRF crumb
            var crumbResponse = await client.GetAsync($"{_jenkinsBaseUrl}/crumbIssuer/api/json");
            var crumbData = await crumbResponse.Content.ReadAsStringAsync();
            var crumb = Newtonsoft.Json.JsonConvert.DeserializeObject<Crumb>(crumbData).crumb;

            // Jenkins API URL to update the job
            var jenkinsUrl = $"{_jenkinsBaseUrl}/job/{input.JobName}/config.xml"; // POST request to update job
            var content = new StringContent(jenkinsScriptContent, Encoding.UTF8, "application/xml");
            content.Headers.Add("Jenkins-Crumb", crumb);

            var response = await client.PostAsync(jenkinsUrl, content);

            if (response.IsSuccessStatusCode)
            {
                await _pipelineService.UpdateAsync(id, input.ToPipeline());

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


    [HttpGet("trigger-job")]
    public async Task<IActionResult> TriggerJob(string caseId, string pipelineId)
    {
        var cas = await _caseService.GetByIdAsync(caseId);
        if (cas is null) return NotFound("The case id is invalid!");

        var job = await _pipelineService.GetByIdAsync(pipelineId);
        if (job is null) return NotFound("The pipeline id is invalid!");

        var caseJobName = cas.JobName;

        var httpClient = new HttpClient();
        try
        {
            // Read Jenkins configuration
            var jenkinsHost = _configuration["AIOJenkins:HostName"];
            var credentials = _configuration["AIOJenkins:Credentials"];

            if (string.IsNullOrEmpty(jenkinsHost) || string.IsNullOrEmpty(job.JobName) || string.IsNullOrEmpty(job.AuthToken) || string.IsNullOrEmpty(credentials))
            {
                return BadRequest(new { Message = "Missing Jenkins configuration settings." });
            }

            // Construct the Jenkins job URL
            var jenkinsUrl = $"{jenkinsHost}/job/{job.JobName}/buildWithParameters?token={job.AuthToken}&caseId={caseId}&jobName={caseJobName}";

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

        var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedReports");
        Directory.CreateDirectory(uploadPath);

        var uploadedFiles = new List<string>();
        var testResultsSummary = new List<CaseResultModel>();

        foreach (var file in files)
        {

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
            throw new Exception("Failed to parse the file.");
        }
    }
}
