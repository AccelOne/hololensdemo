using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ConsoleWindowAR : MonoBehaviour
{


    struct Log
    {
        public string message;
        public string stackTrace;
        public LogType type;

    }

    private Text text;
    List<Log> logs = new List<Log>();
    GameObject consoleTextWindow;
    GameObject consoleCanvas;
    GameObject debugError;

    bool show;
    public int clearCount = 23;


    static readonly Dictionary<LogType, Color> logTypeColors = new Dictionary<LogType, Color>()
    {
        {LogType.Assert, Color.white },
        {LogType.Error, Color.red },
        {LogType.Exception, Color.red },
        {LogType.Log, Color.white },
        {LogType.Warning, Color.yellow },

    };

    void OnEnable()
    {

        Application.logMessageReceived += HandleLog;

        consoleTextWindow = GameObject.Find("ConsoleText");
        consoleCanvas = GameObject.Find("ConsoleCanvas");
        text = consoleTextWindow.GetComponent<Text>();
        show = true;

        Debug.Log("OnEnable");
    }


    void Update()
    {

        if (logs.Count >= clearCount)
        {


            ClearConsole();
        }
        if (Input.GetKeyDown("1"))
        {
            //debugError.name = "nothing";

        }

        if (Input.GetKeyDown("2"))
        {

            RandomMessage();

        }


    }

    public void ShowKeyword()
    {

        Debug.Log("Show");
        consoleCanvas.SetActive(true);
        show = true;

    }

    public void HideKeyword()
    {

        Debug.Log("Hide");
        consoleCanvas.SetActive(false);
        show = false;

    }

    public void ClearConsole()
    {

        logs.Clear();

    }

    void HandleLog(string message, string stackTrace, LogType type)
    {
        string nullstring = "";
        text.text = nullstring;
        string temp = text.text;

        logs.Add(new Log()
        {
            message = message,
            stackTrace = stackTrace,
            type = type,

        });

        for (int i = 0; i < logs.Count; i++)
        {

            var log = logs[i];
            text.color = logTypeColors[log.type];
            text.text += temp + "\n" + log.message;
            text.text += "\n " + log.stackTrace;


        }



    }

    public void RandomKeyboard()
    {
        for (int i = 0; i < 8; i++)
        {
            RandomMessage();

        }


    }

    public void RandomMessage()
    {

        string str;
        int i = Random.Range(1, 20);
        str = Random.Range(0.0000000000000001f, 9999999999.0f).ToString();
        if (i >= 1 && i <= 5)
        {
            HandleLog(str, "somethingIDK", LogType.Exception);
        }
        if (i > 5 && i < 8)
        {

            HandleLog("this is a debug message", "this should be a stacktrace", LogType.Log);
        }
        if (i >= 8 && i < 12)
        {
            HandleLog("this is a Error message", "stackTrace", LogType.Error);
        }
        if (i >= 12 && i < 15)
        {
            HandleLog("this is a Warning message", "stackTrace", LogType.Warning);
        }
        if (i >= 15 && i < 18)
        {
            HandleLog("this is a Exception message", "stackTrace", LogType.Exception);
        }
        if (i >= 18 && i < 21)
        {
            HandleLog("this is a Assertion message", "stackTrace", LogType.Assert);
        }

    }
}
