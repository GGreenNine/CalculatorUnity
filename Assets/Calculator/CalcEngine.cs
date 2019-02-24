using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

class CalcEngine : MonoBehaviour
{
    public static CalcEngine CEngine;

    public Text ResultText;
    public Text InputText;

    public Text RadDeg;
    public static bool IsCalculated;
    public static bool IsRad;
    public static bool IsInversed = false;

    private bool isReversed;

    private void Awake()
    {
        CEngine = this;
    }

    private void Update()
    {
        InputText.text = string.Join("", InputValue.UIResultInput);
    }
    /// <summary>
    /// Основной метод
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public float GeneralCalcMethod(string input)
    {
        string output = MakeRPN(input); //Обратная польская запись
        Debug.Log(output + "Output");
        float result = Calculate(output); //Находим значение выражения
        return result; // Результат

    }
    /// <summary>
    /// Преобразование входных данный в польскую запись
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private string MakeRPN(string input)
    {
        int closeBraket = 0; // кол-во закрытых скобок
        int openBraket = 0; //кол-во открытых скобок
        string output = null; //Строка выражения
        Stack<char> makeRPNStack = new Stack<char>(); //Стек операторов

        for (int i = 0; i < input.Length; i++) //Цикл для всех символов строки
        {
            //пропускаем = или пробел
            if (IsSpaceOrEqually(input[i]))
                continue; //Следующий символ

            //Если цифра
            if (Char.IsDigit(input[i]))
            {
                //Пока не встетим оператор/пробел/=
                while (!IsSpaceOrEqually(input[i]) && !IsOperator(input[i]))
                {
                    output += input[i]; //Добавляем цифру к строке выражения
                    i++; //След символ

                    if (i == input.Length) break; //Если строка закончилась заканчиваем
                }

                output += " "; //Дописываем после числа пробел в строку с выражением
                i--; //Возвращаемся на один символ назад, к символу перед разделителем
            }

            //Если символ - оператор
            if (IsOperator(input[i]))
            {
                switch (input[i])
                {
                    case '-':
                        if (makeRPNStack.Count > 0 && makeRPNStack.Peek() != ')' && IsOperator(makeRPNStack.Peek()))
                        {
                            input = input.Remove(i, 1);
                            var firstPart = input.Substring(0, i + 1);
                            firstPart += "$";
                            var secondPart = input.Substring(i + 1);
                            var newInput = firstPart + secondPart;
                            Debug.Log(newInput + "newInput");
                            var outputnew = MakeRPN(newInput);
                            Debug.Log(outputnew + " outputnew");
                            Debug.Log(Calculate(outputnew) + "CalcNew");
                            return outputnew;
                        }
                        break;
                }


                if (input[i] == '(') //Если открывающая скобка
                {
                    makeRPNStack.Push(input[i]);
                    openBraket++;
                }//Записываем в стек
                else if (input[i] == ')') //Если закрывающая скобка
                {
                    //Передаем все операторы до ( в строку
                    char s = makeRPNStack.Pop();
                    closeBraket++;
                    while (s != '(')
                    {
                        output += s.ToString() + ' ';
                        s = makeRPNStack.Pop();
                    }
                }
                else //Если другой оператор
                {
                    if (makeRPNStack.Count > 0) //Если стек не пустой
                    {
                        if (SetPriority(input[i]) <= SetPriority(makeRPNStack.Peek()))
                        { //Если приоритет входящего оператора ниже или равен последнему из стека
                            output += makeRPNStack.Pop().ToString() + " ";
                        }//Добавляем последний оператор из стека в строку с выражением
                    }
                    makeRPNStack.Push(input[i]); //Если стек пустой/приоретет выше то добавляем оператор в стек

                }
            }
        }

        //Когда прошли циклом по входящим данным, передаем из стека все операторы в строку
        if (closeBraket != openBraket)
        {

            throw new System.InvalidOperationException("Пропущена скобка");

        }

        while (makeRPNStack.Count > 0)
            output += makeRPNStack.Pop() + " ";

        return output; //Возвращаем строку выражения в польской записи Returning the string in Reverse polan notation
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
        float result = 0; //Результат
        Stack<float> calculateStack = new Stack<float>(); 

        for (int i = 0; i < input.Length; i++) 
        {
            //Store the digit in stack
            if (Char.IsDigit(input[i]))
            {
                string a = string.Empty;

                while (!IsSpaceOrEqually(input[i]) && !IsOperator(input[i]))
                {
                    a += input[i];
                    i++;
                    if (i == input.Length) break;
                }
                calculateStack.Push(float.Parse(a));
                i--;
            }
            else if (IsOperator(input[i])) //If operator
            {
                //2 last values from stack
                float a = calculateStack.Pop();
                float b = new float();
                if (input[i] != 's' && input[i] != 'c' && input[i] != 't' && input[i] != '√' && input[i] != 'f' && input[i] != 'l' && input[i] != 'n' && input[i] != 'q' && input[i]!='w' && input[i] != 'y' && input[i] != '$')
                    b = calculateStack.Pop();



                switch (input[i]) //
                {
                    case '+':
                        result = b + a;
                        break;
                    case '-':
                        result = b - a;
                        break;
                    case '*':
                        result = b * a;
                        break;
                    case '/':
                        result = b / a;
                        break;
                    case '^':
                        result = Mathf.Pow(b, a);
                        break;
                    case '√':
                        result = Mathf.Sqrt(a);
                        break;
                    case 'c':
                        if (IsRad) result = Mathf.Cos((float)a);
                        else result = Mathf.Cos((float)a * Mathf.Deg2Rad);
                        break;
                    case 's':
                        if (IsRad) result = Mathf.Sin((float)a);
                        else result = Mathf.Sin((float)a * Mathf.Deg2Rad);
                        break;
                    case 't':
                        if (IsRad) result = Mathf.Tan((float)a);
                        else result = Mathf.Tan((float)a * Mathf.Deg2Rad);
                        break;
                    case 'x':
                        result = b * Mathf.Pow(10, a);
                        break;
                    case 'f':
                        result = Factorial((int)a);
                        break;
                    case 'l':
                        result = Mathf.Log10(a);
                        break;
                    case 'n':
                        result = Mathf.Log(a);
                        break;
                    case 'q':
                        if (IsRad) result = Mathf.Acos((float)a);
                        else result = Mathf.Acos((float)a) * Mathf.Rad2Deg;
                        break;
                    case 'w':
                        if (IsRad) result = Mathf.Asin((float)a);
                        else result = Mathf.Asin((float)a) * Mathf.Rad2Deg;
                        break;
                    case 'y':
                        if (IsRad) result = Mathf.Atan((float)a);
                        else result = Mathf.Atan((float)a) * Mathf.Rad2Deg;
                        break;
                    case '$':
                        result = a*-1;
                        break;

                }
                calculateStack.Push(result); // Store the result
            }
        }
        IsCalculated = true;
        return calculateStack.Peek(); //Return the Result
    }

    static private bool IsSpaceOrEqually(char symbol)
    {
        if ((" =".IndexOf(symbol) != -1))
            return true;
        return false;
    }
    // if operator
    static private bool IsOperator(char c)
    {
        if (("+-/*^√cstxflnqwy$()".IndexOf(c) != -1))
            return true;

        return false;
    }
    //operators priority
    static private int SetPriority(char symbol)
    {
        switch (symbol)
        {
            case '(': return 0;
            case ')': return 1;
            case '+': return 2;
            case '-': return 3;
            case '*': return 4;
            case '/': return 4;
            case '^': return 5;
            case '√': return 5;
            case 's': return 5;
            case 'c': return 5;
            case 't': return 5;
            case 'x': return 4;
            case 'f': return 4;
            case 'l': return 5;
            case 'n': return 5;
            case 'q': return 5;
            case 'w': return 5;
            case 'y': return 5;
            case '$': return 4;
            default: return 5;
        }
    }
}