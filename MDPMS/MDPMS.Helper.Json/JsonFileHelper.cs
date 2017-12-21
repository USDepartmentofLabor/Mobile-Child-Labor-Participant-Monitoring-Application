using Newtonsoft.Json;
using System;
using System.IO;

namespace MDPMS.Helper.Json
{
    public static class JsonFileHelper
    {
        /// <summary>
        /// Save data to JSON (JavaScript Object Notation) file
        /// </summary>
        /// <typeparam name="T">Type T</typeparam>
        /// <param name="data">.NET object</param>
        /// <param name="fullFilePath">Full file path and file name with extension</param>
        /// <exception cref="System.Exception">Exception thrown when an exception occurs</exception>
        public static void SaveDataToJsonFile<T>(T data, string fullFilePath)
        {
            var jsonString = JsonConvert.SerializeObject(data);
            var path = Path.GetDirectoryName(fullFilePath);
            
            try
            {
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            }            
            catch (Exception e)
            {
                throw new Exception(System.Reflection.MethodBase.GetCurrentMethod().Name + @" threw an exception of type " +
                    e.GetType() + @" with message '" +
                    e.Message + @"'.");
            }
            
            File.WriteAllText(fullFilePath, jsonString);            
        }

        /// <summary>
        /// Get data from JSON (JavaScript Object Notation) file
        /// </summary>
        /// <typeparam name="T">Type T</typeparam>
        /// <param name="fullFilePath">Full file path and file name with extension</param>
        /// <returns>Object of Type T</returns>
        /// <exception cref="System.IO.FileNotFoundException">Exception thrown when the specified file can not be found</exception>
        /// <exception cref="System.Exception">Exception thrown when an exception occurs</exception>
        public static T GetDataFromJsonFile<T>(string fullFilePath) where T : new()
        {
            if (!File.Exists(fullFilePath)) throw new FileNotFoundException(@"File not found at '" + fullFilePath + @"'");            
            var fileContents = File.ReadAllText(fullFilePath);

            try
            {
                var data = JsonConvert.DeserializeObject<T>(fileContents);
                return data;
            }
            catch (Exception e)
            {
                throw new Exception(System.Reflection.MethodBase.GetCurrentMethod().Name + @" threw an exception of type " +
                                    e.GetType() + @" with message '" +
                                    e.Message + @"'.");
            }            
        }

        /// <summary>
        /// Parse JSON response for "token" ex: {"token": "0123456789abcdef0123456789abcdef"}
        /// </summary>
        /// <param name="jsonResponse"></param>
        /// <returns></returns>
        public static string ParseTokenResponse(string jsonResponse)
        {
            dynamic parse = JsonConvert.DeserializeObject(jsonResponse);
            return parse.token;            
        }
    }
}
