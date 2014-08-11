using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceInvader
{
    class Invader
    {
        Vector2 AlienPos;
        //vector for assigning position
        public Invader()
        {
            AlienPos = new Vector2();

            AlienPos.X = 0;
            AlienPos.Y = 0;
        }
        //method for moving the invaders horizontally
        public void MoveHorizontal(int amount)
        {
            AlienPos.X = AlienPos.X + amount;
        }
        //method for moving invaders vertically
        public void MoveVertical(int amount)
        {
            AlienPos.Y = AlienPos.Y + amount;
        }

        //setting and getting positions
        public void SetXPos(int pos)
        {
            AlienPos.X = pos;
        }

        public int GetXPos()
        {
            return (int)AlienPos.X;
        }

        public void SetYPos(int pos)
        {
            AlienPos.Y = pos;
        }

        public int GetYPos()
        {
            return (int)AlienPos.Y;
        }


        //returning the position of the invader.
        public Vector2 GetPos()
        {
            return AlienPos;
        }
    }
}
