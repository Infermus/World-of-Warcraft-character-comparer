﻿using System;
using System.Collections.Generic;
using WowCharComparerWebApp.Enums;
using WowCharComparerWebApp.Models.Servers;

namespace WowCharComparerWebApp.Data.Helpers
{
    internal static class RequestLinkFormater
    {
        internal static Uri GenerateAPIRequestLink(BlizzardAPIProfiles profile, RequestLocalization requestLocalization, List<KeyValuePair<string, string>> fields, string endPointPart1 = null, string endPointPart2 = null)
        {
            string processingUriAddress = string.Empty;

            processingUriAddress = processingUriAddress.AddToEndPointSampleToUrl(requestLocalization.CoreRegionUrlAddress);

            processingUriAddress = processingUriAddress.AddToEndPointSampleToUrl(profile.ToString().ToLower());

            processingUriAddress = processingUriAddress.AddToEndPointSampleToUrl(endPointPart1 == null ? string.Empty : endPointPart1.ToLower());
            processingUriAddress = processingUriAddress.AddToEndPointSampleToUrl(endPointPart2 == null ? string.Empty : endPointPart2.ToLower());
            processingUriAddress = processingUriAddress.EndsWith("/") ? processingUriAddress.Remove(processingUriAddress.Length - 1, 1) : processingUriAddress;

            if (requestLocalization.Realm != null)
            {
                fields.Add(new KeyValuePair<string, string>("?locale", requestLocalization.Realm.Locale));
            }

            foreach (KeyValuePair<string, string> parameter in fields)
            {
                processingUriAddress = processingUriAddress.AddParameterToUrl(parameter.Key + "=" + parameter.Value);
            }

            processingUriAddress = processingUriAddress.EndsWith("&") ? processingUriAddress.Remove(processingUriAddress.Length - 1, 1) : processingUriAddress;

            return new Uri(processingUriAddress);
        }

        internal static Uri GenerateRaiderIOApiRequestLink(List<KeyValuePair<string, string>> fields, List<KeyValuePair<string,string>> parameters)
        {
            string processingUriAddress = string.Empty;

            processingUriAddress = processingUriAddress.AddQuestionMarkToUrl(Configuration.APIConf.RaiderIOAdress);

            foreach (KeyValuePair<string, string> parameter in parameters)
            {
                processingUriAddress = processingUriAddress.AddParameterToUrl(parameter.Key + "=" + parameter.Value).ToLower();
            }

            foreach (KeyValuePair<string, string> field in fields)
            {
                processingUriAddress = processingUriAddress.AddParameterToUrl(field.Key + "=" + field.Value);
                processingUriAddress = processingUriAddress.EndsWith("&") ? processingUriAddress.Remove(processingUriAddress.Length - 1, 1) // remove join parameters symbol at ending field
                                                           : processingUriAddress;
            }

            return new Uri(processingUriAddress);
        }

        private static string AddToEndPointSampleToUrl(this string baseText, string textToAdd)
        {
            string localText = baseText;

            if (String.IsNullOrEmpty(textToAdd) == false)
            {
                localText = baseText + textToAdd + "/";
            }

            return localText;
        }

        private static string AddParameterToUrl(this string baseText, string textToAdd)
        {
            string localText = baseText;

            if (String.IsNullOrEmpty(textToAdd) == false)
            {
                localText = baseText + textToAdd + "&";
            }

            return localText;
        }

        internal static string AddFieldToUrl(this string baseText, string textToAdd)
        {
            string localText = baseText;

            if (String.IsNullOrEmpty(textToAdd) == false)
            {
                localText = baseText + textToAdd + "%2C+";
            }

            return localText;
        }

        internal static string AddQuestionMarkToUrl(this string baseText, string textToAdd)
        {
            string localText = baseText;

            if (String.IsNullOrEmpty(textToAdd) == false)
            {
                localText = baseText + textToAdd + "?";
            }

            return localText;
        }
    }
}
