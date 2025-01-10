using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    public GameObject _textSpace;
    public GameObject _imageWAD;
    public TutorialSettings _tutorialSettings;
    [SerializeField] private float _timeTypeSymbolDefault;
    [SerializeField] private float _timeTypeSymbolSpeedUp;
    [SerializeField] private FaceScript _mainFace;
    [SerializeField] private BeatController BC;
    [SerializeField] private RedFaceScript RFS;
    [SerializeField] private GameObject ImageOnBeat;
    [SerializeField] private TutorialAnimationScript TAS;
    private float _timeTypeSymbolCurrent;
    public int _index;
    private bool _isWriting;
    public bool isAnimationEnded = false;



    private void Start()
    {
        _timeTypeSymbolCurrent = _timeTypeSymbolDefault;
        _imageWAD.SetActive(false);
        _isWriting = false;
        SetMessage(0);
    }

    private void Update()
    {
        if (_textSpace.activeInHierarchy && isAnimationEnded && (Input.GetKeyDown(KeyCode.Space) && !_tutorialSettings[_index].isMoving || _tutorialSettings[_index].isMoving && Input.GetKeyDown(KeyCode.Space)))
        {
            _textSpace.SetActive(true);
            StartCoroutine(ToggleSpaceCoroutine());
            if (_isWriting) _timeTypeSymbolCurrent = _timeTypeSymbolSpeedUp;
            else SetMessage(_index + 1);

            if (_tutorialSettings[_index].isMoving && _index == 1)
            {
                //_mainFace.TurnOnInTutorial();
                _imageWAD.SetActive(true);
            }
            if (_tutorialSettings[_index].isBeat)
            {
                BC.isTutorial = false;
                ImageOnBeat.SetActive(true);
            }
            if (_tutorialSettings[_index].isKilling)
            {
                RFS.isTurnOn=true;
            }
            
        }
    }
    private IEnumerator ToggleSpaceCoroutine()
    {
        _textSpace.SetActive(false);
        yield return new WaitForSeconds(2f);
        _textSpace.SetActive(true);
    }

    private void SetMessage(int newIndex)
    {
        if (newIndex < _tutorialSettings.CountMessages)
        {
            _index = newIndex;
            StartCoroutine(TypingText(_tutorialSettings[_index].Message));
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    public void LoseTutorial()
    {
        TypingText("Loser");
    }

    private IEnumerator TypingText(string text)
    {
        _isWriting = true;
        _text.text = "";

        for (int i = 0; i < text.Length; i++)
        {
            _text.text += text[i];
            yield return new WaitForSeconds(_timeTypeSymbolCurrent);
        }
        _isWriting = false;
        _timeTypeSymbolCurrent = _timeTypeSymbolDefault;
        if (_index == 0)
        {
            TAS.StartAnimation();
        }
        yield return null;
    }
}