using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts.Cards
{

    // This class handles all the values for the card. \\
    public struct Card : INetworkSerializable, IEquatable<Card>
    {
        public bool hasBeenPlayed;

        public int handIndex;

        public FixedString128Bytes Color;

        public int ownerID;

        public int CardID;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref hasBeenPlayed);
            serializer.SerializeValue(ref handIndex);
            serializer.SerializeValue(ref Color);
            serializer.SerializeValue(ref ownerID);
            serializer.SerializeValue(ref CardID);
        }

        public bool Equals(Card other)
        {
            return hasBeenPlayed.Equals(other.hasBeenPlayed) && handIndex.Equals(other.handIndex) && Color.Equals(other.Color) && ownerID.Equals(other.ownerID) && CardID.Equals(other.CardID);
        }

        public Card(bool a_hasBeenPlayed, int a_handIndex, FixedString128Bytes color, int a_ownerID, int cardID)
        {
            hasBeenPlayed = a_hasBeenPlayed;
            handIndex = a_handIndex;
            Color = "Color";
            ownerID = a_ownerID;
            CardID = cardID;
        }

    }
}