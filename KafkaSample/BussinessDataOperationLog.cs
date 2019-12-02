using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace KafkaSample
{
    public class BussinessDataOperationLog
    {
        [JsonProperty("optUser")]
        public string OptUser { get; set; }

        [JsonProperty("appId")]
        public string AppId { get; set; }

        [JsonProperty("appName")]
        public string AppName { get; set; }

        [JsonProperty("function")]
        public string Function { get; set; }

        [JsonProperty("beforeModifyData")]
        public string BeforeModifyData{ get; set; }

        [JsonProperty("afterModifyData")]
        public string AfterModifyData { get; set; }

        [JsonProperty("optTime")]
        public string OptTime { get; set; }
    }
}
