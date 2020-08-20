using System;
using System.IO;
using XiangJiang.Core;
using XiangJiang.Infrastructure.Abstractions;
using PSerializer = ProtoBuf.Serializer;

namespace XiangJiang.Infrastructure.Serializer.ProtoBuf
{
    /// <summary>
    ///     基于ProtoBuf持久化实现
    /// </summary>
    public sealed class ProtoBufSerializer : ISerializer
    {
        #region Methods

        /// <summary>
        ///     反序列化
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="data">需要反序列化字符串</param>
        /// <returns>反序列化</returns>
        public T Deserialize<T>(string data)
        {
            Checker.Begin().NotNullOrEmpty(data, nameof(data));
            var buffer = Convert.FromBase64String(data);
            using (var stream = new MemoryStream(buffer))
            {
                return PSerializer.Deserialize<T>(stream);
            }
        }

        /// <summary>
        ///     序列化
        /// </summary>
        /// <param name="serializeObject">需要序列化对象</param>
        /// <returns>Json字符串</returns>
        public string Serialize(object serializeObject)
        {
            Checker.Begin().NotNull(serializeObject, nameof(serializeObject))
                .IsSerializable(serializeObject);
            using (var stream = new MemoryStream())
            {
                PSerializer.Serialize(stream, serializeObject);
                return Convert.ToBase64String(stream.GetBuffer(), 0, (int) stream.Length);
            }
        }

        #endregion Methods
    }
}