using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace aar
{
    public class RenderManager
    {
        frmRenderTarget rt = new frmRenderTarget();

        List<Entity> Entities = new List<Entity>();
        List<Entity> TemporaryEntities = new List<Entity>();

        Thread RenderThread;
        bool RenderThreadRun = false;
        bool AcknowledgeRTStop = false;

        Rectangle Viewport = new Rectangle(0, 0, 640, 400);

        public bool Running
        {
            get
            {
                return RenderThreadRun;
            }
        }

        public void Add(Entity DrawableEntity, bool RemoveAutomaticallyOnRoomChange)
        {
            if (RemoveAutomaticallyOnRoomChange)
                lock (TemporaryEntities)
                    TemporaryEntities.Add(DrawableEntity);
            else
                lock (Entities)
                    Entities.Add(DrawableEntity);
        }

        /// <summary>
        /// Removes non-temporary entities.
        /// </summary>
        /// <param name="DrawableEntity">The Entity (NOT Entities designated for automatic removal!) to remove.</param>
        public void Remove(Entity DrawableEntity)
        {
            lock (Entities)
                Entities.Remove(DrawableEntity);
        }

        public void ClearTemporary()
        {
            lock (TemporaryEntities)
                TemporaryEntities.Clear();
        }

        public void Start()
        {
            RenderThread = new Thread(new ThreadStart(RenderProc));
            RenderThreadRun = true;
            RenderThread.Start();
        }

        public void Stop()
        {
            if (!RenderThreadRun)
            {
                DateTime TimeoutTime = DateTime.UtcNow.AddSeconds(2);

                RenderThreadRun = false;

                while ((DateTime.UtcNow <= TimeoutTime) && !AcknowledgeRTStop)
                {
                    Thread.Sleep(100);
                }

                // Gah.
                if (!AcknowledgeRTStop)
                    RenderThread.Abort();
            }
        }

        ~RenderManager()
        {
            Stop();
        }


        private void RenderProc()
        {
            rt.Show();

            Graphics g = rt.CreateGraphics();

            while (RenderThreadRun && rt.Visible)
            {
                g.Clear(Color.Aquamarine);

                // TODO: Adjust viewport moar!

                Viewport.Width = rt.ClientRectangle.Width;
                Viewport.Height = rt.ClientRectangle.Height;

                lock (Entities)
                {
                    foreach (Entity e in Entities)
                        if (e.IsVisible(g, Viewport))
                            e.Draw(g, Viewport);
                }

                lock (TemporaryEntities)
                {
                    foreach (Entity e in TemporaryEntities)
                        if (e.IsVisible(g, Viewport))
                            e.Draw(g, Viewport);
                }

                g.Flush();

                Application.DoEvents();

                Thread.Sleep(14);
            }
            
            AcknowledgeRTStop = true;
            RenderThreadRun = false;

            rt.Hide();
        }
    }
}
