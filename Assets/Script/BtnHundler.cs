using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class ButtonData
    {
        public Button button;
        public TextMeshProUGUI textMeshPro;
        public Sprite image;
        public string newText;
    }

public class BtnHundler : MonoBehaviour
{


    

   
        public ButtonData[] buttonsData;
        public Sprite defaultImage;
        public string defaultText;

        private void Start()
        {
            // Set default values for each button
            foreach (ButtonData buttonData in buttonsData)
            {
                buttonData.textMeshPro.text = defaultText;
                buttonData.button.image.sprite = defaultImage;
            }
        }

        public void OnButtonClick(int buttonIndex)
        {
            ButtonData buttonData = buttonsData[buttonIndex];

            buttonData.textMeshPro.text = buttonData.newText;
            buttonData.button.image.sprite = buttonData.image;
        }
    



}
