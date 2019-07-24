using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

class CalcEngine : Singleton<CalcEngine>
{
    public static bool IsCalculated;
    public static bool IsRad;
    public static bool IsInversed = false;

    private bool isReversed;


    /// <summary>
    /// Основной метод
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public float GeneralCalcMethod(string input)
    {
        var output = GetReversePolarNotationString(input); 
        Debug.Log(output + "Output");
        var result = Calculate(output); 
        return result; 
    }
    /// <summary>
    /// Преобразование входных данный в польскую запись
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private string GetReversePolarNotationString(string input)
    {
        var closeBraket = 0; 
        var openBraket = 0;
        string output = null; 
        var reversePolarNotataionStack = new Stack<char>(); 

        for (int i = 0; i < input.Length; i++) 
        {
           
            if ((input[i]).SpaceEqual())
                continue; 

           
            if (char.IsDigit(input[i]))
            {
                //Пока не встетим оператор/пробел/=
                while (!(input[i]).SpaceEqual() && !(input[i]).IsSign())
                {
                    output += input[i]; 
                    i++; 

                    if (i == input.Length) break; 
                }

                output += " "; //Дописываем после числа пробел в строку с выражением
                i--; //Возвращаемся на один символ назад, к символу перед разделителем
            }
            
            if (!(input[i]).IsSign()) continue;
            switch (input[i])
            {
                case '-':
                    if (reversePolarNotataionStack.Count > 0 && reversePolarNotataionStack.Peek() != ')' && (reversePolarNotataionStack.Peek()).IsSign())
                    {
                        input = input.Remove(i, 1);
                        var firstPart = input.Substring(0, i + 1);
                        firstPart += "$";
                        var secondPart = input.Substring(i + 1);
                        var newInput = firstPart + secondPart;
                        var outputnew = GetReversePolarNotationString(newInput);
                        return outputnew;
                    }
                    break;
            }


            if (input[i] == '(') 
            {
                reversePolarNotataionStack.Push(input[i]);
                openBraket++;
            }//Записываем в стек
            else if (input[i] == ')') 
            {
                //Передаем все операторы до ( в строку
                char s = reversePolarNotataionStack.Pop();
                closeBraket++;
                while (s != '(')
                {
                    output += s.ToString() + ' ';
                    s = reversePolarNotataionStack.Pop();
                }
            }
            else 
            {
                if (reversePolarNotataionStack.Count > 0) 
                {
                    if (SetUpPriority(input[i]) <= SetUpPriority(reversePolarNotataionStack.Peek()))
                    { //Если приоритет входящего оператора ниже или равен последнему из стека
                        output += reversePolarNotataionStack.Pop().ToString() + " ";
                    } //Добавляем последний оператор из стека в строку с выражением
                }
                reversePolarNotataionStack.Push(input[i]); //Если стек пустой/приоретет выше то добавляем оператор в стек

            }
        }

        //Когда прошли циклом по входящим данным, передаем из стека все операторы в строку
        if (closeBraket != openBraket)
        {
            throw new System.InvalidOperationException("Пропущена скобка");
        }

        while (reversePolarNotataionStack.Count > 0)
            output += reversePolarNotataionStack.Pop() + " ";

        return output; 
    }
    /// <summary>
    /// Factorial calculation
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    int Factorial(int i)
    {
        if (i <= 1)
            return 1;
        return i * Factorial(i - 1);
    }

    /// <summary>
    /// // Calculating the result of string in RPN
    /// //Вычисляем значение выражения строки в польской записи
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private float Calculate(string input)
    {
        var stack = new Stack<float>(); 
        float calcResult = 0;

        for (var i = 0; i < input.Length; i++) 
        {
            if (char.IsDigit(input[i]))
            {
                var a = string.Empty;

                while (!(input[i]).SpaceEqual() && !(input[i]).IsSign() )
                {
                    a += input[i];
                    i++;
                    if (i == input.Length) break;
                }
                stack.Push(float.Parse(a));
                i--;
            }
            else if ((input[i]).IsSign() ) 
            {
                var a = stack.Pop();
                var b = new float();
                if (input[i] != 's' && input[i] != 'c' && input[i] != 't' && input[i] != '√' && 
                    input[i] != 'f' && input[i] != 'l' && input[i] != 'n' && input[i] != 'q' && 
                    input[i]!='w' && input[i] != 'y' && input[i] != '$')
                    b = stack.Pop();



                switch (input[i]) 
                {
                    case '-':
                        calcResult = b - a;
                        break;
                    case '+':
                        calcResult = b + a;
                        break;
                    case '^':
                        calcResult = Mathf.Pow(b, a);
                        break;
                    case '*':
                        calcResult = b * a;
                        break;
                    case '√':
                        calcResult = Mathf.Sqrt(a);
                        break;
                    case '/':
                        calcResult = b / a;
                        break;
                    case 'c':
                        calcResult = IsRad ? Mathf.Cos((float)a) : Mathf.Cos((float)a * Mathf.Deg2Rad);
                        break;
                    case 's':
                        calcResult = IsRad ? Mathf.Sin((float)a) : Mathf.Sin((float)a * Mathf.Deg2Rad);
                        break;
                    case 't':
                        calcResult = IsRad ? Mathf.Tan((float)a) : Mathf.Tan((float)a * Mathf.Deg2Rad);
                        break;
                    case 'x':
                        calcResult = b * Mathf.Pow(10, a);
                        break;
                    case 'f':
                        calcResult = Factorial((int)a);
                        break;
                    case 'l':
                        calcResult = Mathf.Log10(a);
                        break;
                    case 'n':
                        calcResult = Mathf.Log(a);
                        break;
                    case 'q':
                        if (IsRad) calcResult = Mathf.Acos((float)a);
                        else calcResult = Mathf.Acos((float)a) * Mathf.Rad2Deg;
                        break;
                    case 'w':
                        if (IsRad) calcResult = Mathf.Asin((float)a);
                        else calcResult = Mathf.Asin((float)a) * Mathf.Rad2Deg;
                        break;
                    case 'y':
                        if (IsRad) calcResult = Mathf.Atan((float)a);
                        else calcResult = Mathf.Atan((float)a) * Mathf.Rad2Deg;
                        break;
                    case '$':
                        calcResult = a*-1;
                        break;

                }
                stack.Push(calcResult); 
            }
        }
        IsCalculated = true;
        return stack.Peek(); 
    }

    private static int SetUpPriority(char c)
    {
        switch (c)
        {
            case '(': return 0;
            case ')': return 1;
            case '-': return 3;
            case '*': return 4;
            case '+': return 2;
            case '/': return 4;
            case '√': return 5;
            case '^': return 5;
            case 'c': return 5;
            case 's': return 5;
            case 'f': return 4;
            case 't': return 5;
            case 'x': return 4;
            case 'l': return 5;
            case 'w': return 5;
            case 'n': return 5;
            case 'q': return 5;
            case 'y': return 5;
            case '$': return 4;
            default: return 5;
        }
    }
}

public static class StringExtensions
{
    private static bool Contains(this String str, char substring)
    {                            
        if (substring == null)
            throw new ArgumentNullException("substring", 
                "substring cannot be null.");
        return str.IndexOf(substring) >= 0;                      
    }
    public static bool SpaceEqual(this char c)
    {
        return " =".Contains(c);
    }
    public static bool IsSign(this char c)
    {
        return "+-/*^√cstxflnqwy$()".Contains(c);
    }
}