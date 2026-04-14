using UnityEngine;
using System.Collections;

public class AnswerBlock : MonoBehaviour
{
    [Header("Answer Settings")]
    public string answerID;   // A, B, C, or D
    public string answerText; // What the option says

    [Header("Bounce Settings")]
    public float bounceHeight = 0.2f;
    public float bounceSpeed = 10f;

    private bool hasBeenHit = false;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasBeenHit) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                // Hit from below (Mario-style)
                if (contact.normal.y > 0.5f)
                {
                    ActivateBlock();
                    break;
                }
            }
        }
    }

    void ActivateBlock()
    {
        hasBeenHit = true;

        Debug.Log("Selected: " + answerID + " (" + answerText + ")");

        StartCoroutine(Bounce());

        // Send selection to manager (optional system hook)
        QuestionManager.instance.SubmitAnswer(this);
    }

    public void DisableBlock()
    {
        hasBeenHit = true;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.color = Color.gray;
    }

    IEnumerator Bounce()
    {
        Vector3 upPos = startPos + Vector3.up * bounceHeight;

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * bounceSpeed;
            transform.position = Vector3.Lerp(startPos, upPos, t);
            yield return null;
        }

        t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * bounceSpeed;
            transform.position = Vector3.Lerp(upPos, startPos, t);
            yield return null;
        }
    }

    public void ResetBlock()
    {
        hasBeenHit = false;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.color = Color.white;
    }
}