using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    private GameObject player;
    private void Start()
    {
        player = Managers.Player.gameObject;
    }

    private void Update()
    {
        transform.position = player.transform.position;
    }


}
