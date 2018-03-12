using System.IO;
using System.Text;
using MDPMS.Database.Data.Models.Base;
using Newtonsoft.Json;

namespace MDPMS.Database.Data.Models
{
    public class CustomPersonValue : EfBaseModel, IObjectToJsonConvertible<CustomPersonValue>
    {
        public CustomField CustomField { get; set; }
        public string Value { get; set; }
        public Person Person { get; set; }

        public string GetJsonFromObject()
        {
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.None;
                writer.WriteStartObject();
                writer.WritePropertyName(@"custom_value");
                writer.WriteStartObject();
                writer.WritePropertyName("custom_field_id");
                writer.WriteValue(CustomField.ExternalId);
                writer.WritePropertyName("value_text");
                writer.WriteValue(Value);
                writer.WritePropertyName("model_id");
                writer.WriteValue(Person.ExternalId);
                writer.WriteEndObject();
                writer.WriteEndObject();
            }
            return sw.ToString();
        }
    }
}
