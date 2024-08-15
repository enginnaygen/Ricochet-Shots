using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class BallController : MonoBehaviour
{
    Rigidbody2D _rb;
    LineRenderer _lr;
    Camera _cam;
    Vector3 _direction, _startPos, _currentPos;
    Vector2 _mousePos;

    bool _onDrag;
    float _distance;


    [SerializeField] Animation IncreasecoreAnimation;
    [SerializeField] AdsManager adsManager;
    [SerializeField] AdsBanner adsBanner;
    //[SerializeField] AdsReward adsReward;
    [SerializeField] AdsRewardOld adsRewardOld;
    [SerializeField] TextMeshProUGUI scoreText, healthText, overBestScoreText, comboText, increaseScoreText;
    [SerializeField] float forceAdd = 10f;

    [field: SerializeField] public int BallCount { get; set; }
    public bool GameEnd { get; set; } = false;

    public bool EndDrag { get; set; }
    public bool StartDrag { get; set; }
    public bool CanShoot { get; set; }
    public int DeathCount { get; private set; }
    public int Score {  get; set; }
    public int BestScore { get; set; }
    public int Combo { get; set; }


    private void Start()
    {
        increaseScoreText.text = "";
        _mousePos = Vector2.zero;
        BallCount = 10;
        Score = 0;
        CanShoot = true;
        _cam = Camera.main;
        _lr = GetComponent<LineRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _lr.positionCount = 2;
        _lr.SetPosition(0, Vector2.zero);
        _lr.SetPosition(1, Vector2.zero);
        _lr.enabled = false;
        _rb.gravityScale = 1;

        if(PlayerPrefs.HasKey("BestScore"))
        {
            BestScore = PlayerPrefs.GetInt("BestScore");  
        }
        else
        {
            BestScore = 0;
        }
    }

    private void Update()
    { 
        _mousePos = _cam.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        scoreText.text = "Score: " + Score;
        healthText.text = "Ball x" + BallCount;
       
        if (Input.GetMouseButtonDown(0) && !_onDrag && CanShoot&&  !GameManager.Instance.GameStop&& !CanStop())
        {    
            StartDrag = true;
        }

        if (Input.GetMouseButtonUp(0) && _onDrag)
        {
            EndDrag = true;
        }
    }

    private void FixedUpdate()
    {
        if (StartDrag)
        {
            DragStart();
        }

        if (_onDrag)
        {
            Drag();
        }

        if (EndDrag)
        {
            DragEnd();
        }
    }


    private void DragStart()
    {
        _lr.SetPosition(0, transform.position);

        _startPos = _cam.ScreenToWorldPoint(Input.mousePosition);
        _rb.velocity = Vector3.zero;
        _rb.freezeRotation = true;
        _lr.enabled = true;
        _onDrag = true;
         StartDrag = false;
        _rb.isKinematic = true;
        _rb.gravityScale = 0;

    }
    private void Drag()
    {

        _currentPos = _cam.ScreenToWorldPoint(Input.mousePosition);
        _direction = (_startPos - _currentPos).normalized;
        if (_distance < 4f)
        {
            _lr.SetPosition(0, transform.position);
            _distance = Vector2.Distance(_startPos, _currentPos);
            _lr.SetPosition(1, transform.position + _direction * _distance);


        }
        else
        {
            _lr.SetPosition(0, transform.position);
            _distance = Math.Clamp(Vector2.Distance(_startPos, _currentPos), 0, 4f);
            _lr.SetPosition(1, transform.position + _direction * _distance);
        }
    }
    private void DragEnd()
    {
        _rb.isKinematic = false;
        _lr.enabled = false;
        Vector2 _force = _direction * forceAdd * _distance;
        _rb.freezeRotation = false;
        _onDrag = false;
        EndDrag = false;
        if (_distance < 1f) return; //yanlislikla el carpmalarina karsi
        CanShoot = false;
        _rb.gravityScale = 1;
        _rb.angularVelocity = UnityEngine.Random.Range(350f, 550f);
        _rb.AddForce(_force, ForceMode2D.Impulse);
        BallCount--;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            transform.position = new Vector2(-6f, 0f);
            _rb.velocity = Vector2.zero;
            _rb.freezeRotation = true;
            _rb.gravityScale = 0;
            CanShoot = true;
            Combo = 0;
            if (BallCount == 0)
            {
                DeathCount++;
                GameEnd = true;
                CanShoot = false;

                adsRewardOld.LoadRewardedAd();
                adsManager.ShowInterstitialAd();     
            }
        }

        if(collision.gameObject.tag=="StartGround")
        {
            _rb.velocity = new Vector2(0, 7f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Hoop")
        {
            if (Combo>1)
            {
                StartCoroutine(IncreaseScoreText());
                StopCoroutine(IncreaseScoreText());
            }
            else if(Combo<=1)
            {
                StartCoroutine(IncreaseScoreText());
                StopCoroutine(IncreaseScoreText());
            }
            BallCount++;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Hoop")
        {
            StartCoroutine(AfterBasket());
            StopCoroutine(AfterBasket());
        }
    }

    WaitForSeconds _afterBasketDelay = new WaitForSeconds(0.2f);
    IEnumerator AfterBasket()
    {
        yield return _afterBasketDelay;
        transform.position = new Vector2(-6f, 0f);
        _rb.velocity = Vector2.zero;
        _rb.freezeRotation = true;
        _rb.gravityScale = 0;
        CanShoot = true;
    }

    WaitForSeconds _increaseScoreDelay = new WaitForSeconds(5f / 6f);
    IEnumerator IncreaseScoreText()
    {   
        if(Combo<2)
        {
            increaseScoreText.text = "+" + 10;
            IncreasecoreAnimation.Play();
            yield return _increaseScoreDelay;
            increaseScoreText.text = "";
            IncreasecoreAnimation.Stop();
            Score += 10;
            Combo++;
            comboText.text = "x" + Combo;

            if (Score > BestScore)
            {
                BestScore = Score;
                PlayerPrefs.SetInt("BestScore", BestScore);
            }
        }
        else if(Combo>=2)
        {
            increaseScoreText.text = "+" + Combo * 10;
            IncreasecoreAnimation.Play();
            yield return new WaitForSeconds(5f/6f);
            increaseScoreText.text = "";
            IncreasecoreAnimation.Stop();
            Score += 10 * Combo;
            Combo++;
            comboText.text = "x" + Combo;

            if (Score > BestScore)
            {
                BestScore = Score;
                PlayerPrefs.SetInt("BestScore", BestScore);
            }

        }
    }

    public bool CanStop()
    {
       if( _mousePos.x > (_cam.aspect * _cam.GetComponent<Camera>().orthographicSize) - 3.5f && _mousePos.y> _cam.GetComponent<Camera>().orthographicSize - 2.5f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}