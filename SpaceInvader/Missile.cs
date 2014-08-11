using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceInvader
{
    class Missile
    {
        Vector2 Position;

        //missile positions
        public Missile(int XInitialPos, int YInitialPos)
        {
            Position = new Vector2(XInitialPos, YInitialPos);
        }
        //method to move missile 
        public void Move()
        {
            Position.Y = Position.Y - 4;
        }
        //method to move missile down, when invaders ifre
        public void MoveReverse()
        {
            Position.Y = Position.Y + 4;
        }
        //method to retun postion
        public Vector2 GetPosition()
        {
            return Position;
        }
    }
}
