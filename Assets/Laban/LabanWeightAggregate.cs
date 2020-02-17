using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabanWeightAggregate : MonoBehaviour
{
    public float weightValue;
    LabanWeight[] jointWeights;
    // Start is called before the first frame update
    void Start()
    {
        jointWeights = GetComponentsInChildren<LabanWeight>();
    }

    // Update is called once per frame
    void Update()
    {
        if(jointWeights.Length > 0)
        {
            weightValue = 0;
            for (int i = 0; i < jointWeights.Length; i++)
            {
                weightValue += jointWeights[i].GetValue;
            }
        }
    }
}
