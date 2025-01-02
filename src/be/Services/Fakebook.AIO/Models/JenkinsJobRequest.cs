namespace Fakebook.AIO.Models;

public class JenkinsJobRequest
{
    public string JobName { get; set; } = null!;
    public string JenkinsfileContent { get; set; } = null!;
}
public class Crumb
{
    public string _class { get; set; }
    public string crumb { get; set; }
    public string crumbRequestField { get; set; }
}
