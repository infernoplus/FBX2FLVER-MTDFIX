﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBX2FLVER.Solvers
{
    public class BoundingBoxSolver
    {
        private readonly FBX2FLVERImporter Importer;
        public BoundingBoxSolver(FBX2FLVERImporter Importer)
        {
            this.Importer = Importer;
        }

        //private List<FlverVertex> GetVerticesParentedToBone(SoulsFormats.FLVER f, FlverBone b)
        //{
        //    var result = new List<FlverVertex>();
        //    foreach (var sm in f.Meshes)
        //    {
        //        foreach (var v in sm.Vertices)
        //        {
        //            var bones = v.BoneIndices.GetBones();
        //            if (bones.Contains(b))
        //                result.Add(v);
        //        }
        //    }
        //    return result;
        //}

        //private BoundingBox GetBoundingBox(List<Vector3> verts)
        //{
        //    if (verts.Count > 0)
        //        return BoundingBox.CreateFromPoints(verts);
        //    else
        //        return new BoundingBox(Vector3.Zero, Vector3.Zero);
        //}

        //Matrix GetParentBoneMatrix(FlverBone bone)
        //{
        //    FlverBone parent = bone;

        //    var boneParentMatrix = Matrix.Identity;

        //    do
        //    {
        //        boneParentMatrix *= Matrix.CreateScale(parent.Scale);
        //        boneParentMatrix *= Matrix.CreateRotationX(parent.EulerRadian.X);
        //        boneParentMatrix *= Matrix.CreateRotationZ(parent.EulerRadian.Z);
        //        boneParentMatrix *= Matrix.CreateRotationY(parent.EulerRadian.Y);

        //        //boneParentMatrix *= Matrix.CreateRotationY(parent.EulerRadian.Y);
        //        //boneParentMatrix *= Matrix.CreateRotationZ(parent.EulerRadian.Z);
        //        //boneParentMatrix *= Matrix.CreateRotationX(parent.EulerRadian.X);
        //        boneParentMatrix *= Matrix.CreateTranslation(parent.Translation);
        //        //boneParentMatrix *= Matrix.CreateScale(parent.Scale);

        //        parent = parent.GetParent();
        //    }
        //    while (parent != null);

        //    return boneParentMatrix;
        //}

        //private void SetBoneBoundingBox(SoulsFormats.FLVER f, FlverBone b)
        //{
        //    var bb = GetBoundingBox(GetVerticesParentedToBone(f, b).Select(v => (Vector3)v.Position).ToList());
        //    if (bb.Max.LengthSquared() != 0 || bb.Min.LengthSquared() != 0)
        //    {
        //        var matrix = GetParentBoneMatrix(b);
        //        b.BoundingBoxMin = Vector3.Transform(bb.Min, Matrix.Invert(matrix));
        //        b.BoundingBoxMax = Vector3.Transform(bb.Max, Matrix.Invert(matrix));
        //    }
        //    else
        //    {
        //        b.BoundingBoxMin = null;
        //        b.BoundingBoxMax = null;
        //    }
        //}

        //public void FixAllBoundingBoxes(SoulsFormats.FLVER f)
        //{
        //    foreach (var b in f.Bones)
        //    {
        //        SetBoneBoundingBox(f, b);
        //        if (b.Name == "Dummy")
        //            b.Name = "dymmy";
        //        else if (b.Name == "SFX")
        //            b.Name = "SFX用";
        //    }


        //    var submeshBBs = new List<BoundingBox>();

        //    foreach (var sm in f.Meshes)
        //    {
        //        var bb = GetBoundingBox(sm.Vertices.Select(v => new Vector3(v.Position.X, v.Position.Y, v.Position.Z)).ToList());
        //        if (bb.Max.LengthSquared() != 0 || bb.Min.LengthSquared() != 0)
        //        {
        //            submeshBBs.Add(bb);
        //        }
        //    }

        //    if (submeshBBs.Count > 0)
        //    {
        //        var finalBB = submeshBBs[0];
        //        for (int i = 1; i < submeshBBs.Count; i++)
        //        {
        //            finalBB = BoundingBox.CreateMerged(finalBB, submeshBBs[i]);
        //        }

        //        f.Header.BoundingBoxMin = new System.Numerics.Vector3(finalBB.Min.X, finalBB.Min.Y, finalBB.Min.Z);
        //        f.Header.BoundingBoxMax = new System.Numerics.Vector3(finalBB.Max.X, finalBB.Max.Y, finalBB.Max.Z);
        //    }
        //    else
        //    {
        //        f.Header.BoundingBoxMin = new System.Numerics.Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        //        f.Header.BoundingBoxMax = new System.Numerics.Vector3(float.MinValue, float.MinValue, float.MinValue);
        //    }


            
        //}
    }
}
