using UnityEngine;

public class Player2 : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _position;

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        int i = 0;
        while (i < Input.touchCount)
        {
            Touch touch = Input.GetTouch(i);
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            if (touchPosition.y > 0f)
            {
                float movement = touchPosition.x * _speed;
                float targetXPosition = movement;

                ClampPostition(ref targetXPosition);

                transform.position = new Vector3(targetXPosition, transform.position.y, transform.position.z);
            }
            ++i;
        }
    }

    private void ClampPostition(ref float xPosition)
    {
        xPosition = Mathf.Clamp(xPosition, -_position, _position);
    }
}
