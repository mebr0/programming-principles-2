﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public delegate void invoker(string msg);

    public enum State
    {
        Zero,
        AccumulateDigits,
        AccumulateDigitsWithDecimal,
        ComputeWithPending,
        ComputeNoPending,
        ShowError
    }

    class Brain
    {
        string first;
        string second;
        string current;
        string result;
        string op;
        string save;

        bool isResult;
        bool isDecimal;

        public invoker invoker;
        public State state;

        string[] all = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        string[] nonzero = { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        string[] zero = { "0" };
        string[] equal = { "=" };
        string[] operations = { "+", "—", "×", "/" };
        string[] functions = { "%", "√", "x²", "1/x" };
        string[] reverse = { "±" };
        string[] clear = { "С" };
        string[] cleare = { "CE" };
        string[] backspace = { "⌫" };
        string[] separator = { "·" };
        string[] memory = { "MC", "MR", "M+", "M-", "MS" };

        public Brain()
        {
            first = "0";
            second = "0";
            current = "0";
            result = "0";
            op = "+";
            save = "0";
            isResult = false;
            isDecimal = false;
        }

        public void Process(string msg)
        {
            switch (state)
            {
                case State.Zero:
                    Zero(false, msg);
                    break;
                case State.AccumulateDigits:
                    AccumulateDigits(false, msg);
                    break;
                case State.AccumulateDigitsWithDecimal:
                    AccumulateDigitsWithDecimal(false, msg);
                    break;
                case State.ComputeNoPending:
                    ComputeNoPending(false, msg);
                    break;
                case State.ComputeWithPending:
                    ComputeWithPending(false, msg);
                    break;
                case State.ShowError:
                    ShowError(false, msg);
                    break;
            }
        }

        private void Zero(bool isInput, string msg)
        {
            if (isInput)
            {
                first = "0";
                second = "0";
                current = "0";
                result = "0";
                op = "+";
                invoker.Invoke(current);
                state = State.Zero;
            }
            else
            {
                if (nonzero.Contains(msg))
                {
                    AccumulateDigits(true, msg);
                }
                else if (separator.Contains(msg))
                {
                    AccumulateDigitsWithDecimal(true, msg);
                }
            }
        }

        private void AccumulateDigits(bool isInput, string msg)
        {
            if (isInput)
            {
                isDecimal = false;
                if (memory.Contains(msg)) Memory(msg);
                else if (isResult)
                {
                    isResult = false;
                    first = "0";
                    second = "0";
                    current = msg;
                    result = "0";
                    op = "+";
                }
                else
                {
                    if (current == "0") current = msg;
                    else current = current + msg;
                }
                invoker.Invoke(current);
                state = State.AccumulateDigits;
            }
            else
            {
                if (all.Contains(msg))
                {
                    AccumulateDigits(true, msg);
                }
                else if (separator.Contains(msg))
                {
                    AccumulateDigitsWithDecimal(true, msg);
                }
                else if (operations.Contains(msg))
                {
                    ComputeWithPending(true, msg);
                }
                else if (functions.Contains(msg))
                {
                    ComputeNoPending(true, msg);
                }
                else if (memory.Contains(msg))
                {
                    ComputeNoPending(true, msg);
                }
                else if (clear.Contains(msg))
                {
                    Zero(true, msg);
                }
                else if (cleare.Contains(msg))
                {

                }
                else if (equal.Contains(msg))
                {
                    try
                    {
                        ComputeNoPending(true, msg);
                    }
                    catch
                    {
                        ShowError(true, msg);
                    }
                }
            }
        }

        private void AccumulateDigitsWithDecimal(bool isInput, string msg)
        {
            if (isInput)
            {
                if (memory.Contains(msg)) Memory(msg);
                else if (all.Contains(msg))
                {
                    current = current + msg;
                }
                else
                {
                    current = current + ",";
                }
                invoker.Invoke(current);
                state = State.AccumulateDigitsWithDecimal;
            }
            else
            {
                if (all.Contains(msg))
                {
                    AccumulateDigitsWithDecimal(true, msg);
                }
                else if (operations.Contains(msg))
                {
                    ComputeWithPending(true, msg);
                }
                else if (memory.Contains(msg))
                {
                    ComputeNoPending(true, msg);
                }
                else if (clear.Contains(msg))
                {
                    Zero(true, msg);
                }
                else if (equal.Contains(msg))
                {
                    try
                    {
                        ComputeNoPending(true, msg);
                    }
                    catch
                    {
                        ShowError(true, msg);
                    }
                }
            }
        }

        private void ComputeWithPending(bool isInput, string msg)
        {
            if (isInput)
            {
                if (memory.Contains(msg)) Memory(msg);
                else
                {
                    if (operations.Contains(msg))
                    {
                        if(second != "0")
                        {
                            first = result;
                        }
                        else
                        {
                            first = current;
                        }
                        op = msg;
                        isResult = false;
                    }
                    current = "0";
                }
                invoker.Invoke(current);
                state = State.ComputeWithPending;
            }
            else
            {
                if (all.Contains(msg))
                {
                    AccumulateDigits(true, msg);
                }
                else if (memory.Contains(msg))
                {
                    ComputeNoPending(true, msg);
                }
                else if (clear.Contains(msg))
                {
                    Zero(true, msg);
                }
            }
        }

        private void ComputeNoPending(bool isInput, string msg)
        {
            if (isInput)
            {
                if (memory.Contains(msg)) Memory(msg);
                else if (functions.Contains(msg))
                {
                    Function(msg);
                    current = result;
                }
                else if (equal.Contains(msg))
                {
                    second = current;
                    Op();
                    current = result;
                }
                isResult = true;
                invoker.Invoke(current);
                state = State.ComputeNoPending;
            }
            else
            {
                if (zero.Contains(msg))
                {
                    Zero(true, msg);
                }
                else if (nonzero.Contains(msg))
                {
                    AccumulateDigits(true, msg);
                }
                else if (operations.Contains(msg))
                {
                    ComputeWithPending(true, msg);
                }
                else if (functions.Contains(msg) || memory.Contains(msg))
                {
                    ComputeNoPending(true, msg);
                }
                else if (clear.Contains(msg))
                {
                    Zero(true, msg);
                }

            }
        }

        private void ShowError(bool isInput, string msg)
        {
            if (isInput)
            {

                state = State.ShowError;
            }
            else
            {

            }
        }

        private void Memory(string msg)
        {
            switch (msg)
            {
                case "M+":
                    save = (double.Parse(save) + double.Parse(current)).ToString();
                    break;
                case "M-":
                    save = (double.Parse(save) - double.Parse(current)).ToString();
                    break;
                case "MS":
                    save = current;
                    break;
                case "MC":
                    save = "0";
                    break;
                case "MR":
                    current = save;
                    break;
            }
        }

        private void Op()
        {
            switch (op)
            {
                case "+":
                    result = (double.Parse(first) + double.Parse(second)).ToString();
                    break;
                case "—":
                    result = (double.Parse(first) - double.Parse(second)).ToString();
                    break;
                case "×":
                    result = (double.Parse(first) * double.Parse(second)).ToString();
                    break;
                case "/":
                    result = (double.Parse(first) / double.Parse(second)).ToString();
                    break;
            }
        }

        private void Function(string msg)
        {
            switch (msg)
            {
                case "√":
                    result = Math.Sqrt(double.Parse(current)).ToString();
                    break;
                case "x²":
                    result = Math.Pow(double.Parse(current), 2).ToString();
                    break;
                case "1/x":
                    result = (1 / double.Parse(current)).ToString();
                    break;
            }
        }
    }
}