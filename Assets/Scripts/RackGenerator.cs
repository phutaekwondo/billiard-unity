using System.Collections.Generic;
using UnityEngine;

public class RackGenerator : MonoBehaviour
{
    Color RED = Color.red;
    Color YELLOW = Color.yellow;
    Color GREEN = Color.green;
    Color BLUE = Color.blue;
    Color PURPLE = Color.magenta;
    Color ORANGE = new Color(1.0f, 0.5f, 0.0f, 1.0f);
    Color BROWN = new Color(0.5f, 0.25f, 0.0f, 1.0f);
    Color PINK = new Color(1.0f, 0.0f, 1.0f, 1.0f);
    Color WHITE = Color.white;
    Color BLACK = Color.black;

    public GameObject m_ballPrefab = null; // Drag and drop the ball prefab here
    public GameObject m_rack = null; // Drag and drop the rack prefab here
    public GameplayManager m_gameplayManager = null; // Drag and drop the GameplayManager here

    public List<Material> m_ballMaterials = new List<Material>(); // Drag and drop the materials here

    float m_ballRadius;
    List<Vector2> m_15BallsPositionList = new List<Vector2>();
    bool isReseting = false;

    private void Start() {
        //set the ball radius
        if (m_ballPrefab != null)
        {
            m_ballRadius = m_ballPrefab.transform.localScale.x / 2;
        }
        else{
            throw new System.Exception("Ball prefab is null");
        }

        m_gameplayManager.m_OnGameplayRestart += ResetRack;

        SetColorForMaterials();
        Generate15BallsPositionList();
        GenerateRack15Balls();
    }

    private void LateUpdate() {
        if (isReseting)
        {
            //generate new rack of balls
            GenerateRack15Balls();

            isReseting = false;
        }
    }

    public void ResetRack() 
    {
        //destroy all target balls
        foreach (Transform ball in m_rack.transform)
        {
            if (ball.tag == "TargetBall")
            {
                Destroy(ball.gameObject);
            }
        }

        isReseting = true; // generate new rack of balls in LateUpdate()
    }

    void SetColorForMaterials()
    {
        if (m_ballMaterials.Count != 15)
        {
            throw new System.Exception("Ball materials count is not 15 when setting color");
        }
        m_ballMaterials[0].color = m_ballMaterials[8].color = YELLOW;
        m_ballMaterials[1].color = m_ballMaterials[9].color = BLUE;
        m_ballMaterials[2].color = m_ballMaterials[10].color = RED;
        m_ballMaterials[3].color = m_ballMaterials[11].color = PURPLE;
        m_ballMaterials[4].color = m_ballMaterials[12].color = ORANGE;
        m_ballMaterials[5].color = m_ballMaterials[13].color = GREEN;
        m_ballMaterials[6].color = m_ballMaterials[14].color = BROWN;
        m_ballMaterials[7].color = BLACK;
    }

    private void GenerateRack15Balls()
    {
        if (m_rack == null)
        {
            throw new System.Exception("Rack prefab is null");
        }

        //generate 15 balls in m_rack with the positions in m_ballsPositionList
        for (int i = 0; i < 15; i++)
        {
            GameObject ball = Instantiate(m_ballPrefab, m_rack.transform);
            ball.transform.localPosition = new Vector3( m_15BallsPositionList[i].y, m_ballRadius, m_15BallsPositionList[i].x);
            //random rotation
            ball.transform.Rotate(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));

            //set the tag of the ball is "TargetBall"
            ball.tag = "TargetBall";
        }

        //set the materials for the balls
        if ( m_ballMaterials.Count != 15)
        {
            throw new System.Exception("Ball materials count is not 15 when generating 15-balls-rack");
        }

        // read the comments below to understand the array "ballsNumberOrder"
        int[] ballsNumberOrder = new int[15] { 1,9,6,2,8,14,10,7,15,5,3,11,12,4,13};
        // this array is storing the number on the balls
        // and the index show the order of the balls
        // below "rack" is using index to show the how the index refers to the position of the ball
        // (14) (13) (12) (11) (10)
        //  (9)  (8)  (7)  (6)
        //   (5)  (4)  (3)
        //    (2)  (1)
        //     (0) 

        for (int i = 0; i < 15; i++)
        {
            m_rack.transform.GetChild(i).GetComponent<Renderer>().material = m_ballMaterials[ballsNumberOrder[i]-1];
        }
    }

    private void Generate15BallsPositionList()
    {
        //generate 15 balls position
        // ()()()()()
        //  ()()()()
        //   ()()()
        //    ()()
        //     () --> this ball will be (0,0)

        m_15BallsPositionList.Clear();
        m_15BallsPositionList.Add( new Vector2(0,0) ); // row 1 already has 1 ball

        for (int row = 2; row <= 5; row++)
        {
            float xForFirtBallInRow = - (row-1) * m_ballRadius * 2 * Mathf.Cos(Mathf.PI/3);
            float yForFirtBallInRow = (row-1) * m_ballRadius * 2 * Mathf.Sin(Mathf.PI/3);

            Vector2 firstBallPositionInRow = new Vector2(xForFirtBallInRow, yForFirtBallInRow);

            m_15BallsPositionList.Add(firstBallPositionInRow);

            for ( int ball = 2; ball <= row; ball++)
            {
                Vector2 currentBallPosition = new Vector2( firstBallPositionInRow.x + (ball -1) * m_ballRadius * 2 , firstBallPositionInRow.y);
                m_15BallsPositionList.Add(currentBallPosition);
            }
        }
    }

}
