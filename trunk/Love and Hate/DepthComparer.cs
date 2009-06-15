using System;
using System.Collections.Generic;
using System.Text;

namespace FlockU
{
	class DepthComparer : IComparer<Enemy>
	{
        public int Compare(Enemy sprite1, Enemy sprite2)
        {
            if (sprite1.mPositionY > sprite2.mPositionY)
                return 1;

            else if (sprite1.mPositionY < sprite2.mPositionY)
                return -1;

            return 0;
        }
	}
}
