using UnityEngine;

public class PlayerPaddle : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        int i = 0;
        while (i < Input.touchCount)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            if (touchPosition.y < 0f)
            {
                float movement = touchPosition.x * speed;
                float targetXPosition = movement;

                ClampPostition(ref targetXPosition);

                transform.position = new Vector3(targetXPosition, transform.position.y, transform.position.z);
            }
            ++i;
        }

    }

    private void ClampPostition(ref float xPosition)
    {
        xPosition = Mathf.Clamp(xPosition, -1.5f, 1.5f);
    }

}
