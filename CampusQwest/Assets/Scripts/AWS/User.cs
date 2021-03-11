using System.Collections.Generic;
using Newtonsoft.Json;

// Some of these fields should be required or else there's an error going on. 
// Looks like we may need to import another package for this guarantee.
public class User
{
    [JsonProperty("badges")]
    public string[] Badges { get; set; }

    [JsonProperty("campus")]
    public string Campus { get; set; }

    [JsonProperty("currentQwest")]
    public CurrentQwest CurrentQwest { get; set; }

    [JsonProperty("exp")]
    public long Exp { get; set; }

    [JsonProperty("id")]
    private string id { get; set; }

    [JsonProperty("qwestsCompleted")]
    public QwestsCompleted[] QwestsCompleted { get; set; }

    [JsonProperty("rank")]
    public long Rank { get; set; }

    [JsonProperty("selectedAvatar")]
    public long SelectedAvatar { get; set; }

    [JsonProperty("selectedBanner")]
    public long SelectedBanner { get; set; }

    [JsonProperty("trophies")]
    public object[] Trophies { get; set; }

    [JsonProperty("username")]
    public string Username { get; set; }
}

public partial class CurrentQwest
{
    [JsonProperty("locationIndex")]
    public long LocationIndex { get; set; }

    [JsonProperty("qwestId")]
    public long QwestId { get; set; }

    [JsonProperty("timeStarted")]
    public string TimeStarted { get; set; }
}

public partial class QwestsCompleted
{
    [JsonProperty("qwestId")]
    public long QwestId { get; set; }

    [JsonProperty("totalTime")]
    public string TotalTime { get; set; }
}
