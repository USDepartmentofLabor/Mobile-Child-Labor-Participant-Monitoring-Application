using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace MDPMS.Shared.ViewModels.Helpers
{
    public static class CustomValueConverter
    {
        private static string ConcatenateValue(string value)
        {
            return @"{""value_text"":""" + value + @"""}";
        }

        private static string GetJsonValue(string json)
        {
            dynamic jsonResponseParse = JsonConvert.DeserializeObject(json);
            return jsonResponseParse.value_text;
        }

        // text = {"value_text":"text answer"}
        public static string ConvertCustomValueToJsonText(string value)
        {
            return ConcatenateValue(value);
        }

        public static string GetValueFromJsonText(string json)
        {
            return GetJsonValue(json);
        }

        // textarea = {"value_text":"textarea\r\nanswer"}
        public static string ConvertCustomValueToJsonTextArea(string value)
        {
            return ConcatenateValue(value);
        }

        public static string GetValueFromJsonTextArea(string json)
        {
            return GetJsonValue(json);
        }

        // check_box = {"value_text":["A"]}
        //             {"value_text":["A","B","C"]}
        public static string ConvertCustomValueToJsonCheckBox(List<string> selectedValues)
        {
            var sb = new StringBuilder(@"{""value_text"":[");
            var i = 0;
            foreach (var value in selectedValues)
            {
                sb.Append(@"""" + value + @"""");
                if (i != (selectedValues.Count - 1)) sb.Append(@",");
                i++;
            }
            sb.Append(@"]}");
            return sb.ToString();
        }

        public static string GetValueFromJsonCheckBox(string json)
        {
            var rtn = new StringBuilder();
            var valueArray = GetValuesFromJsonCheckBox(json);
            var i = 0;
            foreach (var value in valueArray)
            {
                rtn.Append(value);
                if (i != (valueArray.Count - 1)) rtn.Append(@", ");
                i++;
            }
            return rtn.ToString();
        }

        public static List<string> GetValuesFromJsonCheckBox(string json)
        {
            dynamic jsonResponseParse = JsonConvert.DeserializeObject(json);
            var valueArray = jsonResponseParse.value_text;
            var rtn = new List<string>();
            foreach (var value in valueArray)
            {
                rtn.Add(value.Value);
            }
            return rtn;
        }

        // radio_button = {"value_text":"Household Radio Option A"}
        public static string ConvertCustomValueToJsonRadioButton(string value)
        {
            return ConcatenateValue(value);
        }

        public static string GetValueFromJsonRadioButton(string json)
        {
            return GetJsonValue(json);
        }

        // select = {"value_text":"Household Selection Option A"}
        public static string ConvertCustomValueToJsonSelect(string value)
        {
            return ConcatenateValue(value);
        }

        public static string GetValueFromJsonSelect(string json)
        {
            return GetJsonValue(json);
        }

        // number = {"value_text":"123"}
        public static string ConvertCustomValueToJsonNumber(double value)
        {
            return ConcatenateValue(value.ToString());
        }

        public static double? GetValueFromJsonNumber(string json)
        {
            // TEMP: if no value return 0.0
            try
            {
                return double.Parse(GetJsonValue(json));
            }
            catch
            {
                return null;
            }

            //return double.Parse(GetJsonValue(json));
        }

        // date = {"value_text":{"(1i)":"1901","(2i)":"12","(3i)":"31"}}
        public static string ConvertCustomValueToJsonDate(DateTime date)
        {
            var sb = new StringBuilder(@"{""value_text"":{");
            sb.Append(@"""(1i)"":""" + date.Year + @""",");
            sb.Append(@"""(2i)"":""" + date.Month + @""",");
            sb.Append(@"""(3i)"":""" + date.Day + @"""}}");
            return sb.ToString();
        }

        public static DateTime? GetValueFromJsonDate(string json)
        {
            // TEMP: if error return today
            try
            {
                dynamic jsonResponseParse = JsonConvert.DeserializeObject(json);
                var year = 0;
                var month = 0;
                var day = 0;
                foreach (var x in jsonResponseParse.value_text)
                {
                    if (x.Name == "(1i)") year = x.Value;
                    if (x.Name == "(2i)") month = x.Value;
                    if (x.Name == "(3i)") day = x.Value;
                }
                return new DateTime(year, month, day);
            }
            catch
            {
                return null;
            }

            //dynamic jsonResponseParse = JsonConvert.DeserializeObject(json);
            //var year = 0;
            //var month = 0;
            //var day = 0;
            //foreach (var x in jsonResponseParse.value_text)
            //{
            //    if (x.Name == "(1i)") year = x.Value;
            //    if (x.Name == "(2i)") month = x.Value;
            //    if (x.Name == "(3i)") day = x.Value;
            //}
            //return new DateTime(year, month, day);
        }

        // rank_list = {"order":""}
        // {"value_text":["Household Rank Option A","Household Rank Option B","Household Rank Option C"],"order":"[2,1,0]"}
        // {"value_text":["Household Rank Option A"],"order":"[0]"}
        // {"value_text":["Household Rank Option A","Household Rank Option C"],"order":"[2,0]"}

        // Tuple<OriginalOrdering, DisplayValue>
        public static string ConvertCustomValueToJsonRankList(List<Tuple<int, string>> entries)
        {
            if (!entries.Any()) return @"{""order"":""""}";
            var sb = new StringBuilder();
            sb.Append(@"{""value_text"":[");
            var i = 0;
            foreach (var value in entries.OrderBy(a => a.Item1))
            {
                sb.Append(@"""" + value.Item2 + @"""");
                if (i != (entries.Count -1)) sb.Append(@",");
                i++;
            }
            sb.Append(@"],""order"":""[");

            i = 0;
            foreach (var value in entries.OrderBy(a => a.Item1))
            {
                sb.Append(entries.IndexOf(value).ToString());
                if (i != (entries.Count - 1)) sb.Append(@",");
                i++;
            }
            sb.Append(@"]""}");
            return sb.ToString();
        }

        public static List<Tuple<int, string>> GetValueFromJsonRankList(string json)
        {            
            dynamic jsonResponseParse = JsonConvert.DeserializeObject(json);

            // TEMP: if no value or {"order":""} then return new list
            if (jsonResponseParse.value_text == null) return new List<Tuple<int, string>>();

            var values = new List<string>();
            foreach (var value in jsonResponseParse.value_text)
            {
                values.Add(value.Value);
            }

            // [1,0,2]
            string orderValue = jsonResponseParse.order.Value;
            var orders = new List<int>();
            orderValue = orderValue.TrimStart('[');
            orderValue = orderValue.TrimEnd(']');
            var ordersRaw = orderValue.Split(',');
            for (var i = 0; i < ordersRaw.Length; i++)
            {
                orders.Add(int.Parse(ordersRaw[i]));
            }

            var rtn = new List<Tuple<int, string>>();
            foreach (var x in orders.OrderBy(a => a))
            {
                var displayValue = values[orders.IndexOf(x)];
                rtn.Add(new Tuple<int, string>(values.IndexOf(displayValue), displayValue));
            }

            return rtn;
        }

        public static string GetDisplayValueFromJsonRankList(string json)
        {
            var values = GetValueFromJsonRankList(json);
            var sb = new StringBuilder();
            var i = 0;
            foreach (var value in values)
            {
                sb.Append(value.Item2);
                if (i != (values.Count - 1)) sb.Append(@", ");
                i++;
            }
            return sb.ToString();
        }
    }
}
