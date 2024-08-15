using UnityEngine;

public class BonusSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] bonuses;
    Hoop _hoop;
    public float Time { get;  set; }
    GameObject _spawnObject;
    BallController _ballController;

    private void Start()
    {
        _hoop = FindObjectOfType<Hoop>().GetComponent<Hoop>();
        _ballController = FindObjectOfType<BallController>().GetComponent<BallController>();
    }

    private void Update()
    {
        if (_ballController.GameEnd) return;

        Time += UnityEngine.Time.deltaTime;       
        if (Time >= 18f)
        {
            _spawnObject = Instantiate(bonuses[Random.Range(0, bonuses.Length)], this.transform);
            _spawnObject.transform.position = new Vector2(Random.Range(-2f, _hoop.transform.position.x-1), Random.Range(5f, 9f));
            Time = 0;
        }
    }
}
