using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : ItemBase
{
    public Text text;


    public override void SetData(object data)
    {
        if (!string.IsNullOrEmpty((string)data))
            text.text = (string)data;
    }

}
