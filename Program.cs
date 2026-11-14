using System.Text.Json;

try
{
    if (args.Length != 1) throw new ArgumentException("Pass one JSON input file");
    using var data = JsonDocument.Parse(File.ReadAllText(args[0]));
    Console.WriteLine(JsonSerializer.Serialize(Analyze(data.RootElement)));
    return 0;
}
catch (Exception error) when (error is IOException or JsonException or FormatException
                               or ArgumentException or InvalidOperationException
                               or KeyNotFoundException or OverflowException)
{
    Console.Error.WriteLine(error.Message);
    return 2;
}

static object Analyze(JsonElement input)
{
    var desks=input.GetProperty("desks").GetInt32();
    if(desks<1||desks>10000) throw new ArgumentException("Use 1 to 10000 desks");
    var free=new PriorityQueue<int,(decimal time,int desk)>();
    for(var i=0;i<desks;i++) free.Enqueue(i,(0,i));
    decimal previous=0,maxWait=0,sumWait=0,lastFinish=0;
    var log=new List<object>();
    foreach(var job in input.GetProperty("jobs").EnumerateArray())
    {
        if(job.GetArrayLength()!=2) throw new ArgumentException("Expected arrival and duration");
        var arrival=job[0].GetDecimal();
        var duration=job[1].GetDecimal();
        if(arrival<previous||duration<=0) throw new ArgumentException("Expected ordered arrivals and positive durations");
        previous=arrival;
        free.TryDequeue(out var desk,out var available);
        var start=Math.Max(available.time,arrival);
        var wait=start-arrival;
        var finish=start+duration;
        maxWait=Math.Max(maxWait,wait);
        lastFinish=Math.Max(lastFinish,finish);
        sumWait+=wait;
        free.Enqueue(desk,(finish,desk));
        log.Add(new { desk=desk+1,arrival,start,finish,wait });
    }
    return new { maxWait,meanWait=log.Count==0?0:sumWait/log.Count,lastFinish,log };
}
