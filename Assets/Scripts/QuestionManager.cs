using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class QuestionManager : MonoBehaviour
{
    public static QuestionManager instance;

    [System.Serializable]
    public class Question
    {
        public string questionText;
        public string[] answers; // must be 4 answers (A–D)
    }

    [System.Serializable]
    public class AnswerRecord
    {
        public string question;
        public string answerID;
        public string answerText;
    }

    [Header("Questions")]
    public List<Question> questions = new List<Question>();

    [Header("Scene References")]
    public AnswerBlock[] answerBlocks;

    [Header("UI")]
    public TextMeshProUGUI questionTextUI;

    public TextMeshProUGUI answerAUI;
    public TextMeshProUGUI answerBUI;
    public TextMeshProUGUI answerCUI;
    public TextMeshProUGUI answerDUI;

    private int currentQuestionIndex = 0;
    private bool questionAnswered = false;

    private List<AnswerRecord> results = new List<AnswerRecord>();

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        LoadQuestion(0);
    }

    public void SubmitAnswer(AnswerBlock block)
    {
        if (questionAnswered) return;

        questionAnswered = true;

        // Save result
        results.Add(new AnswerRecord
        {
            question = questions[currentQuestionIndex].questionText,
            answerID = block.answerID,
            answerText = block.answerText
        });

        Debug.Log("==================================");
        Debug.Log("Q" + (currentQuestionIndex + 1) +
                  " Selected: " + block.answerID + " (" + block.answerText + ")");
        Debug.Log("==================================");

        Invoke(nameof(NextQuestion), 1.5f);
    }

    void NextQuestion()
    {
        currentQuestionIndex++;

        if (currentQuestionIndex >= questions.Count)
        {
            ShowFullResults();
            return;
        }

        LoadQuestion(currentQuestionIndex);
    }

    void LoadQuestion(int index)
    {
        questionAnswered = false;

        Question q = questions[index];

        // Question UI
        if (questionTextUI != null)
            questionTextUI.text = q.questionText;

        // Answer UI labels
        if (answerAUI != null)
            answerAUI.text = "A: " + q.answers[0];

        if (answerBUI != null)
            answerBUI.text = "B: " + q.answers[1];

        if (answerCUI != null)
            answerCUI.text = "C: " + q.answers[2];

        if (answerDUI != null)
            answerDUI.text = "D: " + q.answers[3];

        Debug.Log("==================================");
        Debug.Log("QUESTION " + (index + 1) + ": " + q.questionText);
        Debug.Log("==================================");

        for (int i = 0; i < answerBlocks.Length; i++)
        {
            answerBlocks[i].ResetBlock();

            answerBlocks[i].answerID = ((char)('A' + i)).ToString();
            answerBlocks[i].answerText = q.answers[i];

            Debug.Log(answerBlocks[i].answerID + ": " + q.answers[i]);
        }
    }

    void ShowFullResults()
    {
        Debug.Log("========== TURING TEST COMPLETE ==========");

        for (int i = 0; i < results.Count; i++)
        {
            Debug.Log(
                "Q" + (i + 1) + ": " + results[i].question +
                " | Answer: " + results[i].answerID +
                " (" + results[i].answerText + ")"
            );
        }

        Debug.Log("==========================================");
    }
}