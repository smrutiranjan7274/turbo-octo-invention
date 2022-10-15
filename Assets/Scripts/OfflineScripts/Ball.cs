using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;
    public Vector2 _velocity { get; private set; }

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _resetTime;
    [SerializeField] private float _maxBounceAngle;
    [SerializeField] private float _speedIncreaseFactor;
    [SerializeField] private Color[] colors;

    private bool _overridePosition;
    //private int direction = 1;
    private float _initialSpped;
    private float _time = 0;
    private float _lerpTime;
    private int _colorIndex = 0;
    private int _length = 0;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            _speedIncreaseFactor = PlayerPrefs.GetFloat("difficulty") == 5 ? 1 : _speedIncreaseFactor;
            _moveSpeed = PlayerPrefs.GetFloat("difficulty") == 5 ? 15 : _moveSpeed;
        }

        _initialSpped = _moveSpeed;
        rb = GetComponent<Rigidbody2D>();
        _length = colors.Length;
        StartCoroutine(ServeBall());
    }

    private void Update()
    {
        if (_spriteRenderer != null)
        {
            _spriteRenderer.color = Color.Lerp(_spriteRenderer.color, colors[_colorIndex], _lerpTime * Time.deltaTime);
            
            _time = Mathf.Lerp(_time, 1f, _lerpTime * Time.deltaTime);
            if(_time > .9f)
            {
                _time = 0;
                _colorIndex++;
                _colorIndex = (_colorIndex >= _length) ? 0 : _colorIndex;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!_overridePosition)
        {
            rb.velocity = new Vector2(_velocity.x, _velocity.y);
            _moveSpeed = _moveSpeed > 16.5f ? 17.0f : _moveSpeed;
            _lerpTime = (_lerpTime > 15f) ? 15.0f : _lerpTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _moveSpeed += _speedIncreaseFactor * Time.fixedDeltaTime;
        _lerpTime += _speedIncreaseFactor;
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
            BounceFromPaddle(collision.collider);
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
        _velocity = new Vector2(Random.Range(0, 2) == 0 ? -1 : 1, Random.Range(0, 2) == 0 ? -1 : 1) * _moveSpeed;
    }

    public void Reset()
    {
        StartCoroutine(ResetRoutine());
    }

    private IEnumerator ResetRoutine()
    {
        _overridePosition = true;
        transform.position = Vector2.zero;
        rb.velocity = Vector2.zero;
        _moveSpeed = _initialSpped;
        _lerpTime = 0;
        //direction = -direction;
        yield return new WaitForSeconds(_resetTime);
        _overridePosition = false;
    }
}
