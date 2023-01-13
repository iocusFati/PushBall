using System.Collections;
using UnityEngine;

namespace Infrastructure
{
    public  interface ICoroutineRunner
    {
        public Coroutine StartCoroutine(IEnumerator coroutine);
        public void StopCoroutine(Coroutine coroutine);
        public void StopCoroutine(IEnumerator coroutine);
    }
}