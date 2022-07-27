using UnityEngine;

public class Creature : MonoBehaviour
{
    public enum Team { Player = 0,Enemy = 1 }
    public Transform head;
    public Team team;

}
