using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InputValue : MonoBehaviour
{
    private static string _resultInput;
    private static readonly List<string> UiResultInput = new List<string>();
    private static readonly List<string> ResList = new List<string>();
    
    public Text ResultText;
    public Text InputText;
    public Text RadDeg;
    
    [FormerlySerializedAs("InverseButtons")] public GameObject[] inverseButtons;
    [FormerlySerializedAs("ReverseButtons")] public GameObject[] reverseButtons;

    
    private void UpdateText()
    {
        InputText.text = string.Join("", InputValue.UiResultInput);
    }
    
    public void Input(string value)
    {
        if (!"=←pe".Contains(value))
        {
            ResList.Add(value);
        }

        switch (value)
        {
            case "=":
                _resultInput = string.Join("", ResList);
                ResultText.text = CalcEngine.Instance.GeneralCalcMethod(_resultInput + "=").ToString();
                break;
            case "←":
                if (ResList.Any() && !CalcEngine.IsCalculated)
                {
                    ResList.RemoveAt(ResList.Count - 1);
                    UiResultInput.RemoveAt(UiResultInput.Count -1);
                }
                break;
            case "i":
                    // you can make Reverse Methods here
                break;
            case "CE":
                ResList.Clear();
                UiResultInput.Clear();
                CalcEngine.IsCalculated = false;
                break;
            case "p":
                ResList.Add("3,141");
                UiResultInput.Add("3,141"); ;
                break;
            case "e":
                ResList.Add("2.718");
                UiResultInput.Add("2,718");
                break;
            case "s":
                UiResultInput.Add("sin");
                break;
            case "c":
                UiResultInput.Add("cos");
                break;
            case"t":
                UiResultInput.Add("tan");
                break;
            case "f":
                UiResultInput.Add("n!");
                break;
            case "l":
                UiResultInput.Add("Log10");
                break;
            case "n":
                UiResultInput.Add("Log");
                break;
            case "q":
                UiResultInput.Add("Acos");
                break;
            case "w":
                UiResultInput.Add("Asin");
                break;
            case "y":
                UiResultInput.Add("Atan");
                break;
            case "$":
                UiResultInput.Add("*-1");
                break;
            default:
                UiResultInput.Add(value);
                break;

        }

        UpdateText();
    }
    public void RadDegSwitch()
    {
        if (CalcEngine.IsRad)
        {
            CalcEngine.IsRad = false;
            RadDeg.text = "Deg";
        }
        else
        {
            CalcEngine.IsRad = true;
            RadDeg.text = "Rad";
        }
            
    }



}
