using HmsPlugin;
using HuaweiMobileServices.Game;
using HuaweiMobileServices.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Rock : MonoBehaviour
{
    /*public Transform Target;
    public float firingAngle = 45.0f;
    public float gravity = 9.8f;

    public Transform Projectile;
    private Transform myTransform;

    public Vector3 offset;

    void Awake()
    {
        myTransform = transform;
    }

    public void ThrowRock()
    {
        StartCoroutine(SimulateProjectile());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ThrowRock();
        }
    }

    IEnumerator SimulateProjectile()
    {
        // Short delay added before Projectile is thrown
        yield return new WaitForSeconds(1.5f);

        // Move projectile to the position of throwing object + add some offset if needed.
        Projectile.position = myTransform.position + offset;

        // Calculate distance to target
        float target_Distance = Vector3.Distance(Projectile.position, Target.position);

        // Calculate the velocity needed to throw the object to the target at specified angle.
        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

        // Extract the X  Y componenent of the velocity
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

        // Calculate flight time.
        float flightDuration = target_Distance / Vx;

        // Rotate projectile to face the target.
        Projectile.rotation = Quaternion.LookRotation(Target.position - Projectile.position);

        float elapse_time = 0;

        while (elapse_time < flightDuration)
        {
            Projectile.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);

            elapse_time += Time.deltaTime;

            yield return null;
        }
    }*/

    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public CircleCollider2D col;
    public GameObject gameOverPanel;

    [HideInInspector] public Vector3 pos { get { return transform.position; } }
    Vector3 initialPos;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        initialPos = transform.position;
        DesactivateRb();
        HMSAchievementsManager.Instance.OnGetAchievementsListSuccess = OnGetAchievemenListSuccess;
        HMSLeaderboardManager.Instance.OnSubmitScoreSuccess = OnSubmitScoreSuccess;
        HMSLeaderboardManager.Instance.OnSubmitScoreFailure = OnSubmitScoreFailure;
        HMSLeaderboardManager.Instance.SubmitScore(HMSLeaderboardConstants.HitTheTargetGeneralLeaderboard, 99);
    }

    private void OnSubmitScoreFailure(HMSException obj)
    {
        Debug.Log("Submit Score failed. HMSException is: " + obj.GetBaseException());
    }

    private void OnSubmitScoreSuccess(ScoreSubmissionInfo obj)
    {
        Debug.Log("Submit Score succeed.");
    }

    public void Push(Vector2 force)
    {
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    public void ActivateRb()
    {
        rb.isKinematic = false;
    }

    public void DesactivateRb()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0f;
        rb.isKinematic = true;
    }

    void cloneRock()
    {
        if(GameManager.rockCount > 0)
        {
            var rock_clone = Instantiate(gameObject, initialPos, Quaternion.identity); //recreate rock
            GameManager.Instance.rock = rock_clone.GetComponent<Rock>();
            GameManager.Instance.isThrowing = false;
        }
        else
        {
            HMSLeaderboardManager.Instance.SubmitScore(HMSLeaderboardConstants.HitTheTargetGeneralLeaderboard, GameManager.score);
            HMSAchievementsManager.Instance.GetAchievementsList();
            GameManager.Instance.finalScoreText.text = "Score: " + GameManager.score;
            gameOverPanel.SetActive(true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            cloneRock();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D hitObject)
    {
        if (hitObject.tag == "Target")
        {
            GameManager.score++;
            GameManager.Instance.scoreText.text = "Score: " + GameManager.score;
        }
    }

    private void OnGetAchievemenListSuccess(IList<Achievement> achievementList)
    {
        Achievement beginnerScorer = achievementList.First(ach => ach.Id == HMSAchievementConstants.justTheBeginning); //Score of 6 is needed
        Achievement mediumScorer = achievementList[1]; //Score of 10 is needed
        Achievement masterScorer = achievementList[2]; //Score of 15 is needed

        if (GameManager.score >= 6 && beginnerScorer.State != 3)
        {
            HMSAchievementsManager.Instance.UnlockAchievement(HMSAchievementConstants.justTheBeginning);
            HMSAchievementsManager.Instance.RevealAchievement(HMSAchievementConstants.gettingGood);
        }
        else if (GameManager.score >= 10 && beginnerScorer.State == 3 && mediumScorer.State != 3)
        {
            HMSAchievementsManager.Instance.UnlockAchievement(HMSAchievementConstants.gettingGood);
            HMSAchievementsManager.Instance.RevealAchievement(HMSAchievementConstants.masterOfThrow);
        }
        else if (GameManager.score >= 15 && mediumScorer.State == 3 && masterScorer.State != 3)
        {
            HMSAchievementsManager.Instance.UnlockAchievement(HMSAchievementConstants.masterOfThrow);
        }
    }
}