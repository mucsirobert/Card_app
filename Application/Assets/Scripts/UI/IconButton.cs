using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconButton : Button {

    public Image icon;

    public  Color normalIconColor = new Color(255, 255, 255, 255);
    public  Color pressedIconColor = new Color(255, 255, 255, 255);

    public Color disabledIconColor = new Color(255, 255, 255, 255);



    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        base.DoStateTransition(state, instant);

        if (icon != null)
        {
            if (state == SelectionState.Disabled)
            {
                icon.color = disabledIconColor;

            }
            else if (state == SelectionState.Pressed)
            {
                icon.color = pressedIconColor;
            }
            else //if (state == SelectionState.Normal)
            {
                icon.color = normalIconColor;
            }
        }
    }




}
