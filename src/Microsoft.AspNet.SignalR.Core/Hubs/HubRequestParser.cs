﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.md in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.AspNet.SignalR.Hubs
{
    internal class HubRequestParser : IHubRequestParser
    {
        private static readonly IJsonValue[] _emptyArgs = new IJsonValue[0];

        public HubRequest Parse(string data)
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new SipHashBasedDictionaryConverter());
            var serializer = new JsonNetSerializer(settings);
            var deserializedData = serializer.Parse<HubInvocation>(data);

            var request = new HubRequest();

            request.Hub = deserializedData.Hub;
            request.Method = deserializedData.Method;
            request.Id = deserializedData.Id;
            request.State = deserializedData.State ?? new Dictionary<string, object>();
            request.ParameterValues = (deserializedData.Args != null) ? deserializedData.Args.Select(value => new JRawValue(value)).ToArray() : _emptyArgs;

            return request;
        }

        private class HubInvocation
        {
            [JsonProperty("H")]
            public string Hub { get; set; }
            [JsonProperty("M")]
            public string Method { get; set; }
            [JsonProperty("I")]
            public string Id { get; set; }
            [JsonProperty("S")]
            public IDictionary<string, object> State { get; set; }
            [JsonProperty("A")]
            public JRaw[] Args { get; set; }
        }
    }
}
