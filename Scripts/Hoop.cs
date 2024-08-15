using System.Collections;
using UnityEngine;

public class Hoop : MonoBehaviour
{
    Vector3 _newTransform;
    Rigidbody2D _rbHoop;


    private void Start()
    {
        _rbHoop = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            _newTransform = new Vector2(Random.Range(2f, 10f), Random.Range(1.4f, -2f));
            if(this.transform.localScale.x==0.75f)
            {
                this.transform.localScale = new Vector2(-0.75f, 0.75f);
            }
            else if (this.transform.localScale.x == -0.75f)
            {
                this.transform.localScale = new Vector2(-0.75f, 0.75f);
            }
            else if(this.transform.localScale.x==0.95f)
            {
                this.transform.localScale = new Vector2(-0.95f, 0.95f);

            }
            StartCoroutine(PotChange());
            StopCoroutine(PotChange());
        }
    }

    WaitForSeconds _potChangeDelay1 = new WaitForSeconds(0.6f);
    WaitForSeconds _potChangeDelay2 = new WaitForSeconds(1f);
    IEnumerator PotChange()
    {
        yield return _potChangeDelay1;
        Vector3 direction = (_newTransform - this.transform.position).normalized;
        _rbHoop.velocity = direction * 2;
        yield return _potChangeDelay2;
        _rbHoop.velocity = Vector3.zero;        
    }
}
