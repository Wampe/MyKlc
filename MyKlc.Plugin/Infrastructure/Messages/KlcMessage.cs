using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MyKlc.Plugin.Infrastructure.Messages
{
    [Serializable]
    public class KlcMessage
    {
        public KlcAction Action { get; set; }
        public object Payload { get; set; }

        public static byte[] ToStream(KlcMessage message)
        {
            using (var memoryStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, message);

                return memoryStream.ToArray();
            }
        }

        public static KlcMessage FromStream(byte[] streamData)
        {
            KlcMessage message = default;
            using (var memoryStream = new MemoryStream(streamData))
            {
                var binaryFormatter = new BinaryFormatter();
                message = binaryFormatter.Deserialize(memoryStream) as KlcMessage;
            }

            return message;
        }
    }
}
