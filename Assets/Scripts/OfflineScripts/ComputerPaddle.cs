using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerPaddle : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    private Ball _ball;

    private void Start()
    {
        _ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<Ball>();
    }
    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (_ball.transform.position.y > 0f)
        {
            float targetXPosition = Mathf.MoveTowards(transform.position.x, _ball.transform.position.x, _speed * Time.deltaTime);

            ClampPostition(ref targetXPosition);

            transform.position = new Vector3(targetXPosition, transform.position.y, transform.position.z);
        }
        else
            transform.position = new Vector3(Mathf.MoveTowards(transform.position.x, 0, _speed * Time.deltaTime), transform.position.y, transform.position.z);

    }

    private void ClampPostition(ref float xPosition)
    {
        xPosition = Mathf.Clamp(xPosition, -1.5f, 1.5f);
    }
}
