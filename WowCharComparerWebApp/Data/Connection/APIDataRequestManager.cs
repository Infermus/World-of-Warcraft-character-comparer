﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WowCharComparerWebApp.Configuration;
using WowCharComparerWebApp.Data.Database;
using WowCharComparerWebApp.Data.Database.Repository.BlizzardApiClient;
using WowCharComparerWebApp.Data.Helpers;
using WowCharComparerWebApp.Models.Abstract;
using WowCharComparerWebApp.Models.APIResponse;
using WowCharComparerWebApp.Models.Internal;

namespace WowCharComparerWebApp.Data.Connection
{
    internal class APIDataRequestManager : IAPIDataRequestManager
    {
        private readonly ComparerDatabaseContext _comparerDatabaseContext;

        public APIDataRequestManager(ComparerDatabaseContext comparerDatabaseContext)
        {
            _comparerDatabaseContext = comparerDatabaseContext;
        }

        public async Task<T> GetDataByHttpRequest<T>(Uri uriAddress) where T : CommonAPIResponse, new()
        {
            OAuth2Token token = null;
            CommonAPIResponse apiResponse = new T();

            try
            {
                if (apiResponse is BlizzardAPIResponse)
                {
                    APIClient apiClient = new DbAccessApiAuthorization(_comparerDatabaseContext).GetClientInformation(APIConf.BlizzardAPIWowMainClientID);
                    token = GetBearerAuthenticationToken(apiClient);
                }

                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = token == null ? default(AuthenticationHeaderValue)
                                                                                   : new AuthenticationHeaderValue(token.TokenType, token.TokenValue);

                    apiResponse.Data = await httpClient.GetStringAsync(uriAddress.AbsoluteUri);
                }
            }
            catch (Exception ex)
            {
                apiResponse = Activator.CreateInstance<T>();
                apiResponse.Data = string.Empty;
                apiResponse.Exception = ex;
            }

            return (T)apiResponse;
        }

        private OAuth2Token GetBearerAuthenticationToken(APIClient apiClient)
        {
            OAuth2Token receivedOAuth2Token = new OAuth2Token();

            FormUrlEncodedContent requestContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>(APIConf.BlizzardAPIWowClientIDParameter, apiClient.ClientID),
                new KeyValuePair<string, string>(APIConf.BlizzardAPIWowClientSecretParameter, apiClient.ClientSecret),
                new KeyValuePair<string, string>(APIConf.BlizzardAPIWowGrantTypeParameter, APIConf.BlizzardAPIWowClientCredentialsParameter)
            });

            using (HttpClient httpClient = new HttpClient())
            {
                var response = httpClient.PostAsync(APIConf.BlizzardAPIWowOAuthTokenAdress, requestContent);
                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    var responseResult = response.Result.Content.ReadAsStringAsync().Result;
                    receivedOAuth2Token = JsonProcessing.DeserializeJsonData<OAuth2Token>(responseResult);
                }
                else
                {
                    throw new Exception($"{InternalMessages.BearerAuthFailed }. {response.Result.ReasonPhrase}");
                }
            }

            return receivedOAuth2Token;
        }
    }
}