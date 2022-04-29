using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PlayerController : MonoBehaviour
{
    
    [SerializeField] GameObject canonPipe;
    [SerializeField] GameObject canonPipe2;
    [SerializeField] GameObject canonPipe3;
    [SerializeField] ParticleSystem[] canonParticle;
    [SerializeField] ParticleSystem playerParticle;

    bool began=true;


    RouteSwitch _bezierFollower;
    EnemyController _enemy;
    private void Awake()
    {
        _bezierFollower = GetComponent<RouteSwitch>();
        _enemy = FindObjectOfType<EnemyController>();

        _bezierFollower._follower.follow = false;
        transform.position = new Vector3(0, -90f, 158f);
        
    }
   
    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && began)
        {
            transform.DOMove(new Vector3(0, -92f, 156.5f), 0.4f);
            canonPipe.transform.DOScale(new Vector3(1.1f, 1f, 0.8f), 0.4f).OnComplete(()=>
            {
                canonPipe.transform.DOScale(new Vector3(1f, 1.1f, 1.1f), 0.1f);
                foreach (var p in canonParticle)
                {
                    p.Play();
                }
                playerParticle.Play();
                StartCoroutine(_bezierFollower.FirstJump());
            });
            canonPipe2.transform.DOScale(new Vector3(1.1f, 1f, 0.8f), 0.4f).OnComplete(() =>
            {
                canonPipe2.transform.DOScale(new Vector3(1f, 1.1f, 1.1f), 0.1f);
                StartCoroutine(_enemy.FirstJump());
            });
            canonPipe3.transform.DOScale(new Vector3(1.1f, 1f, 0.8f), 0.4f).OnComplete(() =>
            {
                canonPipe3.transform.DOScale(new Vector3(1f, 1.1f, 1.1f), 0.1f);

            });


            began = false;
        }
    }
    
}
