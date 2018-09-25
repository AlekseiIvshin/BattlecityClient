using System.Collections.Generic;
using UnityEngine;

public class Tank: BaseEntity
{ 
    public int direction;
    public string name;
    public float rotationSpeed;
    public CharacterController characterController;
    public List<TankDelta> deltas = new List<TankDelta>();
}