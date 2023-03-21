using UnityEngine;
using System.Collections.Generic;

public class BallsSetConfig : MonoBehaviour
{
    private enum Type
    {
        Nine,
        Fifteen
    }
    private float m_ballRadius = 0.032f;
    List<Vector2> m_15BallsPositionList = new List<Vector2>();
    [SerializeField] float m_configBallRadius = 0.032f;

    public void SetBallsRadius()
    {
        //set the ball radius
        //calculate the positions
        //set the positions
        //set the scale
    }
}
