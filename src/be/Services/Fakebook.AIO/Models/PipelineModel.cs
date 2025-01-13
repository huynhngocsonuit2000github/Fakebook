using Fakebook.AIO.Entity;

namespace Fakebook.AIO.Models;

public class PipelineCreateModel
{
    public string JobName { get; set; } = null!;
    public string JobDescription { get; set; } = null!;
    public string AuthToken { get; set; } = null!;
    public string PipelineContent { get; set; } = null!;

    public Pipeline ToPipeline()
    {
        return new Pipeline()
        {
            JobName = JobName,
            JobDescription = JobDescription,
            AuthToken = AuthToken,
            PipelineContent = PipelineContent,
        };
    }
}
public class PipelineUpdateModel
{
    public string JobName { get; set; } = null!;
    public string JobDescription { get; set; } = null!;
    public string AuthToken { get; set; } = null!;
    public string PipelineContent { get; set; } = null!;

    public Pipeline ToPipeline()
    {
        return new Pipeline()
        {
            JobName = JobName,
            JobDescription = JobDescription,
            AuthToken = AuthToken,
            PipelineContent = PipelineContent,
        };
    }
}