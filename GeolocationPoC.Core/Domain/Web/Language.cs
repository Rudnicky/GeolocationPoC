using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace GeolocationPoC.Core.Domain.Web
{
    public class Language
    {
        [Key]
        public int Id { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("native")]
        public string Native { get; set; }
    }
}
