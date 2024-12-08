using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Nels.SemanticKernel.Text
{
    public class SseData
    {
        public SseData(string eventType, string data, string id = null)
        {
            Event = eventType;
            Data = [data];
            Id = id;
        }
        public SseData(string eventType, IList<string> data, string id = null)
        {
            Event = eventType;
            Data = data;
            Id = id;
        }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("event")]
        public string Event { get; set; }

        [JsonPropertyName("data")]
        public IList<string> Data { get; set; }

        [JsonPropertyName("retry")]
        public int? Retry { get; set; }

        public byte[] ToBytes()
        {
            var stringData = new StringBuilder();

            // 添加 id 字段
            if (!string.IsNullOrEmpty(Id?.Trim()))
            {
                stringData.AppendLine($"id:{Id?.Trim()}");
            }

            // 添加 event 字段
            stringData.AppendLine($"event:{Event}");

            // 添加 data 字段
            foreach (var item in Data)
            {
                stringData.AppendLine($"data:{item}");
            }

            // 每条消息以两个换行符结尾
            stringData.AppendLine();

            var bytes = Encoding.UTF8.GetBytes(stringData.ToString());
            return bytes;
        }
    }
}
