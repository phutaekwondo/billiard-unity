using UnityEngine; 
using System.Collections.Generic;
using System;

public class RackGenerator 
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

    float m_ballRadius;
    List<Vector2> m_ballsPositionList = new List<Vector2>();

    public RackGenerator(){}

    private void GenerateBallsPositionList()
    {
        //generate 15 balls positio
        // ()()()()()
        //  ()()()()
        //   ()()()
        //    ()()
        //     () --> this ball will be (0,0)

        Vector2 firstBall = new Vector2(0,0);
        List<Vector2> currentRow = new List<Vector2>();
        currentRow.Add(firstBall);

        for (int row = 1; row <= 5; row++)
        {

        }
    }
}