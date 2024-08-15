using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] AdsManager adsManager;
    //[SerializeField] AdsReward adsReward;
    public static GameManager  Instance;

    
    void Singleton()
    {
        if(Instance!=null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    [SerializeField] GameObject overPanelRewardAd,overPanel, stopPanel, stopButton, ballCountText, scoreText, comboText;
    [SerializeField] BallController ballController;
    [SerializeField] Hoop hoop;
    [SerializeField] BonusSpawner _bonusSpawner;
    [SerializeField] AudioSource audioSource;
    [SerializeField] Button soundButton;
    [SerializeField] Color yellow, red;
    [SerializeField] Transform ceil, leftWall, rightWall,background;
    [SerializeField] TextMeshProUGUI overPanelScoreTextAd, overPanelBestScoreTextAd,overPanelBestScoreText,overPanelScoreText;
    [SerializeField] RectTransform countTransformBiggerHoop,countTransformSmallBall;

    Camera _cam;
    LineRenderer _lr;
    float _ceilPos;


    public bool SoundOpen { get; set; } =true;
    public bool AddShow { get; set; } = false;
    public bool GameStop { get; private set; }
    public bool StopRestart { get;  set; }

    private void Awake()
    {
        Singleton();
        _cam = Camera.main;
        GameStop = false;
        StopRestart = false;
    }

    private void Start()
    {
        StartCoroutine(WallArrangenment());
        StopCoroutine(WallArrangenment());
    }

    private void Update()
    {
        if(ballController.Combo>=2)
        {
            comboText.SetActive(true);
        }
        if(ballController.Combo<2)
        {
            comboText.SetActive(false);
        }

        if (ballController.GameEnd && !GameStop && ballController.DeathCount%2==0)
        {
            OpenEndPanel();            
        }

        if (ballController.GameEnd && !GameStop && ballController.DeathCount % 2 == 1)
        {
            OpenEndPanelAd();
        }
    }

   
    public void ReStart()
    {
        ballController.GameEnd = false;
        StopRestart = true;
        ballController.StartDrag = false;
        _bonusSpawner.Time = 0f;
        _lr = FindObjectOfType<LineRenderer>().GetComponent<LineRenderer>();
        _lr.SetPosition(0, Vector2.zero);
        _lr.SetPosition(1, Vector2.zero);
        StartCoroutine(Wait());
        StopCoroutine(Wait());
        hoop.transform.position = new Vector2(6.68f, 1f);
        hoop.transform.localScale = new Vector2(-0.75f, 0.75f);
        ballController.transform.localScale = new Vector2(0.085f, 0.085f);
        countTransformBiggerHoop.anchoredPosition = new Vector2(38f, 2500f);
        countTransformSmallBall.anchoredPosition = new Vector2(38f, 2500f);

        GameStop = false;
        overPanel.SetActive(false);
        overPanelRewardAd.SetActive(false);
        stopButton.SetActive(true);
        scoreText.SetActive(true);
        ballCountText.SetActive(true);
        adsManager.LoadLoadInterstitialAd();
        ballController.CanShoot = true;
        ballController.BallCount = 10;
        ballController.Score = 0;
    }


    public void Quit()
    {
        Application.Quit();
    }

    public void SoundArrange()
    {
        if(SoundOpen)
        {
            audioSource.Pause();
            audioSource.volume = 0f;
            soundButton.GetComponent<Image>().color = red;
            SoundOpen = false;
        }
        else if (!SoundOpen)
        {
            audioSource.Play();
            audioSource.volume = 0.1f;
            soundButton.GetComponent<Image>().color = yellow;
            SoundOpen = true;        }
    }

    public void Continue()
    {
        ballController.StartDrag = false;
        stopPanel.SetActive(false);
        Time.timeScale = 1f;
        GameStop = false;
    }

    public void StopGame()
    {
        ballController.StartDrag = false;
        GameStop = true;
        ballController.StartDrag = false;
        Time.timeScale = 0;
        stopPanel.SetActive(true);
        adsManager.LoadLoadInterstitialAd();
    }

    public void StopReStart()
    {
        ballController.GameEnd = false;
        StopRestart = true;
        ballController.StartDrag = false;
        _bonusSpawner.Time = 0f;
        _lr = FindObjectOfType<LineRenderer>().GetComponent<LineRenderer>();
        _lr.SetPosition(0, Vector2.zero);
        _lr.SetPosition(1, Vector2.zero);
        StartCoroutine(Wait());
        StopCoroutine(Wait());
        ballController.Combo = 0;
        Time.timeScale = 1.0f;
        ballController.BallCount = 10;
        ballController.Score = 0;
        hoop.transform.position = new Vector2(6.68f, 1f);
        hoop.transform.localScale = new Vector2(-0.75f, 0.75f);
        ballController.transform.position= new Vector2(-6f, 0f);      
        ballController.transform.localScale = new Vector2(0.085f, 0.085f);
        ballController.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        ballController.GetComponent<Rigidbody2D>().gravityScale = 0f;
        ballController.GetComponent<Rigidbody2D>().freezeRotation = true;
        stopPanel.SetActive(false);
        GameStop = false;
        ballController.CanShoot = true;
        adsManager.ShowInterstitialAd();
    } 

    public void CloseEndPanel()
    {
        overPanelRewardAd.SetActive(false);
        scoreText.SetActive(true);
        ballCountText.SetActive(true);
        stopButton.SetActive(true);
    }

    public void OpenEndPanel()
    {
        overPanel.SetActive(true);
        overPanelScoreText.text = scoreText.GetComponent<TextMeshProUGUI>().text;
        overPanelBestScoreText.text = "Best Score: " + PlayerPrefs.GetInt("BestScore");
        scoreText.SetActive(false);
        //bestScoreText.SetActive(false);
        ballCountText.SetActive(false);
        stopButton.SetActive(false);
    }
    public void OpenEndPanelAd()
    {
        overPanelRewardAd.SetActive(true);
        overPanelScoreTextAd.text = scoreText.GetComponent<TextMeshProUGUI>().text;
        overPanelBestScoreTextAd.text = "Best Score: " + PlayerPrefs.GetInt("BestScore");
        scoreText.SetActive(false);
        //bestScoreText.SetActive(false);
        ballCountText.SetActive(false);
        stopButton.SetActive(false);
    }

    /*IEnumerator RewardAdd()
    {
        adsReward.LoadRewardedInterstitialAd();
        yield return new WaitForSeconds(3f);
        adsReward.ShowRewardedInterstitialAd();
    }*/

    IEnumerator WallArrangenment()
    {
        yield return new WaitForSeconds(1f);
        leftWall.transform.position = new Vector2(-_cam.aspect * _cam.orthographicSize - 0.5f, leftWall.transform.position.y);
        rightWall.transform.position = new Vector2(_cam.aspect * _cam.orthographicSize + 0.5f, rightWall.transform.position.y);
        _ceilPos = _cam.GetComponent<Camera>().orthographicSize;
        ceil.transform.position = new Vector2(ceil.transform.position.x, _ceilPos + 0.5f);
    }

    WaitForSeconds _delay = new WaitForSeconds(0.2f);
    IEnumerator Wait()
    {
        yield return _delay;
        StopRestart = false;
    }
}
