using Fakebook.AIO.Entity;

namespace Fakebook.AIO.Models;

public class CaseCreateModel
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string JobName { get; set; } = null!;

    public Case ToCase()
    {
        return new Case()
        {
            Name = Name,
            Description = Description,
            JobName = JobName,
        };
    }
}
public class CaseUpdateModel
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string JobName { get; set; } = null!;

    public Case ToCase()
    {
        return new Case()
        {
            Name = Name,
            Description = Description,
            JobName = JobName,
        };
    }
}

public class CaseResultModel
{
    public string File { get; set; } = null!;
    public int Total { get; set; }
    public int Passed { get; set; }
    public int Failed { get; set; }
}