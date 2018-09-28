using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tank: BaseEntity
{ 
    public int direction;
    public string name;
    public float rotationSpeed;
    public Transform cloud;
    public Text nameCloud;
    public List<TankDelta> deltas = new List<TankDelta>();
}