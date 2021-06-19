using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int level;
    public bool hidingAll;
    public bool autoDisappear;
    public float timeToDisappear;

    public NumberController numberPrefab;
    public RectTransform numberParent;
    public List<NumberController> numbers;

    private int lastNumberPressed;


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
        InitializeLevel(4);
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
            Debug.LogError("YOU LOSE");
        }

        lastNumberPressed = number;

        if (lastNumberPressed == level)
        {
            Debug.Log("YOU WIN");
        }
    }

    public void HideAll()
    {
        foreach (NumberController number in numbers)
        {
            number.ChangeBackground(false);
        }
        hidingAll = true;
    }

    IEnumerator WaitToHideAll()
    {
        yield return new WaitForSeconds(timeToDisappear);
        HideAll();
    }
}
