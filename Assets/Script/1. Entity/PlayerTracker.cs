using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTracker : MonoBehaviour
{
    [SerializeField]private Transform _target;

    private void Start()
    {
        //_target = Managers.Player.transform; 
    }

    private void Update()
    {
        transform.position = _target.position;
    }
}
