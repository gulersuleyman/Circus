using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using Cinemachine;
using DG.Tweening;
public class RouteSwitch : MonoBehaviour
{
    [SerializeField] GameObject _character;
    [SerializeField] Transform _rightCamTransform;
    [SerializeField] Transform _leftCamTransform;
    [SerializeField] Transform firstLeftPos, firstFrontPos, firstRightPos;


    public static bool isDeath = false;


    public bool canJumpBetweenSplines=false;
    public bool onNode = false;


    double currentPercent;
    double percent;
    float shakeTimer;
    float startingIntensity;
    float shakeTimerTotal;
    float firstMousePosition;
    float currentMousePosition;
    float playerRotation;
    float desiredSide;
    bool isJumping = false;
    bool onRight = false;
    bool onLeft = false;


    public Transform nextSplineTransform;
    public SplineFollower _follower;


    
    public SplineComputer nextSpline;
    public SplineComputer mainSpline;
    public SplineProjector mainProjector;
    BezierJump _bezier;
    public  LastOfBezier _lastPoint;
    CinemachineVirtualCamera vcam;
    Rigidbody _rigidbody;
    Front _front;
    Left _left;
    Right _right;
    Top _top;
    AnimationController _animationController;


    public delegate void JumpAction();
    public static event JumpAction OnJump;


    private void Awake()
    {
        _follower = GetComponent<SplineFollower>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
        _lastPoint = FindObjectOfType<LastOfBezier>();
        _bezier = GetComponent<BezierJump>();
        
        _front = GetComponentInChildren<Front>();
        _left = GetComponentInChildren<Left>();
        _right= GetComponentInChildren<Right>();
        _top = GetComponentInChildren<Top>();
        _rigidbody = GetComponent<Rigidbody>();
        vcam = FindObjectOfType<CinemachineVirtualCamera>();
        mainProjector = GetComponent<SplineProjector>();
        _animationController = GetComponent<AnimationController>();
        mainSpline = _follower.spline;
        mainProjector.spline = mainSpline ;
        _follower.onNode += OnNodePassed;
        canJumpBetweenSplines = true;

    }
    
    private void Update()
    {

        if(!isDeath && !isJumping)
        {
            DragMouse();
            if (mainProjector.result.percent >= 0.99)
            {
                mainProjector.spline = _follower.spline;
            }
            if (canJumpBetweenSplines)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    desiredSide = _character.transform.localEulerAngles.z;


                      if((_left.isLeft && desiredSide<180) || (_right.isRight && desiredSide > 180) || (_front.isFront))
                        {
                            StartCoroutine(Jump());
                        
                        }
                      else if(onNode && ((mainProjector.targetObject.transform.position.x < transform.position.x && desiredSide < 180)
                         || (mainProjector.targetObject.transform.position.x > transform.position.x && desiredSide > 180)))
                        {
                            StartCoroutine(JumpToMainSpline());
                        
                        }
                          
                }
            }
            else if((mainProjector.result.percent + 0.10 <= 0.99 && !canJumpBetweenSplines))
            {
                if(Input.GetMouseButtonUp(0))
                {

                    desiredSide = _character.transform.localEulerAngles.z;
                    
                     if ((mainProjector.targetObject.transform.position.x<transform.position.x && desiredSide<180)
                         || (mainProjector.targetObject.transform.position.x > transform.position.x && desiredSide > 180))
                     {
                         StartCoroutine(JumpToMainSpline());
                     }

                }
            }
            if (shakeTimer > 0)
            {
                shakeTimer -= Time.deltaTime;

                CinemachineBasicMultiChannelPerlin perlin = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                perlin.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, shakeTimer / shakeTimerTotal);

            }
            if (_follower.follow)
            {
                currentPercent = _follower.result.percent;
            }
            if (currentPercent>= 0.99)
            {
                OnDeath();
            }
            if (Input.GetMouseButtonUp(0))
            {
                _character.transform.DORotate(Vector3.zero, 0.3f); 
            }

            

        }
 
    }
    void DragMouse()
    {
        if(!isJumping)
        {
            if (Input.GetMouseButtonDown(0))
            {
                firstMousePosition = Input.mousePosition.x;
            }
            if (Input.GetMouseButton(0))
            {
                currentMousePosition = Input.mousePosition.x;
                playerRotation = (firstMousePosition - currentMousePosition) / 3f;
                playerRotation = Mathf.Clamp(playerRotation, -45f, 45f);
                _character.transform.eulerAngles = new Vector3(0, 0, playerRotation);
            }
            if (Input.GetMouseButtonUp(0))
            {
                _character.transform.DORotate(Vector3.zero, 0.3f);
            }
        }
        
    }

    public IEnumerator FirstJump()
    {
        isJumping = true;
        
        LineRenderer line = _bezier.m_lineRenderer;
        _follower.follow = false;
        
        _animationController.JumpAnimation(true);

        canJumpBetweenSplines = false;
        
        _lastPoint.OnFirstJump();
        _bezier.UpdatePoints(transform.localPosition);
        

        _character.transform.LookAt(_lastPoint.transform.position);
        for (int i = 0; i < 100; i++)
        {
            transform.position = line.GetPosition(i);

            yield return new WaitForSeconds(0.02f);

        }
        _animationController.JumpAnimation(false);
        
        ShakeCamera(0.5f, .2f);

        _character.transform.LookAt(-transform.forward);
        _follower.follow = true;
        _character.transform.localEulerAngles = Vector3.zero;
        SetSideColliderActivity(true);
        isJumping = false;


        yield return null;
    }

    IEnumerator Jump()
    {
        isJumping = true;
        bool jumpedFront = false;
        LineRenderer line = _bezier.m_lineRenderer;
        _follower.follow = false;

        _animationController.JumpAnimation(true);

        canJumpBetweenSplines = false;
        _bezier.UpdatePoints(transform.localPosition);
      
        if(mainProjector.result.percent >= 0.99)
        {
            mainProjector.spline = _follower.spline;
        }
        if (_front.isFront)
        {
            JumpToSide(_front);
            jumpedFront = true;
            vcam.Follow = this.transform;
        }

        if (_left.isLeft && desiredSide < 180)
        {
            onLeft = true;
            
            
            if(onRight)
            {
                vcam.Follow = this.transform;
                onRight = false;
            }
            else
            {
                vcam.Follow = _leftCamTransform;
            }
            JumpToSide(_left);
            OnJump.Invoke();
        }
        
        
        if(_right.isRight && desiredSide > 180) 
        {
            onRight = true;

            
            if(onLeft)
            {
                vcam.Follow = this.transform;
                onLeft = false;
            }
            else
            {
                vcam.Follow = _rightCamTransform;  //!!!hata!!!

            }
            JumpToSide(_right);
            OnJump.Invoke();
            
        }
        
        if (_top.isTop)
        {
            JumpToSide(_top);
        }
        _lastPoint.OnJump();
        _bezier.UpdatePoints(transform.localPosition);

        ResetSidePositions();
        SetSideColliderActivity(false);

        _right.isRight = false;
        _left.isLeft = false;
        _front.isFront = false;
        _top.isTop = false;

        _follower.spline = nextSpline;
        _follower.SetPercent(percent);
        _character.transform.LookAt(_lastPoint.transform.position);
        for (int i = 0; i < 100; i++)
        {
            transform.position = line.GetPosition(i);
            
            yield return new WaitForSeconds(0.01f);
            
        }
        _animationController.JumpAnimation(false);
        if (jumpedFront)
        {
            mainSpline = _follower.spline;
            mainProjector.spline = mainSpline;
            jumpedFront = false;
        }

        ShakeCamera(0.5f, .2f);
        
        _character.transform.LookAt(-transform.forward);
        _follower.follow = true;
        _character.transform.localEulerAngles = Vector3.zero;
        SetSideColliderActivity(true);
        isJumping = false;
        yield return null;
    }
    private void JumpToSide(Sides  selectedSide)
    {
        nextSpline = selectedSide._nextSpline;
        percent = selectedSide._projector.result.percent;
        selectedSide._follower.follow = true;
        selectedSide._follower.SetPercent(percent);
        selectedSide._follower.followSpeed = 0;
        selectedSide._follower.follow = false;
       

        _bezier.bezierPointListObjects[_bezier.bezierPointListObjects.Count - 1].transform.position = selectedSide._projector.targetObject.transform.position;
        _lastPoint.transform.position = selectedSide._projector.targetObject.transform.position;
        
    }
    
    void SetSideColliderActivity(bool col)
    {
        _front._collider.enabled = col;
        _left._collider.enabled = col;
        _right._collider.enabled = col;
    }
    IEnumerator JumpToMainSpline()
    {
        isJumping = true;
        mainProjector.SetPercent(mainProjector.result.percent + 0.10);
        _animationController.JumpAnimation(true);
        LineRenderer line = _bezier.m_lineRenderer;
        _follower.follow = false;
        _follower.spline = mainProjector.spline;

        _bezier.jumpToMain = true;

        if (mainProjector.result.percent >= 0.99)
        {
            mainProjector.spline = _follower.spline;
        }

        SetSideColliderActivity(false);
        _bezier.UpdatePoints(transform.position);
        _lastPoint.transform.position = mainProjector.targetObject.transform.position;
        _lastPoint.OnJump();
        _bezier.UpdatePoints(transform.position);
        _lastPoint.OnJump();
        OnJump.Invoke();
        for (int i = 0; i < 100; i++)
        {
            transform.position = line.GetPosition(i);

            yield return new WaitForSeconds(0.01f);

        }
        ShakeCamera(0.5f, .2f);

        _animationController.JumpAnimation(false);
        _follower.SetPercent(mainProjector.result.percent);
        _character.transform.LookAt(-transform.forward);
        _follower.follow = true;
        _character.transform.localEulerAngles = Vector3.zero;
        vcam.Follow = this.transform;
        _bezier.jumpToMain = false;
        SetSideColliderActivity(true);
        onNode = false;
        isJumping = false;
        yield return null;

    }
    void ResetSidePositions()
    {
        // if (_right.isRight)
        _right._follower.follow = false;
        _left._follower.follow = false;
        _front._follower.follow = false;
        _right.gameObject.transform.position = firstRightPos.position;
        // else if (_left.isLeft)
        _left.gameObject.transform.position = firstLeftPos.position;
        // else if (_front.isFront)
        _front.gameObject.transform.position = firstFrontPos.position;
        // else if (_top.isTop) _top.gameObject.transform.position = new Vector3(0, _front.gameObject.transform.position.y, _left.gameObject.transform.position.z - 8f);
    }
    public void ShakeCamera(float intensity ,float time)
    {
        CinemachineBasicMultiChannelPerlin perlin = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_AmplitudeGain = intensity;
        startingIntensity = intensity;
        shakeTimerTotal = time;
        shakeTimer = time;
    }

    void OnDeath()
    {
        _follower.follow = false;
        _rigidbody.isKinematic = false;
        _rigidbody.AddTorque(new Vector3(5f, 1f, 10f) * 8000f * Time.deltaTime, ForceMode.Impulse);
        _rigidbody.AddForce(Vector3.forward * 8000f * Time.deltaTime, ForceMode.Impulse);
        vcam.Follow = null;
        isDeath = true;
    }
    
    private void OnNodePassed(List<SplineTracer.NodeConnection> passed)
    {
        SplineTracer.NodeConnection nodeConnection = passed[0];
        
        double nodePercent = (double)nodeConnection.point / (_follower.spline.pointCount - 1);
        double followerPercent =_follower.UnclipPercent( _follower.result.percent);
        float distancePastNode = _follower.spline.CalculateLength(nodePercent, followerPercent);

        desiredSide = _character.transform.localEulerAngles.z;

        if (nodeConnection.node.name=="right" && desiredSide > 180)
        {
            StartCoroutine(CutFollow());
            onNode = true;
            Node.Connection[] connections = nodeConnection.node.GetConnections();
            int rnd = Random.Range(0, connections.Length);
            _follower.spline = connections[0].spline;
            double newNodePercent = (double)connections[0].pointIndex / (connections[0].spline.pointCount - 1);
            double newPercent = connections[0].spline.Travel(newNodePercent, distancePastNode, _follower.direction);
            _follower.SetPercent(newPercent);
            
        }
        if (nodeConnection.node.name == "left" && desiredSide < 180) // index node spline sırasına göre belirlenir
        {
            StartCoroutine(CutFollow());
            onNode = true;
            Node.Connection[] connections = nodeConnection.node.GetConnections();
            int rnd = Random.Range(0, connections.Length);
            _follower.spline = connections[0].spline;
            double newNodePercent = (double)connections[0].pointIndex / (connections[0].spline.pointCount - 1);
            double newPercent = connections[0].spline.Travel(newNodePercent, distancePastNode, _follower.direction);
            _follower.SetPercent(newPercent);

        }

    }
    IEnumerator CutFollow()
    {
        Transform current = vcam.Follow;
        Transform currentLook = vcam.LookAt;

        vcam.LookAt = null;
        vcam.Follow = null;
        yield return new WaitForSeconds(0.1f);

        vcam.Follow = current;
        vcam.LookAt = currentLook;

        yield return null;
    }

}
