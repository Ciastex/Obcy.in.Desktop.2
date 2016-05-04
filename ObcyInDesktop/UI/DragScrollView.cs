using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace ObcyInDesktop.UI
{
    public class DragScrollView : ScrollViewer
    {
        private class Vector
        {
            public Vector(double x, double y)
            {
                X = x;
                Y = y;
            }

            public double X { get; }
            public double Y { get; }

            public double Length => Math.Sqrt(X * X + Y * Y);

            public static Vector operator *(Vector vector, double scalar)
            {
                return new Vector(vector.X * scalar, vector.Y * scalar);
            }
        }

        private const double DragPollingInterval = 10;
        private const double DefaultFriction = 0.2;

        private double _friction = DefaultFriction;

        private Point _previousPreviousPoint;
        private Point _previousPoint;
        private Point _currentPoint;

        private DispatcherTimer _dragScrollTimer;

        private bool _mouseDown;
        private bool _isDragging;

        private Vector Velocity => new Vector(_currentPoint.X - _previousPoint.X, _currentPoint.Y - _previousPoint.Y);
        private Vector PreviousVelocity => new Vector(_previousPoint.X - _previousPreviousPoint.X, _previousPoint.Y - _previousPreviousPoint.Y);

        private Vector Momentum { get; set; }

        private void BeginDrag()
        {
            _mouseDown = true;
        }

        private void CancelDrag(Vector velocityToUse)
        {
            if (_isDragging)
                Momentum = velocityToUse;

            _isDragging = false;
            _mouseDown = false;
        }

        private void TickDragScroll(object sender, EventArgs e)
        {
            if (_isDragging)
            {

                var generalTransform = TransformToVisual(this);
                var childToParentCoordinates = generalTransform.Transform(new Point(0, 0));
                var bounds = new Rect(childToParentCoordinates, RenderSize);

                if (bounds.Contains(_currentPoint))
                {
                    PerformScroll(PreviousVelocity);
                }

                if (!_mouseDown)
                {
                    CancelDrag(Velocity);
                }
                _previousPreviousPoint = _previousPoint;
                _previousPoint = _currentPoint;
            }
            else if (Momentum.Length > 0)
            {
                Momentum *= (1.0 - _friction/4.0);
                PerformScroll(Momentum);
            }
            else
            {
                if (_dragScrollTimer != null)
                {
                    _dragScrollTimer.Tick -= TickDragScroll;
                    _dragScrollTimer.Stop();
                    _dragScrollTimer = null;
                }
            }
        }

        private void PerformScroll(Vector displacement)
        {
            var verticalOffset = Math.Max(0.0, VerticalOffset - displacement.Y);
            ScrollToVerticalOffset(verticalOffset);

            var horizontalOffset = Math.Max(0.0, HorizontalOffset - displacement.X);
            ScrollToHorizontalOffset(horizontalOffset);
        }

        private void DragScroll()
        {
            if (_dragScrollTimer == null)
            {
                _dragScrollTimer = new DispatcherTimer();
                _dragScrollTimer.Tick += TickDragScroll;
                _dragScrollTimer.Interval = new TimeSpan(0, 0, 0, 0, (int)DragPollingInterval);
                _dragScrollTimer.Start();
            }
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            CancelDrag(PreviousVelocity);
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            _currentPoint = _previousPoint = _previousPreviousPoint = e.GetPosition(this);
            Momentum = new Vector(0, 0);
            BeginDrag();
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            _currentPoint = e.GetPosition(this);
            if (_mouseDown && !_isDragging)
            {
                _isDragging = true;
                DragScroll();
            }
        }

        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            CancelDrag(PreviousVelocity);
        }
    }
}
