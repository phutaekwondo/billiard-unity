using UnityEngine;

public class TargetBallDestroyer : MonoBehaviour
{
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("TargetBall")) {
            Destroy(other.gameObject);
        }
    }
}
