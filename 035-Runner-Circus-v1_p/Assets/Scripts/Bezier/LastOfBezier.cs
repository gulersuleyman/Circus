using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class LastOfBezier : MonoBehaviour
{

    [SerializeField] GameObject _character;

    BezierJump _bezier;
    RouteSwitch _player;
    public float firstSpeed;

    

    Right _right;
    Left _left;
    Front _front;
    Top _top;
    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<RouteSwitch>();
        _bezier = FindObjectOfType<BezierJump>();

        _right = FindObjectOfType<Right>();
        _left = FindObjectOfType<Left>();
        _front = FindObjectOfType<Front>();
        _top = FindObjectOfType<Top>();
        
    }

    public void OnJump()
    {
        float distance;
        float desiredSide;
        Vector3 distanceVector;

        desiredSide = _character.transform.localEulerAngles.z;

        distanceVector = _bezier.bezierPointListObjects[0].transform.position - _bezier.bezierPointListObjects[_bezier.bezierPointList.Count - 1].transform.position;
        distance = Mathf.Sqrt(Mathf.Pow(distanceVector.x, 2) + Mathf.Pow(distanceVector.y, 2) + Mathf.Pow(distanceVector.z, 2));
        if(_front.isFront)
        {
            ChangeMiddlePoints(new Vector3(0, distance / 3, distance / 3), new Vector3(0, distance * 2 / 3, distance * 2 / 3));
        }

        if(_right.isRight && desiredSide>180)
        {
            ChangeMiddlePoints(new Vector3(distance / 3, distance / 3, distance / 3), new Vector3(distance * 2 / 3, distance * 2 / 3, distance * 2 / 3));
        }
        if(_left.isLeft && desiredSide<180)
        {
            ChangeMiddlePoints(-new Vector3(distance / 3, -distance / 3, -distance / 3), -new Vector3(distance * 2 / 3, -distance * 2 / 3, -distance * 2 / 3));
        }
        if(_top.isTop)
        {
            ChangeMiddlePoints(new Vector3(0, 0, distance / 3), new Vector3(0, 0, distance * 2 / 3));
        }
        if(_bezier.jumpToMain && _player.mainProjector.targetObject.transform.position.x>_player.transform.position.x)
        {
            if(_player.onNode)
            {
                ChangeMiddlePoints(new Vector3(0, distance / 3, distance / 3), new Vector3(0, distance * 2 / 3, distance * 2 / 3));
            }
            else
            {
                ChangeMiddlePoints(new Vector3(distance / 3, distance / 3, distance / 3), new Vector3(distance * 2 / 3, distance * 2 / 3, distance * 2 / 3));
            }
             
            
        }
        if (_bezier.jumpToMain && _player.mainProjector.targetObject.transform.position.x < _player.transform.position.x)
        {
            if(_player.onNode)
            {
                ChangeMiddlePoints(new Vector3(0, distance / 3, distance / 3), new Vector3(0, distance * 2 / 3, distance * 2 / 3));
            }
            else
            {
                ChangeMiddlePoints(-new Vector3(distance / 3, -distance / 3, -distance / 3), -new Vector3(distance * 2 / 3, -distance * 2 / 3, -distance * 2 / 3));
            }
            
            
        }


    }

    private void ChangeMiddlePoints(Vector3 first,Vector3 second)
    {
        _bezier.bezierPointListObjects[1].transform.position = _bezier.bezierPointListObjects[0].transform.position + first;
        _bezier.bezierPointListObjects[2].transform.position = _bezier.bezierPointListObjects[0].transform.position + second;
    }
   
    public void OnFirstJump()
    {
        float distance;
        float desiredSide;
        Vector3 distanceVector;

        desiredSide = _character.transform.localEulerAngles.z;
        _bezier.bezierPointListObjects[3].transform.localPosition = new Vector3(0, 90f, 243f);
        distanceVector = _bezier.bezierPointListObjects[0].transform.position - _bezier.bezierPointListObjects[3].transform.position;
        distance = Mathf.Sqrt(Mathf.Pow(distanceVector.x, 2) + Mathf.Pow(distanceVector.y, 2) + Mathf.Pow(distanceVector.z, 2));
        
        ChangeMiddlePoints(new Vector3(0, distance / 3, distance / 3), new Vector3(0, distance * 2 / 3, distance * 2 / 3));
        

    }
}
