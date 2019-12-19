using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OgreVector3 = org.ogre.Vector3;
using VectorBase3 = org.ogre.VectorBase3;

namespace PacmanOgre.Utilities
{
    public static class VectorUtils
    {
        public static class Vector3
        {
            public static OgreVector3 ZERO              { get; } = VectorBase3.ZERO;
            public static OgreVector3 UNIT_X            { get; } = VectorBase3.UNIT_X;
            public static OgreVector3 UNIT_Y            { get; } = VectorBase3.UNIT_Y;
            public static OgreVector3 UNIT_Z            { get; } = VectorBase3.UNIT_Z;
            public static OgreVector3 NEGATIVE_UNIT_X   { get; } = VectorBase3.NEGATIVE_UNIT_X;
            public static OgreVector3 NEGATIVE_UNIT_Y   { get; } = VectorBase3.NEGATIVE_UNIT_Y;
            public static OgreVector3 NEGATIVE_UNIT_Z   { get; } = VectorBase3.NEGATIVE_UNIT_Z;
            public static OgreVector3 UNIT_SCALE        { get; } = VectorBase3.UNIT_SCALE;
        }
    }
}
