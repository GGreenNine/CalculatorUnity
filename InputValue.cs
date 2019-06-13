using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;

public class InputValue : MonoBehaviour
{
    public static string ResultInput;
    public static List<string> UIResultInput = new List<string>();
    public static List<string> ResList = new List<string>();
    public GameObject[] InverseButtons, ReverseButtons;

    public void Input(string value)
    {
        if (value != "" && value != "=" && value != "←" && value != "p" && value != "e")
        {
            ResList.Add(value);
        }

        switch (value)
        {
            case "=":
                ResultInput = string.Join("", ResList);
                CalcEngine.Instance.ResultText.text = CalcEngine.Instance.GeneralCalcMethod(ResultInput + "=").ToString();
                break;
            case "←":
                if (ResList.Any() && !CalcEngine.IsCalculated)
                {
                    ResList.RemoveAt(ResList.Count - 1);
                    UIResultInput.RemoveAt(UIResultInput.Count -1);
                }
                break;
            case "i":
                    // you can make Reverse Methods here
                break;
            case "CE":
                ResList.Clear();
                UIResultInput.Clear();
                CalcEngine.IsCalculated = false;
                break;
            case "p":
                ResList.Add("3,141");
                UIResultInput.Add("3,141"); ;
                break;
            case "e":
                ResList.Add("2.718");
                UIResultInput.Add("2,718");
                break;
            case "s":
                UIResultInput.Add("sin");
                break;
            case "c":
                UIResultInput.Add("cos");
                break;
            case"t":
                UIResultInput.Add("tan");
                break;
            case "f":
                UIResultInput.Add("n!");
                break;
            case "l":
                UIResultInput.Add("Log10");
                break;
            case "n":
                UIResultInput.Add("Log");
                break;
            case "q":
                UIResultInput.Add("Acos");
                break;
            case "w":
                UIResultInput.Add("Asin");
                break;
            case "y":
                UIResultInput.Add("Atan");
                break;
            case "$":
                UIResultInput.Add("*-1");
                break;
            default:
                UIResultInput.Add(value);
                break;

        }
    }
    public void RadDegSwitch()
    {
        if (CalcEngine.IsRad)
        {
            CalcEngine.IsRad = false;
            CalcEngine.Instance.RadDeg.text = "Deg";
        }
        else
        {
            CalcEngine.IsRad = true;
            CalcEngine.Instance.RadDeg.text = "Rad";
        }
            
    }



}
