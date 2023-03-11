using UnityEngine;

public class GameConfig : MonoBehaviour
{
    public static RackGenerator.RackType m_rackType = RackGenerator.RackType.NineBalls;
    public static float m_ballRadius = 0.032f; // just don't use this for now
    public static float m_ballOnTableYPosition = 0.032f;
}
