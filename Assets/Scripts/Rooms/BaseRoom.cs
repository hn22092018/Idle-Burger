using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomID {
    Lobby,
    Table1,
    Table2,
    Table3,
    Table4,
    Table5,
    Table6,
    BigTable1,
    BigTable2,
    BigTable3,
    BigTable4,
    BigTable5,
    BigTable6,
    BigTable7,
    BigTable8,
    BigTable9,
    BigTable10,
    BigTable11,
    BigTable12,
    BigTable13,
    BigTable14,
    Kitchen,
    Restroom,
    Restroom2,
    Power,
    Manager,
    DeliverRoom
}
[Serializable]
public class BaseRoom<T> {
    /// <summary>
    /// Room ID
    /// </summary>
    public RoomID roomID;
    /// <summary>
    /// group ID
    /// </summary>
    public int GroupID;
    /// <summary>
    ///  All Model In Room 
    /// </summary>
    public List<ModelPosition<T>> modelPositions;

}
