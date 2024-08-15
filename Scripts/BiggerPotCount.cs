using UnityEngine;

public class BiggerPotCount : MonoBehaviour
{
    RectTransform _potCount;
    BallController _ballController;
    void Start()
    {
        _ballController = FindObjectOfType<BallController>().GetComponent<BallController>();
        _potCount = GetComponent<RectTransform>();
    }

    void Update()
    {
        if ( _ballController.GameEnd && !GameManager.Instance.GameStop && !GameManager.Instance.StopRestart)
        {
            _potCount.anchoredPosition = new Vector2(38f, 2500f);
        }
        if (GameManager.Instance.StopRestart)
        {
            _potCount.anchoredPosition = new Vector2(38f, 2500f);
        }
    }
}
