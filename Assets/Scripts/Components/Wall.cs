using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : BaseEntity
{
    public int destroyed = WallProcessor.NOT_DESTROYED;
 
    public Transform transform;
}
