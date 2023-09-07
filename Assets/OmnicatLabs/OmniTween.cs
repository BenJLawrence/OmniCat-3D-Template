using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace OmnicatLabs.Tween
{
    public static class TransformExtensions
    {
        public static void TweenPosition(this Transform transform, Vector3 newPosition, float amountOfTime, UnityAction onComplete)
        {
            Vector3 startingPos = transform.position;

            OmniTween.tweens.Add(new Tween(1f, onComplete, (tween) =>
            {
                if (tween.timeElapsed < tween.tweenTime)
                {
                    transform.position = Vector3.Lerp(startingPos, newPosition, tween.timeElapsed / tween.tweenTime);
                    tween.timeElapsed += Time.deltaTime;
                    Debug.Log(transform.position);
                }
                else
                {
                    transform.position = newPosition;
                    tween.completed = true;
                    Debug.Log(transform.position);
                }
            }));
            //float startingVal = 1f;
            //float endingVal = 0f;
            //float timeElapsed = 0f;
            //float tempval = 0f;
            //    tempval = Mathf.Lerp(startingVal, endingVal, timeElapsed / amountOfTime);
            //    timeElapsed += Time.deltaTime;
            //    Debug.Log(tempval);
        }
    }

    public class Tween
    {
        public float tweenTime;
        public float timeElapsed = 0f;
        public UnityAction<Tween> tweenAction;
        public bool completed = false;
        public UnityAction onComplete;
        public void DoTween()
        {
            tweenAction.Invoke(this);
        }

        public Tween(float _tweenTime, UnityAction _onComplete, UnityAction<Tween> _tweenAction)
        {
            tweenTime = _tweenTime;
            tweenAction = _tweenAction;
            onComplete = _onComplete;
        }
    }

    public class OmniTween : MonoBehaviour
    {
        public static List<Tween> tweens = new List<Tween>();

        private void Update()
        {
            foreach (Tween tween in tweens)
            {
                tween.DoTween();
                if (tween.completed)
                {
                    tween.onComplete.Invoke();
                }
            }

            tweens.RemoveAll(tween => tween.completed);
        }
    }
}

