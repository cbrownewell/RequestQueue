using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrototypeManager : MonoBehaviour
{
    [SerializeField]
    private Barracks barracks;

    [SerializeField]
    Image[] processingIcons;

    [SerializeField]
    TextMeshProUGUI processingLabel;

    [SerializeField]
    TextMeshProUGUI moneyLabel;

    [SerializeField]
    Image processBar;

    [SerializeField]
    Sprite blank;

    [SerializeField]
    int money;

    public void UpdateProcessingIcon(Sprite spritenew, int i) => processingIcons[i].sprite = spritenew;
    public void UpdateProcessingName(string newName) => processingLabel.text = newName;
    public void UpdateProgressBar(float value) => processBar.fillAmount = value;

    private void Awake()
    {
        RequestProcessor.QueueChange += UpdateUI;
        RequestProcessor.NewRequestAdded += RemoveCostForRequest;
        RequestProcessor.ProcessingTimeLeft += UpdateProgressBar;
        RequestProcessor.CancelExistingRequest += RefundRequest;
    }
    private void Start()
    {
        UpdateMoneyUI(money);
    }

    private void UpdateUI(QueueRequest request, int position)
    {
        if (request is null)
        {
            UpdateProcessingIcon(blank, position);
            if (position is 0)
            {
                UpdateProcessingName("");
                UpdateProgressBar(0);
            }
        }
        else
        {
            UpdateProcessingIcon(request.requestIcon, position);
            if (position is 0)
                UpdateProcessingName(request.requestName);
        }
    }

    private void UpdateMoneyUI(int newValue)
    {
        moneyLabel.text = "Money: " + newValue.ToString("N0");
    }

    void RemoveCostForRequest(QueueRequest request)
    {
        if (request is not null)
            SubtractMoney(request.requestCost);
    }

    void RefundRequest(QueueRequest request)
        => AddMoney(request.requestCost);
    void AddMoney(int value)
        => ModifyMoney(value);
    void SubtractMoney(int value)
        => ModifyMoney(-value);
    private void ModifyMoney(int value)
    {
        money += value;
        Debug.Log("Modifying money by: " + value);
        UpdateMoneyUI(money);
    }

}
