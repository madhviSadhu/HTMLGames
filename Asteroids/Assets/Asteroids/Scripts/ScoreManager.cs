﻿using UnityEngine;
using OptionalText = InspectorEditable.Score.OptionalText;
using OptionalSound = InspectorEditable.Score.OptionalSound;
    public sealed class ScoreManager : MonoBehaviour
    {

        [SerializeField]
        OptionalText optionalText;

        [SerializeField]
        OptionalSound optionalSound;

        void OnEnable()
        {
            optionalText.SetText(Score.earned);
            optionalText.Show();
            Score.onEarn += ScoreEarned;
        }

        void OnDisable()
        {
            optionalText.Hide();
            Score.onEarn -= ScoreEarned;
        }

        void ScoreEarned(int points)
        {
            optionalText.SetText(Score.earned);
            optionalSound.Play();
        }

        void Reset()
        {
            InitialComponentsConstruction();
            ResetComponents();
            ProvideComponentsGameObject();
        }

        void Validate()
        {
            ProvideComponentsGameObject();
        }

        void InitialComponentsConstruction()
        {
            if (optionalText == null) optionalText = new OptionalText();
            if (optionalSound == null) optionalSound = new OptionalSound();
        }

        void ResetComponents()
        {
            optionalText.Reset();
            optionalSound.Reset();
        }

        void ProvideComponentsGameObject()
        {
            optionalText.ChanceToGetDefaultReferences(gameObject);
            optionalSound.ChanceToGetDefaultReferences(gameObject);
        }
    }

    namespace InspectorEditable.Score
    {
        using UnityEngine.UI;
        using System;

        [Serializable]
        public class OptionalText
        {
            [SerializeField]
            Text text;

            [SerializeField]
            string scoreFormat = DefaultFormat;

            const string DefaultFormat = "Total score: {0}";

            public void ChanceToGetDefaultReferences(GameObject gameObject)
            {
                if (!text) text = gameObject.GetComponentInChildren<Text>();
            }

            public void Reset()
            {
                scoreFormat = DefaultFormat;
            }

            public void SetText(int score)
            {
                if (text)
                {
                    text.text = TryFormatScoreString(score);

                }

            }

            string TryFormatScoreString(int score)
            {
                try
                {
                    return FormatScoreString(score);
                }
                catch
                {
                    return FormatDefaultScoreString(score);
                }
            }

            string FormatScoreString(int score)
            {
                return string.Format(scoreFormat, score);
            }

            static string FormatDefaultScoreString(int score)
            {
                return string.Format(DefaultFormat, score);
            }

            public void Show()
            {
                if (text)
                    text.enabled = true;
            }

            public void Hide()
            {
                if (text)
                    text.enabled = false;
            }

            public void SetColor(Color textColor)
            {
                if (text)
                    text.color = textColor;
            }
        }
    }

    namespace InspectorEditable.Score
    {
        using UnityEngine;
        using System;

        [Serializable]
        public class OptionalSound
        {
            [SerializeField]
            AudioSource source;

            [SerializeField]
            AudioClip optionalClip;

            [SerializeField]
            bool forceNoLoop = true;

            public void ChanceToGetDefaultReferences(GameObject gameObject)
            {
                if (!source)
                    source = gameObject.GetComponentInChildren<AudioSource>();
            }

            public void Reset()
            {
                forceNoLoop = true;
            }

            public void Play()
            {
                OptionallyDisableLooping();
                TryPlayWithOptionalClip();
            }

            void TryPlayWithOptionalClip()
            {
                if (!source || source.isPlaying)
                    return;

                if (optionalClip)
                    source.PlayOneShot(optionalClip);
                else
                    source.Play();
            }

            void OptionallyDisableLooping()
            {
                if (source && forceNoLoop)
                    source.loop = false;
            }
        }
    }