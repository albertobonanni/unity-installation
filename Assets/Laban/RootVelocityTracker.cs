using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootVelocityTracker : MonoBehaviour, IVelocityTracker
{
    public float Velocity
    {
        get
        {
            return v.magnitude;
        }
    }

    public float SmoothVelocity
    {
        get
        {
            float sv = 0;
            for (int i = 0; i < BufferSize; i++)
            {
                sv += velocityBuffer[i];
            }
            sv /= BufferSize;
            return sv;
        }
    }
    public int BufferSize = 16;
    float[] velocityBuffer;
    int vbi = 0;
    Vector3 v;
    Vector3 lp;
    float t;
    // Start is called before the first frame update
    void Start()
    {
        lp = transform.position;
        velocityBuffer = new float[BufferSize];
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;

        if (lp == transform.position) return;
        v = transform.position - lp;
        v /= t;

        velocityBuffer[vbi] = Velocity;
        vbi++;
        if (vbi >= BufferSize) vbi = 0;

        lp = transform.position;
        t = 0;
    }
}
