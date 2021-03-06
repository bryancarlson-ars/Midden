﻿using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Caf.Midden.Core.Services.Metadata
{
    public class MetadataParser : IMetadataParser
    {
        private readonly IMetadataConverter converter;

        public MetadataParser(IMetadataConverter converter)
        {
            this.converter = converter;
        }

        public Models.v0_1.Metadata Parse(
            string json)
        {
            string version = GetVersion(json);
            Models.v0_1.Metadata result = version switch
            {
                "v0.1.0-alpha1" or 
                "v0.1.0-alpha2" or 
                "v0.1.0-alpha3" => Deserialize_v0_1_0alpha3(json),
                "v0.1.0-alpha4" or
                "v0.1"          => Deserialize_v0_1(json),
                _ => throw new ArgumentException("Unable to parse JSON"),
            };
            return result;
        }

        private Models.v0_1.Metadata Deserialize_v0_1_0alpha3(
            string json)
        {
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                IgnoreNullValues = true,
                WriteIndented = true,
                Converters = { new JsonStringEnumConverter() },
                Encoder = System.Text.Encodings.Web
                    .JavaScriptEncoder
                    .UnsafeRelaxedJsonEscaping
            };

            Models.v0_1_0alpha3.Metadata m = 
                JsonSerializer.Deserialize<Models.v0_1_0alpha3.Metadata>(
                    json, options);
            Models.v0_1.Metadata result = 
                converter.Convert(m);

            return result;
        }
        private Models.v0_1.Metadata Deserialize_v0_1(
            string json)
        {
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                IgnoreNullValues = true,
                WriteIndented = true,
                Converters = { new JsonStringEnumConverter() },
                Encoder = System.Text.Encodings.Web
                    .JavaScriptEncoder
                    .UnsafeRelaxedJsonEscaping
            };

            Models.v0_1.Metadata m = 
                JsonSerializer.Deserialize<Models.v0_1.Metadata>(
                    json, options);
            Models.v0_1.Metadata result = 
                converter.Convert(m);

            return result;
        }

        private string GetVersion(string json)
        {
            var jsonDoc = JsonDocument.Parse(json);

            string version;
            if (jsonDoc.RootElement.TryGetProperty(
                "file", 
                out _))
            {
                version = jsonDoc
                    .RootElement
                    .GetProperty("file")
                    .GetProperty("schema-version")
                    .GetString();
            }
            else
            {
                version = jsonDoc
                    .RootElement
                    .GetProperty("schemaVersion")
                    .GetString();
            }

            return version;
        }
    }
}
