using System.Threading.Tasks;
using AdventCode.Utils;
using Microsoft.Extensions.Logging;
using AdventCode.Tasks;

namespace AdventCode.Tasks2021;

public class Day6Task : BaseCodeTask, IAdventCodeTask
{
    public override int TaskDay => 6;
    private readonly ILogger<Day6Task> _logger;
    #region TestData
    protected override string TestData => @"";
    #endregion

    public Day6Task(IAdventWebClient client, ILogger<Day6Task> logger) : base(client)
    {
        _logger = logger;
    }

    public override async Task<string?> GetFirstTaskAnswerAsync()
    {
        _ = await GetDataAsListAsync<string>();
        throw new TaskIncompleteException();
    }

    public override async Task<string?> GetSecondTaskAnswerAsync()
    {
        _ = await GetDataAsListAsync<string>();
        throw new TaskIncompleteException();
    }
}