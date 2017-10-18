using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class DebugText : MonoBehaviour
{
    Text keyText;
    Text valueText;

    RectTransform valueRectTransform;
    static int textLength = 0;

    static Dictionary<string, string> outputMessage = new Dictionary<string, string>();

    void Start(){
        keyText =  gameObject.transform.Find("Key").GetComponent<Text>();
        valueText =  gameObject.transform.Find("Value").GetComponent<Text>();
        valueRectTransform = valueText.gameObject.GetComponent<RectTransform>();
    }

    void Update(){
        string outputKey = "";
        string outputValue = "";

        foreach (KeyValuePair<string, string> pair in outputMessage){
            outputKey += pair.Key + ":\n";
            outputValue += pair.Value + "\n";
        }
        keyText.text = outputKey;
        valueText.text = outputValue;

        valueRectTransform.localPosition = new Vector3((textLength + 3) * 30, -10, 0);
    }

    public static void RemoveInfo(string name){
        outputMessage.Remove(name);
    }

    public static void UpdateInfo<Type>(string key, Type value){
        if (outputMessage.ContainsKey(key)){
            outputMessage[key] = value.ToString();
        }else{
            outputMessage.Add(key, value.ToString());
            textLength = key.Length > textLength ? key.Length : textLength;
        }
    }

    /*
    public static void UpdateInfo(string key, int value){
        if (outputMessage.ContainsKey(key) ){
            outputMessage[key] = value.ToString();
        }else{
            outputMessage.Add(key, value.ToString());
            textLength = key.Length > textLength ? key.Length : textLength;
        }
    }

    public static void UpdateInfo(string key, float value){
        if (outputMessage.ContainsKey(key) ){
            outputMessage[key] = value.ToString();
        }else{
            outputMessage.Add(key, value.ToString());
            textLength = key.Length > textLength ? key.Length : textLength;
        }
    }
     */
}