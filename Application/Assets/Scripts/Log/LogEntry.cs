using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogEntry
{
    private LogEntryData data;

    public LogEntryData Data
    {
        get
        {
            return data;
        }
    }

    public string TypeText
    {
        get
        {
            return data.typeText;
        }
        set
        {
            data.typeText = value;
        }
    }
    public bool Active
    {
        get
        {
            return data.active;
        }
        set
        {
            data.active = value;
        }
    }
    public string Text
    {
        get
        {
            /* string t = string.IsNullOrEmpty(TypeText) ? "" : TypeText + ": ";
             if (Active)
                 return t + data.text;
             else
                 return t + "<i>" + data.text + "</i>";*/
            return data.Text;
        }
        set
        {
            data.Text = value;
        }
    }


    public LogEntry(string text)
    {
        Text = text;
        data.time = DateTime.Now.Ticks;
    }

    /*public LogEntry(LogEntryData data)
    {
        this.data = data;
    }*/

    public static string NormalLogEntryPart(string s)
    {
        Color textColor;
        string text;

        textColor = Color.white;
        text = s;


        return text;
    }

    public static string ZoneLogEntryPart(Zone zone)
    {
        Color textColor = zone.zoneColor;

        string text = "";
        text += "<b><color=#" + ColorUtility.ToHtmlStringRGBA(textColor) + ">";
        text += zone.zoneName;
        text += "</color></b>";
        return text;
    }

    public static string PlayerLogEntryPart(Player player)
    {

        string text = "";
        text += "<b>";
        text += player.playerName;
        text += "</b>";
        return text;
    }


}

public struct LogEntryData
{
    //These are public, because private fields don't get serielized through the netwrok
    public bool active;
    public long time;
    public string text;
    public string typeText;

    public string Text
    {
        get
        {
            string t = string.IsNullOrEmpty(typeText) ? "" : typeText + ": ";
            if (active)
                return t + text;
            else
                return t + "<i>" + text + "</i>";
        }
        set
        {
            text = value;
        }
    }

    public DateTime Time
    {
        get
        {
            return new DateTime(time);
        }
    }


    public LogEntryData(string text, string typeText, long time, bool active = false)
    {
        this.active = active;
        this.text = text;
        this.time = time;
        this.typeText = typeText;
    }
}
