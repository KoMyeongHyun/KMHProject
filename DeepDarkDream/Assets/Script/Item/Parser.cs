using UnityEngine;
using System.Text;

public class Parser : MonoBehaviour
{
    protected bool loadingCompletion;
    public bool LoadingCompletion
    {
        get
        {
            return loadingCompletion;
        }
    }

    //XML을 다음과 같이 읽어 올 수도 있다.
    //TextAsset textXml = Resources.Load("파일경로", typeof(TextAsset)) as TextAsset;
    protected string GetPath(string _fileName)
    {
        StringBuilder path = new StringBuilder();
#if (UNITY_EDITOR || UNITY_STANDALONE_WIN)
        path.AppendFormat("file:///{0}/{1}", Application.streamingAssetsPath, _fileName);
#elif UNITY_ANDROID
        path.AppendFormat("jar:file://{0}!/assets/{1}", Application.dataPath, _fileName);
#endif

        return path.ToString();
    }
}
