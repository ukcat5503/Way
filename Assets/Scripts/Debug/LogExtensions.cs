using System.Diagnostics;
using Debug = UnityEngine.Debug;

/// <summary>
/// 整形したテキストをDebug.Logで出力します。
/// 実行環境では無効になり、関数の呼び出しも発生しません。
/// </summary>
public static class ForLogExtensions
{
    delegate void LogType(string s);
    
    /// <summary>
    /// テキストを決められたフォーマットで整形します。
    /// </summary>
    /// <param name="colorCode">Unityリッチテキストで使用できるカラー指定子、またはアルファ値込みカラーコード(#ffffffff)</param>
    /// <returns>整形後の文字列</returns>
    private static string formatText(string colorCode){
        return "<color=" + colorCode + "><b>[{0}]</b></color> {1}";
    }


    [Conditional("UNITY_EDITOR")]
    /// <summary>
    /// Debug.Logを実行します。
    /// 型によって自動的に色が設定されます。
    /// </summary>
    /// <param name="t">出力したいオブジェクト</param>
    public static void Log(this object t){
        output(t, Debug.Log);
    }

    [Conditional("UNITY_EDITOR")]
    /// <summary>
    /// Debug.LogWarningを実行します。
    /// 型によって自動的に色が設定されます。
    /// </summary>
    /// <param name="t">出力したいオブジェクト</param>
    public static void LogWarning(this object t){
        output(t, Debug.LogWarning);
    }

    [Conditional("UNITY_EDITOR")]
    /// <summary>
    /// Debug.LogErrorを実行します。
    /// 型によって自動的に色が設定されます。
    /// </summary>
    /// <param name="t">出力したいオブジェクト</param>
    public static void LogError(this object t){
        output(t, Debug.LogError);
    }

    [Conditional("UNITY_EDITOR")]
    /// <summary>
    /// 出力する根底のスクリプト
    /// </summary>
    /// <param name="t"></param>
    private static void output(this object t, LogType type){
        if(t is UnityEngine.Vector3){
            type(string.Format(formatText("Blue"), "Vector3", t));
        }else if(t is UnityEngine.Quaternion){
            type(string.Format(formatText("Yellow"), "Quaternion", t));
        }else if(t is string){
            type(string.Format(formatText("Black"), "string", t));
        }else{
            type(string.Format(formatText("#333333ff"), "Other", t));
        }
    }
}