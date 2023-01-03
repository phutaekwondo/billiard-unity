using UnityEngine;

public delegate void OnCollision(string senderTag, string collisionObjectTag);

public class CollisionChecker : MonoBehaviour
{
    public event OnCollision m_onCollision;

    private void OnCollisionEnter(Collision other) {
        if (m_onCollision != null) {
            m_onCollision(gameObject.tag, other.gameObject.tag);
        }
    }
}
