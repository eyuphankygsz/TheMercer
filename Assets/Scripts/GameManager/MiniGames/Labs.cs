using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Labs : MiniGames
{

    [SerializeField]
    private int[] _formula1, _formula2, _formula3;
    private bool _f1Done, _f2Done, _f3Done;
    [SerializeField]
    private GameObject[] _items;
    private Vector3[] _oldScales, _oldPositions;

    private bool _dragging;
    private Transform _dragObj;
    [SerializeField] private Transform _pot;
    private bool _done;

    private int _selectedIndex;
    private List<int> _line = new List<int>();
    [SerializeField] private Image[] _added;
    private int _addLine;

    [SerializeField] private GameObject[] _xEs;
    public override void HideMiniGame()
    {
        gameObject.SetActive(false);
    }

    public override void Lost()
    {
        throw new System.NotImplementedException();
    }

    public override void MiniUpdate()
    {
        if (_done) return;
        CheckClick();
        Drag();
    }
    private void Drag()
    {
        if (_dragObj != null)
            _dragObj.position = Input.mousePosition;
    }
    private void Drop()
    {
        if (_dragObj == null)
            return;

        AddMaterial();
        _dragObj.transform.position = _oldPositions[_selectedIndex];
        _dragObj.transform.localScale = _oldScales[_selectedIndex];

        _dragging = false;
        _dragObj = null;

    }
    private void CheckMaterial()
    {
        int[] lineArray = _line.ToArray();
        

        if (lineArray.SequenceEqual(_formula1) && !_f1Done)
        {
            Debug.Log("F1");
            _addLine = 0;
            _f1Done = true;
            _xEs[0].SetActive(true);
            _line.Clear();
            Reset();
        }
        else if (lineArray.SequenceEqual(_formula2) && !_f2Done)
        {
            Debug.Log("F2");
            _addLine = 0;
            _f2Done = true;
            _xEs[1].SetActive(true);
            _line.Clear();
            Reset();
        }
        else if (lineArray.SequenceEqual(_formula3) && !_f3Done)
        {
            Debug.Log("F3");
            _addLine = 0;
            _f3Done = true;
            _xEs[2].SetActive(true);
            _line.Clear();
            Reset();
        }
        if (_line.Count >= 4)
            Reset();

        if (_f1Done && _f2Done && _f3Done)
        {
            Debug.Log("COMPLETED");
            _done = true;
            Won();
        }
    }
    private void AddMaterial()
    {
        if (_dragObj != null)
            if (Vector2.Distance(_dragObj.position, _pot.position) < 200)
            {
                _added[_addLine].sprite = _items[_selectedIndex].GetComponent<Image>().sprite;
                _added[_addLine].gameObject.SetActive(true);
                _addLine = (_addLine + 1) % _added.Length;

                _line.Add(_selectedIndex);
                CheckMaterial();
            }
    }

    private void CheckClick()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Drop();
        }
        if (_dragging) return;
        if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject())
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            List<RaycastResult> raysastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raysastResults);

            for (int i = 0; i < raysastResults.Count; i++)
            {
                for (int j = 0; j < _items.Length; j++)
                {
                    if (raysastResults[i].gameObject == _items[j])
                    {
                        _dragging = true;
                        _dragObj = _items[j].transform;
                        _selectedIndex = j;
                        _dragObj.transform.localScale += Vector3.one * 0.5f;
                    }
                }
            }
        }
    }
    public override void ShowMiniGame()
    {
        gameObject.SetActive(true);
        _oldPositions = new Vector3[_items.Length];
        _oldScales = new Vector3[_items.Length];
        for (int i = 0; i < _items.Length; i++)
        {
            _oldPositions[i] = _items[i].transform.position;
            _oldScales[i] = _items[i].transform.localScale;
        }
    }
    private void Reset()
    {
        _line.Clear();
        for (int i = 0; i < _added.Length; i++)
        {
            _added[i].gameObject.SetActive(false);
        }
    }
    public override void Won()
    {
        _done = true;
        HideMiniGame();
        SceneManager.Instance.GetScene(true);
    }

}
