using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vertigoGames.hexColorGame
{
    public class removeAfterTime : MonoBehaviour
    {
        public float removeAfterTimeObject = 1;
        void Start()
        {
            Destroy(gameObject, removeAfterTimeObject);
        }
    }
}