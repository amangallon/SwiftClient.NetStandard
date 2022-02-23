﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SwiftClient.Extensions;

namespace SwiftClient
{
    public partial class Client : ISwiftClient, IDisposable
    {

        public SwiftCredentials GetCredentials()
        {
            return RetryManager.AuthManager.Credentials;
        }

        public Task<SwiftAccountResponse> HeadAccountAsync()
        {
            return AuthorizeAndExecute(async (auth) =>
            {
                var url = SwiftUrlBuilder.GetAccountUrl(auth.StorageUrl);

                var request = new HttpRequestMessage(HttpMethod.Head, url);

                FillRequest(request, auth);

                try
                {
                    using (var response = await _client.SendAsync(request).ConfigureAwait(false))
                    {
                        var result = GetResponse<SwiftAccountResponse>(response);

                        if (response.IsSuccessStatusCode)
                        {
                            long totalBytes, containersCount, objectsCount;

                            if (long.TryParse(response.GetHeader(SwiftHeaderKeys.AccountBytesUsed), out totalBytes))
                            {
                                result.TotalBytes = totalBytes;
                            }

                            if (long.TryParse(response.GetHeader(SwiftHeaderKeys.AccountContainerCount), out containersCount))
                            {
                                result.ContainersCount = containersCount;
                            }

                            if (long.TryParse(response.GetHeader(SwiftHeaderKeys.AccountObjectCount), out objectsCount))
                            {
                                result.ObjectsCount = objectsCount;
                            }
                        }

                        return result;
                    }
                }
                catch (Exception ex)
                {
                    return GetExceptionResponse<SwiftAccountResponse>(ex, auth.StorageUrl);
                }
            });
        }

        public Task<SwiftAccountResponse> GetAccountAsync(Dictionary<string, string> queryParams = null)
        {
            return AuthorizeAndExecute(async (auth) =>
            {
                if (queryParams == null)
                {
                    queryParams = new Dictionary<string, string>();
                }

                queryParams["format"] = "json";

                var url = SwiftUrlBuilder.GetAccountUrl(auth.StorageUrl, queryParams);

                var request = new HttpRequestMessage(HttpMethod.Get, url);

                FillRequest(request, auth);

                try
                {
                    using (var response = await _client.SendAsync(request).ConfigureAwait(false))
                    {
                        var result = GetResponse<SwiftAccountResponse>(response);

                        if (response.IsSuccessStatusCode)
                        {
                            long totalBytes, containersCount, objectsCount;

                            if (long.TryParse(response.GetHeader(SwiftHeaderKeys.AccountBytesUsed), out totalBytes))
                            {
                                result.TotalBytes = totalBytes;
                            }

                            if (long.TryParse(response.GetHeader(SwiftHeaderKeys.AccountContainerCount), out containersCount))
                            {
                                result.ContainersCount = containersCount;
                            }

                            if (long.TryParse(response.GetHeader(SwiftHeaderKeys.AccountObjectCount), out objectsCount))
                            {
                                result.ObjectsCount = objectsCount;
                            }

                            var info = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                            if (!string.IsNullOrEmpty(info))
                            {
                                result.Containers = JsonConvert.DeserializeObject<List<SwiftContainerModel>>(info);
                            }
                        }

                        return result;
                    }
                }
                catch (Exception ex)
                {
                    return GetExceptionResponse<SwiftAccountResponse>(ex, auth.StorageUrl);
                }
            });
        }

        public Task<SwiftResponse> PostAccountAsync(Dictionary<string, string> headers = null)
        {
            return AuthorizeAndExecute(async (auth) =>
            {
                var url = SwiftUrlBuilder.GetAccountUrl(auth.StorageUrl);

                var request = new HttpRequestMessage(HttpMethod.Post, url);

                FillRequest(request, auth, headers);

                try
                {
                    using (var response = await _client.SendAsync(request).ConfigureAwait(false))
                    {
                        return GetResponse<SwiftResponse>(response);
                    }
                }
                catch (Exception ex)
                {
                    return GetExceptionResponse<SwiftResponse>(ex, auth.StorageUrl);
                }
            });
        }

    }
}
