using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
        private int _breathCount;
        
        public bool IsReachedMax(int max)
        { 
                return _breathCount >= max;
        }
        
        public void Breathe()
        {
                _breathCount++;

        }

        public void ResetBreathCount()
        {
                _breathCount = 0;
        }
}