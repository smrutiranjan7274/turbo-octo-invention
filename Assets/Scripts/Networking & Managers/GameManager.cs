using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{

    public static GameManager instance { get; private set; }

    [SerializeField] private TextMeshProUGUI gameInfoText;
    [SerializeField] private TextMeshProUGUI playerScoreText;
    [SerializeField] private TextMeshProUGUI computerScoreText;

    [SerializeField] private int scoreToWin;

    private Ball _ball;

    private int playerScore, computerScore;

    private void Awake()
    {
        // Set instance
        instance = this;

        // Set Info texts.
        gameInfoText.text = Application.productName + " | " + SceneManager.GetActiveScene().name + " | v" + Application.version;
        var currResolution = Screen.currentResolution.ToString();
        var refreshRate = (currResolution.Substring(currResolution.Length - 4)).Substring(0, 2);
        Application.targetFrameRate = int.Parse(refreshRate);

        if (SceneManager.GetActiveScene().buildIndex == 1)
            _ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<Ball>();

    }

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            playerScore = 0;
            computerScore = 0;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (_ball.transform.position.y >= 4.42f)
            {
                playerScore++;
                _ball.Reset();
            }
            else if (_ball.transform.position.y <= -4.42f)
            {
                computerScore++;
                _ball.Reset();
            }

            playerScoreText.text = playerScore.ToString();
            computerScoreText.text = computerScore.ToString();

            if (playerScore == scoreToWin || computerScore == scoreToWin)
                _ball.gameObject.SetActive(false);
        }

    }

    public void LoadLevel(int level)
    {
        SceneManager.LoadSceneAsync(level);
    }

    public void QuitGame()
    {
        Debug.Log("Bye!");
        Application.Quit();
    }
}
