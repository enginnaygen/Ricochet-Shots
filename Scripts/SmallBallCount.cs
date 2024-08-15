using UnityEngine;

public class SmallBallCount : MonoBehaviour
{
    RectTransform _smallCount;
    BallController _ballController;

    void Start()
    {
        _smallCount = GetComponent<RectTransform>();
        _ballController = FindObjectOfType<BallController>().GetComponent<BallController>();
    }

    void Update()
    {
        if (_ballController.GameEnd && !GameManager.Instance.GameStop && !GameManager.Instance.StopRestart)
        {
            _smallCount.anchoredPosition = new Vector2(38f, 2500f);
        }
        if(GameManager.Instance.StopRestart)
        {
            _smallCount.anchoredPosition = new Vector2(38f, 2500f);
        }
    }
}
