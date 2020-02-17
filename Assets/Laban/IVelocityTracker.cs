using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVelocityTracker 
{
    float Velocity
    {
        get;
    }

    float SmoothVelocity
    {
        get;
    }
}
