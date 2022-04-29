using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
public class EnemyController : MonoBehaviour
{
    [SerializeField] float followSpeed=55;
    [SerializeField] Transform frontTransform;


    double percent;


    SplineFollower _follower;
    EnemyBezierJump _bezier;
    EnemyFront _front;
    LastOfEnemyBezier _lastPoint;
    // Start is called before the first frame update
    void Start()
    {
        _lastPoint = FindObjectOfType<LastOfEnemyBezier>();
        _follower = GetComponent<SplineFollower>();
        _bezier = GetComponent<EnemyBezierJump>();
        _front = GetComponentInChildren<EnemyFront>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Finish") && _front.canJumpFront)
        {
            
            percent = _front._follower.result.percent;
            _bezier.UpdatePoints(transform.position);
            _lastPoint.gameObject.transform.position = _front.gameObject.transform.position;
            _bezier.UpdatePoints(transform.position);
            _follower.follow = false;
            _front._follower.follow = false;
            _front.gameObject.transform.position = frontTransform.position;
            
            
            StartCoroutine(Jump());

        }
    }
    IEnumerator Jump()
    {
        LineRenderer line = _bezier.m_lineRenderer;

        _bezier.UpdatePoints(transform.position);

        for (int i = 0; i < 100; i++)
        {
            transform.position = line.GetPosition(i);

            yield return new WaitForSeconds(0.007f);

        }
        _follower.spline = _front._follower.spline;
        _follower.SetPercent(percent);
        _follower.follow = true;
        _follower.followSpeed = followSpeed;
        _front.canJumpFront = false;
        yield return null;
    }
    public IEnumerator FirstJump()
    {
        LineRenderer line = _bezier.m_lineRenderer;

        _bezier.UpdatePoints(transform.position);
        _lastPoint.OnFirstJump();

        for (int i = 0; i < 100; i++)
        {
            transform.position = line.GetPosition(i);

            yield return new WaitForSeconds(0.02f);

        }
       
        _follower.SetPercent(0.0);
        _follower.follow = true;
        _follower.followSpeed = followSpeed;
        _front.canJumpFront = false;


        yield return null;
    }
}
