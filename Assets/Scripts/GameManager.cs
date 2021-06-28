using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int level;
    private int highestLevel;
    private int winStreak;
    public int winStreakRequiredToIncrease;
    private int loseStreak;
    public int loseStreakRequiredToDecrease;
    public bool hidingAll;
    public bool autoDisappear;
    public float timeToDisappear;

    public NumberController numberPrefab;
    public RectTransform numberParent;
    public List<NumberController> numbers;

    private int lastNumberPressed;

    [Header("UI")]
    public TMP_Text hideDelayText;
    public TMP_Text highestLevelText;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        numbers = new List<NumberController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        highestLevel = 0;
        InitializeLevel(2);
    }

    private void InitializeLevel(int lvl)
    {
        level = lvl;
        
        for (int i = 0; i < level; i++)
        {
            if (i >= numbers.Count)
            {
                NumberController num = Instantiate(numberPrefab, numberParent);
                num.Initialize();
                numbers.Add(num);
            }else
            {
                numbers[i].ResetObj();
            }
        }
        for (int i = level; i < numbers.Count; i++)
        {
            numbers[i].gameObject.SetActive(false);
        }

        bool overlapping = false;
        for (int i = 0; i < numbers.Count; i++)
        {
            numbers[i].SetNumber(i + 1);
            for (int x = 0; x < 10000; x++)
            {
                numbers[i].RandomizePosition();
                overlapping = false;
                for (int j = 0; j < i; j++)
                {
                    if (numbers[i].Overlap(numbers[j]))
                    {
                        overlapping = true;
                        break;
                    }
                }
                if (!overlapping)
                    break;
            }
        }


        lastNumberPressed = 0;
        hidingAll = false;
        if (autoDisappear)
            StartCoroutine(WaitToHideAll());
    }

    public void Pressed(int number)
    {
        if (!hidingAll)
            HideAll();

        if (lastNumberPressed + 1 != number)
        {
            OnLose();
            return;
        }

        lastNumberPressed = number;

        if (lastNumberPressed == level)
        {
            OnWin();
        }
    }

    public void OnWin()
    {
        winStreak++;
        loseStreak = 0;
        if (winStreak >= winStreakRequiredToIncrease)
        {
            if (level > highestLevel)
                highestLevel = level;
            highestLevelText.SetText("Highest Level\n{0}", highestLevel);
            level++;
            winStreak = 0;
            InitializeLevel(level);
            return;
        }
        InitializeLevel(level);
    }

    public void OnLose()
    {
        loseStreak++;
        winStreak = 0;
        if (loseStreak >= loseStreakRequiredToDecrease)
        {
            level--;
            level = level < 2 ? 2 : level;
            loseStreak = 0;
            InitializeLevel(level);
            return;
        }
        InitializeLevel(level);
    }

    public void HideAll()
    {
        foreach (NumberController number in numbers)
        {
            number.ChangeBackground(false);
        }
        hidingAll = true;
        StopAllCoroutines();
    }

    IEnumerator WaitToHideAll()
    {
        yield return new WaitForSeconds(timeToDisappear);
        HideAll();
    }

    public void ToggleAutoHide(bool toggle)
    {
        autoDisappear = toggle;
    }

    public void ChangeHideDelay(float value)
    {
        timeToDisappear = value;
        hideDelayText.SetText("Time Visable:\n{0}", value);
    }
}
