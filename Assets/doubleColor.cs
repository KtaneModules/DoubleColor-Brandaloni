using KMHelper;
using Random = UnityEngine.Random;
using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System;

public class doubleColor : MonoBehaviour {

    private static int _moduleIdCounter = 1;
    public KMAudio newAudio;
    public KMBombModule module;
    public KMBombInfo info;
    private int _moduleId = 0;
    private bool _isSolved = false, _lightsOn = false;

    public GameObject colorblindObj;
    public TextMesh colorblindText;

    private Color red = new Color(255, 0, 0), green = new Color(0, 255, 0), blue = new Color(0, 0, 255), yellow = new Color(255, 255, 0), pink = new Color(255,0,255);

    public KMSelectable submit;

    public MeshRenderer light1, light2, light3, screen, stage1, stage2;

    private bool danger = false, manualColorblind = false;

    private int stageNumber = 1;

    private int screenColor; //0=green, 1=blue, 2=red, 3=pink, 4=yellow

    private int correctDidget;

    public Material screenOff, screenOn, screenRed, screenBlue, screenPink, screenGreen, screenYellow;

    void Start()
    {
        _moduleId = _moduleIdCounter++;
        module.OnActivate += Activate;
    }

    void Activate()
    {
        Init();
        _lightsOn = true;
    }

    private void Awake()
    {
        submit.OnInteract += delegate
        {
            handleSubmit();
            return false;
        };
    }

    void handleSubmit()
    {
        newAudio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, submit.transform);
        if (!_lightsOn || _isSolved) return;

        Debug.LogFormat("[Double Color #{0}] Submit button pressed", _moduleId);
        string time = info.GetFormattedTime();
        if (danger)
        {
            module.HandleStrike();
            Debug.LogFormat("[Double Color #{0}] Strike! Submit button was pressed when all three lights were on!", _moduleId);
            StartCoroutine(reset());
        }
        else if (time.Contains(correctDidget.ToString()))
        {
            if (stageNumber == 2) { 
                module.HandlePass();
                Debug.LogFormat("[Double Color #{0}] Module passed!", _moduleId);
                stage2.material = screenOn;
                newAudio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.CorrectChime, submit.transform);
                _isSolved = true;
            } else
            {
                Debug.LogFormat("[Double Color #{0}] Stage 1 complete!", _moduleId);
                newAudio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.CorrectChime, submit.transform);
                StartCoroutine(passStageOne());
            }
        } else
        {
            module.HandleStrike();
            Debug.LogFormat("[Double Color #{0}] Strike! Submit button at wrong time! There wasn't a {1} in the bomb timer!", _moduleId, correctDidget);
            StartCoroutine(reset());
        }

    }

    void Init()
    {
        MainScreenSetup();
        StartCoroutine(Lights());
        getCorrectDidget();
    }

    void getCorrectDidget()
    {
        if (stageNumber == 1)
        {
            switch (info.GetBatteryCount())
            {
                case 0:
                    switch (screenColor)
                    {
                        case 0:
                            correctDidget = 1;
                            break;
                        case 1:
                            correctDidget = 0;
                            break;
                        case 2:
                            correctDidget = 9;
                            break;
                        case 3:
                            correctDidget = 8;
                            break;
                        case 4:
                            correctDidget = 7;
                            break;
                    }
                    break;
                case 1:
                    switch (screenColor)
                    {
                        case 0:
                            correctDidget = 2;
                            break;
                        case 1:
                            correctDidget = 7;
                            break;
                        case 2:
                            correctDidget = 6;
                            break;
                        case 3:
                            correctDidget = 5;
                            break;
                        case 4:
                            correctDidget = 6;
                            break;
                    }
                    break;
                case 2:
                    switch (screenColor)
                    {
                        case 0:
                            correctDidget = 3;
                            break;
                        case 1:
                            correctDidget = 8;
                            break;
                        case 2:
                            correctDidget = 1;
                            break;
                        case 3:
                            correctDidget = 4;
                            break;
                        case 4:
                            correctDidget = 5;
                            break;
                    }
                    break;
                case 3:
                    switch (screenColor)
                    {
                        case 0:
                            correctDidget = 4;
                            break;
                        case 1:
                            correctDidget = 9;
                            break;
                        case 2:
                            correctDidget = 2;
                            break;
                        case 3:
                            correctDidget = 3;
                            break;
                        case 4:
                            correctDidget = 4;
                            break;
                    }
                    break;
                case 4:
                    switch (screenColor)
                    {
                        case 0:
                            correctDidget = 5;
                            break;
                        case 1:
                            correctDidget = 0;
                            break;
                        case 2:
                            correctDidget = 1;
                            break;
                        case 3:
                            correctDidget = 2;
                            break;
                        case 4:
                            correctDidget = 3;
                            break;
                    }
                    break;
                default:
                    switch (screenColor)
                    {
                        case 0:
                            correctDidget = 6;
                            break;
                        case 1:
                            correctDidget = 7;
                            break;
                        case 2:
                            correctDidget = 8;
                            break;
                        case 3:
                            correctDidget = 9;
                            break;
                        case 4:
                            correctDidget = 0;
                            break;
                    }
                    break;

            }
        }
        else
        {
            switch (info.GetBatteryCount())
            {
                case 0:
                    switch (screenColor)
                    {
                        case 0:
                            correctDidget = 0;
                            break;
                        case 1:
                            correctDidget = 2;
                            break;
                        case 2:
                            correctDidget = 6;
                            break;
                        case 3:
                            correctDidget = 8;
                            break;
                        case 4:
                            correctDidget = 5;
                            break;
                    }
                    break;
                case 1:
                    switch (screenColor)
                    {
                        case 0:
                            correctDidget = 4;
                            break;
                        case 1:
                            correctDidget = 9;
                            break;
                        case 2:
                            correctDidget = 9;
                            break;
                        case 3:
                            correctDidget = 0;
                            break;
                        case 4:
                            correctDidget = 2;
                            break;
                    }
                    break;
                case 2:
                    switch (screenColor)
                    {
                        case 0:
                            correctDidget = 1;
                            break;
                        case 1:
                            correctDidget = 7;
                            break;
                        case 2:
                            correctDidget = 5;
                            break;
                        case 3:
                            correctDidget = 9;
                            break;
                        case 4:
                            correctDidget = 6;
                            break;
                    }
                    break;
                case 3:
                    switch (screenColor)
                    {
                        case 0:
                            correctDidget = 4;
                            break;
                        case 1:
                            correctDidget = 2;
                            break;
                        case 2:
                            correctDidget = 0;
                            break;
                        case 3:
                            correctDidget = 8;
                            break;
                        case 4:
                            correctDidget = 3;
                            break;
                    }
                    break;
                case 4:
                    switch (screenColor)
                    {
                        case 0:
                            correctDidget = 6;
                            break;
                        case 1:
                            correctDidget = 8;
                            break;
                        case 2:
                            correctDidget = 4;
                            break;
                        case 3:
                            correctDidget = 7;
                            break;
                        case 4:
                            correctDidget = 1;
                            break;
                    }
                    break;
                default:
                    switch (screenColor)
                    {
                        case 0:
                            correctDidget = 1;
                            break;
                        case 1:
                            correctDidget = 3;
                            break;
                        case 2:
                            correctDidget = 7;
                            break;
                        case 3:
                            correctDidget = 3;
                            break;
                        case 4:
                            correctDidget = 5;
                            break;
                    }
                    break;
            }
        }
        Debug.LogFormat("[Double Color #{0}] Correct didget for Stage {1}: {2}", _moduleId, stageNumber, correctDidget);
    }

    void MainScreenSetup()
    {
        screenColor = Random.Range(0, 5);
        switch (screenColor)
        {
            case 0:
                screen.material = screenGreen;
                Debug.LogFormat("[Double Color #{0}] Screen Color for Stage {1}: Green", _moduleId, stageNumber);
                colorblindText.text = "G";
                colorblindText.color = green;
                break;
            case 1:
                screen.material = screenBlue;
                Debug.LogFormat("[Double Color #{0}] Screen Color for Stage {1}: Blue", _moduleId, stageNumber);
                colorblindText.text = "B";
                colorblindText.color = blue;
                break;
            case 2:
                screen.material = screenRed;
                Debug.LogFormat("[Double Color #{0}] Screen Color for Stage {1}: Red", _moduleId, stageNumber);
                colorblindText.text = "R";
                colorblindText.color = red;
                break;
            case 3:
                screen.material = screenPink;
                Debug.LogFormat("[Double Color #{0}] Screen Color for Stage {1}: Pink", _moduleId, stageNumber);
                colorblindText.text = "P";
                colorblindText.color = pink;
                break;
            case 4:
                screen.material = screenYellow;
                Debug.LogFormat("[Double Color #{0}] Screen Color for Stage {1}: Yellow", _moduleId, stageNumber);
                colorblindText.text = "Y";
                colorblindText.color = yellow;
                break;

        }
        if (GetComponent<KMColorblindMode>().ColorblindModeActive || manualColorblind)
        {
            colorblindObj.SetActive(true);
            screen.material = screenOff;
            Debug.LogFormat("[Double Color #{0}] Colorblind mode enabled.", _moduleId);
        } else
        {
            colorblindObj.SetActive(false);
        }
    }

    private IEnumerator Lights()
    {
        while (!_isSolved)
        {
            danger = false;
            light1.material = screenOff;
            light2.material = screenOff;
            light3.material = screenOff;
            yield return new WaitForSeconds(2.4f);
            light1.material = screenRed;
            yield return new WaitForSeconds(0.5f);
            light2.material = screenRed;
            yield return new WaitForSeconds(0.5f);
            light3.material = screenRed;
            danger = true;
            yield return new WaitForSeconds(3.0f);
        }
        light1.material = screenOff;
        light2.material = screenOff;
        light3.material = screenOff;
        screen.material = screenOff;
        colorblindObj.SetActive(false);
    }

    private IEnumerator reset()
    {
        stageNumber = 1;
        stage1.material = screenOff;
        screen.material = screenOff;
        colorblindObj.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        MainScreenSetup();
        getCorrectDidget();
    }

    private IEnumerator passStageOne()
    {
        stage1.material = screenOn;
        screen.material = screenOff;
        colorblindObj.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        stageNumber = 2;
        MainScreenSetup();
        getCorrectDidget();
    }

#pragma warning disable 414

    private string TwitchHelpMessage = "Press submit when there is a 7 in any position with !{0} submit at 7. Use !{0} colorblind to turn on colorblind mode.";

#pragma warning restore 414
    IEnumerator ProcessTwitchCommand(string input)
    {
        Regex rgx = new Regex(@"^(press|submit) (at|on|with) [0-9]$");
        if (rgx.IsMatch(input))
        {
            string[] split = input.ToLowerInvariant().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            yield return null;

            while (!info.GetFormattedTime().Contains(split[2]) || danger) yield return "trycancel Submit wasn't pressed due to either danger or no number.";

            handleSubmit();
        } else if (input.ToLowerInvariant().Equals("colorblind"))
        {
            yield return null;
            colorblindObj.SetActive(true);
            screen.material = screenOff;
            Debug.LogFormat("[Double Color #{0}] Colorblind mode enabled via TP.", _moduleId);
            manualColorblind = true;
            yield break;
        }
    }
}
