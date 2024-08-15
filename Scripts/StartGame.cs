using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class StartGame : MonoBehaviour
{
    
    [SerializeField] BallController ballController;
    [SerializeField] GameObject startGround, ballCountText, scoreText,bonusSpawner,stopButton;
    [SerializeField] TextMeshProUGUI startText;

    bool gameStart = false;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0)&& !gameStart && !ballController.CanStop())
        {
            startGround.SetActive(false);
            ballController.GetComponent<Rigidbody2D>().isKinematic = true;
            ballController.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            ballController.transform.position =  new Vector2(-6f, 0f);
            startGround.SetActive(false);
            ballCountText.SetActive(true); 
            scoreText.SetActive(true);
            startText.text = "";
            bonusSpawner.SetActive(true);
            stopButton.SetActive(true);
            gameStart = true;
            Destroy(this.gameObject,1f);
        }
    }
}
