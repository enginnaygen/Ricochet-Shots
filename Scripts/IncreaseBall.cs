using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseBall : MonoBehaviour
{
    BallController _ballController;

    private void Start()
    {
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
        if(collision.gameObject.tag=="Player")
        {
            _ballController.BallCount += 2;
            Destroy(this.gameObject);
        }
       if(collision.gameObject.tag=="Ground")
        {
            Destroy(this.gameObject);
        }
    }
}
