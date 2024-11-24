using UnityEngine;

public class SetupAudioManager : MonoBehaviour
{
   private AudioManager audioManager;

   private void Start()
   {
      FoundAudioManager();
      audioManager.ApplyVolume();
   }

   private void FoundAudioManager()
   {
      audioManager = AudioManager.Instance;
   }
}
