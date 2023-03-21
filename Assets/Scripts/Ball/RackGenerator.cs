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
        GenerateRack();
    }
    private void LateUpdate() {
        if (isReseting)
        {
            GenerateRack();

            isReseting = false;
        }
    }

    public void ClearRack()
    {
        foreach (Transform ball in m_rack.transform)
        {
            if (ball.tag == "TargetBall")
            {
                Destroy(ball.gameObject);
            }
        }

    }

    public void ResetRack() 
    {
        ClearRack();
        isReseting = true; // generate new rack of balls in LateUpdate()
    }
    private void GenerateRack()
    {
        if (m_rack == null)
        {
            throw new System.Exception("Rack prefab is null");
        }

        if (GameConfig.m_rackType == RackType.NineBalls)
        {
            GenerateRack9Balls();
        }
        else if (GameConfig.m_rackType == RackType.FifteenBalls)
        {
            GenerateRack15Balls();
        }
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
    private void GenerateBallsWithPositions(List<Vector2> positionList)
    {
        foreach ( Vector2 position in positionList)
        {
            GameObject ball = Instantiate(m_ballPrefab, m_rack.transform);
            ball.transform.localPosition = new Vector3( position.y, m_ballRadius, position.x);
            //random rotation
            ball.transform.Rotate(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));

            ball.tag = "TargetBall";
        }
    }
    private void SetMaterialForBalls(List<int> materialIndexes)
    {
        if (materialIndexes.Count != m_rack.transform.childCount)
        {
            throw new System.Exception("Material indexes count is not equal to rack's child count");
        }
        // read the comments below to understand the array "ballsNumberOrder"
        // this array is storing the number on the balls
        // and the index show the order of the balls
        // below "rack" is using index to show the how the index refers to the position of the ball
        //
        // fifteen balls rack
        // (14) (13) (12) (11) (10)
        //  (9)  (8)  (7)  (6)
        //   (5)  (4)  (3)
        //    (2)  (1)
        //     (0) 
        // nine balls rack
        //       (8)          
        //     (7)(6)     
        //   (5)(4)(3)
        //    (2)(1)
        //     (0) 

        for (int i = 0; i < materialIndexes.Count; i++)
        {
            m_rack.transform.GetChild(i).GetComponent<Renderer>().material = m_ballMaterials[materialIndexes[i]-1];
        }
    }
    public void GenerateRack9Balls()
    {
        //get list of position
        int[] takePositionIndexes = new int[] { 0, 1, 2, 3, 4, 5, 7, 8, 12 }; 
        List<Vector2> positionList = new List<Vector2>();
        for (int i = 0; i < takePositionIndexes.Length; i++)
        {
            positionList.Add(m_15BallsPositionList[takePositionIndexes[i]]);
        }
        //generate 9 balls in m_rack with the positions in that list
        GenerateBallsWithPositions(positionList);
        //set the balls's material
        List<int> materialIndexes = new List<int>() {1,2,3,4,9,5,6,7,8};
        SetMaterialForBalls(materialIndexes);
    }
    public void GenerateRack15Balls()
    {
        GenerateBallsWithPositions(m_15BallsPositionList);

        List<int> materialIndexes = new List<int>(){1,9,6,2,8,14,10,7,15,5,3,11,12,4,13};
        SetMaterialForBalls(materialIndexes);
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

    public enum RackType
    {
        NineBalls,
        FifteenBalls
    }
}
