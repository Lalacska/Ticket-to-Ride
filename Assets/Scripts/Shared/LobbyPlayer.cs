using System;
using Unity.Collections;
using Unity.Netcode;

// A struct to save some data from the players 
[Serializable]
public struct LobbyPlayer: INetworkSerializeByMemcpy, IEquatable<LobbyPlayer>
{
    public FixedString64Bytes ID;
    public FixedString64Bytes PlayerName;
    public ulong ClientId;

    public LobbyPlayer(ulong clientId, FixedString64Bytes id, FixedString64Bytes playername) 
    {
        ID = id;
        PlayerName = playername;
        ClientId = clientId;
    }


    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref ID);
        serializer.SerializeValue(ref ClientId);
        serializer.SerializeValue(ref PlayerName);
    }

    public bool Equals(LobbyPlayer other)
    {
        return ID.Equals(other.ID) && 
            PlayerName.Equals(other.PlayerName) && 
            ClientId.Equals(other.ClientId);
    }
}
