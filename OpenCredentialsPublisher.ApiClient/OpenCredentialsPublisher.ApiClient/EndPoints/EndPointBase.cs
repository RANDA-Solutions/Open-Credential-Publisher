﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OpenCredentialsPublisher.ApiClient.EndPoints
{
    static class EndPointBase
    {
        #region Statics
        public static async Task<T> ConnectJson<T>(string EndPoint, object RequestVM) where T : new() {
            var c = new HttpClient() {
                BaseAddress = Runtime.ApiBaseUri
            };

            c.DefaultRequestHeaders
                .Accept
                .Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var content = new StringContent(JsonConvert.SerializeObject(RequestVM), Encoding.UTF8, "application/json");

            var result = await c.PostAsync(EndPoint, content);

            if (result.IsSuccessStatusCode) {
                string data = await result.Content.ReadAsStringAsync();
                T vm = JsonConvert.DeserializeObject<T>(data);
                return vm;
            } else {
                throw new Exception($"Unable to connect. {result.StatusCode}: {result.ReasonPhrase}");
            }
        }

        public static async Task<T> ConnectForm<T>(string EndPoint, Dictionary<string,string> FormData) where T : new() {
            var c = new HttpClient() {
                BaseAddress = Runtime.ApiBaseUri
            };

            var content = new FormUrlEncodedContent(FormData);

            var result = await c.PostAsync(EndPoint, content);

            if (result.IsSuccessStatusCode) {
                string data = await result.Content.ReadAsStringAsync();
                T vm = JsonConvert.DeserializeObject<T>(data);
                return vm;
            } else {
                throw new Exception($"Unable to connect. {result.StatusCode}: {result.ReasonPhrase}");
            }
        }
        #endregion
    }
}