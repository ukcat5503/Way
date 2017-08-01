using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

[RequireComponent (typeof(Text))]
/// <summary>
/// デバッグ用 FSPを表示するだけ
/// </summary>
public class DebugText : MonoBehaviour
{
    Text text;

    static Dictionary<string, string> outputMessage = new Dictionary<string, string>();

    void Start(){
        text = gameObject.GetComponent<Text>();
    }

    void Update(){
        string output = "";
        foreach (KeyValuePair<string, string> pair in outputMessage){
            output += String.Format("{0, -20}",pair.Key + ": ") + pair.Value + "\n";
        }
        text.text = output;
    }

    public static void RemoveInfo(string name){
        outputMessage.Remove(name);
    }

    public static void UpdateInfo(string key, string value){
        if (outputMessage.ContainsKey(key) ){
            outputMessage[key] = value;
        }else{
            outputMessage.Add(key, value);
        }
    }
}