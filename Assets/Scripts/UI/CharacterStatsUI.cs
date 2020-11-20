using RPG.Core;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class CharacterStatsUI : MonoBehaviour
    {
        public Text characterName = null;
        public Text characterHealth = null;
        // CharacterScriptableObject character = null;

        private void Start()
        {
            characterName.text = "MC";
            characterHealth.text = "100%";
        }

        public void InitializeCharacterUI(CharacterScriptableObject character)
        {
            this.characterName.text = character.name;
        }
    }
}