namespace Skaia.Serialization
{
    public interface INetworkSerializable
    {
        byte[] Serialize();

        void Deserialize(byte[] data);
    }
}