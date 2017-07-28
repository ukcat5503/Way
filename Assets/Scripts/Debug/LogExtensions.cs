using System.Diagnostics;
using Debug = UnityEngine.Debug;

public static class ForLogExtensions
{
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
    /// 出力するオブジェクトの型によって出力形式を変えて出力する。
    /// </summary>
    /// <param name="t">出力したいオブジェクト</param>
    public static void Log(this object t){
        if(t is UnityEngine.Vector3){
            Debug.Log(string.Format(formatText("Blue"), "Vector3", t));
        }else if(t is UnityEngine.Quaternion){
            Debug.Log(string.Format(formatText("Yellow"), "Quaternion", t));
        }else if(t is string){
            Debug.Log(string.Format(formatText("Black"), "string", t));
        }else{
            Debug.Log(string.Format(formatText("#333333ff"), "Other", t));
        }
    }
}