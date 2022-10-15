using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [SerializeField] private TextMeshProUGUI gameInfoText;
    [SerializeField] private TextMeshProUGUI playerScoreText;
    [SerializeField] private TextMeshProUGUI player2ScoreText;
    [SerializeField] private TextMeshProUGUI fpsText;

    [SerializeField] private TextMeshProUGUI _scoreResultText;

    [SerializeField] private int scoreToWin;

    [SerializeField] private float _playerPost;
    [SerializeField] private float _player2Post;

    [SerializeField] private GameObject _resultPanel;

    //public Text rr;

    private Ball _ball;

    private FPSCounter _fpsCounter;

    private int player1Score, player2Score;

    private void Awake()
    {
        // Set instance
        instance = this;

        if (SceneManager.GetActiveScene().buildIndex is 1 or 2)
        {
            // Assign objects
            _fpsCounter = GetComponent<FPSCounter>();
            _ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<Ball>();
        }

        // Set application target frame rate.
        Application.targetFrameRate = 90;

    }

    // Start is called before the first frame update
    void Start()
    {
        // Some stuff with screen refresh rates
        //Resolution[] resolutions = Screen.resolutions;

        //if (SceneManager.GetActiveScene().buildIndex == 0)
        //{
        //    Debug.Log(Screen.currentResolution.refreshRate);
        //    rr.text = Screen.currentResolution.refreshRate.ToString();
        //}
            
        
        // Print the resolutions
        //foreach (var res in resolutions)
        //{
        //    Debug.Log(res.width + "x" + res.height + " : " + res.refreshRate);
        //}

        player1Score = 0;
        player2Score = 0;

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
        //// For MainMenu - AutoPlay
        //if (SceneManager.GetActiveScene().buildIndex == 0)
        //{
        //    if (_ball.transform.position.y >= _player2Post)
        //    {
        //        _ball.Reset();
        //    }
        //    else if (_ball.transform.position.y <= _playerPost)
        //    {
        //        _ball.Reset();
        //    }
        //}

        // For Offline Modes
        if (SceneManager.GetActiveScene().buildIndex == 1 || SceneManager.GetActiveScene().buildIndex == 2)
        {

            fpsText.text = "FPS: " + _fpsCounter.FramesPerSec.ToString();

            // Check goal
            if (_ball.transform.position.y >= _player2Post)
            {
                player1Score++;
                _ball.Reset();
            }
            else if (_ball.transform.position.y <= _playerPost)
            {
                player2Score++;
                _ball.Reset();
            }

            // Update Score Texts
            playerScoreText.text = player1Score.ToString();
            player2ScoreText.text = player2Score.ToString();

            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                // check score
                if (player1Score == scoreToWin)
                {
                    _ball.gameObject.SetActive(false);
                    _scoreResultText.text = "You Won!";
                    //_resultPanel.SetActive(true);

                }
                else if (player2Score == scoreToWin)
                {
                    _ball.gameObject.SetActive(false);
                    _scoreResultText.text = "You Lose!";
                    //_resultPanel.SetActive(true);
                }
            }

            if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                if (player1Score == scoreToWin)
                {
                    _ball.gameObject.SetActive(false);
                    _scoreResultText.text = "Player 1 Won!";
                    _resultPanel.SetActive(true);

                }
                else if (player2Score == scoreToWin)
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

        Texture2D image = ScreenCapture.CaptureScreenshotAsTexture();
        NativeGallery.SaveImageToGallery(image, Application.productName, DateTime.Now.ToString("yy'-'MM'-'dd'-'hh'-'mm'-'ss"));

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
