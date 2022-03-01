using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace SwiftClient.AspNetCore
{
    public class SwiftService : Client, ISwiftService
    {
        public SwiftServiceOptions Options { get; private set; }

        public SwiftService(IOptions<SwiftServiceOptions> options,
            ISwiftAuthManager authManager,
            IHttpClientFactory httpClientFactory,
            ISwiftLogger logger,
            string httpClientName = "swift") : base(authManager, logger)
        {
            Options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            SetRetryCount(Options.RetryCount);
            SetHttpClient(httpClientFactory, httpClientName, Options.NoHttpDispose);
            SetRetryPerEndpointCount(Options.RetryPerEndpointCount);
        }

        public Task<SwiftResponse> DeleteDefaultContainerObjectsAsync(IEnumerable<string> objectIds)
            => DeleteObjectsAsync(Options.DefaultContainer, objectIds);

        public Task<SwiftResponse> DeleteObjectAsync(string objectId, Dictionary<string, string> queryParams = null)
            => DeleteObjectAsync(Options.DefaultContainer, objectId, queryParams);

        public Task<SwiftResponse> DeleteObjectChunkAsync(string objectId, int segment)
            => DeleteObjectChunkAsync(Options.DefaultContainer, objectId, segment);

        public Task<SwiftResponse> GetObjectAsync(string objectId, Dictionary<string, string> headers = null, Dictionary<string, string> queryParams = null)
            => GetObjectAsync(Options.DefaultContainer, objectId, headers, queryParams);

        public Task<SwiftResponse> GetObjectRangeAsync(string objectId, long start, long end, Dictionary<string, string> headers = null, Dictionary<string, string> queryParams = null)
            => GetObjectRangeAsync(Options.DefaultContainer, objectId, start, end, headers, queryParams);

        public Task<SwiftResponse> HeadObjectAsync(string objectId, Dictionary<string, string> headers = null, Dictionary<string, string> queryParams = null)
            => HeadObjectAsync(Options.DefaultContainer, objectId, headers, queryParams);

        public Task<SwiftResponse> PostObjectAsync(string objectId, Dictionary<string, string> headers = null)
            => PostObjectAsync(Options.DefaultContainer, objectId, headers);

        public Task<SwiftResponse> PutManifestAsync(string objectId, Dictionary<string, string> headers = null, Dictionary<string, string> queryParams = null)
            => PutManifestAsync(Options.DefaultContainer, objectId, headers, queryParams);

        public Task<SwiftResponse> PutObjectAsync(string objectId, Stream data, Dictionary<string, string> headers = null, Dictionary<string, string> queryParams = null)
            => PutObjectAsync(Options.DefaultContainer, objectId, data, headers, queryParams);

        public Task<SwiftResponse> PutObjectAsync(string objectId, byte[] data, Dictionary<string, string> headers = null, Dictionary<string, string> queryParams = null)
            => PutObjectAsync(Options.DefaultContainer, objectId, data, headers, queryParams);

        public Task<SwiftResponse> PutObjectChunkAsync(string objectId, byte[] data, int segment, Dictionary<string, string> headers = null, Dictionary<string, string> queryParams = null)
            => PutObjectChunkAsync(Options.DefaultContainer, objectId, data, segment, headers, queryParams);

        public Task<SwiftResponse> PutPseudoDirectoryAsync(string objectId, Dictionary<string, string> headers = null, Dictionary<string, string> queryParams = null)
            => PutPseudoDirectoryAsync(Options.DefaultContainer, objectId, headers, queryParams);
    }
}
