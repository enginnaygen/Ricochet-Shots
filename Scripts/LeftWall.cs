using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftWall : MonoBehaviour
{
    [SerializeField] float wallPower=10f;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            collision.rigidbody.AddForce(new Vector2(wallPower, 0f), ForceMode2D.Impulse);
        }
    }
}
