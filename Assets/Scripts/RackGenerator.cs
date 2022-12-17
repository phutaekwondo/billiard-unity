using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RackGenerator : MonoBehaviour
{
    Color RED = Color.red;
    Color YELLOW = Color.yellow;
    Color GREEN = Color.green;
    Color BLUE = Color.blue;
    Color PURPLE = Color.magenta;
    Color ORANGE = Color.cyan;
    Color BROWN = new Color(0.5f, 0.25f, 0.0f, 1.0f);
    Color PINK = new Color(1.0f, 0.0f, 1.0f, 1.0f);
    Color WHITE = Color.white;
    Color BLACK = Color.black;

    public Material m_ballMaterial = null; // Drag and drop the ball material here
    public GameObject m_ballPrefab = null; // Drag and drop the ball prefab here
    public GameObject m_rack = null; // Drag and drop the rack prefab here

    float m_ballRadius;
    List<Vector2> m_ballsPositionList = new List<Vector2>();

    private void Start() {
        //set the ball radius
        if (m_ballPrefab != null)
        {
            m_ballRadius = m_ballPrefab.GetComponent<SphereCollider>().radius;
        }
        else{
            throw new System.Exception("Ball prefab is null");
        }

        GenerateBallsPositionList();
    }

    public List<Vector2> GetBallsPositionList()
    {
        return m_ballsPositionList;
    }

    private void GenerateBallsPositionList()
    {
        //generate 15 balls positio
        // ()()()()()
        //  ()()()()
        //   ()()()
        //    ()()
        //     () --> this ball will be (0,0)

        m_ballsPositionList.Clear();
        m_ballsPositionList.Add( new Vector2(0,0) ); // row 1 already has 1 ball

        for (int row = 2; row <= 5; row++)
        {
            float xForFirtBallInRow = (row-1) * m_ballRadius * 2 * Mathf.Sin(Mathf.PI/3);
            float yForFirtBallInRow = (row-1) * m_ballRadius * 2 * Mathf.Sin(Mathf.PI/3);

            Vector2 firstBallPositionInRow = new Vector2(xForFirtBallInRow, yForFirtBallInRow);

            m_ballsPositionList.Add(firstBallPositionInRow);

            for ( int ball = 2; ball <= row; ball++)
            {
                Vector2 currentBallPosition = new Vector2( firstBallPositionInRow.x + (ball -1) * m_ballRadius * 2 , firstBallPositionInRow.y);
                m_ballsPositionList.Add(currentBallPosition);
            }
        }
    }

}
