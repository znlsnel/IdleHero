using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSensorHandler : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayer;

    private HashSet<GameObject> targets = new HashSet<GameObject>();
    public HashSet<GameObject> OverlabTargets => targets;
   
    void OnTriggerEnter(Collider other)
    {
        if (targetLayer == (targetLayer | (1 << other.gameObject.layer)))
        {
            targets.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other) 
    {
        if (targetLayer == (targetLayer | (1 << other.gameObject.layer)))
        {
            targets.Remove(other.gameObject);
        }
    }
} 
