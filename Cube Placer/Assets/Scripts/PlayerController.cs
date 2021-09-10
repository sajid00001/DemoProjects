using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dreamteck.Splines;
public class PlayerController : MonoBehaviour
{
    BoxCollider boxCollider;

    public InputController inputHandler;

    public float moveSpeed;

    public Animator playerAnimator;

    int scoreCount;

    public TextMeshProUGUI scoreText;

    public GameController gameController;

    public PlayerGreet playerGreet;

    SplineFollower splineFollower;

    public HolesRecord holePositionsRecord;
    private void Awake()
    {
        splineFollower = transform.parent.gameObject.GetComponent<SplineFollower>();

        if (PlayerPrefs.GetInt("Info") == 0)
        {
            StartCoroutine(ShowInfo());
        }
    }

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();

        isGameFinished = false;
        scoreText.text = "SCORE\n" + scoreCount.ToString();

        ResetCollider();
    }

    float timer;

    void ResetCollider()
    {
        boxCollider.size = new Vector3(0.75f, transform.childCount, 0.75f);
        float height = transform.childCount / 2f;
        boxCollider.center = new Vector3(0f,height + 0.5f , 0f); ;
    }


    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 0.1f && !isGameFinished)
        {
            scoreCount++;
            scoreText.text = scoreCount.ToString();
            timer = 0.0f;
        }
    }

    bool cubeSet = false;
    private void FixedUpdate()
    {
        transform.position += new Vector3(inputHandler.movementDir.x * moveSpeed, 0f, 0f);

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x,transform.parent.transform.position.x - 7f, 
            transform.parent.transform.position.x + 7f);
        transform.position = pos;

        for (int i = 0; i < holePositionsRecord.holePositions.Count;i++)
        {
            Vector3 holePos = holePositionsRecord.holePositions[i];
            if (Vector3.Distance(transform.position,holePos) < 0.75f)
            {
                if (cubeSet || transform.childCount <= 1)
                    return;
                
                if (transform.childCount > 1)
                {
                    transform.position = holePos;
                    GameObject tmp = transform.GetChild(1).gameObject;
                    //tmp.GetComponent<Rigidbody>().isKinematic = false;

                    LeanTween.moveLocalY(tmp, 0f, 0.25f);

                    tmp.transform.parent = null;

                    cubeSet = true;
                    StartCoroutine(ArrangePlayerAndCubes());
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("FinishLine"))
        {
            splineFollower.follow = false;

            playerAnimator.SetBool("levelClear", true);

            if (!isGameFinished)
            {
                gameController.LevelFinished(scoreCount);
                isGameFinished = true;
            }
            
            playerAnimator.SetBool("levelClear", true);            

        }
        else if (other.gameObject.CompareTag("Pole"))
        {
            Debug.Log("Detected Pole!");
            HitPole();
        }
    }


    public void HitPole()
    {
        if (transform.childCount > 1)
        {
            splineFollower.follow = false;

            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<Rigidbody>().isKinematic = false;
            }

            isGameFinished = true;

            gameController.LevelFailed();
        }        
    }


    IEnumerator ArrangePlayerAndCubes()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject tmp = transform.GetChild(i).gameObject;
            LeanTween.moveLocalY(tmp, tmp.transform.position.y - 1f, 0.25f);

            if (transform.childCount == 1)
                playerAnimator.SetBool("onFoot", true);
        }

        playerGreet.GreetPlayer();

        PlayerScored();

        ResetCollider();
        yield return new WaitForSeconds(0.25f);
        cubeSet = false;        
    }


    void PlayerScored()
    {
        scoreCount += 100;
        scoreText.text = scoreCount.ToString() + "\n" + "100";
    }



    public GameObject infoPanel, gamePanel;
    IEnumerator ShowInfo()
    {
        splineFollower.follow = false;

        gamePanel.SetActive(false);
        infoPanel.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        infoPanel.SetActive(false);
        gamePanel.SetActive(true);;

        splineFollower.follow = true;
        PlayerPrefs.SetInt("Info", 100);
    }

    bool isGameFinished;
}
