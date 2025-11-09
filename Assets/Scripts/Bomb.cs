using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Serializable]
    private struct GravityBomb
    {
        public float MaxForce;
        public float Acceleration;
        public float MaxAcceleration;
    }

    [SerializeField] private GravityBomb _BombPhysics = new GravityBomb();

    private float _currentHorizontalVelocity = 0.0f;
    private Rigidbody2D _rigidbody = null;
    private Vector2 _forceToAdd = Vector2.zero;

    [Header("Bomb")]
    [SerializeField] public float KnockbackForce = 8f;
    [SerializeField] private float lifeTime = 3f;

    //Gravity
    private float _currentGravity = 0.0f;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        lifeTime -= Time.fixedDeltaTime;

        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }

        Gravity();

        _rigidbody.velocity += _forceToAdd;
    }

    public void BombExplosion()
    {

    }

    private void Gravity()
    {
        float acceleration = _BombPhysics.Acceleration * 1.0f * Time.fixedDeltaTime;

        float maxGravityForce = _BombPhysics.MaxForce;
        _currentGravity = Mathf.MoveTowards(_currentGravity, maxGravityForce, acceleration);

        float velocityDelta = _currentGravity - _rigidbody.velocity.y;

        velocityDelta = Mathf.Clamp(velocityDelta, -_BombPhysics.MaxAcceleration, 0.0f);

        _forceToAdd.y += velocityDelta;
    }
}
