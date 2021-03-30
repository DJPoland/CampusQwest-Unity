using Newtonsoft.Json;

// Some of these fields should be required or else there's an error going on. 
// Looks like we may need to import another package for this guarantee.
public class Leaderboard 
{
    [JsonProperty("qwestId")]
    public long QwestId { get; set; }
    
    [JsonProperty("totalTime")] 
    public string TotalTime { get; set; }
    
    [JsonProperty("username")] 
    public string Username { get; set; }
    
    [JsonProperty("selectedAvatar")]
    public string SelectedAvatar { get; set; }
    
}
