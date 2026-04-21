using UnityEngine;
using TMPro;

public class ResultsDisplay : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI resultsText; // A large TextMeshPro area (ideally inside a ScrollView)

    void Start()
    {
        DisplayFinalResults();
    }

    void DisplayFinalResults()
    {
        // Access the static list from your QuestionManager
        var finalData = QuestionManager.GetGlobalResults();

        if (finalData.Count == 0)
        {
            resultsText.text = "No data recorded.";
            return;
        }

        resultsText.text = "<b>TURING TEST COMPLETE</b>\n\n";

        for (int i = 0; i < finalData.Count; i++)
        {
            resultsText.text += $"<b>Q{i + 1}:</b> {finalData[i].question}\n";
            resultsText.text += $"<color=#00FF00>Chosen: {finalData[i].answerID} - {finalData[i].answerText}</color>\n\n";
        }
    }
}