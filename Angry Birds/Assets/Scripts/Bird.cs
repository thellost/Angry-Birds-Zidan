using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bird : MonoBehaviour
{

    public UnityAction OnBirdDestroyed = delegate { };
    public UnityAction<Bird> OnBirdShot = delegate { };
    public enum BirdState { Idle, Thrown, HitSomething }
    public Rigidbody2D RigidBody;
    public CircleCollider2D Collider;
    public BirdState State;

    private float _minVelocity = 0.05f;
    private bool _flagDestroy = false;

    void Start()
    {
        RigidBody.bodyType = RigidbodyType2D.Kinematic;
        Collider.enabled = false;
        State = BirdState.Idle;
    }

    void FixedUpdate()
    {
        if (State == BirdState.Idle &&
            RigidBody.velocity.sqrMagnitude >= _minVelocity)
        {
            State = BirdState.Thrown;
        }

        if ( (State == BirdState.Thrown || State == BirdState.HitSomething )&&
            RigidBody.velocity.sqrMagnitude < _minVelocity &&
            !_flagDestroy)
        {
            //Hancurkan gameobject setelah 2 detik
            //jika kecepatannya sudah kurang dari batas minimum
            _flagDestroy = true;
            StartCoroutine(DestroyAfter(2));
        }

    }

    private IEnumerator DestroyAfter(float second)
    {
        yield return new WaitForSeconds(second);
        Destroy(gameObject);
    }

    public void MoveTo(Vector2 target, GameObject parent)
    {
        gameObject.transform.SetParent(parent.transform);
        gameObject.transform.position = target;
    }

    public void Shoot(Vector2 velocity, float distance, float speed)
    {
        Collider.enabled = true;
        RigidBody.bodyType = RigidbodyType2D.Dynamic;
        RigidBody.velocity = distance * speed * velocity;
        OnBirdShot(this);
    }

    void OnDestroy()
    {
        if (State == BirdState.Thrown || State == BirdState.HitSomething)
        {
            OnBirdDestroyed();
        }
    }
    public virtual void OnCollisionEnter2D(Collision2D col)
    {
        State = BirdState.HitSomething;
        Debug.Log("Hit something Debug");
    }

    
    public virtual void OnTap()
    {
        //Do nothing
    }
}
