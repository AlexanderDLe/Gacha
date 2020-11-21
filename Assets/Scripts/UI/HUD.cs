using RPG.Core;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class HUD : MonoBehaviour
    {
        public Text characterName = null;
        public Text characterHealth = null;
        public StateManager stateManager = null;

        private void Start()
        {
            Debug.Log(stateManager.characterName);
            InitializeCharacterUI();
        }

        private void Update()
        {
            Debug.Log(stateManager.characterName);
        }

        public void InitializeCharacterUI()
        {
            this.characterName.text = stateManager.characterName;
            this.characterHealth.text = stateManager.characterHealth;
        }
    }
}