using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;
using TMPro;

class SerialHandler : MonoBehaviour
{
    // so recieve servo packets, and then write position values or whatnot
    [SerializeField]
    UnitySerialPort serial_port;

    [SerializeField]
    public TMP_Text serial_data;

    string prev_serial_data;

    void Update()
    {
        if (serial_data.text != prev_serial_data)
        {
            // parse it looking for a servo thing, if it exists then you should update the joint positions
            List<float> float_list = new List<float>();
            if (TryExtractFloats(serial_data.text, out float_list))
            {
                // this is valid joint data
            }

            prev_serial_data = serial_data.text;
        }
    }

    private List<float> GetServoCommands(string input)
    {
        string command_sig = "servo:";
        if (input.StartsWith(command_sig))
        {
            string numbersPart = input.Substring(command_sig.Length);
            string[] numberStrings = numbersPart.Split(',');

            floatList = new List<float>();

            foreach (string numberString in numberStrings)
            {
                if (float.TryParse(numberString, out float number))
                {
                    floatList.Add(number);
                }
                else
                {
                    Console.WriteLine($"Error converting '{numberString}' to float.");
                    return false; // Stop processing if a conversion error occurs
                }
            }

            return true; // Successfully extracted and converted numbers
        }

        return false; // String does not have the 'servo:' prefix
    }

    private string getServoSerialString(string header, float[] data)
    {
        string dataToSend = header + data[0].ToString("F2");
        for (int i = 1; i < data.Length; i++)
        {
            dataToSend += "," + data[i].ToString("F2");
        }
        return dataToSend;
    }

    public void sendResetSignal()
    {
        string reset_signal = "r";
        Debug.Log(reset_signal);
        serial_port.SendSerialDataAsLine(reset_signal);
    }

    public void sendInitSignal()
    {
        string serial_data = "v";
        Debug.Log(serial_data);
        serial_port.SendSerialDataAsLine(serial_data);
    }
};

