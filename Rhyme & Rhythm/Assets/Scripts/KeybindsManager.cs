using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeybindsManager : MonoBehaviour
{
    

    private string key1 = "E";
    private string key2 = "R";
    private string key3 = "U";
    private string key4 = "I";
    private string key5 = "D";
    private string key6 = "F";
    private string key7 = "J";
    private string key8 = "K";

    Text textField;
    GameObject panel;
    List<string> keyList;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);

        // make list of keys
        keyList = new List<string>();
        string[] tempList = {key1, key2, key3, key4, key5, key6, key7, key8};
        keyList.AddRange(tempList);
        Debug.Log("keyList working properly");

        panel = GameObject.FindGameObjectWithTag("keybinds_panel");
        StartPanel();


        
    }

    // Update is called once per frame
    void Update()
    {

    }

    // initialize the panel setup
    public void StartPanel()
    {
        //textField = GameObject.Find("key1").GetComponent<Text>();
        //textField.text = "" + key1;
        //Debug.Log("textField");
        //Debug.Log("set text");
        panel.SetActive(false);

        GameObject[] keyTags = GameObject.FindGameObjectsWithTag("key_tag");
        Debug.Log("made keytags list");
        foreach (GameObject obj in keyTags)
        {
            Debug.Log("test");
            Debug.Log(obj);
        }
    }

    public void OpenPanel()
    {
        panel.SetActive(true);
    }

    public void ClosePanel()
    {
        panel.SetActive(false);
    }

}
