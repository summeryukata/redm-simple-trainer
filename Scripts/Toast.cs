using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client.Scripts
{
    public static class Toast
    {
        class Toasty
        {
            public string Text { get; set; }
            public float X { get; set; }
            public float Y { get; set; }

            public int StartTime { get; set; }
            public int Duration { get; set; }

            bool m_isFadingIn = true;
            bool m_isFadingOut = false;

            int m_alpha = 0;
            float m_fadeX;
            bool m_firstDraw = true;

            private bool FadeIn()
            {
                m_alpha += (int)(1.0f + Function.Call<float>(Hash.GET_FRAME_TIME)) * 10;
                
                if (m_alpha < 200)
                {
                    return true;
                }

                m_alpha = 200;

                return false;
            }

            private void FadeOut()
            {
                m_alpha -= (int)(1.0f + Function.Call<float>(Hash.GET_FRAME_TIME)) * 10;

                if (m_alpha < 0)
                {
                    m_alpha = 0;
                }
            }

            public void DrawToast()
            {
                if (m_firstDraw)
                {
                    m_fadeX = X + 0.1f;
                    m_firstDraw = false;
                }

                if (m_fadeX > X) m_fadeX -= Function.Call<float>(Hash.GET_FRAME_TIME) * 1f;
                if (m_fadeX < X) m_fadeX = X;

                float height = 0.035f;

                if (m_isFadingIn)
                {
                    m_isFadingIn = FadeIn();
                }

                m_isFadingOut = (Function.Call<int>(Hash.GET_GAME_TIMER) - StartTime) > Duration - 100;

                if (m_isFadingOut)
                {
                    FadeOut();
                }

                Drawing.DrawText(Text, Drawing.g_optionsFont, m_fadeX - (0.3f / 2) + 0.01f, Y - (height / 2) + 0.002f, 0.1f, 0.4f, 255, 255, 255, m_alpha, false);
                Drawing.DrawTexture("boot_flow", "selection_box_bg_1d", X, Y, 0.3f, height, 0f, Globals.g_backgroundRed, Globals.g_backgroundGreen, Globals.g_backgroundBlue, m_alpha, true);
            }
        }

        static List<Toasty> ms_activeToasts = new List<Toasty>();
        static List<Toasty> ms_toastsToRemove = new List<Toasty>();

        public static void AddToast(string text, int durationMsec, float xPos, float yPos)
        {
            if (ms_activeToasts.Any(t => t.Y == yPos))
            {
                ms_toastsToRemove.AddRange(ms_activeToasts.Where(t => t.Y == yPos));
            }

            ms_activeToasts.Add(new Toasty()
            {
                Text = text,
                X = xPos,
                Y = yPos,
                Duration = durationMsec,
                StartTime = Function.Call<int>(Hash.GET_GAME_TIMER)
            });
        }

        public static async Task Tick()
        {
            if (ms_activeToasts.Count > 0)
            {
                ms_activeToasts.ForEach(t =>
                {
                    t.DrawToast();

                    int currentTime = Function.Call<int>(Hash.GET_GAME_TIMER);
                    if (currentTime - t.StartTime > t.Duration)
                    {
                        ms_toastsToRemove.Add(t);
                    }
                });
            }

            if (ms_toastsToRemove.Count > 0)
            {
                ms_toastsToRemove.ForEach(t => ms_activeToasts.Remove(t));
                ms_toastsToRemove.Clear();
            }

            await Task.FromResult(0);
        }
    }
}
