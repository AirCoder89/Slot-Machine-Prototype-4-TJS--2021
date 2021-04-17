using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace Core.Audio
{
    public class AudioSystem : BaseMonoBehaviour
    {
        [Range(0,1)] public float masterVolume = 0.5f;
        public List<SfxMachine> sfxList;
        public int testIndex;

        private Dictionary<SfxList, SfxMachine> _sfxMap;
        [ContextMenu("Play")]
        private void Play()
        {
            sfxList[testIndex].Play(1f);
        }
        
        [ContextMenu("Stop")]
        private void Stop()
        {
            sfxList[testIndex].Stop();
        }

        protected override void ReleaseReferences()
        {
            sfxList.Clear();
            sfxList = null;
        }

        public void Initialize()
        {
            _sfxMap = new Dictionary<SfxList, SfxMachine>();
            sfxList.ForEach(sfx =>
            {
                var src = gameObject.AddComponent<AudioSource>();
                sfx.Initialize(src, this);
                _sfxMap.Add(sfx.label, sfx);
            });
        }

        private void OnValidate()
            => sfxList.ForEach(sfx => {  sfx.ClampValues();  });
        
        public void Tick()
            => sfxList.ForEach(sfx =>  { sfx.Tick(); });

        public void StopAllSounds()
            => sfxList.ForEach(sfx =>  { sfx.Stop(); });
        
        public void Play(SfxList inSfx, bool stopOther = true, float pitchMultiplier = 1f)
        {
            if (!_sfxMap.ContainsKey(inSfx))
            {
                throw new WarningException($"you have to assign the sfx machine in the system before use it!");
                return;
            }
            if(stopOther) StopAllSounds();
            _sfxMap[inSfx].Play(pitchMultiplier);
        }

        public void Stop(SfxList inSfx)
        {
            if (!_sfxMap.ContainsKey(inSfx))
            {
                throw new WarningException($"you have to assign the sfx machine in the system before use it!");
                return;
            }
            _sfxMap[inSfx].Stop();
        }
        
    }
}
