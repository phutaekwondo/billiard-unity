using UnityEngine;
using System.Collections.Generic;

public class BallsSetConfig : MonoBehaviour
{
    public enum SetType
    {
        Nine,
        Fifteen
    }
    private float m_ballRadius = 0.032f;
    private SetType m_type = SetType.Nine;
    List<Vector2> m_15BallsPositionList = new List<Vector2>();
    [Tooltip("To apply this radius, Click the \"Set Balls Radius\" button")]
    [SerializeField] float m_configBallRadius = 0.032f;

    //private methods
    private void Start() 
    {
        if (this.gameObject.transform.childCount > 9)
        {
            m_type = SetType.Fifteen;
        }
        else
        {
            m_type = SetType.Nine;
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
        //result of this function will be stored in m_15BallsPositionList

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

    //public methods
    public void SetBallsRadius()
    {
        //in case the Start() didn't run
        Start();
        //set the m_ballRadius 
        m_ballRadius = m_configBallRadius;
        //calculate the positions
        Generate15BallsPositionList();
        //set the positions
        List<Vector2> positionList = new List<Vector2>();
        if (m_type == SetType.Nine)
        {
            //get list of position
            int[] takePositionIndexes = new int[] { 0, 1, 2, 3, 4, 5, 7, 8, 12 }; 
            for (int i = 0; i < takePositionIndexes.Length; i++)
            {
                positionList.Add(m_15BallsPositionList[takePositionIndexes[i]]);
            }
        }
        else
        {
            positionList = m_15BallsPositionList;
        }
        Debug.Log(positionList);
        for ( int i = 0; i < positionList.Count && i < this.transform.childCount; i++)
        {
            Vector3 position = positionList[i];
            Transform ballTransform = this.transform.GetChild(i);
            ballTransform.localPosition = new Vector3(position.y,m_ballRadius,position.x);
            //set the scale
            ballTransform.localScale = new Vector3( m_ballRadius*2,m_ballRadius*2,m_ballRadius*2 );
        }
    }
}
