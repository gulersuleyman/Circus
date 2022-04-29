using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bezier : MonoBehaviour
{
    

    public List<GameObject> bezierPointListObjects;
    public List<Vector3> bezierPointList = new List<Vector3>();
    public LineRenderer m_lineRenderer;

    
    public void ResetBezier()
    {
        Vector3 point;
        bezierPointList.Clear();
        for (int i = 0; i < 4; i++)
        {

            point = bezierPointListObjects[i].transform.localPosition;
            bezierPointList.Add(point);
        }
    }public void SetBezier(Vector3 playerPosition)
    {
        for (float i = 0; i < 100; i++)
        {
            float k = i / 100;
            Vector3 m_positionLinePoint = bezierPointList[0] * Mathf.Pow((1 - k), 3) +
                                          bezierPointList[1] * Mathf.Pow((1 - k), 2) * k * 3 +
                                          3 * (1 - k) * k * k * bezierPointList[2] + k * k * k * bezierPointList[3] + playerPosition;
            m_lineRenderer.SetPosition((int)i, m_positionLinePoint);
        }
    }
}
