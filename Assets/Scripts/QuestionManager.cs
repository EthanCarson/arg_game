using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class QuestionManager : MonoBehaviour
{
    public static QuestionManager instance;

    // Persists across Scene 1, 2, and 3
    private static List<AnswerRecord> globalResults = new List<AnswerRecord>();

    [System.Serializable]
    public class Question
    {
        public string questionText;
        public string[] answers; 
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

    [Header("Audio Settings")]
    public AudioClip newQuestionSound;
    public AudioClip completionSound;
    private AudioSource audioSource;

    private int currentQuestionIndex = 0;
    private bool questionAnswered = false;

    void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // Reset data if we are starting fresh in Scene 1
        // (Assuming your first question scene is at Build Index 2)
        if (SceneManager.GetActiveScene().buildIndex == 2 && currentQuestionIndex == 0)
        {
            globalResults.Clear();
        }
    }

    void Start()
    {
        LoadQuestion(0);
    }

    public static List<AnswerRecord> GetGlobalResults() { return globalResults; }

    public void SubmitAnswer(AnswerBlock block)
    {
        if (questionAnswered) return;
        questionAnswered = true;

        globalResults.Add(new AnswerRecord
        {
            question = questions[currentQuestionIndex].questionText,
            answerID = block.answerID,
            answerText = block.answerText
        });

        Invoke(nameof(NextQuestion), 1.5f);
    }

    void NextQuestion()
    {
        currentQuestionIndex++;
        if (currentQuestionIndex >= questions.Count)
        {
            HandleSceneCompletion();
            return;
        }
        LoadQuestion(currentQuestionIndex);
    }

    void LoadQuestion(int index)
    {
        questionAnswered = false;
        if (audioSource != null && newQuestionSound != null) audioSource.PlayOneShot(newQuestionSound);

        Question q = questions[index];
        if (questionTextUI != null) questionTextUI.text = q.questionText;
        if (answerAUI != null) answerAUI.text = "A: " + q.answers[0];
        if (answerBUI != null) answerBUI.text = "B: " + q.answers[1];
        if (answerCUI != null) answerCUI.text = "C: " + q.answers[2];
        if (answerDUI != null) answerDUI.text = "D: " + q.answers[3];

        for (int i = 0; i < answerBlocks.Length; i++)
        {
            answerBlocks[i].ResetBlock();
            answerBlocks[i].answerID = ((char)('A' + i)).ToString();
            answerBlocks[i].answerText = q.answers[i];
        }
    }

    void HandleSceneCompletion()
    {
        if (audioSource != null && completionSound != null) audioSource.PlayOneShot(completionSound);

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        // If the next index in your Build Settings is a valid scene, go there.
        // This will naturally lead into your ResultsScene if it's placed after Scene 3.
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            Invoke(nameof(TransitionToNext), 2.0f);
        }
        else
        {
            Debug.Log("End of Build List reached.");
        }
    }

    void TransitionToNext()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}