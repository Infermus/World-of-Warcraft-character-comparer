﻿using System;
using System.Collections.Generic;
using System.IO;
using WowCharComparerWebApp.Models.Statistics;

namespace WowCharComparerWebApp.Data.Helpers
{
   public static class JsonProcessing
    {
        public static T GetDataFromJsonFile<T>(string fileName) where T: class
        {
            T parsedResult = (T)Activator.CreateInstance(typeof(T));

            try
            {
                using (StreamReader reader = new StreamReader(Directory.GetCurrentDirectory() + fileName))
                {
                    string json = reader.ReadLine();
                    parsedResult = ResponseResultFormater.DeserializeJsonData<T>(json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format(" {0}. Message: {1}", "Invalid type of model to parse from file", ex.Message));
            }

            return parsedResult;
        }
    }
}