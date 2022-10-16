using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System;
using System.Text;
public class item
{
    public string no;
    public int no_int;
    public int frame = 0;
    public string question;
    public string answer;

    public item(int _i, int _f, string _q, string _a)
    {
        no = "s" + _i.ToString("0000");
        no_int = _i;
        frame = _f;
        question = _q;
        answer = _a;
    }
    public item(string _i, string _f, string _q, string _a)
    {
        int i = int.Parse(_i);

        no = "s" + i.ToString("0000");
        no_int = i;
        frame = int.Parse(_f);
        question = _q;
        answer = _a;
    }
    
    
}

public class CSVReader
{

    public static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    public static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    public static char[] TRIM_CHARS = { '\"' };

    public static List<Dictionary<string, object>> Read(string file)
    {
        Debug.Log(Resources.Load(file)==null);
        var list = new List<Dictionary<string, object>>();
        TextAsset data = Resources.Load(file) as TextAsset;
        Debug.Log(data.text);
        var lines = Regex.Split(data.text, LINE_SPLIT_RE);
        Debug.Log(lines.Length);
        if (lines.Length <= 1) return list;

        var header = Regex.Split(lines[0], SPLIT_RE);
        for (var i = 1; i < lines.Length; i++)
        {

            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;

            var entry = new Dictionary<string, object>();
            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                object finalvalue = value;
                int n;
                float f;
                if (int.TryParse(value, out n))
                {
                    finalvalue = n;
                }
                else if (float.TryParse(value, out f))
                {
                    finalvalue = f;
                }
                entry[header[j]] = finalvalue;
            }
            list.Add(entry);
        }
        return list;
    }
}


 
public class go_data : MonoBehaviour
{
    public AnimationClip ac;
    public Text questionText;
    public List<item> items = new List<item>();
    public List<item> items_long = new List<item>();
    public string _exp;
    // Start is called before the first frame update

    public int getFrame(string _no)
    {
        //Debug.Log(items.Count);
        int frame = 0;
        foreach (item i in items)
        {
             
            if (i.no_int.ToString() == _no)
            {

                //Debug.Log("getFrame " + _no + " " + i.frame);
                frame = i.frame;

            }

        }
        return frame;
    }
    void Start()
    {

 
    
        List<Dictionary<string, object>> data = CSVReader.Read("kimgu_csv");

        for (var i = 0; i < data.Count; i++)
        {
            //Debug.Log("index " + (i).ToString() + " : " + data[i]["id"] + " " + data[i]["frame"] + " " + data[i]["q"]);
            items.Add(
                new item( 
                    data[i]["id"].ToString(), 
                    data[i]["frame"].ToString(), 
                    data[i]["q"].ToString(), 
                    data[i]["a"].ToString()
                )
                );
            
        }
        List<Dictionary<string, object>> data_long = CSVReader.Read("kimgu_csv");

        for (var i = 0; i < data_long.Count; i++)
        {
            //Debug.Log("index " + (i).ToString() + " : " + data[i]["id"] + " " + data[i]["frame"] + " " + data[i]["q"]);
            items_long.Add(
                new item(
                    data_long[i]["id"].ToString(),
                    data_long[i]["frame"].ToString(),
                    data_long[i]["q"].ToString(),
                    data_long[i]["a"].ToString()
                )
                );
        }
        foreach(item _i in items)
        {
            //SaveTxt(_i.question, _i.answer);
        }
        //SaveTxt("a", "b");
    }
    public void QuestionRef()
    {
        List<item> q = items_long.OrderBy(arg => Guid.NewGuid()).Take(3).ToList();
        questionText.text = "- " + q[0].question + "\n- " + q[1].question + "\n- " + q[2].question;
    }
    public void QuestionRef(int s, int e)
    {
        List<int> l = new List<int>();
        for (int i = s; i <= e; i++) l.Add( i);
        List<int> q = l.OrderBy(arg => Guid.NewGuid()).Take(3).ToList();
        questionText.text = "- " + GetQuestion(q[0]) + "\n- " + GetQuestion(q[1]) + "\n- " + GetQuestion(q[2]);
    }
    public void QuestionRef(int s, int e, int s1, int e1)
    {
        List<int> l = new List<int>();
        for (int i = s; i <= e; i++) l.Add(i);
        //for (int j = s1; j <= e1; j++) l.Add(j);
        List<int> q = l.OrderBy(arg => Guid.NewGuid()).Take(3).ToList();
        questionText.text = "- " + GetQuestion(q[0]) + "\n- " + GetQuestion(q[1]) + "\n- " + GetQuestion(q[2]);
    }
    public void QuestionRef2(int s, int e)
    {
        List<int> l = new List<int>();
        for (int i = 1; i <= 478; i++) l.Add(items_long[i].no_int);
       
        List<int> q = l.OrderBy(arg => Guid.NewGuid()).Take(3).ToList();
        questionText.text = "- " + GetQuestion(q[0]) + "\n- " + GetQuestion(q[1]) + "\n- " + GetQuestion(q[2]);
    }
    string GetQuestion(int _i)
    {
        for(int i = 0; i < items.Count; i++)
        {
            if (items[i].no_int == _i) return items[i].question;
        }
        return "";
    }
    public void SaveTxt(string _q, string _a)
    {
        string text = File.ReadAllText("//cotax1/backup1/ktk/kimgu/facial2022/kimgu_agent2/temp.json");
        text = text.Replace("_txt01_", _q);
        text = text.Replace("_txt02_", _a);

        File.WriteAllText("D:/unity2022/kimkoo_agent/intents/a" + _a  + ".json", text);

        string text2 = File.ReadAllText("//cotax1/backup1/ktk/kimgu/facial2022/kimgu_agent2/temp_usersays_ko.json");
        text2 = text2.Replace("_txt03_", _q);
        File.WriteAllText("D:/unity2022/kimkoo_agent/intents/a" + _a + "_usersays_ko.json", text2);

        Debug.Log(text);
    }

}



