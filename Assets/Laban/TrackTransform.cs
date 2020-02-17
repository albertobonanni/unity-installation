using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackTransform : MonoBehaviour
{
    public Transform target;
    IVelocityTracker vl;
    // Start is called before the first frame update
    void Start()
    {
        vl = GetComponent<IVelocityTracker>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!target) return;
        transform.position = target.position;
        transform.rotation = target.rotation;
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, vl!=null ? .1f + (.5f*vl.SmoothVelocity/5f) : .1f);
    }
}
