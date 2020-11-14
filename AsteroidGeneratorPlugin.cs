using Godot;
using System;

public class AsteroidGeneratorPlugin : EditorPlugin
{
    AsteroidGeneratorDock dock;
    AsteroidGenerator _gen;
    
    public override void _EnterTree()
    {
        dock = (AsteroidGeneratorDock)GD.Load<PackedScene>("res://AsteroidGeneratorDock.tscn").Instance();
        AddControlToDock(DockSlot.RightUr, dock);
        _gen = GetParent<AsteroidGenerator>();
        dock.BindToGenerator(_gen);
    }

    public override void _ExitTree()
    {
        RemoveControlFromDocks(dock);
        dock.Free();
    }
}
