using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dreamteck.Splines;
public class PlayerController : MonoBehaviour
{
    Rigidbody playerBody;

    BoxCollider playerCollider;
    CapsuleCollider finalCollider;

    public InputController inputHandler;

    public Animator playerAnimator;

    int scoreCount;

    public TextMeshProUGUI scoreText;

    public GameController gameController;

    public PlayerGreet playerGreet;

    SplineFollower splineFollower;

    [SerializeField]
    GameObject pipe1, pipe2;

    [SerializeField]
    GameObject footStep1, footStep2;

    [SerializeField]
    float heightToReach;

    private void Awake()
    {
        GameController.GameisStarted += StartGame;
    }

    void Start()
    {
        splineFollower = transform.parent.GetComponent<SplineFollower>();
        splineFollower.follow = false;

        playerBody = GetComponent<Rigidbody>();
        playerCollider = GetComponent<BoxCollider>();
        finalCollider = GetComponent<CapsuleCollider>();

        isGameFinished = true;
    }


    void StartGame()
    {
        splineFollower.follow = true;
        isGameFinished = false;

        playerAnimator.SetBool("start", true);
    }


    float timer;

    private void Update()
    {
        if (!isGameFinished)
        {
            timer += Time.deltaTime;

            if (timer >= 0.1f)
            {
                scoreCount++;
                scoreText.text = scoreCount.ToString();
                timer = 0.0f;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isGameFinished)
        {
            transform.position += new Vector3(inputHandler.movementDir.x * 0.0025f, 0f,0f);

            Vector3 pos = transform.position;
            pos.x = Mathf.Clamp(pos.x, transform.parent.transform.position.x - 6f, transform.parent.transform.position.x + 6f);
            transform.position = pos;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pipe"))
        {
            //playerGreet.GreetPlayer();
            other.gameObject.SetActive(false);

            transform.position += Vector3.up * 0.9f;
            playerCollider.size = (playerCollider.size + new Vector3(0f, 1.2f, 0f)); 

            pipe1.transform.localScale += new Vector3(0f, 0f, 0.15f);
            pipe2.transform.localScale += new Vector3(0f, 0f, 0.15f);

            AddScoreBonus();

            playerGreet.GreetPlayer();
        }

        else if (other.gameObject.CompareTag("Scissor"))
        {
            other.gameObject.transform.parent = pipe1.transform;
            other.gameObject.transform.localPosition = Vector3.zero;

            scoreCount -= 100;
            scoreText.text = "SCORE\n" + scoreCount.ToString() + "\n +100";

            if (pipe1.transform.localScale.z > 0.3f)
            {
                transform.position -= Vector3.up * 0.9f;
                playerCollider.size = (playerCollider.size - new Vector3(0f, 1.2f, 0f));

                pipe1.transform.localScale -= new Vector3(0f, 0f, 0.15f);
                pipe2.transform.localScale -= new Vector3(0f, 0f, 0.15f);

                SimulatePipeCutting();
            }

            StartCoroutine(DisableObject(other.gameObject));
        }

        else if (other.gameObject.CompareTag("FinishLine"))
        {   
            gameController.LevelFinished(scoreCount);
            playerAnimator.SetBool("levelClear", true);           
        }

        else if (other.gameObject.CompareTag("Obstacle"))
        {
            if (!isGameFinished)
            {
                splineFollower.follow = false;

                if (transform.position.y >= heightToReach)
                {
                    finalCollider.enabled = true;
                    HideSticks();

                    playerBody.isKinematic = false;
                    playerBody.AddRelativeForce(new Vector3(0, -50f, 450f));
                }
                else
                {
                    playerAnimator.SetBool("fail", true);
                    gameController.LevelFailed();
                }

                isGameFinished = true;
            }
        }
        else if (other.gameObject.CompareTag("Object"))
        {
            splineFollower.follow = false;
            playerBody.freezeRotation = false;

            playerBody.isKinematic = false;
            HideSticks();

            playerBody.AddRelativeTorque(Vector3.right * 50f);
            playerBody.AddRelativeForce(Vector3.forward * 50f);

            playerAnimator.SetBool("fail", true);

            gameController.LevelFailed();

            isGameFinished = true;
        }
    }

    void SimulatePipeCutting()
    {
        GameObject tmp = Instantiate(pipe1.transform.GetChild(0).gameObject);
        tmp.transform.position = transform.position;
        tmp.AddComponent<BoxCollider>();
        tmp.AddComponent<Rigidbody>();      

        GameObject tmp2 = Instantiate(pipe2.transform.GetChild(0).gameObject);
        tmp2.transform.position = transform.position;
        tmp2.AddComponent<BoxCollider>();
        tmp2.AddComponent<Rigidbody>();      
    }


    void HideSticks()
    {
        pipe1.SetActive(false);
        pipe2.SetActive(false);

        footStep1.SetActive(false);
        footStep2.SetActive(false);
    }

    IEnumerator DisableObject(GameObject tmp)
    {
        yield return new WaitForSeconds(0.25f);
        tmp.SetActive(false);
    }

    void AddScoreBonus()
    {
        scoreCount += 100;
        scoreText.text = scoreCount.ToString() + "\n +100";
    }

    private void OnDisable()
    {
        GameController.GameisStarted -= StartGame;
    }

    bool isGameFinished;
}
