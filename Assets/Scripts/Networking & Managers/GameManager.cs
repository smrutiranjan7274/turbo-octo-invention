using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{

    public static GameManager instance { get; private set; }

    [SerializeField] private TextMeshProUGUI gameInfoText;
    [SerializeField] private TextMeshProUGUI playerScoreText;
    [SerializeField] private TextMeshProUGUI computerScoreText;
    [SerializeField] private TextMeshProUGUI fpsText;

    [SerializeField] private int scoreToWin;

    [SerializeField] private float _playerPost;
    [SerializeField] private float _computerPost;

    private Ball _ball;

    private FPSCounter _fpsCounter;

    private int playerScore, computerScore;

    private string refreshRate;

    private void Awake()
    {
        // Set instance
        instance = this;

        _fpsCounter = GetComponent<FPSCounter>();

        // Set Info texts.
        gameInfoText.text = Application.productName + " | " + SceneManager.GetActiveScene().name + " | v" + Application.version;
        var currResolution = Screen.currentResolution.ToString();
        refreshRate = (currResolution.Substring(currResolution.Length - 4)).Substring(0, 2);

        _ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<Ball>();

    }

    // Start is called before the first frame update
    void Start()
    {
        playerScore = 0;
        computerScore = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if ( SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (_ball.transform.position.y >= _computerPost)
            {
                playerScore++;
                _ball.Reset();
            }
            else if (_ball.transform.position.y <= _playerPost)
            {
                computerScore++;
                _ball.Reset();
            }                
        }

        if (SceneManager.GetActiveScene().buildIndex == 1 || SceneManager.GetActiveScene().buildIndex == 2)
        {
            Application.targetFrameRate = int.Parse(refreshRate);
            fpsText.text = "FPS: " + _fpsCounter.FramesPerSec.ToString();

            if (_ball.transform.position.y >= _computerPost)
            {
                playerScore++;
                _ball.Reset();
            }
            else if (_ball.transform.position.y <= _playerPost)
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

    public void LoadLevelWithDifficulty(float _difficulty)
    {
        PlayerPrefs.SetFloat("difficulty",_difficulty); 
        SceneManager.LoadScene(1);
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
