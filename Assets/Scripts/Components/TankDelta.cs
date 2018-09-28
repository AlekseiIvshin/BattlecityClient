using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankDelta {
    public Quaternion rotationTarget;
    public Vector3 positionTarget;
    public bool isRotaion = false;
    public bool wasTeleported = false;

    public static TankDelta rotateTo(Quaternion rotationTarget)
    {
        return new TankDelta
        {
            isRotaion = true,
            rotationTarget = rotationTarget
        };
    }

    public static TankDelta rotateTo(int angle)
    {
        return new TankDelta
        {
            isRotaion = true,
            rotationTarget = Quaternion.Euler(0, angle, 0)
        };
    }

    public static TankDelta rotateToDirection(int direction)
    {
        return new TankDelta
        {
            isRotaion = true,
            rotationTarget = Quaternion.Euler(0, MapUtils.getAngle(direction), 0)
        };
    }

    public static TankDelta moveTo(Vector3 positionTarget)
    {
        return new TankDelta
        {
            isRotaion = false,
            positionTarget = positionTarget,
        };
    }

    public static TankDelta teleportTo(Vector3 positionTarget)
    {
        return new TankDelta
        {
            isRotaion = false,
            wasTeleported = true,
            positionTarget = positionTarget,
        };
    }

}
