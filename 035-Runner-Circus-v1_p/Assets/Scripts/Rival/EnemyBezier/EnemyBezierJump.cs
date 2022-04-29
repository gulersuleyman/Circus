using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBezierJump : Bezier
{
    [SerializeField] GameObject _bezierParent;

    
    private void Start()
    {
        m_lineRenderer = GetComponent<LineRenderer>();

    }
    public void UpdatePoints(Vector3 playerPosition)
    {
        ResetBezier();
        _bezierParent.transform.position = transform.position;
        SetBezier(playerPosition);
    }
    
}
