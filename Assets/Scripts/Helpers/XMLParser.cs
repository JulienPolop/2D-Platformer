using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    class XMLParser
    {
        public static string[] XMLtoArray(TextAsset TextToDisplay, string DialogueName)
        {
            XDocument myDoc = XDocument.Parse(TextToDisplay.text);

            XElement nodes = (from n in myDoc.Descendants()
                              where n.Name == DialogueName
                              select n).First();

            List<XElement> listElement = nodes.Descendants().ToList();

            List<string> texts = new List<string>();
            foreach (XElement element in listElement)
            {
                texts.Add(element.Value);
            }

            return texts.ToArray();
        }

    }
}
