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
    [SerializeField] private float _serveAngle;

    private bool _overridePosition;
    private int direction = 1;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(ServeBall());
    }

    private void FixedUpdate()
    {
        if(!_overridePosition)
            rb.velocity = direction * _velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
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
            _velocity = new Vector2(-_velocity.x,-_velocity.y);
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
        yield return new WaitForSeconds(3);
        _velocity =  new Vector2(Random.Range(-1.0f, 1.0f) == 0 ? -0.5f : +0.5f, Random.Range(-1, 1) == 0 ? 1:1) * _moveSpeed;
    }

    public void Reset()
    {
        StartCoroutine(ResetRoutine());
    }

    private IEnumerator ResetRoutine()
    {
        transform.position = Vector2.zero;
        rb.velocity = Vector2.zero;
        direction = -direction;
        _overridePosition = true;
        yield return new WaitForSeconds(_resetTime);
        _overridePosition = false;
    }
}
