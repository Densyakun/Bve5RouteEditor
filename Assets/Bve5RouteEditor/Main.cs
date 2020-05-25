using SFB;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class Main : MonoBehaviour
{

    public Route route;

    void Start()
    {
        var paths = StandaloneFileBrowser.OpenFilePanel("Open scenario file", "", "", false);
        if (paths.Length > 0)
            StartCoroutine(CallGetWebRequest(new Uri(paths[0]).AbsoluteUri));
    }

    private IEnumerator CallGetWebRequest(string uri)
    {
        var request = UnityEngine.Networking.UnityWebRequest.Get(uri);
        yield return request.SendWebRequest();
        if (!request.isHttpError && !request.isNetworkError)
            loadScenario(request.downloadHandler.data);
    }

    public void loadScenario(byte[] data)
    {
        route = new Route();

        var t = Encoding.Default.GetString(data.TakeWhile(i => i != 10).ToArray());
        var a = t.Trim().Split(':');
        var e = Encoding.Default;
        if (a.Length > 1)
            try { e = Encoding.GetEncoding(a[1].Trim()); } catch { }
        t = e.GetString(data);

        a = a[0].Trim().Split(' ');
        route.version = a[a.Length - 1].Trim();

        {
            a = t.Split('\n');
            string l;
            for (var i = 1; i < a.Length; i++)
            {
                l = a[i];

                var c = l.IndexOf('#');
                c = c == -1 ? l.IndexOf(';') : Math.Min(c, l.IndexOf(';'));

                if (c != -1)
                {
                    route.comments.Add(l.Substring(c + 1));
                    l = l.Substring(0, c).Trim();
                }
                /*else
                    l = l.Trim();*/

                var b = l.IndexOf('=');
                if (b != -1)
                {
                    /*var n = l.Substring(0, b).Trim();
                    var r = l.Substring(b + 1).Trim();
                    if (n == "RouteTitle")
                        scenario.routeTitle = r;*/
                }
            }
        }

        {
            var l = new GameObject("Line").AddComponent<LineRenderer>();
            var p = new Vector3[] { Vector3.zero, Vector3.forward };
            l.SetPositions(p);
        }
    }

    public void save(Route route)
    {
        /*var path = StandaloneFileBrowser.SaveFilePanel("Save route file", "", "map.txt", "");
        if (!string.IsNullOrEmpty(path))
            File.WriteAllText(path, String.Join("\n", route.text));*/
    }
}
