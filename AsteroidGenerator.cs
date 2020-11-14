using Godot;
using System;

[Tool]
public class AsteroidGenerator : Spatial
{
	Icosphere sphere;
	AsteroidGeneratorPlugin dock;

	[Export]
	public int Seed = 0;
	[Export]
	public float Period = 16f;
	[Export]
	public int Octaves = 5;
	[Export]
	public float Persistence = .5f;
	[Export]
	public float Lacunarity = 2.0f;
	[Export]
	public float MoveFactor = 10;
	private Material _material;
	[Export]
	public Material Material
	{
		get { return _material; }
		set
		{
			_material = value;
			UpdateMaterial();
		}
	}

	public override void _Ready()
	{
		//when we open the generator, add our dock to the editor
		dock = new AsteroidGeneratorPlugin();
		AddChild(dock);
		sphere = GetNode<Icosphere>("IcosphereInstance");
	}

	internal void Generate()
	{
		sphere.Generate();
		Deform();
	}

	internal void UpdateMaterial()
	{
		if (Material != null && sphere!=null && sphere.Mesh!=null)
		{
			sphere.Mesh.SurfaceSetMaterial(0, Material);
		}
	}

	private void Deform()
	{
		var sn = new OpenSimplexNoise();
		sn.Period = Period;
		sn.Octaves = Octaves;
		sn.Seed = Seed;
		sn.Persistence = Persistence;
		sn.Lacunarity = Lacunarity;

		var mesh = sphere.Mesh;
		var st = new SurfaceTool();
		st.CreateFrom(mesh, 0);
		
		var array = st.Commit();

		var dt = new MeshDataTool();

		dt.CreateFromSurface(array, 0);

		var count = dt.GetVertexCount();
		var origin = Vector3.Zero;
		for (int i = 0; i < count; i++)
		{
			var vertex = dt.GetVertex(i);
			var n = -vertex.DirectionTo(origin);
			var noise = sn.GetNoise3d(vertex.x, vertex.y, vertex.z);
			noise = Mathf.Clamp(noise, -1f, 1f);
			vertex += n * noise * MoveFactor;
			dt.SetVertex(i, vertex);
		}
		var surfCount = array.GetSurfaceCount();
		for (int i = 0; i < surfCount; i++)
		{
			array.SurfaceRemove(i);
		}

		dt.CommitToSurface(array);
		st.Begin(Mesh.PrimitiveType.Triangles);
		st.CreateFrom(array, 0);
		st.GenerateNormals();

		sphere.Mesh = st.Commit();

		UpdateMaterial();
	}
}
