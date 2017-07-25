/*  ログの色を設定するstatic関数群
 *  実行環境では必要がない。
 *  
 *  使用する色は以下の通り
 *  Load(blue): プレハブなど 何かをロードしたときのログに使用。
 * 
 */
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogPlus : MonoBehaviour {
    public static void Load(string message){
        Debug.Log("<color=blue>[Load]</color> " + message);
    }
}