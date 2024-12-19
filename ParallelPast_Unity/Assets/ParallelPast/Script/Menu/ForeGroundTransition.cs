using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

public class ForeGroundTransition : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve _transitionCurve;

    [SerializeField]
    public UnityEvent OnCloseTransitionExecute;


    [SerializeField]
    private UnityEvent _onOpenTransitionExecute;
    public UnityEvent OnOpenTransitionExecute => _onOpenTransitionExecute;

    [NonSerialized]
    public GameManager SceneLoader;

    public float transitionValue
    {
        get { return GetComponent<Image>().material.GetFloat("_TransitionValue"); }
        set { GetComponent<Image>().material.SetFloat("_TransitionValue", value); }
    }

    public void SetUpTransition(So_LevelTransitionType levelTransitionType)
    {
        Image foregroundImage = GetComponent<Image>();

        foregroundImage.material = levelTransitionType.transitionMaterial;
        foregroundImage.color = levelTransitionType.transitionColor;
    }

    public IEnumerator CloseTransition(float transitionDuration, float delayBeforeExecute = 0, bool executeEvent = true)
    {
        if(EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
        
        float time = 0;
        transitionValue = 0;

        yield return new WaitForSeconds(delayBeforeExecute);
        

        while (time < transitionDuration)
        {

            transitionValue = _transitionCurve.Evaluate(time / transitionDuration);

            time += Time.deltaTime;
            yield return null;
        }

        transitionValue = 1;


        if (executeEvent)
        {
            OnCloseTransitionExecute.Invoke();
            OnCloseTransitionExecute.RemoveAllListeners();
        }
        
    }


    public IEnumerator OpenTransition(float transitionDuration, float delayBeforeExecute = 0, bool executeEvent = true)
    {
        //EventSystem.current.SetSelectedGameObject(null);
        float time = 0;
        transitionValue = 1;

        yield return new WaitForSeconds(delayBeforeExecute);

        while (time < transitionDuration)
        {

            transitionValue = 1 - _transitionCurve.Evaluate(time / transitionDuration);

            time += Time.deltaTime;
            yield return null;
        }

        transitionValue = 0;

        if (executeEvent)
        {
            _onOpenTransitionExecute.Invoke();
            _onOpenTransitionExecute.RemoveAllListeners();
        }
       
    }

    public void LoadNewScene()
    {
        if(SceneLoader != null)
        {
            SceneLoader.LoadTransitionScene();
        }
        else
        {
            Debug.LogError("Can't find Scene loader");
        }
    }

}
