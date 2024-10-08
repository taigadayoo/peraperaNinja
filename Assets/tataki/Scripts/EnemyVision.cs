using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    public bool _Player = false;

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Target"))
        {
            Debug.Log(collision.name);
            _Player = true;
        }
    }
}
