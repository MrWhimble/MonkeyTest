using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class NumberController : MonoBehaviour, IPointerDownHandler
{
    public int number;
    public bool isShowing;
    [SerializeField] private Image background;
    [SerializeField] private TMP_Text numberText;

    public RectTransform rectTransform { get; private set; }
    private RectTransform parent;

    public void OnPointerDown(PointerEventData eventData)
    {
        gameObject.SetActive(false);
        GameManager.instance.Pressed(number);
    }

    // Start is called before the first frame update
    void Awake()
    {
        parent = transform.parent.GetComponent<RectTransform>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetNumber(int num)
    {
        number = num;
        numberText.text = num.ToString();
    }

    public void ChangeBackground(bool show)
    {
        isShowing = show;
        background.color = new Color(1, 1, 1, isShowing ? 0 : 1);
    }

    public void Initialize()
    {
        if (background == null)
            background = GetComponent<Image>();

        if (numberText == null)
            numberText = transform.GetChild(0).GetComponent<TMP_Text>();

        ResetObj();
    }

    public void ResetObj()
    {
        gameObject.SetActive(true);
        SetNumber(0);
        ChangeBackground(true);
    }

    public void RandomizePosition()
    {
        //Debug.Log(rectTransform);
        //Debug.Log(parent);
        //Debug.Log((rectTransform.sizeDelta.x / 2f));
        //Debug.Log(parent.rect.width);
        transform.localPosition = new Vector3(Random.Range((rectTransform.sizeDelta.x / 2f), parent.rect.width - (rectTransform.sizeDelta.x / 2f)), Random.Range((rectTransform.sizeDelta.y / 2f), parent.rect.height - (rectTransform.sizeDelta.y / 2f)));
    }

    public bool Overlap(NumberController other)
    {
        //float leftSide = rectTransform.localPosition.x - (rectTransform.sizeDelta.x / 2f);
        //float leftSideOther = other.rectTransform.localPosition.x + (other.rectTransform.sizeDelta.x / 2f);
        //Debug.Log($"{number}:{leftSide} ~ {other.number}:{leftSideOther}");

        //return ((rectTransform.localPosition.x - (rectTransform.sizeDelta.x / 2f) <= other.rectTransform.localPosition.x + (other.rectTransform.sizeDelta.x / 2f) || rectTransform.localPosition.x + (rectTransform.sizeDelta.x / 2f) >= other.rectTransform.localPosition.x - (other.rectTransform.sizeDelta.x / 2f)) && (rectTransform.localPosition.y - (rectTransform.sizeDelta.y / 2f) <= other.rectTransform.localPosition.y + (other.rectTransform.sizeDelta.y / 2f) || rectTransform.localPosition.y + (rectTransform.sizeDelta.y / 2f) >= other.rectTransform.localPosition.y - (other.rectTransform.sizeDelta.y / 2f)));
        return ((rectTransform.localPosition.x - (rectTransform.sizeDelta.x * 0.5f) <= other.rectTransform.localPosition.x - (other.rectTransform.sizeDelta.x * 0.5f) &&
                 rectTransform.localPosition.x + (rectTransform.sizeDelta.x * 0.5f) >= other.rectTransform.localPosition.x - (other.rectTransform.sizeDelta.x * 0.5f)) ||
                (rectTransform.localPosition.x - (rectTransform.sizeDelta.x * 0.5f) <= other.rectTransform.localPosition.x + (other.rectTransform.sizeDelta.x * 0.5f) &&
                 rectTransform.localPosition.x + (rectTransform.sizeDelta.x * 0.5f) >= other.rectTransform.localPosition.x + (other.rectTransform.sizeDelta.x * 0.5f))) &&
               ((rectTransform.localPosition.y - (rectTransform.sizeDelta.y * 0.5f) <= other.rectTransform.localPosition.y - (other.rectTransform.sizeDelta.y * 0.5f) &&
                 rectTransform.localPosition.y + (rectTransform.sizeDelta.y * 0.5f) >= other.rectTransform.localPosition.y - (other.rectTransform.sizeDelta.y * 0.5f)) ||
                (rectTransform.localPosition.y - (rectTransform.sizeDelta.y * 0.5f) <= other.rectTransform.localPosition.y + (other.rectTransform.sizeDelta.y * 0.5f) &&
                 rectTransform.localPosition.y + (rectTransform.sizeDelta.y * 0.5f) >= other.rectTransform.localPosition.y + (other.rectTransform.sizeDelta.y * 0.5f)));

        //return false;
    }


}
