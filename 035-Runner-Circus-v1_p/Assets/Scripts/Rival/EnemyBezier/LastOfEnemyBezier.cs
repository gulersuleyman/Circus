using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastOfEnemyBezier : MonoBehaviour
{
    [SerializeField] GameObject _enemy;


    EnemyBezierJump _bezier;

    private void Start()
    {
        _bezier = FindObjectOfType<EnemyBezierJump>();
    }

    public void OnFirstJump()
    {
        float distance;
        float desiredSide;
        Vector3 distanceVector;

        desiredSide = _enemy.transform.localEulerAngles.z;
        _bezier.bezierPointListObjects[3].transform.localPosition = new Vector3(-211f, 90f, 243f);
        distanceVector = _bezier.bezierPointListObjects[0].transform.position - _bezier.bezierPointListObjects[3].transform.position;
        distance = Mathf.Sqrt(Mathf.Pow(distanceVector.x, 2) + Mathf.Pow(distanceVector.y, 2) + Mathf.Pow(distanceVector.z, 2));

        ChangeMiddlePoints(new Vector3(0, distance / 3, distance / 3), new Vector3(0, distance * 2 / 3, distance * 2 / 3));
    }
    private void ChangeMiddlePoints(Vector3 first, Vector3 second)
    {
        _bezier.bezierPointListObjects[1].transform.position = _bezier.bezierPointListObjects[0].transform.position + first;
        _bezier.bezierPointListObjects[2].transform.position = _bezier.bezierPointListObjects[0].transform.position + second;
    }
}
