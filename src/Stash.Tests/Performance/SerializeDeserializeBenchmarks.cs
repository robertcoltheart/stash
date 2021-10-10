using System;
using System.Buffers;
using System.Formats.Cbor;
using System.IO;
using System.Text;
using System.Text.Json;
using AutoBogus;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using Bogus;
using Dahomey.Cbor;
using Dahomey.Cbor.Util;
using MessagePack;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace Stash.Tests.Performance
{
    [SimpleJob(RuntimeMoniker.Net50)]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    public class SerializeDeserializeBenchmarks
    {
        private const string Serialize = nameof(Serialize);

        private const string Deserialize = nameof(Deserialize);

        private JsonSerializerOptions options;

        private CborWriter cborWriter;

        private CborOptions cborOptions;

        private ManyTypesModel model;

        private ArrayBufferWriter<byte> buffer;

        private Memory<byte> memory;

        private string json;

        private byte[] msgpack;

        private byte[] bson;

        private byte[] cbor;

        private UTF8Encoding encoding = new UTF8Encoding(false, true);

        private LiteDB.BsonDocument document;

        [GlobalSetup]
        public void Setup()
        {
            Randomizer.Seed = new Random(56178921);

            options = new JsonSerializerOptions();
            model = AutoFaker.Generate<ManyTypesModel>();
            cborWriter = new CborWriter();
            cborOptions = new CborOptions();
            buffer = new ArrayBufferWriter<byte>(1024 * 16);

            json = JsonSerializer.Serialize(model);
            msgpack = MessagePackSerializer.Serialize(model);
            bson = model.ToBson();
            var stream = new MemoryStream();
            Cbor.SerializeAsync(model, stream).Wait();
            cbor = stream.ToArray();
            document = new LiteDB.BsonDocument
            {
                ["StringValue"] = model.StringValue
            };

            memory = buffer.GetMemory(1024);
        }

        //[Benchmark]
        //public byte[] Utf8Encoding()
        //{
        //    return encoding.GetBytes(model.StringValue);
        //}

        //[Benchmark]
        //public byte[] Unsafe()
        //{
        //    var byteSpan = MemoryMarshal.AsBytes(model.StringValue.AsSpan());

        //    JsonWriterHelper.ToUtf8(byteSpan, memory.Span, out var consumed, out var written);

        //    return cbor;
        //}

        //[Benchmark]
        //public byte[] Optimized()
        //{
        //    encoding.GetBytes(model.StringValue, memory.Span);

        //    return cbor;
        //}

        [Benchmark(Baseline = true)]
        [BenchmarkCategory(Serialize)]
        public string SerializeJson()
        {
            return JsonSerializer.Serialize(model, options);
        }

        [Benchmark]
        [BenchmarkCategory(Serialize)]
        public byte[] SerializeBson()
        {
            return model.ToBson();
        }

        [Benchmark]
        [BenchmarkCategory(Serialize)]
        public byte[] SerializeLiteDb()
        {
            return LiteDB.BsonSerializer.Serialize(document);
        }

        [Benchmark]
        [BenchmarkCategory(Serialize)]
        public byte[] SerializeMsgPack()
        {
            return MessagePackSerializer.Serialize(model);
        }

        [Benchmark]
        [BenchmarkCategory(Serialize)]
        public Span<byte> SerializeCbor()
        {
            using (var writer = new ByteBufferWriter())
            {
                Cbor.Serialize(model, writer, cborOptions);

                return writer.GetSpan();
            }
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory(Deserialize)]
        public ManyTypesModel DeserializeJson()
        {
            return JsonSerializer.Deserialize<ManyTypesModel>(json, options);
        }

        [Benchmark]
        [BenchmarkCategory(Deserialize)]
        public ManyTypesModel DeserializeBson()
        {
            return BsonSerializer.Deserialize<ManyTypesModel>(bson);
        }

        [Benchmark]
        [BenchmarkCategory(Deserialize)]
        public ManyTypesModel DeserializeLiteDb()
        {
            var d = LiteDB.BsonSerializer.Deserialize(bson);

            return new ManyTypesModel
            {
                StringValue = d["StringValue"]
            };
        }

        [Benchmark]
        [BenchmarkCategory(Deserialize)]
        public ManyTypesModel DeserializeMsgPack()
        {
            return MessagePackSerializer.Deserialize<ManyTypesModel>(msgpack);
        }

        [Benchmark]
        [BenchmarkCategory(Deserialize)]
        public ManyTypesModel DeserializeCbor()
        {
            return Cbor.Deserialize<ManyTypesModel>(cbor);
        }
    }
}
