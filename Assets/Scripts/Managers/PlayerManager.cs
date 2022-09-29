using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singeltone<PlayerManager>
{
    public List<Player> players = new List<Player>();
}
