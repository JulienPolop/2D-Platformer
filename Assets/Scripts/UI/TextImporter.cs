using UnityEngine;
using System.Collections;

public class TextImporter : MonoBehaviour
{

    public TextAsset textfile;
    public string[] textlines;

    // Use this for initialization
    void Start()
    {
        if (textfile != null)
        {
            textlines = (textfile.text.Split('\n'));
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
