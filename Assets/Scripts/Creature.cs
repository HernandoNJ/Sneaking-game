using UnityEngine;
using UnityEngine.Serialization;

public class Creature : MonoBehaviour
{
    public enum Team { Player = 0,Enemy = 1 }
    [FormerlySerializedAs("_head")]
    public Transform head;
    public Team team;

}
