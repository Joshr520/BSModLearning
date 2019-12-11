using System;
using UnityEngine;
using System.Linq;
using System.Collections;


namespace ScoreInfo
{
    class ScorePercentage : MonoBehaviour
    {
        public static int totalScore = 0;
        public static int numNotes = 0;
        ScoreController scoreController;
        private NoteCutInfo noteCutInfo;
        public static int calculateMaxScore(int blockCount)
        {
            int maxScore;
            if (blockCount < 14)
            {
                if (blockCount == 1)
                {
                    maxScore = 115;
                }
                else if (blockCount < 5)
                {
                    maxScore = (blockCount - 1) * 230 + 115;
                }
                else
                {
                    maxScore = (blockCount - 5) * 460 + 1035;
                }
            }
            else
            {
                maxScore = (blockCount - 13) * 920 + 4715;
            }
            return maxScore;
        }

        public static double calculatePercentage(int maxScore, int resultScore)
        {
            double resultPercentage = Math.Round((double)(100 / (double)maxScore * (double)resultScore), 2);
            return resultPercentage;
        }

        void Start()
        {
            StartCoroutine(FindScoreController());
        }

        IEnumerator FindScoreController()
        {
            yield return new WaitUntil(() => Resources.FindObjectsOfTypeAll<ScoreController>().Any());
            scoreController = Resources.FindObjectsOfTypeAll<ScoreController>().First();
            scoreController.noteWasCutEvent += OnNoteHit;
        }

        void OnNoteHit(NoteData data, NoteCutInfo info, int score)
        {
            bool done = false;
            void addScore(SaberSwingRatingCounter counter)
            {
                ScoreController.RawScoreWithoutMultiplier(info, out int beforeCutRawScore, out int afterCutRawScore, out int cutDistanceRawScore);
                numNotes++;
                totalScore += beforeCutRawScore + afterCutRawScore + cutDistanceRawScore;
                Logger.log.Debug("Swing Score: " + (beforeCutRawScore + afterCutRawScore + cutDistanceRawScore).ToString());
                done = true;
                info.swingRatingCounter.didFinishEvent -= addScore;
            }
            if (done)
            {
                return;
            }
            else
            { 
                noteCutInfo = info;
                info.swingRatingCounter.didFinishEvent += addScore;
            }

        }

    }
}