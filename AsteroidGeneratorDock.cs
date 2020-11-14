using Godot;
using System;

[Tool]
public class AsteroidGeneratorDock : Control
{
    AsteroidGenerator _gen;
    FileDialog _fd;

    public void BindToGenerator(AsteroidGenerator gen)
    {
        _gen = gen;
    }

    public void OnGenerate()
    {
        _gen.Generate();
    }

    public void OnSaveMesh()
    {
        _fd = new FileDialog();
        _fd.Mode = FileDialog.ModeEnum.SaveFile;
        _fd.AddFilter("*.tres, *.res ; Resource files");
        _fd.Connect("file_selected", this, "OnMeshSaveOK");
        _fd.DialogHideOnOk = true;
        AddChild(_fd);
        _fd.PopupCentered(new Vector2(800,600));
    }

    public void OnSaveTrimeshShape()
    {
        _fd = new FileDialog();
        _fd.Mode = FileDialog.ModeEnum.SaveFile;
        _fd.AddFilter("*.tres, *.res ; Resource files");
        _fd.Connect("file_selected", this, "OnTrimeshShapeSaveOK");
        _fd.DialogHideOnOk = true;
        AddChild(_fd);
        _fd.PopupCentered(new Vector2(800, 600));
    }

    public void OnSaveConvexShape()
    {
        _fd = new FileDialog();
        _fd.Mode = FileDialog.ModeEnum.SaveFile;
        _fd.AddFilter("*.tres, *.res ; Resource files");
        _fd.Connect("file_selected", this, "OnConvexShapeSaveOK");
        _fd.DialogHideOnOk = true;
        AddChild(_fd);
        _fd.PopupCentered(new Vector2(800, 600));
    }

    public void OnMeshSaveOK(string path)
    {
        var sphere = _gen.GetNode<Icosphere>("IcosphereInstance");
        ResourceSaver.Save(path, sphere.Mesh);
    }

    public void OnTrimeshShapeSaveOK(string path)
    {
        var sphere = _gen.GetNode<Icosphere>("IcosphereInstance");
        var shape = sphere.Mesh.CreateTrimeshShape();
        ResourceSaver.Save(path, shape);
    }

    public void OnConvexShapeSaveOK(string path)
    {
        var sphere = _gen.GetNode<Icosphere>("IcosphereInstance");
        var shape = sphere.Mesh.CreateConvexShape();
        ResourceSaver.Save(path, shape);
    }
}
