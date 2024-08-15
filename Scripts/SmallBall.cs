using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class SmallBall : MonoBehaviour
{
    BallController _ballController;
    [SerializeField] RectTransform countTransform;
    [SerializeField] Text countText;
    float _time = 5f;

    void Start()
    {
        countTransform = FindObjectOfType<SmallBallCount>().GetComponent<RectTransform>();
        countText = FindObjectOfType<SmallBallCount>().GetComponent<Text>();
        this.GetComponent<Collider2D>().enabled = true;
        this.GetComponent<SpriteRenderer>().enabled = true;
        _ballController = FindObjectOfType<BallController>().GetComponent<BallController>();
    }

    void Update()
    {
        _time -= Time.deltaTime;
        countText.text = _time.ToString();

        if (_ballController.GameEnd && !GameManager.Instance.GameStop)
        {
            Destroy(this.gameObject);
        }
        if (GameManager.Instance.StopRestart)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            this.GetComponent<Collider2D>().enabled = false;
            this.GetComponent<SpriteRenderer>().enabled = false;
            countTransform.anchoredPosition = new Vector2(0f, 250f);
            _time = 9.9f;
            StartCoroutine(SmallBallMethod());
            StopCoroutine(SmallBallMethod());
        }
        if (collision.gameObject.tag == "Ground")
        {
            Destroy(this.gameObject);
        }
    }

    WaitForSeconds _smallBallDelay1 = new WaitForSeconds(9.8f);
    WaitForSeconds _smallBallDelay2 = new WaitForSeconds(0.1f);
    private IEnumerator SmallBallMethod()
    {
        _ballController.transform.localScale = new Vector2(0.055f, 0.055f);
        yield return _smallBallDelay1;
        countTransform.anchoredPosition = new Vector2(0f, 2500f);
        _time = 9.9f;
        _ballController.transform.localScale = new Vector2(0.085f, 0.085f);
        yield return _smallBallDelay2;
        Destroy(this.gameObject);

    }
}
