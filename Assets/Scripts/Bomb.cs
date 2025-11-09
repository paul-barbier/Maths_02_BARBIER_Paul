using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerCharacter;

public class Bomb : MonoBehaviour
{
    [Serializable]
    private struct MovementBomb
    {
        public float MaxSpeed;
        public float Acceleration;
        public float MaxAcceleration;
    }

    [Serializable]
    private struct GravityValues
    {
        public float MaxForce;
        public float Acceleration;
        public float MaxAcceleration;
        [Tooltip("Range [0, 1]")] public AnimationCurve GravityRemapFromCoyoteTime;
    }

    public bool IsGrounded { get; private set; } = true;

    [SerializeField] private MovementBomb _BombPhysics = new MovementBomb();
    [SerializeField] private GravityValues _gravityParameters = new GravityValues();

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

        BombMovement();

        _rigidbody.velocity += _forceToAdd;
    }

    public void BombExplosion()
    {

    }

    private void BombMovement()
    {
        // Je comprend quand je lis les lignes mais j'arrive pas à mettre en place ce que je veux faire 

        float maxSpeed = _BombPhysics.MaxAcceleration * Time.fixedDeltaTime;
        Vector2 force = new Vector2(maxSpeed, -5.0f);
        _rigidbody.AddForce(force, ForceMode2D.Impulse);

        float acceleration = _gravityParameters.Acceleration * Time.fixedDeltaTime;

        float maxGravityForce = _gravityParameters.MaxForce;
        _currentGravity = Mathf.MoveTowards(_currentGravity, maxGravityForce, acceleration);

        float velocityDelta = _currentGravity - _rigidbody.velocity.y;

        if (IsGrounded)
            _forceToAdd.y += velocityDelta * 0.5f; 

        _forceToAdd.y += velocityDelta;
    }
}
