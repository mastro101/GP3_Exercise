using System.Collections.Generic;
using UnityEngine;

public class PolygonGenerator : MonoBehaviour
{
    [SerializeField] MeshFilter mf;
    [SerializeField] SkinnedMeshRenderer smr;
    [Space]
    [SerializeField][Range(1,360)] int sides;
    [SerializeField] float radius;

    Mesh mesh;
    List<Vector3> vertices;
    int[] indices;
    List<Vector2> UVs;

    Transform bonesParentTransform;

    Transform[] bones;
    Matrix4x4[] bindPoses;

    public void GenerateMesh()
    {
        vertices = new List<Vector3>();
        indices = new int[sides * 3];
        UVs = new List<Vector2>(sides + 1);

        if (mesh)
            DestroyImmediate(mesh);
        if (bonesParentTransform)
            DestroyImmediate(bonesParentTransform.gameObject);

        mesh = new Mesh();
        mf.mesh = mesh;

        Vector3 upVector = Vector3.up * radius;
        float delta = 360 / sides;
        // set vertices
        vertices.Add(Vector3.zero);
        for (int i = 0; i < sides; i++)
        {
            vertices.Add((Quaternion.Euler(0,0,delta * i) * upVector));
        }
        mesh.SetVertices(vertices);

        // set indices
        int l = sides - 1;
        for (int i = 0; i < l; i++)
        {
            indices[i*3]     = 0;
            indices[i*3 + 1] = i + 2;
            indices[i*3 + 2] = i + 1;
        }
        indices[l*3]     = 0;
        indices[l*3 + 2] = sides;
        indices[l*3 + 1] = 1; 
        mesh.SetIndices(indices, MeshTopology.Triangles, 0);

        // Set UV
        for (int i = 0; i < vertices.Count; i++)
        {
            UVs.Add(vertices[i]);
        }
        mesh.SetUVs(0, UVs);

        mesh.RecalculateNormals();

        // Set BoneWeight
        BoneWeight[] weights = new BoneWeight[vertices.Count];
        for (int i = 0; i < weights.Length; i++)
        {
            weights[i].boneIndex0 = i;
            weights[i].weight0 = 1;
        }

        mesh.boneWeights = weights;

        bonesParentTransform = new GameObject("Bones").transform;
        bonesParentTransform.position = transform.position;
        bonesParentTransform.transform.SetParent(transform);
        bonesParentTransform.gameObject.AddComponent<Movement>();

        bones = new Transform[vertices.Count];
        bindPoses = new Matrix4x4[vertices.Count];

        CreateBones(0, bonesParentTransform);

        for (int i = 1; i < bones.Length; i++)
        {
            CreateBones(i);
        }
        mesh.bindposes = bindPoses;

        smr.bones = bones;
        smr.sharedMesh = mesh;

        // Set Joint and distance joint
        for (int i = 0; i < bones.Length; i++)
        {
            for (int n = 0; n < bones.Length; n++)
            {
                if (i == n) continue;

                Rigidbody2D rbBone = bones[n].gameObject.GetComponent<Rigidbody2D>();
                DistanceJoint2D maxDist = bones[i].gameObject.AddComponent<DistanceJoint2D>();
                maxDist.autoConfigureDistance = false;
                maxDist.autoConfigureConnectedAnchor = false;
                maxDist.maxDistanceOnly = true;
                maxDist.distance = Vector3.Distance(bones[n].position, bones[i].position);
                maxDist.connectedBody = rbBone;
                maxDist.enableCollision = true;
                SpringJoint2D spring = bones[i].gameObject.AddComponent<SpringJoint2D>();
                spring.connectedBody = rbBone;
                spring.enableCollision = true;
                spring.autoConfigureDistance = false;
                spring.distance = Vector3.Distance(bones[n].position, bones[i].position);
            }
        }
    }

    void CreateBones(int i)
    {
        bones[i] = new GameObject(i.ToString()).transform;
        bones[i].SetParent(bonesParentTransform);
        bones[i].localRotation = Quaternion.identity;
        bones[i].localPosition = vertices[i];
        bones[i].gameObject.AddComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        bones[i].gameObject.AddComponent<CircleCollider2D>().radius = 0.1f;
        bindPoses[i] = bones[i].worldToLocalMatrix * transform.localToWorldMatrix;
    }

    void CreateBones(int i, Transform _transform)
    {
        bones[i] = _transform;
        bones[i].SetParent(bonesParentTransform);
        bones[i].localRotation = Quaternion.identity;
        bones[i].localPosition = vertices[i];
        bones[i].gameObject.AddComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        bones[i].gameObject.AddComponent<CircleCollider2D>().radius = 0.1f;
        bindPoses[i] = bones[i].worldToLocalMatrix * transform.localToWorldMatrix;
    }
}