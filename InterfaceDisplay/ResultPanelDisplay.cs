using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultPanelDisplay : MonoBehaviour
{
    [SerializeField] private MessageDisplay messageDisplay; // Reference to the message display component

    [Header("Corrections")]
    [SerializeField] private Text verticalCorrection; // Text displaying vertical correction
    [SerializeField] private Text horizontalCorrection; // Text displaying horizontal correction
    [SerializeField] private Text flightTime; // Text displaying flight time

    [Header("Markers")]
    [SerializeField] private int pixelsForMils = 30; // Pixels per mil used for marker positioning
    [SerializeField] private RectTransform corImgVer; // Vertical correction image
    [SerializeField] private RectTransform corImgHor; // Horizontal correction image
    [SerializeField] private RectTransform corImgCen; // Central correction image

    [Header("Table")]
    [SerializeField] private GameObject parent; // Parent object of the table
    [SerializeField] private GameObject columnPrefab; // Prefab for the table column

    private void Start()
    {
        // Subscribe to the event when calculation is finished
        Ballistics.OnCalculatingFinished += PointMassSimulation_OnCalculatingFinished;
    }

    private void PointMassSimulation_OnCalculatingFinished(List<ResultModel> results)
    {
        // Hide the info panel and handle the results
        // Assuming the fifth result exists
        //messageDisplay.HideInfoPanel();
        HandleResults(results[4]);
        CreateTable(results);
    }

    private void HandleResults(ResultModel result)
    {
        // Update the UI with result data
        verticalCorrection.text = "KLIKI: " + GetCorrection(result.y, true);
        horizontalCorrection.text = "KLIKI: " + GetCorrection(result.x, false);
        flightTime.text = $"{result.time:F2}s";

        // Update marker positions based on the result data
        corImgCen.localPosition = new Vector3(result.x, result.y, 0) * -pixelsForMils;
        corImgHor.localPosition = new Vector3(result.x, 0, 0) * -pixelsForMils;
        corImgVer.localPosition = new Vector3(0, result.y, 0) * -pixelsForMils;
    }

    public void CreateTable(List<ResultModel> results)
    {
        // Clear existing table columns
        foreach (Transform child in parent.transform)
        {
            Destroy(child.gameObject);
        }

        // Create new table columns based on the results
        for (int i = 0; i < results.Count + 1; i++)
        {
            GameObject column = Instantiate(columnPrefab, parent.transform);

            // Populate the table columns with data
            column.transform.GetChild(1).GetComponent<Text>().text = i == 0 ? "(m)" : results[i - 1].distance.ToString();
            column.transform.GetChild(2).GetComponent<Text>().text = i == 0 ? "↑↓" : GetCorrection(results[i - 1].y, true);
            column.transform.GetChild(3).GetComponent<Text>().text = i == 0 ? "←→" : GetCorrection(results[i - 1].x, false);
            column.transform.GetChild(4).GetComponent<Text>().text = i == 0 ? "V (m/s)" : results[i - 1].velocity.ToString();
            column.transform.GetChild(5).GetComponent<Text>().text = i == 0 ? "E (J):" : results[i - 1].energy.ToString();
        }
    }

    private string GetCorrection(float clicksfloat, bool vertical)
    {
        // Get correction string based on the number of clicks and direction
        int clicks = (int)Math.Round(clicksfloat);
        char arrow = vertical ? (clicks > 0 ? '↑' : '↓') : (clicks > 0 ? '→' : '←');
        return $"{Math.Abs(clicks)} {arrow}";
    }
}
