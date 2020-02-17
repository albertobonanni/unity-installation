using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapWeightToScale : MonoBehaviour
{
    public LabanWeightAggregate source;
    public float scalar = 2;
    Vector3 baseSize;

    // Start is called before the first frame update
    void Start()
    {
        baseSize = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (!source) return;
        transform.localScale = baseSize + (baseSize*source.weightValue * scalar);
    }
}
