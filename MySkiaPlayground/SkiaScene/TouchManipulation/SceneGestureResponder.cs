using System;
using System.Diagnostics;
using System.Linq;
using MySkiaPlayground.Effects;
using MySkiaPlayground.SkiaScene;
using MySkiaPlayground.SkiaScene.SkiaObjects;
using SkiaSharp;

namespace SkiaScene.TouchManipulation
{
    public class SceneGestureResponder : ISceneGestureResponder
    {
        private readonly ISKScene _skScene;
        private readonly ITouchGestureRecognizer _touchGestureRecognizer;

        public SceneGestureResponder(ISKScene skScene, ITouchGestureRecognizer touchGestureRecognizer)
        {
            _skScene = skScene;
            _touchGestureRecognizer = touchGestureRecognizer;
        }

        public TouchManipulationMode TouchManipulationMode { get; set; }
        public bool EnableTwoFingersPanInIsotropicScaleMode { get; set; }
        public float DoubleTapScaleFactor { get; set; } = 2f;

        public void StartResponding()
        {
            _touchGestureRecognizer.OnPan += TouchGestureRecognizerOnPan;
            _touchGestureRecognizer.OnPinch += TouchGestureRecognizerOnPinch;
            _touchGestureRecognizer.OnDoubleTap += TouchGestureRecognizerOnDoubleTap;
        }
        public void StopResponding()
        {
            _touchGestureRecognizer.OnPan -= TouchGestureRecognizerOnPan;
            _touchGestureRecognizer.OnPinch -= TouchGestureRecognizerOnPinch;
            _touchGestureRecognizer.OnDoubleTap -= TouchGestureRecognizerOnDoubleTap;
        }


        protected virtual void TouchGestureRecognizerOnPinch(object sender, PinchEventArgs args)
        {
            if (args.TouchActionType != TouchActionType.Moved)
                return;

            var previousPoint = args.PreviousPoint;
            var newPoint = args.NewPoint;
            var pivotPoint = args.PivotPoint;
            var transformedPivotPoint = _skScene.GetCanvasPointFromViewPoint(pivotPoint);

            SKPoint oldVector = previousPoint - pivotPoint;
            SKPoint newVector = newPoint - pivotPoint;

            if (TouchManipulationMode == TouchManipulationMode.ScaleRotate)
            {
                float angle = GetAngleBetweenVectors(oldVector, newVector);

                Debug.WriteLine($"moved rotate to {newVector}");

                _skScene.RotateByRadiansDelta(transformedPivotPoint, angle);

                // Effectively rotate the old vector
                float magnitudeRatio = oldVector.GetMagnitude() / newVector.GetMagnitude();
                oldVector.X = magnitudeRatio * newVector.X;
                oldVector.Y = magnitudeRatio * newVector.Y;
            }
            else if (TouchManipulationMode == TouchManipulationMode.IsotropicScale
                && EnableTwoFingersPanInIsotropicScaleMode)
            {
                float angle = GetAngleBetweenVectors(oldVector, newVector);

                // Calculate the movement as a distance between old vector and a new vector
                // but in orthogonal direction to the old vector.

                float oldVectorOriginPoint = newVector.GetMagnitude() * (float)Math.Cos(angle);
                float magnitudeRatio = oldVectorOriginPoint / oldVector.GetMagnitude();
                SKPoint oldVectorOrigin = new SKPoint(oldVector.X * magnitudeRatio, oldVector.Y * magnitudeRatio);
                SKPoint moveVector = newVector - oldVectorOrigin;
                SKPoint dividedMoveVector = new SKPoint(moveVector.X * 0.5f, moveVector.Y * 0.5f);

                Debug.WriteLine($"moved pinch to {dividedMoveVector}");
                _skScene.MoveByVector(dividedMoveVector);
            }

            var scale = newVector.GetMagnitude() / oldVector.GetMagnitude();

            if (!float.IsNaN(scale) && !float.IsInfinity(scale))
            {
                _skScene.ZoomByScaleFactor(transformedPivotPoint, scale);
            }
        }

        protected virtual void TouchGestureRecognizerOnPan(object sender, PanEventArgs args)
        {
            //scene check if tap is in any object square
            //make object selectable
            //pan object by coords change

            if (args.TouchActionType != TouchActionType.Moved)
                return;
            SKPoint resultVector = args.NewPoint - args.PreviousPoint;

            //var figure = _skScene.Figures?.FirstOrDefault(f =>
            //{
            //    if (f is Rect rect)
            //        return rect.IsPointInRect(args.NewPoint);
            //    return false;
            //});
            //if (figure != null)
            //{
            //    figure.X += resultVector.X;
            //    figure.Y += resultVector.Y;
            //}

            _skScene.MoveByVector(resultVector);
        }

        protected virtual void TouchGestureRecognizerOnDoubleTap(object sender, TapEventArgs args)
        {
            SKPoint scenePoint = _skScene.GetCanvasPointFromViewPoint(args.ViewPoint);
            _skScene.ZoomByScaleFactor(scenePoint, DoubleTapScaleFactor);
        }

        private float GetAngleBetweenVectors(SKPoint oldVector, SKPoint newVector)
        {
            // Find angles from pivot point to touch points
            float oldAngle = (float)Math.Atan2(oldVector.Y, oldVector.X);
            float newAngle = (float)Math.Atan2(newVector.Y, newVector.X);

            float angle = newAngle - oldAngle;
            return angle;
        }
    }
}