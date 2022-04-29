
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BezierJump : Bezier
{
    [SerializeField] GameObject _bezierParent;

    public bool jumpToMain = false;
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
