using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooltip : MonoBehaviour
{

    public string name = "";

    public List<string> text = new List<string>();
    public List<string> actions = new List<string>();

    public string Name { get => name; set => name = value; }


}
