using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BiggerHoop : MonoBehaviour
{
    [SerializeField] RectTransform countTransform;
    [SerializeField] Text countText;
    float _time = 5f;

    Hoop _hoop;
    BallController _ballController;

    private void Start()
    {
        countTransform = FindObjectOfType<BiggerPotCount>().GetComponent<RectTransform>();
        countText = FindObjectOfType<BiggerPotCount>().GetComponent<Text>();
        this.GetComponent<Collider2D>().enabled = true;
        this.GetComponent<SpriteRenderer>().enabled = true;
        _hoop = FindObjectOfType<Hoop>().GetComponent<Hoop>();
        _ballController = FindObjectOfType<BallController>().GetComponent<BallController>();
    }

    private void Update()
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
        if(collision.gameObject.tag=="Player")
        {
            this.GetComponent<Collider2D>().enabled = false;
            this.GetComponent<SpriteRenderer>().enabled = false;
            countTransform.anchoredPosition = new Vector2(0f, 250f);
            _time = 9.9f;
            StartCoroutine(BiggerHoopp());
            StopCoroutine(BiggerHoopp());

        }
        if(collision.gameObject.tag=="Ground")
        {
            Destroy(this.gameObject);
        }
    }

    WaitForSeconds _biggerHoopDelay1 = new WaitForSeconds(9.8f);
    WaitForSeconds _biggerHoopDelay2 = new WaitForSeconds(0.1f);
    private IEnumerator BiggerHoopp()
    {   if(_hoop.transform.localScale.x<0)
        {
            _hoop.transform.localScale = new Vector2(-0.95f, 0.95f);
        }
        else if(_hoop.transform.localScale.x>0)
        {
            _hoop.transform.localScale = new Vector2(0.95f, 0.95f);

        }
        yield return _biggerHoopDelay1;
        countTransform.anchoredPosition = new Vector2(0f, 2500f);
        _time = 9.9f;
        if (_hoop.transform.localScale.x < 0)
        {
            _hoop.transform.localScale = new Vector2(-0.75f, 0.75f);
        }
        else if (_hoop.transform.localScale.x > 0)
        {
            _hoop.transform.localScale = new Vector2(0.75f, 0.75f);

        }
        yield return _biggerHoopDelay2;
        Destroy(this.gameObject);
    }
}
