﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatSystem
{
    class PlayerInput
    {
        KeyboardState oldState;
        ContentManager content;
        public const int MAX_MESSAGES = 7;
        SpriteFont arial;
        string playerChatText = "";
        public string PlayerChatText { get {return playerChatText;} set { playerChatText = value; } }

        public List<string> messages = new List<string>();

        public PlayerInput(ContentManager content)
        {
            this.content = content;
        }

        public void handleOverallInput() //get updated 16.6 times every 1 second
        {
            KeyboardState key = new KeyboardState();
            Keys[] multipleKeys;
            key = Keyboard.GetState();
            multipleKeys = key.GetPressedKeys();
            handleChatBoxInput(multipleKeys);
        }

        public void handleChatBoxInput(Keys[] keys)
        {
            KeyboardState newState = Keyboard.GetState();
            foreach (Keys key in keys)
            {
                if (newState.IsKeyDown(key))
                {
                    if (!oldState.IsKeyDown(key))
                    {
                        int keyValue = (int)key;
                        if ((keyValue >= 0x30 && keyValue <= 0x39) // numbers
                            || (keyValue >= 0x41 && keyValue <= 0x5A) // letters
                            || (keyValue >= 0x60 && keyValue <= 0x69)) //numpad
                        {
                            PlayerChatText += key;
                        }
                        switch (keyValue)
                        {
                            case 0x20:
                                PlayerChatText += " "; //we do this because otherwise we'll get like SPACE instead of " "
                                break;
                            case 0xBF:
                                PlayerChatText += ":";
                                break;
                            case 0xDB:
                                PlayerChatText += ")";
                                break;
                            case 8:
                                if (PlayerChatText.Length > 0)
                                    playerChatText = playerChatText.Substring(0, playerChatText.Length - 1);
                                break;
                            case 13:
                                if (!PlayerChatText.Equals(""))
                                    messages.Add(playerChatText.ToLower());
                                playerChatText = "";
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            oldState = newState;
        }

        public void LoadContent()
        {
            arial = content.Load<SpriteFont>("Arial");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(arial, PlayerChatText.ToLower(), new Vector2(100, 80), Color.White);
            for (int i = 0; i < messages.Count; i++)
            {
                if (i < PlayerInput.MAX_MESSAGES)
                {
                    spriteBatch.DrawString(arial, messages[i], new Vector2(100, 100 + (i * 20)), Color.White);
                }
                else
                {
                    messages.RemoveAt(0);
                }
            }
        }
    }
}
