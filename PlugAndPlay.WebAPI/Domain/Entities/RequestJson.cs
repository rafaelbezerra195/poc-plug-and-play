using System.Text.Json;
using Newtonsoft.Json;

namespace PlugAndPlay.WebAPI.Domain.Entities;

public class RequestJson
{
    [JsonProperty("requestOrigin")]
    public string requestOrigin { get; set; }

    [JsonProperty("requestType")]
    public string requestType { get; set; }

    [JsonProperty("requestKey")]
    public string requestKey { get; set; }

    [JsonProperty("requestRequester")]
    public string requestRequester { get; set; }

    [JsonProperty("requestStatus")]
    public string requestStatus { get; set; }
    
    [JsonProperty("requestDate")]
    public DateTime requestDate { get; set; }


    [JsonProperty("approvers")]
    public List<object> approvers { get; set; }

    [JsonProperty("attachments")]
    public List<object> attachments { get; set; }

    [JsonProperty("fieldGroups")]
    public List<FieldGroup> fieldGroups { get; set; }
}

public class FieldGroup
{
    [JsonProperty("fieldGroupName")]
    public string fieldGroupName { get; set; }
    
    [JsonProperty("fields")]
    public List<JsonElement> records { get; set; }
}