using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class PlayerMovement : Movement
    {

        public PlayerMovement(float acceleration, float decelaration, float maxVelocity) : base(acceleration, decelaration, maxVelocity)
        {
        }
        public override void Update()
        {
            HandleFriction();
            HandleKeyInput();
            base.Update();
        }
        private void UpdateSpeed(Directions direction)
        {
            switch (direction)
            {
                case Directions.Up:
                    Velocity.Y -= Velocity.Y <= 0 ? Acceleration : Deceleration;
                    if (-Velocity.Y > MaxVelocity) Velocity.Y = -MaxVelocity;
                    break;
                case Directions.Right:
                    Velocity.X += Velocity.X >= 0 ? Acceleration : Deceleration;
                    if (Velocity.X > MaxVelocity) Velocity.X = MaxVelocity;
                    break;
                case Directions.Down:
                    Velocity.Y += Velocity.Y >= 0 ? Acceleration : Deceleration;
                    if (Velocity.Y > MaxVelocity) Velocity.Y = MaxVelocity;
                    break;
                case Directions.Left:
                    Velocity.X -= Velocity.X <= 0 ? Acceleration : Deceleration;
                    if (-Velocity.X > MaxVelocity) Velocity.X = -MaxVelocity;
                    break;
            }


        }
        private float GetFriction()
        {
            return 0.03f;
        }

        private void HandleFriction()
        {
            float friction = GetFriction();

            if (Velocity.Y != 0) Velocity.Y += Velocity.Y > 0 ? -friction : friction;
            if (Velocity.X != 0) Velocity.X += Velocity.X > 0 ? -friction : friction;
            if (System.Math.Abs(Velocity.Y) < friction) Velocity.Y = 0;
            if (System.Math.Abs(Velocity.X) < friction) Velocity.X = 0;
        }
        private void HandleKeyInput()
        {
            foreach (Keys key in Keyboard.GetState().GetPressedKeys())
            {
                switch (key)
                {
                    case Keys.W:
                        UpdateSpeed(Directions.Up);
                        break;
                    case Keys.D:
                        UpdateSpeed(Directions.Right);
                        break;
                    case Keys.S:
                        UpdateSpeed(Directions.Down);
                        break;
                    case Keys.A:
                        UpdateSpeed(Directions.Left);
                        break;
                }
            }
        }
        enum Directions
        {
            Up,
            Right,
            Down,
            Left
        }
    }
}
