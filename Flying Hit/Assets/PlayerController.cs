using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;
using TMPro;
public class PlayerController : MonoBehaviour
{

    Rigidbody playerBody;

    PathFollower pathFollower;

    public InputController inputHandler;

    public float moveSpeedX;

    public float moveSpeedZ;

    public Animator playerAnimator;

    int scoreCount;

    public TextMeshProUGUI scoreText;

    public GameController gameController;

    public PlayerGreet playerGreet;

    private void Awake()
    {
        pathFollower = GetComponent<PathFollower>();

        if (PlayerPrefs.GetInt("Info") == 0)
        {
            StartCoroutine(ShowInfo());
        }
    }

    void Start()
    {
        playerBody = GetComponent<Rigidbody>();

        isGameFinished = false;

        scoreText.text = "SCORE\n" + scoreCount.ToString();
    }

    float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 0.1f)
        {
            scoreCount++;
            scoreText.text = "SCORE\n" + scoreCount.ToString();
            timer = 0.0f;
        }
    }

    void FixedUpdate()
    {       
        if (isGameFinished == false)
        {
            pathFollower.offsetPos += (new Vector3(inputHandler.movementDir.x * 0.001f, inputHandler.movementDir.y * 0.001f, 0f));
            pathFollower.offsetPos.x = Mathf.Clamp(pathFollower.offsetPos.x, -7f, 7f);
            pathFollower.offsetPos.y = Mathf.Clamp(pathFollower.offsetPos.y, -7f, 7f);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("FinishLine"))
        {
            playerBody.transform.position = new Vector3(playerBody.transform.position.x, 0.6f, playerBody.transform.position.z);

            pathFollower.pathCreator = null;

            playerBody.isKinematic = true;

            if (!isGameFinished)
            {
                gameController.LevelFinished(scoreCount);
                isGameFinished = true;
            }
            
            playerAnimator.SetBool("levelClear", true);            

        }
        else if (other.gameObject.CompareTag("Ring"))
        {
            playerGreet.GreetPlayer();

            scoreCount += 100;
            scoreText.text = "SCORE\n" + scoreCount.ToString() + "\n +100";
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Ground"))
        {
            pathFollower.pathCreator = null;

            playerBody.isKinematic = true;

            isGameFinished = true;

            gameController.LevelFailed();
        }
    }

    public GameObject infoPanel, gamePanel;
    IEnumerator ShowInfo()
    {
        pathFollower.enabled = false;
        gamePanel.SetActive(false);
        infoPanel.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        infoPanel.SetActive(false);
        gamePanel.SetActive(true);
        pathFollower.enabled = true;

        PlayerPrefs.SetInt("Info", 100);
    }

    bool isGameFinished;
}
