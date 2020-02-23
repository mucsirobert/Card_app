using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconToggle : Toggle {

    public Image icon;

    public  Color normalIconColor;
    public  Color pressedIconColor;

    public Color disabledIconColor;

    protected override void Start()
    {
        base.Start();

        onValueChanged.AddListener(OnPressed);
    }

    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        base.DoStateTransition(state, instant);

        if (icon != null)
        {
            if (state == SelectionState.Disabled)
            {
                icon.color = disabledIconColor;

            }
            /*else if (state == SelectionState.Pressed)
            {
                if (isOn)
                {
                    icon.color = normalIconColor;
                }
                else
                {
                    icon.color = pressedIconColor;
                }
            }
            else if (state == SelectionState.Normal)
            {
                if (isOn)
                {
                    icon.color = normalIconColor;
                }
                else
                {
                    icon.color = pressedIconColor;
                }
            }*/
        }
    }

    private void OnPressed(bool pressed)
    {
        if (!pressed)
        {
            icon.color = pressedIconColor;
        } else
        {
            icon.color = normalIconColor;
        }
    }




}
