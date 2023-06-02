﻿using Engine;

namespace Game {
    public class GVSignTextData {
        public Point3 Point;
        public Vector3 FloatPosition;

        public string Line = string.Empty;

        public Color Color = Color.White;
        public Color FloatColor = Color.White;

        public string Url = string.Empty;

        public int? TextureLocation;

        public float UsedTextureWidth;

        public float UsedTextureHeight;
        public float FloatSize;

        public int ToBeRenderedFrame;

        public int Light;
        public int FloatLight;
        public Vector3 FloatRotation;
    }
}