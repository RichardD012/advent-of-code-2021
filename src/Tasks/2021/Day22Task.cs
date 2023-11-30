using System.Text.RegularExpressions;

namespace AdventCode.Tasks2021;


public class Day22Task : BaseCodeTask, IAdventCodeTask
{
    public override int TaskDay => 22;
    private readonly ILogger<Day22Task> _logger;
    #region TestData
    protected override string TestData => @"";
    #endregion

    public Day22Task(IAdventWebClient client, ILogger<Day22Task> logger) : base(client)
    {
        _logger = logger;
    }

    public override async Task<string?> GetFirstTaskAnswerAsync()
    {
        var data = await GetDataAsync();
        return ActiveCubesInRange(data, 50).ToString();
    }

    public override async Task<string?> GetSecondTaskAnswerAsync()
    {
        var data = await GetDataAsync();
        return ActiveCubesInRange(data, int.MaxValue).ToString();
    }
    public long ActiveCubesInRange(string input, int range)
    {
        var cmds = Parse(input);

        // Recursive approach

        // If we can determine the number of active cubes in subregions
        // we can compute the effect of the i-th cmd as well:
        long activeCubesAfterIcmd(int icmd, Region region)
        {

            if (region.IsEmpty || icmd < 0)
            {
                return 0; // empty is empty
            }
            else
            {
                var intersection = region.Intersect(cmds[icmd].region);
                var activeInRegion = activeCubesAfterIcmd(icmd - 1, region);
                var activeInIntersection = activeCubesAfterIcmd(icmd - 1, intersection);
                var activeOutsideIntersection = activeInRegion - activeInIntersection;

                // outside the intersection is unaffected, the rest is either on or off:
                return cmds[icmd].turnOff ? activeOutsideIntersection : activeOutsideIntersection + intersection.Volume;
            }
        }

        return activeCubesAfterIcmd(
            cmds.Length - 1,
            new Region(
                new Segment(-range, range),
                new Segment(-range, range),
                new Segment(-range, range)));
    }

    Cmd[] Parse(string input)
    {
        var res = new List<Cmd>();
        foreach (var line in input.Split("\n"))
        {
            var turnOff = line.StartsWith("off");
            // get all the numbers with a regexp:
            var m = Regex.Matches(line, "-?[0-9]+").Select(m => int.Parse(m.Value)).ToArray();
            res.Add(new Cmd(turnOff, new Region(new Segment(m[0], m[1]), new Segment(m[2], m[3]), new Segment(m[4], m[5]))));
        }
        return res.ToArray();
    }

    record Cmd(bool turnOff, Region region);

    record Segment(int from, int to)
    {
        public bool IsEmpty => from > to;
        public long Length => IsEmpty ? 0 : to - from + 1;

        public Segment Intersect(Segment that) =>
            new Segment(Math.Max(this.from, that.from), Math.Min(this.to, that.to));
    }

    record Region(Segment x, Segment y, Segment z)
    {
        public bool IsEmpty => x.IsEmpty || y.IsEmpty || z.IsEmpty;
        public long Volume => x.Length * y.Length * z.Length;

        public Region Intersect(Region that) =>
            new Region(this.x.Intersect(that.x), this.y.Intersect(that.y), this.z.Intersect(that.z));
    }
}
