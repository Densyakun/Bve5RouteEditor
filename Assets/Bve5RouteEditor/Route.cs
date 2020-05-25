using System.Collections.Generic;
using System.Text;

public class Route
{

    public string version;
    public Encoding encoding;
    public List<string> comments;
    //public string route;

    public Route()
    {
        comments = new List<string>();
    }
}
