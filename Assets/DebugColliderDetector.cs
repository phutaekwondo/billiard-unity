using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugColliderDetector : MonoBehaviour
{
    float onTableY;

    private void Start() {
        onTableY = transform.position.y;
    }

    private void Update() {
        if (IsJumped()) {
            //debug log with color red
            Debug.Log("<color=red>Jumped</color>");
        }
    }
    
    private bool IsJumped()
    {
        return transform.position.y > onTableY * 2;
    }

    private void OnCollisionEnter(Collision other) {
        Debug.Log( "CollisionEnter: " + other.gameObject.name );
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log( "TriggerEnter: " + other.gameObject.name );
    }
}
