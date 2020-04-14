using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class Player : Actor
    {
        public Player(int maxHealth, int health, Vector2 spriteIndex, float acceleration, float deceleration, float maxVelocity) : base(maxHealth, health, spriteIndex, acceleration, deceleration, maxVelocity)
        {
            movement = new PlayerMovement(acceleration, deceleration, maxVelocity);
        }
    }
}
