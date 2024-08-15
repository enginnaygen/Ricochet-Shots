using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseHoop : MonoBehaviour
{
    Hoop _hoop;
    BallController _ballController;

    private void Start()
    {
        this.GetComponent<Collider2D>().enabled = true;
        this.GetComponent<SpriteRenderer>().enabled = true;
        _hoop = FindObjectOfType<Hoop>().GetComponent<Hoop>();
        _ballController = FindObjectOfType<BallController>().GetComponent<BallController>();
    }

    private void Update()
    {
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
            StartCoroutine(ReverseHoopp());
            StopCoroutine(ReverseHoopp());
        }
        if (collision.gameObject.tag == "Ground")
        {
            Destroy(this.gameObject);
        }
    }

    WaitForSeconds _reverseHoopDelay = new WaitForSeconds(1f);
    private IEnumerator ReverseHoopp()
    {
        _hoop.transform.localScale = new Vector2(0.75f, 0.75f);
        if (_hoop.transform.position.y > -1f)
        {
            _hoop.GetComponent<Rigidbody2D>().velocity = 2*Vector2.down;

            yield return _reverseHoopDelay;

            _hoop.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        if(_hoop.transform.position.x>8f)
        {
            _hoop.GetComponent<Rigidbody2D>().velocity = 2 * Vector2.left;

            yield return _reverseHoopDelay;

            _hoop.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        Destroy(this.gameObject);
    }
}
