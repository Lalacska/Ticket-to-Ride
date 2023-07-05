using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts.Cards
{
    public class Card : Singleton<Card>
    {
        private CardStruct cardStruct;
        public CardStruct Cardstruct{ get { return cardStruct; } set { cardStruct = value; } }

        [System.Serializable]
        public struct CardStruct : INetworkSerializable, IEquatable<CardStruct>
        {
            //public Transform transform;
            public FixedString128Bytes CardName;

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
                serializer.SerializeValue(ref CardName);
            }

            public bool Equals(CardStruct other)
            {
                return hasBeenPlayed.Equals(other.hasBeenPlayed) && handIndex.Equals(other.handIndex) && Color.Equals(other.Color) && ownerID.Equals(other.ownerID) && CardID.Equals(other.CardID) && CardName.Equals(other.CardName);
            }

            public CardStruct(bool a_hasBeenPlayed, int a_handIndex, FixedString128Bytes color, int a_ownerID, int cardID, FixedString128Bytes a_cardname /*Transform a_transform*/)
            {
                hasBeenPlayed = a_hasBeenPlayed;
                handIndex = a_handIndex;
                Color = "Color";
                ownerID = a_ownerID;
                CardID = cardID;
                CardName = a_cardname;
                //transform = a_transform;
            }

        }
    }
    // This class handles all the values for the card. \\
    
    
}