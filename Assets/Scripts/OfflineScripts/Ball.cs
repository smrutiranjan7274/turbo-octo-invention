using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;
    public Vector2 _velocity { get; private set; }

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _resetTime;
    [SerializeField] private float _maxBounceAngle;
    [SerializeField] private float _speedIncreaseFactor;

    private bool _overridePosition;
    private int direction = 1;
    private float _initialSpped;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(ServeBall());
        _initialSpped = _moveSpeed;
        _speedIncreaseFactor = PlayerPrefs.GetFloat("difficulty") == 5 ? 1 : _speedIncreaseFactor;
    }

    private void FixedUpdate()
    {
        if (!_overridePosition)
        {
            rb.velocity = new Vector2(_velocity.x, direction * _velocity.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _moveSpeed += _speedIncreaseFactor * Time.fixedDeltaTime;
        if (collision.collider.tag == "Player")
            BounceFromPaddle(collision.collider);
        else
            Bounce(collision);
    }

    private void Bounce(Collision2D collision)
    {
        if (collision.collider.tag == "Walls")
            _velocity = new Vector2(-_velocity.x, _velocity.y);
        else
            _velocity = -_velocity;
    }

    private void BounceFromPaddle(Collider2D collider)
    {
        float colXExtent = collider.bounds.extents.x;
        float xOffset = transform.position.x - collider.transform.position.x;

        float xRatio = xOffset / colXExtent;
        float bounceAngle = _maxBounceAngle * xRatio * Mathf.Deg2Rad;

        Vector2 bounceDirection = new Vector2(Mathf.Sin(bounceAngle), Mathf.Cos(bounceAngle));

        bounceDirection.y *= Mathf.Sign(-_velocity.y);

        _velocity = bounceDirection * _moveSpeed;
    }

    private IEnumerator ServeBall()
    {
        yield return new WaitForSeconds(2);
        //_velocity = Vector2.up * _moveSpeed;
        _velocity = new Vector2(Random.Range(0, 2) == 0 ? -1 : 1, Random.Range(0, 2) == 0 ? -1 : 1) * _moveSpeed;
    }

    public void Reset()
    {
        StartCoroutine(ResetRoutine());
    }

    private IEnumerator ResetRoutine()
    {
        transform.position = Vector2.zero;
        rb.velocity = Vector2.zero;
        _moveSpeed = _initialSpped;
        direction = -direction;
        _overridePosition = true;
        yield return new WaitForSeconds(_resetTime);
        _overridePosition = false;
    }
}
