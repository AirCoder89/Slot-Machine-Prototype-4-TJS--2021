using System.Collections;
using System.Collections.Generic;
using AirCoder.TJ.Core;
using AirCoder.TJ.Core.Extensions;
using UnityEngine;

namespace AirCoder.Examples.Scripts
{
    public class Waypoints : MonoBehaviour
    {
        public Transform target;
        
        public List<Vector3> waypoints;
        public EaseType selectedEase;
        public float duration;
        public float delay;
        public bool loop;
        
        IEnumerator Start()
        {
            yield return new WaitForSeconds(delay);
            MoveTo(0);
        }

        [ContextMenu("Add")]
        private void Add()
        {
            if(waypoints == null) waypoints = new List<Vector3>();
            waypoints.Add(transform.position);
        }

        [ContextMenu("move")]
        private void Move()
        {
            Debug.Log($" TJSystem.Instance : { TJSystem.Instance}");
        }

        private void MoveTo(int index)
        {
            if(waypoints == null || waypoints.Count == 0) return;
            if (index < waypoints.Count)
            {
                Debug.Log($"move to : {waypoints[index]}");
                transform.TweenPosition(waypoints[index], duration)
                    .SetEase(selectedEase)
                    .OnComplete(() =>
                    {
                        index++;
                        MoveTo(index);
                    })
                    .Play();
            }
            else PathCompleted();
        }

        
        private void PathCompleted()
        {
            if (loop)
            {
                MoveTo(0);
                return;
            }

            Debug.Log($"Path Completed !");
        }

    }
}
