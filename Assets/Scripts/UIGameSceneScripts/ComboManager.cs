using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ComboManager : MonoBehaviour
{
    [SerializeField] private TMP_Text comboText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text scoreLoseText; 
    [SerializeField] private TMP_Text scoreWinText;
    public Image comboImage; 
    public Sprite[] comboSprites;
    [SerializeField] private RhythmManager RM;
    [SerializeField] private BeatController BC;
    [SerializeField] private TimerController TC;
    private int comboCount = 0;
    private bool comboTime = false;
    private float comboTimer;
    private int score;
    private int previousComboCount;
    private bool inProcess = false;

    [Header("Key Bindings")]
    public KeyCode keyLeft = KeyCode.A;
    public KeyCode keyTop = KeyCode.W;
    public KeyCode keyRight = KeyCode.D;

    private void Start()
    {
        comboText.text = "x0";
        scoreText.text = "0";
        scoreLoseText.text = "0";
        scoreWinText.text = "0";
        score = 0;
        keyRight = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("RightButtonSymbol"));
        keyLeft = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("LeftButtonSymbol"));
        keyTop = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("TopButtonSymbol"));
    }

    public int GetScore()
    {
        return score;
    }

    private void Update()
    {
        if (BC.canCombo)
        {
            comboTime = true;
        }
        else
        {
            comboTime = false;
        }
        if ((Input.GetKeyDown(keyLeft) && !Input.GetKeyDown(keyTop) && !Input.GetKeyDown(keyRight)) || (Input.GetKeyDown(keyTop) && !Input.GetKeyDown(keyLeft) && !Input.GetKeyDown(keyRight)) || (Input.GetKeyDown(keyRight) && !Input.GetKeyDown(keyTop) && !Input.GetKeyDown(keyLeft)))
        {
            if (!inProcess && TC.timeElapsed < TC.totalTime && TC.isTurnOn)
            {
                inProcess = true;
                if (comboTime)
                {
                    score += 20;
                    scoreText.text = score.ToString();
                    scoreLoseText.text = score.ToString();
                    scoreWinText.text = score.ToString();
                    comboCount++;
                    UpdateComboDisplay();
                    ResetComboTimer();
                }
                else
                {
                    score += 10;
                    previousComboCount = comboCount;
                    comboCount = 0;
                    scoreText.text = score.ToString();
                    scoreLoseText.text = score.ToString();
                    scoreWinText.text = score.ToString();
                    UpdateComboDisplay();
                }
                StartCoroutine(ResetCooldown());
            }
            

        }
        if (comboCount > 0)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer <= 0)
            {
              
                previousComboCount = comboCount;
                comboCount = 0;
                UpdateComboDisplay();
                comboTimer = 1.5f * RM.beatInterval;
            }
        }

    }

    private IEnumerator ResetCooldown()
    {
        yield return new WaitForSeconds(RM.beatInterval * 0.75f); 
        inProcess = false; 
    }

    public void ResetComboTimer()
    {
        comboTimer = 1.5f * RM.beatInterval;
    }

    public void Double()
    {
        score = score * 2;
        scoreText.text = score.ToString();
        scoreLoseText.text = score.ToString();
        scoreWinText.text = score.ToString();
    }

    private void UpdateComboDisplay()
    {
        comboText.text = "x" + comboCount.ToString();
        if (comboCount == 0 || comboCount == 5 || (comboCount > 5 && (comboCount - 5) % 10 == 0))
        {
            ChangeComboImage();
        }
    }

    private void ChangeComboImage()
    {
        if (comboSprites.Length > 0)
        {
            int spriteIndex = (comboCount - 5) / 10;
            if (spriteIndex < comboSprites.Length)
            {
                comboImage.sprite = comboSprites[spriteIndex];
            }
            if (comboCount == 0)
            {
                comboImage.sprite = comboSprites[0];
                score += (int)(Math.Round(0.5f * Math.Pow(previousComboCount, 2)));
                scoreText.text = score.ToString();
                scoreLoseText.text = score.ToString();
                scoreWinText.text = score.ToString();
                //Debug.Log((int)(Math.Round(0.5f * Math.Pow(previousComboCount, 2))));
            }
        }
        
    }
}