using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabanWeight : MonoBehaviour
{
    // Root = 4;
    // Arm = 2;
    // Other = 1;
    public float GetValue { get
        {
            if (vl == null) return 0;
            return weightCoefficient * (vl.SmoothVelocity * vl.SmoothVelocity);
        }
    }
    public int weightCoefficient = 1;
    IVelocityTracker vl;
    // Start is called before the first frame update
    void Start()
    {
        vl = GetComponent<IVelocityTracker>();
    }
}
