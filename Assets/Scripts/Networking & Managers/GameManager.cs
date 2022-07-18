using System;
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

    [SerializeField] private TextMeshProUGUI _scoreResultText;

    [SerializeField] private int scoreToWin;

    [SerializeField] private float _playerPost;
    [SerializeField] private float _computerPost;

    [SerializeField] private GameObject _resultPanel;

    private Ball _ball;

    private FPSCounter _fpsCounter;

    private int playerScore, computerScore;

    private string refreshRate;

    private void Awake()
    {
        // Set instance
        instance = this;

        // Set target framerates to screen's max refresh rate
        var currResolution = Screen.currentResolution.ToString();
        refreshRate = (currResolution.Substring(currResolution.Length - 4)).Substring(0, 2);
        Application.targetFrameRate = int.Parse(refreshRate);

        // Assign objects
        _fpsCounter = GetComponent<FPSCounter>();
        _ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<Ball>();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerScore = 0;
        computerScore = 0;

        //Disable screen dimming
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        gameInfoText.text = Application.productName + " | " + SceneManager.GetActiveScene().name + " | v" + Application.version;

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            // Get & set difficulty level 
            string difficultyLevel = "";
            if (PlayerPrefs.GetFloat("difficulty") == 1.5)
                difficultyLevel = "Easy";
            else if (PlayerPrefs.GetFloat("difficulty") == 2)
                difficultyLevel = "Medium";
            else if (PlayerPrefs.GetFloat("difficulty") == 5)
                difficultyLevel = "Insane";

            // Set ScoreToWin according to Difficulty
            scoreToWin = PlayerPrefs.GetFloat("difficulty") == 5 ? 10 : 5;

            // Set Info texts.
            gameInfoText.text = Application.productName + " | " + SceneManager.GetActiveScene().name + " | " + difficultyLevel;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // For MainMenu - AutoPlay
        if (SceneManager.GetActiveScene().buildIndex == 0)
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

        // For Offline Modes
        if (SceneManager.GetActiveScene().buildIndex == 1 || SceneManager.GetActiveScene().buildIndex == 2)
        {

            fpsText.text = "FPS: " + _fpsCounter.FramesPerSec.ToString();

            // Check goal
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

            // Update Score Texts
            playerScoreText.text = playerScore.ToString();
            computerScoreText.text = computerScore.ToString();

            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                // check score
                if (playerScore == scoreToWin)
                {
                    _ball.gameObject.SetActive(false);
                    _scoreResultText.text = "You Won!";
                    _resultPanel.SetActive(true);

                }
                else if (computerScore == scoreToWin)
                {
                    _ball.gameObject.SetActive(false);
                    _scoreResultText.text = "You Lose!";
                    _resultPanel.SetActive(true);
                }
            }

            if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                if (playerScore == scoreToWin)
                {
                    _ball.gameObject.SetActive(false);
                    _scoreResultText.text = "Player 1 Won!";
                    _resultPanel.SetActive(true);

                }
                else if (computerScore == scoreToWin)
                {
                    _ball.gameObject.SetActive(false);
                    _scoreResultText.text = "Player 2 Won!";
                    _resultPanel.SetActive(true);
                }
            }
        }
    }

    public void LoadLevelWithDifficulty(float _difficulty)
    {
        PlayerPrefs.SetFloat("difficulty", _difficulty);
        SceneManager.LoadScene(1);
    }

    public void LoadLevel(int level)
    {
        SceneManager.LoadSceneAsync(level);
    }


    public void TakeScreenShot()
    {
        /*
         * ScreenCapture.CaptureScreenshot(GetAndroidExternalStoragePath() + "/" + Application.productName + "-" + DateTime.Now.ToString("yy'-'MM'-'dd'-'hh'-'mm'-'ss"));
         * ScreenCapture.CaptureScreenshot(Application.persistentDataPath + Application.productName + "-" + DateTime.Now.ToString("yy'-'MM'-'dd'-'hh'-'mm'-'ss"));
         * ScreenCapture.CaptureScreenshot(Application.persistentDataPath + "/" + Application.productName + "-" + DateTime.Now.ToString("yy'-'MM'-'dd'-'hh'-'mm'-'ss"));
         */

        NativeGallery.SaveImageToGallery(ScreenCapture.CaptureScreenshotAsTexture(), Application.productName, DateTime.Now.ToString("yy'-'MM'-'dd'-'hh'-'mm'-'ss"));
    }

    /*
 
    private string GetAndroidExternalStoragePath()
    {
        if (Application.platform != RuntimePlatform.Android)
            return Application.persistentDataPath;

        var jc = new AndroidJavaClass("android.os.Environment");
        var path = jc.CallStatic<AndroidJavaObject>("getExternalStoragePublicDirectory", jc.GetStatic<string>("DIRECTORY_DCIM")).Call<string>("getAbsolutePath");
        return path;
    }

    */

    public void QuitGame()
    {
        Debug.Log("Bye!");
        Application.Quit();
    }
}
