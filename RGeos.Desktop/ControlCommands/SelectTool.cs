﻿using System.Drawing;
using RGeos.Display;
using RGeos.Controls;
using RGeos.Geometries;
using RGeos.Core;
using RGeos.Carto;
using System.Collections.Generic;
namespace RGeos.Desktop
{
    class SelectTool : RBaseCommand
    {
        RgMapControl mMapCtrl = null;
        public IDisplayFeedback DrawPhase = null;

        public override string Name { get; set; }
        SelectionRectangle m_selection = null;
        IScreenDisplay mScreenDisplay = null;
        IScreenDisplayDraw mScreenDisplayDraw = null;

        Map mMap = null;
        public override void OnCreate(HookHelper hook)
        {
            Name = "选择要素";
            mMapCtrl = hook.MapControl as RgMapControl;
            mScreenDisplay = mMapCtrl.ScreenDisplay;
            mScreenDisplayDraw = mScreenDisplay as IScreenDisplayDraw;
            mMap = mMapCtrl.Map as Map;
        }

        public override void OnClick()
        {

        }

        public override void OnMouseMove(int x, int y)
        {
            if (m_selection != null)
            {
                Graphics dc = Graphics.FromHwnd(mMapCtrl.Handle);
                m_selection.SetMousePoint(mScreenDisplay as ScreenDisplay, dc, new PointF(x, y));
                dc.Dispose();
                return;
            }
        }
        Rectangle screenSelRect = Rectangle.Empty;
        public override void OnMouseDown(int x, int y, System.Windows.Forms.MouseEventArgs e)
        {
            PointF m_mousedownPoint = new PointF(x, y);
            m_selection = new SelectionRectangle(m_mousedownPoint);
        }

        public override void OnMouseUp(int x, int y)
        {
            if (m_selection != null)
            {
                List<Feature> SelectFeatures = new List<Feature>();
                screenSelRect = m_selection.ScreenRect();
                RgPoint lowLeft = mScreenDisplay.DisplayTransformation.ToUnit(new PointF(screenSelRect.Left, screenSelRect.Bottom));
                RgPoint upRight = mScreenDisplay.DisplayTransformation.ToUnit(new PointF(screenSelRect.Right, screenSelRect.Top));
                BoundingBox box = new BoundingBox(lowLeft, upRight);
                //RectangleF selectionRect = m_selection.Selection(m_canvaswrapper);
                if (screenSelRect != Rectangle.Empty)
                {
                    // is any selection rectangle. use it for selection
                    for (int i = 0; i < mMap.Layers.Count; i++)
                    {
                        ILayer lyr = mMap.Layers[i];
                        if (lyr is FetureLayer)
                        {
                            Carto.FetureLayer featlyr = lyr as Carto.FetureLayer;
                            SelectFeatures = featlyr.GetHitObjects(box, m_selection.AnyPoint());
                            if (SelectFeatures != null)
                            {
                                for (int j = 0; j < SelectFeatures.Count; j++)
                                {
                                    SelectFeatures[j].IsSelected = true;
                                }
                                if (mMap.Selection == null)
                                {
                                    mMap.Selection = new MapSelection();
                                }

                                mMap.Selection.SelectedFeatures.AddRange(SelectFeatures);
                            }
                        }

                    }
                }
                else
                {
                    for (int i = 0; i < mMap.Layers.Count; i++)
                    {
                        ILayer lyr = mMap.Layers[i];
                        if (lyr is FetureLayer)
                        {
                            Carto.FetureLayer featlyr = lyr as Carto.FetureLayer;
                            RgPoint mousepoint = mScreenDisplay.DisplayTransformation.ToUnit(new PointF(x, y));
                            SelectFeatures = featlyr.GetHitObjects(mousepoint, 0.1);
                            if (SelectFeatures != null)
                            {
                                for (int j = 0; j < SelectFeatures.Count; j++)
                                {
                                    SelectFeatures[j].IsSelected = true;
                                }
                                if (mMap.Selection == null)
                                {
                                    mMap.Selection = new MapSelection();
                                }

                                mMap.Selection.SelectedFeatures.AddRange(SelectFeatures);
                            }
                        }
                    }
                }
                mMapCtrl.Refresh();
                m_selection = null;
            }
        }
    }
}
