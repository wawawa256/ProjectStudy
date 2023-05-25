using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    [SerializeField] ObjectCollection objectCollection;

    [SerializeField] GameObject PrintfButton;
    [SerializeField] GameObject IfStartButton;
    [SerializeField] GameObject IfEndButton;
    [SerializeField] GameObject ForStartButton;
    [SerializeField] GameObject ForEndButton;
    [SerializeField] GameObject WhileStartButton;
    [SerializeField] GameObject WhileEndButton;
    [SerializeField] GameObject BreakButton;

    int ifFlag;
    int forFlag;
    int whileFlag;

    public void Start()
    {
        ButtonUnlock();

        ifFlag = 0;
        forFlag = 0;
        whileFlag = 0;
    }

    public void IfButtonClicked()
    {
        switch (ifFlag)
        {
            case 0:
                ButtonLock();
                IfEndButton.SetActive(true);
                ifFlag = 1;
                break;
            case 1:
                ButtonUnlock();
                ifFlag = 0;
                break;
        }
    }

    public void ForButtonClicked()
    {
        switch (forFlag)
        {
            case 0:
                ButtonLock();
                ForEndButton.SetActive(true);
                forFlag = 1;
                break;
            case 1:
                ButtonUnlock();
                forFlag = 0;
                break;
        }
    }

    public void WhileButtonClicked()
    {
        switch (whileFlag)
        {
            case 0:
                ButtonLock();
                WhileEndButton.SetActive(true);
                whileFlag = 1;
                break;
            case 1:
                ButtonUnlock();
                whileFlag = 0;
                break;
        }
    }

    void ButtonLock()
    {
        PrintfButton.SetActive(false);
        IfStartButton.SetActive(false);
        ForStartButton.SetActive(false);
        WhileStartButton.SetActive(false);
        BreakButton.SetActive(false);

        IfEndButton.SetActive(false);
        ForEndButton.SetActive(false);
        WhileEndButton.SetActive(false);
    }

    void ButtonUnlock()
    {
        PrintfButton.SetActive(true);
        IfStartButton.SetActive(true);
        ForStartButton.SetActive(true);
        WhileStartButton.SetActive(true);
        BreakButton.SetActive(true);

        IfEndButton.SetActive(false);
        ForEndButton.SetActive(false);
        WhileEndButton.SetActive(false);
    }
}
