﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using Engine;
using Engine.Graphics;
using TemplatesDatabase;
using XamariNES.Controller;
using XamariNES.Emulator;

namespace Game {
    public class SubsystemNesEmulatorBlockBehavior : SubsystemEditableItemBehavior<EditGVNesEmulatorDialogData>, IDrawable {
        public SubsystemSky m_subsystemSky;
        public SubsystemGameInfo m_subsystemGameInfo;
        public PrimitivesRenderer3D m_primitivesRenderer = new();
        public Dictionary<GVNesEmulatorGlowPoint, bool> m_glowPoints = new();
        public readonly NESEmulator _emu;
        readonly BitmapRenderer _renderer;
        byte[] _frame = new byte[256 * 240];
        public bool EmuStarted;
        public bool RomValid;

        public SubsystemNesEmulatorBlockBehavior() : base(GVNesEmulatorBlock.Index) {
            _emu = new NESEmulator(GetByteFromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("Game.MoreLeds.NesEmulator.nestest.nes")), GetFrameFromEmulator);
            RomValid = true;
            _renderer = new BitmapRenderer();
        }

        public override void Load(ValuesDictionary valuesDictionary) {
            base.Load(valuesDictionary);
            m_subsystemSky = Project.FindSubsystem<SubsystemSky>(true);
            m_subsystemGameInfo = Project.FindSubsystem<SubsystemGameInfo>(true);
            EditGVNesEmulatorDialogData data = GetBlockData(new Point3(-GVNesEmulatorBlock.Index));
            if (data != null
                && data.Data.Length > 0) {
                try {
                    LoadRomFromPath(data.Data);
                }
                catch (Exception ex) {
                    Log.Error(ex);
                }
            }
        }

        public override int[] HandledBlocks => [GVNesEmulatorBlock.Index];

        public override bool OnEditInventoryItem(IInventory inventory, int slotIndex, ComponentPlayer componentPlayer) {
            if (componentPlayer.DragHostWidget.IsDragInProgress) {
                return false;
            }
            EditGVNesEmulatorDialogData Data = GetBlockData(new Point3(-GVNesEmulatorBlock.Index)) ?? new EditGVNesEmulatorDialogData();
            DialogsManager.ShowDialog(componentPlayer.GuiWidget, new EditGVNesEmulatorDialog(Data, this, delegate { SetBlockData(new Point3(-GVNesEmulatorBlock.Index), Data); }));
            return true;
        }

        public override bool OnEditBlock(int x, int y, int z, int value, ComponentPlayer componentPlayer) {
            EditGVNesEmulatorDialogData Data = GetBlockData(new Point3(-GVNesEmulatorBlock.Index)) ?? new EditGVNesEmulatorDialogData();
            DialogsManager.ShowDialog(
                componentPlayer.GuiWidget,
                new EditGVNesEmulatorDialog(
                    Data,
                    this,
                    delegate {
                        SetBlockData(new Point3(-GVNesEmulatorBlock.Index), Data);
                        int face = ((GVNesEmulatorBlock)BlocksManager.Blocks[GVNesEmulatorBlock.Index]).GetFace(value);
                        SubsystemGVElectricity subsystemGVElectricity = SubsystemTerrain.Project.FindSubsystem<SubsystemGVElectricity>(true);
                        GVElectricElement GVElectricElement = subsystemGVElectricity.GetGVElectricElement(x, y, z, face);
                        if (GVElectricElement != null) {
                            subsystemGVElectricity.QueueGVElectricElementForSimulation(GVElectricElement, subsystemGVElectricity.CircuitStep + 1);
                        }
                    }
                )
            );
            return true;
        }

        DateTime _lastUpdatedTime = DateTime.MinValue;

        public static int[] m_drawOrders = [111];

        public TexturedBatch3D cachedBatch;
        public int[] DrawOrders => m_drawOrders;

        public void Draw(Camera camera, int drawOrder) {
            bool powerOn = false;
            bool reset = false;
            byte controller1 = 0;
            //byte controller2 = 0;
            foreach (GVNesEmulatorGlowPoint key in m_glowPoints.Keys) {
                if (key.GetPowerOn()) {
                    powerOn = true;
                }
                if (key.GetReset()) {
                    reset = true;
                }
                controller1 |= key.GetController1();
                //controller2 |= key.GetController2();
            }
            if (reset) {
                _emu.Reset();
                EmuStarted = false;
            }
            else {
                if (powerOn) {
                    if (EmuStarted) {
                        _emu.Continue();
                    }
                    else {
                        _emu.Start();
                        EmuStarted = true;
                    }
                    ((NESController)_emu.Controller1).ButtonStates = controller1;
                }
                else {
                    if (EmuStarted) {
                        _emu.Stop();
                    }
                }
            }
            if (!reset
                && powerOn
                && m_glowPoints.Count > 0) {
                DateTime now = DateTime.Now;
                if ((now - _lastUpdatedTime).TotalMilliseconds > 33) {
                    _lastUpdatedTime = now;
                    if (!RomValid) {
                        _frame = _renderer.GenerateNoise();
                    }
                    if (cachedBatch != null) {
                        cachedBatch.Texture.Dispose();
                    }
                    cachedBatch = m_primitivesRenderer.TexturedBatch(
                        Texture2D.Load(_renderer.Render(_frame)),
                        false,
                        0,
                        DepthStencilState.DepthRead,
                        RasterizerState.CullCounterClockwiseScissor,
                        BlendState.AlphaBlend,
                        SamplerState.PointClamp
                    );
                }
                foreach (GVNesEmulatorGlowPoint key in m_glowPoints.Keys) {
                    if (key.GetPowerOn()) {
                        float halfSize = key.GetSize() * 0.5f;
                        Vector3 right = key.Right * halfSize;
                        Vector3 up = key.Up * halfSize * 0.9375f;
                        Vector3[] offsets = [right - up, right + up, -right - up, -right + up];
                        Vector3 min = Vector3.Zero;
                        Vector3 max = Vector3.Zero;
                        foreach (Vector3 offset in offsets) {
                            min.X = Math.Min(min.X, offset.X);
                            min.Y = Math.Min(min.Y, offset.Y);
                            min.Z = Math.Min(min.Z, offset.Z);
                            max.X = Math.Max(max.X, offset.X);
                            max.Y = Math.Max(max.Y, offset.Y);
                            max.Z = Math.Max(max.Z, offset.Z);
                        }
                        if (camera.ViewFrustum.Intersection(new BoundingBox(key.Position + min, key.Position + max))) {
                            Vector3 p = key.Position - right - up;
                            Vector3 p2 = key.Position + right - up;
                            Vector3 p3 = key.Position + right + up;
                            Vector3 p4 = key.Position - right + up;
                            switch (key.GetRotation()) {
                                case 1:
                                    cachedBatch.QueueQuad(
                                        p,
                                        p2,
                                        p3,
                                        p4,
                                        new Vector2(1f, 0f),
                                        new Vector2(1f, 1f),
                                        new Vector2(0f, 1f),
                                        new Vector2(0f, 0f),
                                        Color.White
                                    );
                                    break;
                                case 2:
                                    cachedBatch.QueueQuad(
                                        p,
                                        p2,
                                        p3,
                                        p4,
                                        new Vector2(0f, 0f),
                                        new Vector2(1f, 0f),
                                        new Vector2(1f, 1f),
                                        new Vector2(0f, 1f),
                                        Color.White
                                    );
                                    break;
                                case 3:
                                    cachedBatch.QueueQuad(
                                        p,
                                        p2,
                                        p3,
                                        p4,
                                        new Vector2(0f, 1f),
                                        new Vector2(0f, 0f),
                                        new Vector2(1f, 0f),
                                        new Vector2(1f, 1f),
                                        Color.White
                                    );
                                    break;
                                default:
                                    cachedBatch.QueueQuad(
                                        p,
                                        p2,
                                        p3,
                                        p4,
                                        new Vector2(1f, 1f),
                                        new Vector2(0f, 1f),
                                        new Vector2(0f, 0f),
                                        new Vector2(1f, 0f),
                                        Color.White
                                    );
                                    break;
                            }
                        }
                    }
                }
                m_primitivesRenderer.Flush(camera.ViewProjectionMatrix);
            }
        }

        public GVNesEmulatorGlowPoint AddGlowPoint() {
            GVNesEmulatorGlowPoint glowPoint = new();
            m_glowPoints.Add(glowPoint, true);
            return glowPoint;
        }

        public void RemoveGlowPoint(GVNesEmulatorGlowPoint glowPoint) {
            m_glowPoints.Remove(glowPoint);
        }

        public void LoadRomFromPath(string path) {
            byte[] bytes = null;
            try {
                // ReSharper disable once StringLiteralTypo
                if (path == "nestest") {
                    bytes = GetByteFromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("Gigavolt.Expand.NesEmulator.nestest.nes"));
                }
                else if (uint.TryParse(path, NumberStyles.HexNumber, null, out uint uintResult)
                    && GVStaticStorage.GVMBIDDataDictionary.TryGetValue(uintResult, out GVArrayData data)) {
                    if (data.m_worldDirectory == null) {
                        data.m_worldDirectory = m_subsystemGameInfo.DirectoryName;
                        data.LoadData();
                    }
                    bytes = data.GetBytes();
                }
                else {
                    using (Stream stream = Storage.OpenFile(path, OpenFileMode.Read)) {
                        bytes = GetByteFromStream(stream);
                    }
                }
            }
            catch (Exception e) {
                Log.Error(e);
            }
            if (bytes != null) {
                _emu._cartridge.LoadROM(bytes);
                _emu.LoadRom(bytes);
            }
        }

        /// <summary>
        ///     Delegate to receive frame that's ready from the emulator and
        ///     trigger a draw event.
        ///     TODO: Because this isn't thread safe, this might lead to some
        ///     screen tearing. Probably need to refactor this.
        /// </summary>
        /// <param name="frame"></param>
        void GetFrameFromEmulator(byte[] frame) {
            _frame = frame;
            //MessagingCenter.Send(this, "InvalidateEmulatorSurface");
        }

        /// <summary>
        ///     Reads a stream resource to a byte array
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] GetByteFromStream(Stream stream) {
            byte[] output = new byte[stream.Length];
            _ = stream.Read(output, 0, (int)stream.Length);
            return output;
        }
    }
}