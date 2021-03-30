using Newtonsoft.Json;

public class Qwest
{
    [JsonProperty("campus")]
    public string Campus { get; set; }

    [JsonProperty("completeBadge")]
    public string CompleteBadge { get; set; }

    [JsonProperty("difficulty")]
    public string Difficulty { get; set; }

    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("locations")]
    public Location[] Locations { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("timerBadge")]
    public string TimerBadge { get; set; }
}

public partial class Location
{
    [JsonProperty("assetImage")]
    public string AssetImage { get; set; }

    [JsonProperty("clue")]
    public string Clue { get; set; }

    [JsonProperty("latitude")]
    public float Latitude { get; set; }

    [JsonProperty("longitude")]
    public float Longitude { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }
}

