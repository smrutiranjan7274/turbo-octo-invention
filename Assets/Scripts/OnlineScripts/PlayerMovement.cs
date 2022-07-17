using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 0;

    private Rigidbody2D rb;
    //private float minXValue, maxXValue;
    private PhotonView photonView;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        photonView = GetComponent<PhotonView>();
    }

    void HandleMovement()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0);

            transform.position += movement * speed * Time.deltaTime;
        }

        int i = 0;
        while (i < Input.touchCount)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            if (touchPosition.y < 0f)
            {
                touchPosition.z = 0;
                if (PhotonNetwork.IsMasterClient)
                    touchPosition.y = -4f;
                else
                    touchPosition.y = 4f;

                transform.position = touchPosition;
            }
            ++i;
        }


        var pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x, -1.45f, 1.45f);
        transform.position = pos;
    }

    void FixedUpdate()
    {
        HandleMovement();
    }
}
