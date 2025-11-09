using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FlyingEnemy : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _MovementRange = 5;

    [Serializable]
    private struct MovementValues
    {
        public float MaxSpeed;
        public float Acceleration;
        public float MaxAcceleration;
    }

    [SerializeField] private MovementValues _FlyingPhysics = new MovementValues();

    private float _currentHorizontalVelocity = 0.0f;
    private Rigidbody2D _rigidbody = null;
    private Vector2 _forceToAdd = Vector2.zero;

    [SerializeField] private bool _stunned = false;
    [SerializeField] private float stunTiming = 1f;


    // Start is called before the first frame update
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        _forceToAdd = Vector2.zero;

        if (!_stunned)
        {
            FlyingMouvement();
            _rigidbody.velocity += _forceToAdd;
        }
        else
        {
            _rigidbody.velocity = Vector2.zero;
        }

    }

    private void FlyingMouvement()
    {
        float maxSpeed = Mathf.Sin(Time.time * _FlyingPhysics.MaxSpeed) * _MovementRange;
        _currentHorizontalVelocity = Mathf.MoveTowards(_currentHorizontalVelocity, maxSpeed, _FlyingPhysics.Acceleration);
        float velocityDelta = _currentHorizontalVelocity - _rigidbody.velocity.x;
        velocityDelta = Mathf.Clamp(velocityDelta, -_FlyingPhysics.MaxAcceleration, _FlyingPhysics.MaxAcceleration);
        _forceToAdd.x += velocityDelta;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Touched");
        Bomb bomb = collision.GetComponent<Bomb>();

        if (bomb != null)
        {
            Destroy(bomb.gameObject);
            _stunned = true;

            Vector3 direction = bomb.transform.forward.normalized;

            _rigidbody.transform.position += new Vector3(bomb.KnockbackForce * (-1.0f), 0.0f, 0.0f);
            //_rigidbody.transform.position += new Vector3(bomb.KnockbackForce * direction.x, 0.0f, 0.0f);

            StartCoroutine(BombStun(stunTiming));
        }
    }

    private IEnumerator BombStun(float duration)
    {
        yield return new WaitForSeconds(duration);
        _stunned = false;
    }
}
