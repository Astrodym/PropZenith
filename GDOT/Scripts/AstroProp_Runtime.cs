// Jonathan (JC) Ross 
// 09 01 2023 C


// FYU

// Hello, welcome to the math 
// If you're coming from Unity, you will have a problem assigning references to nodes in the code
// If you need to set a reference at runtime, simply leave the global variable empty and assign it in the 'Ready()' function

// Happy hardcoding o7





using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Godot;
using static AstroProp_Runtime;


public partial class AstroProp_Runtime : Node3D
{
    // Let's get the signals over with
    [Signal]
    public delegate void FrameUpdateEventHandler();

    public void SY4(ref Godot.Vector3 PosCartesian, ref Godot.Vector3 VelCartesian, Godot.Vector3 InstantaneousAccel, double MET, ref CelestialRender MainSatelliteProx)
    {
        double curt2 = 1.25992104989;
        double w0 = -(curt2 / (2 - curt2));
        double w1 = (1 / (2 - curt2));

        double c1 = w1 / 2;
        double c4 = w1 / 2;
        double c2 = (w0 + w1) / 2;
        double c3 = (w0 + w1) / 2;

        double d1 = w1;
        double d3 = w1;
        double d2 = w0;

       
        //CelestialRender MainSatelliteProx = KeplerContainers[1];

        float MainSOIAccel = 0;

        // PosCartesian = PosCartesian * (float)ScaleConversion("ToRealUnits"); do this when calling le function

        // GD.Print(PosCartesian);
        Godot.Vector3 x1 = PosCartesian + VelCartesian * (float)(c1 * Reference.Dynamics.TimeStep);
        Godot.Vector3 a1 = new Godot.Vector3();
        GravityMain_SOI(x1, MET, ref a1);

        foreach (var CelestialRender in KeplerContainers)
        {
            Godot.Vector3 TempAccel = new Godot.Vector3();
            GravityGradient(CelestialRender, x1, MET, ref TempAccel);
            //GD.Print(TempAccel);
            a1 += TempAccel;

            if (TempAccel.Length() > MainSOIAccel)
            {
                MainSOIAccel = TempAccel.Length();
                MainSatelliteProx = CelestialRender;//reassign the reference to a beefier SOI
            }
            //Debug.Log(CelestialRender.Name.ToString());
        }
        // GD.Print(a1);
        if (MainSOIAccel*2 < a1.Length())
        {
            //MainSOIProx = true;
            //MainSatelliteProx
            MainSatelliteProx = null;
            //GD.Print(MainSOIAccel + " " + a1.Length());
        }
        else
        {
            //GD.Print(MainSatelliteProx.Name);
        }
        a1 += InstantaneousAccel;

        Godot.Vector3 v1 = VelCartesian + (float)(d1) * a1 * (float)Reference.Dynamics.TimeStep;
        Godot.Vector3 x2 = x1 + (float)c2 * v1 * (float)Reference.Dynamics.TimeStep;
        
        Godot.Vector3 a2 = new Godot.Vector3();
        GravityMain_SOI(x2, MET, ref a2);
        foreach (var CelestialRender in KeplerContainers)
        {
            Godot.Vector3 TempAccel = new Godot.Vector3();
            GravityGradient(CelestialRender, x2, MET, ref TempAccel);
            a2 += TempAccel;
            //Debug.Log(CelestialRender.Name.ToString());
        }
        a2 += InstantaneousAccel;

        Godot.Vector3 v2 = v1 + (float)(d2) * a2 * (float)Reference.Dynamics.TimeStep;
        Godot.Vector3 x3 = x2 + (float)c3 * v2 * (float)Reference.Dynamics.TimeStep;

        Godot.Vector3 a3 = new Godot.Vector3();
        GravityMain_SOI(x3, MET, ref a3);
        foreach (var CelestialRender in KeplerContainers)
        {
            Godot.Vector3 TempAccel = new Godot.Vector3();
            GravityGradient(CelestialRender, x3, MET, ref TempAccel);
            a3 += TempAccel;
            //Debug.Log(CelestialRender.Name.ToString());
        }
        a3 += InstantaneousAccel;

        Godot.Vector3 v3 = v2 + (float)(d3) * a3 * (float)Reference.Dynamics.TimeStep;
        Godot.Vector3 x4 = x3 + (float)c4 * v3 * (float)Reference.Dynamics.TimeStep;

        Godot.Vector3 v4 = v3;
        //GD.Print(v4 );//* (float)ScaleConversion("ToUnityUnits")
        PosCartesian = x4;
        VelCartesian = v4;
    }
    public void SY4_Host(ref Godot.Vector3 PosCartesian, ref Godot.Vector3 VelCartesian, double MET)
    {
        double curt2 = 1.25992104989;
        double w0 = -(curt2 / (2 - curt2));
        double w1 = (1 / (2 - curt2));

        double c1 = w1 / 2;
        double c4 = w1 / 2;
        double c2 = (w0 + w1) / 2;
        double c3 = (w0 + w1) / 2;

        double d1 = w1;
        double d3 = w1;
        double d2 = w0;

        // PosCartesian = PosCartesian * (float)ScaleConversion("ToRealUnits"); do this when calling le function

        // GD.Print(PosCartesian);
        Godot.Vector3 x1 = PosCartesian + VelCartesian * (float)(c1 * Reference.Dynamics.TimeStep);
        Godot.Vector3 a1 = new Godot.Vector3();
        GravityMain_SOI(x1, MET, ref a1);
        

        Godot.Vector3 v1 = VelCartesian + (float)(d1) * a1 * (float)Reference.Dynamics.TimeStep;
        Godot.Vector3 x2 = x1 + (float)c2 * v1 * (float)Reference.Dynamics.TimeStep;

        Godot.Vector3 a2 = new Godot.Vector3();
        GravityMain_SOI(x2, MET, ref a2);
        

        Godot.Vector3 v2 = v1 + (float)(d2) * a2 * (float)Reference.Dynamics.TimeStep;
        Godot.Vector3 x3 = x2 + (float)c3 * v2 * (float)Reference.Dynamics.TimeStep;

        Godot.Vector3 a3 = new Godot.Vector3();
        GravityMain_SOI(x3, MET, ref a3);
        

        Godot.Vector3 v3 = v2 + (float)(d3) * a3 * (float)Reference.Dynamics.TimeStep;
        Godot.Vector3 x4 = x3 + (float)c4 * v3 * (float)Reference.Dynamics.TimeStep;

        Godot.Vector3 v4 = v3;
        //GD.Print(v4 );//* (float)ScaleConversion("ToUnityUnits")
        PosCartesian = x4;
        VelCartesian = v4;
    }

    public class StateVectors // the properties of a vessel at a given timestep. Complex n-body, all bodies considered 
    {
        public Godot.Vector3 PosCartesian = new Godot.Vector3();
        public Godot.Vector3 VelCartesian = new Godot.Vector3();

        public Godot.Vector3 PrevPosLerp = new Godot.Vector3(); //as of now, an unassigned variable. I have future plans for this fella

        //noballs variable lmao:
        public Godot.Vector3 InstantaneousAccel = new Godot.Vector3(); // Instantaneous propulsion by engines to be considered by ship. 
    }
    public class DiscreteTimestep // the statevectors of a celestial body at a given timestep. 1-body computation only concerned with host body soi
    {
        public int MET = 0;

        public Godot.Vector3 PosCartesian = new Godot.Vector3();
        public Godot.Vector3 VelCartesian = new Godot.Vector3();

        public double AxialRotation = 0;

        public DiscreteTimestep Clone()
        {
            DiscreteTimestep DTClone = new DiscreteTimestep();

            DTClone.MET = this.MET;
            DTClone.PosCartesian = this.PosCartesian;
            DTClone.VelCartesian = this.VelCartesian;
            DTClone.AxialRotation = this.AxialRotation;

            return DTClone;
        }

    }
    public class SegmentStepFrame
    {
        public double MET = 0;

        public Node3D ObjectRef;

        public StateVectors StateVectors = new StateVectors();

        public double DeltaV;

        public string Event1; // misc register name
        public string Event2;
        public string Event3;

        public double Data1; // register data
        public double Data2;
        public double Data3;

       
        
        public SegmentStepFrame(
            double MET,
            Godot.Vector3 Prev_Pos,
            Godot.Vector3 Prev_Vel,
            Godot.Vector3 Des_Accel,

            bool PredictPrevPosLerp
           )
        {
            this.MET = MET;
            
            this.StateVectors.PosCartesian = Prev_Pos;
            this.StateVectors.VelCartesian = Prev_Vel;
            this.StateVectors.InstantaneousAccel = Des_Accel;

            
        }
    }
    
    public class ProjectOry
    {
        public string Name;
        public string Description;

       

        public int StartMET;
        public int InterruptMET;

        public Godot.ImmediateMesh TrackStripMesh;

        public Node3D ParentRef;// = GetNode<Node3D>("Global"); // default is just the global scene (main soi relative)
        public MeshInstance3D ObjectRef; //this is the node3d that the primitive line strip is parented to

        public StateVectors StateVectors = new StateVectors(); // state at the start of the trajectory
      


        public Dictionary<int, SegmentStepFrame> Trajectory;
        public ProjectOry( // rendered trajectory
           Node3D ParentRef,
           string Name,
           string Description,
           Godot.Vector3 PosCartesian,
           Godot.Vector3 VelCartesian,
           Godot.Vector3 InstantaneousAccel,
           int StartMET,
           int InterruptMET
            
           )
        {
            this.ParentRef = ParentRef;
            this.Name = Name;
            this.Description = Description;
            this.StateVectors.PosCartesian = PosCartesian;
            this.StateVectors.VelCartesian = VelCartesian;
            this.StateVectors.InstantaneousAccel = InstantaneousAccel;

            this.StartMET = StartMET;
            this.InterruptMET = InterruptMET;

        }

       

        // create another list of the projected segments, then tell them to remove themselves as their MET is surpassed
    }
    public void SetUpProjectOry(
           ref ProjectOry ProjectOry,
           Node3D ParentRef
          


    )
    {
        GD.Print(
            ProjectOry,
            ParentRef
            );
        Godot.OrmMaterial3D LineMat = new Godot.OrmMaterial3D();
        LineMat.ShadingMode = BaseMaterial3D.ShadingModeEnum.PerPixel; 
        
        //LineMat.ShadingMode = BaseMaterial3D.ShadingModeEnum.Unshaded;
        //LineMat.DistanceFadeMaxDistance = 100;
        //LineMat.DistanceFadeMinDistance = -80;
        //LineMat.DistanceFadeMode = BaseMaterial3D.DistanceFadeModeEnum.PixelAlpha;

        LineMat.EmissionEnabled = true;
        LineMat.DisableAmbientLight = true;
        LineMat.DisableReceiveShadows = true;
        LineMat.Emission = Color.FromHtml("#FF14AF");
        LineMat.EmissionIntensity = 10;
        LineMat.Roughness = 1;
        LineMat.MetallicSpecular = 0;
        LineMat.AlbedoColor = Color.FromHtml("#FF14AF");

        ProjectOry.ParentRef = ParentRef;
        ProjectOry.Name = Name;
        ProjectOry.ObjectRef = new Godot.MeshInstance3D(); // the track itself
        ProjectOry.TrackStripMesh = new Godot.ImmediateMesh();
        ProjectOry.TrackStripMesh.SurfaceBegin(Godot.Mesh.PrimitiveType.LineStrip, LineMat);
        ProjectOry.ObjectRef.Mesh = ProjectOry.TrackStripMesh;
        Godot.Vector3 LS_P = ProjectOry.StateVectors.PosCartesian;
        Godot.Vector3 LS_V = ProjectOry.StateVectors.VelCartesian;
        ProjectOry.TrackStripMesh.SurfaceAddVertex(LS_P * (float)ScaleConversion("ToUnityUnits"));
        
        
        ProjectOry.Trajectory = new Dictionary<int, SegmentStepFrame>(10000000);
        Godot.Vector3 LastVertexPosGlobalSpace = LS_P * (float)ScaleConversion("ToUnityUnits");




        CelestialRender MainSOISatellite = null;
        CelestialRender MainSOIUpdate = MainSOISatellite;

        Godot.MeshInstance3D LocalLineMesh = null;
        Godot.ImmediateMesh LocalImmediateMesh = null;

        Godot.Vector3 LastLocalDistance = new Godot.Vector3(0,0,0); //
        float LastLocalDistance_Rate = 0; //VERY simple periapse/apoapse/closest approach method

        Godot.Vector3 LastGlobalDistance = new Godot.Vector3(0, 0, 0); //
        float LastGlobalDistance_Rate = 0; //VERY simple periapse/apoapse/closest approach method
        int LGD_SE = -1; // -1 is the unclaimed position, 0 is last event = apo, 1 is last event = peri. Redundancy.

        Godot.Node3D LastApoGlobal = null;
        float LastApoGlobal_Distance = 0;

        Godot.Node3D LastPeriGlobal = null;
        float LastPeriGlobal_Distance = 999999999999; //A very large number, so the initial state of the perigee can begin


        for (int i = ProjectOry.StartMET; i < ProjectOry.InterruptMET; i++)
        {
            //if (i == ProjectOry.InterruptMET-1)
            //{
              //  GD.Print("We're here: " + i);
                //GD.Print(LS_P);
            //}
            
            SY4(ref LS_P, ref LS_V, ProjectOry.StateVectors.InstantaneousAccel, i, ref MainSOISatellite);
            SegmentStepFrame Iter_Frame = new SegmentStepFrame(i, LS_P, LS_V, ProjectOry.StateVectors.InstantaneousAccel, false);
            Godot.Vector3 NewGlobalSpace = LS_P * (float)ScaleConversion("ToUnityUnits");
            
            ProjectOry.Trajectory.Add(((int)Iter_Frame.MET), Iter_Frame); 

            if (!(MainSOISatellite == null) & (LocalLineMesh == null))
            {
                NLM_ReturnPacket NewPacket = NewLineMesh(Color.FromHtml("#66ff66"));
                LocalLineMesh = NewPacket.LineObj;
                LocalImmediateMesh = NewPacket.TrackStripMesh;
                LocalImmediateMesh.SurfaceAddVertex((LS_P - FindStateSOI(MainSOISatellite,(int)Iter_Frame.MET).PosCartesian) * (float)ScaleConversion("ToUnityUnits"));
                GD.Print("NewLine!! local to moon");
                LastLocalDistance = (LS_P - FindStateSOI(MainSOISatellite, (int)Iter_Frame.MET).PosCartesian);
            }
            
            if ((MainSOISatellite == null) & !(LocalLineMesh == null))
            {
                //ProjectOry.ObjectRef.AddChild(LocalLineMesh);
                LocalImmediateMesh.SurfaceAddVertex((LS_P - FindStateSOI(MainSOIUpdate, (int)Iter_Frame.MET).PosCartesian) * (float)ScaleConversion("ToUnityUnits"));
                LocalImmediateMesh.SurfaceEnd();
                //LocalLineMesh.TopLevel = true;

                //Godot.RemoteTransform3D RemoteRef = new Godot.RemoteTransform3D();
                MainSOIUpdate.ObjectRef.AddChild(LocalLineMesh);

                //RemoteRef.RemotePath = RemoteRef.GetPathTo(LocalLineMesh);
                //RemoteRef.UseGlobalCoordinates = true;
               // RemoteRef.UpdatePosition = true;
                //MainSOIUpdate.ObjectRef

                //GD.Print("Ended " + MainSOIUpdate.Name);
                //MainSOISatellite.ObjectRef.
                //LocalLineMesh

                LocalLineMesh = null; //set the null reference, freeing the temporary local track mesh, while also closing it and setting its parent
                LocalImmediateMesh = null;
            }
            if ((NewGlobalSpace -LastVertexPosGlobalSpace).Length() > .05)
            {
                LastVertexPosGlobalSpace = NewGlobalSpace;
                ProjectOry.TrackStripMesh.SurfaceAddVertex(NewGlobalSpace);
                if (!(MainSOISatellite == null) & !(LocalLineMesh == null))
                {
                    LocalImmediateMesh.SurfaceAddVertex((LS_P - FindStateSOI(MainSOISatellite, (int)Iter_Frame.MET).PosCartesian) * (float)ScaleConversion("ToUnityUnits"));
                }
            }
            if (!(MainSOISatellite == null) & !(LocalLineMesh == null))
            {
                float LastRateGlobal = LastLocalDistance_Rate;
                Godot.Vector3 NewLocalDistance = (LS_P - FindStateSOI(MainSOISatellite, (int)Iter_Frame.MET).PosCartesian);
                LastLocalDistance_Rate = -(LastLocalDistance - NewLocalDistance).Length();
                if (NewLocalDistance.Length() < LastLocalDistance.Length())
                {
                    LastLocalDistance_Rate = LastLocalDistance_Rate * (-1);
                }
                LastLocalDistance = NewLocalDistance;
                if ((LastRateGlobal * LastLocalDistance_Rate) < 0 ) // look for a negative last rate (since the closing rates pass through zero, multiplying them will always yield a negative at an event)
                {
                    if (LastLocalDistance_Rate > 0)
                    {
                        Godot.Node3D Apo = NewSpatialEvent(
                            "FURTHEST [APO]",
                            (int)Iter_Frame.MET,
                            "RDIST "+ (Math.Round(LastLocalDistance.Length()/100)) /10 + " [km]" + //tolerance of one decimal ((int)dist/100)/10
                            "\r\n" +
                            "RVEL "+ Math.Abs((int)LastLocalDistance_Rate) + " [m/s]", 
                            Color.FromHtml("#66ff66"));

                        
                        SpatialEventData SED = new SpatialEventData();
                        SED.Address = Apo;
                        SED.MET = (int)Iter_Frame.MET;

                        SpatialEventManager.Add(SED);

                        LocalLineMesh.AddChild(Apo);
                        SpatialUpdateEnumerable(Apo, (int)Iter_Frame.MET);
                        Apo.Position = (LS_P - FindStateSOI(MainSOISatellite, (int)Iter_Frame.MET).PosCartesian) * (float)ScaleConversion("ToUnityUnits");
                        //GD.Print("Apo " + ((int)LastLocalDistance.Length() / 100) / 10 + " [km]");
                    }
                    else
                    {
                        Godot.Node3D CA = NewSpatialEvent(
                           "CLOSEST [PERI]",
                           (int)Iter_Frame.MET,
                           "RDIST " + (Math.Round(LastLocalDistance.Length() / 100)) / 10 + " [km]" + //tolerance of one decimal ((int)dist/100)/10
                           "\r\n" +
                           "RVEL " + Math.Abs((int)LastLocalDistance_Rate) + " [m/s]",
                           Color.FromHtml("#66ff66"));
                       
                        SpatialEventData SED = new SpatialEventData();
                        SED.Address = CA;
                        SED.MET = (int)Iter_Frame.MET;

                        SpatialEventManager.Add(SED);

                        LocalLineMesh.AddChild(CA);
                        SpatialUpdateEnumerable(CA, (int)Iter_Frame.MET);
                        CA.Position = (LS_P - FindStateSOI(MainSOISatellite, (int)Iter_Frame.MET).PosCartesian) * (float)ScaleConversion("ToUnityUnits");
                        //GD.Print("CA " + ((int)LastLocalDistance.Length() / 100) / 10 + " [km]");
                    }
                }
            }
            float LastRate = LastGlobalDistance_Rate;
            Godot.Vector3 NewGlobalDistance = (LS_P);
            LastGlobalDistance_Rate = -(LastGlobalDistance - NewGlobalDistance).Length();
            if (NewGlobalDistance.Length() < LastGlobalDistance.Length())
            {
                LastGlobalDistance_Rate = LastGlobalDistance_Rate * (-1);
            }
            LastGlobalDistance = NewGlobalDistance;
            if ((LastRate * LastGlobalDistance_Rate) < 0) // look for a negative last rate (since the closing rates pass through zero, multiplying them will always yield a negative at an event)
            {
                if ((LastGlobalDistance_Rate > 0)  & (LastApoGlobal_Distance < LastGlobalDistance.Length())) //& !(LGD_SE == 0)
                {
                    LGD_SE = 0;
                    Godot.Node3D Apo = NewSpatialEvent(
                        "FURTHEST [APO]",
                        (int)Iter_Frame.MET,
                        "GDIST " + (Math.Round(LastGlobalDistance.Length() / 100)) / 10 + " [km]" + //tolerance of one decimal ((int)dist/100)/10
                        "\r\n" +
                        "GVEL " + Math.Abs((int)LastGlobalDistance_Rate) + " [m/s]",
                        Color.FromHtml("#66ff66"));
                    
                    SpatialEventData SED = new SpatialEventData();
                    SED.Address = Apo;
                    SED.MET = (int)Iter_Frame.MET;

                    SpatialEventManager.Add(SED);

                    ProjectOry.ObjectRef.AddChild(Apo);
                    SpatialUpdateEnumerable(Apo, (int)Iter_Frame.MET);
                    Apo.Position = (LS_P) * (float)ScaleConversion("ToUnityUnits");
                    if (LastApoGlobal != null)
                    {
                        ProjectOry.ObjectRef.RemoveChild(LastApoGlobal); // may need to sanity check here, just in case it cannot realize that null is not a node. food 4 thought. Yep, correct
                    }
                    LastApoGlobal = Apo;
                    LastApoGlobal_Distance = LastGlobalDistance.Length();
                    GD.Print("Apo " + ((int)LastGlobalDistance.Length() / 100) / 10 + " [km]");
                    GD.Print(LS_V+" [m/s}");
                }
                if ((LastGlobalDistance_Rate < 0) & (LastPeriGlobal_Distance > LastGlobalDistance.Length())) // & !(LGD_SE == 1) you may remove this last statement once you have double precision, some weird stuff.
                {
                    LGD_SE = 1;
                    Godot.Node3D CA = NewSpatialEvent(
                       "CLOSEST [PERI]",
                       (int)Iter_Frame.MET,
                       "GDIST " + (Math.Round(LastGlobalDistance.Length() / 100)) / 10 + " [km]" + //tolerance of one decimal ((int)dist/100)/10
                       "\r\n" +
                       "GVEL " + Math.Abs((int)LastGlobalDistance_Rate) + " [m/s]",
                       Color.FromHtml("#66ff66"));
                    
                    SpatialEventData SED = new SpatialEventData();
                    SED.Address = CA;
                    SED.MET = (int)Iter_Frame.MET;

                    SpatialEventManager.Add(SED);

                    ProjectOry.ObjectRef.AddChild(CA);
                    SpatialUpdateEnumerable(CA, (int)Iter_Frame.MET);
                    CA.Position = (LS_P * (float)ScaleConversion("ToUnityUnits"));
                    if (LastPeriGlobal != null)
                    {
                        ProjectOry.ObjectRef.RemoveChild(LastPeriGlobal); ; // may need to sanity check here, just in case it cannot realize that null is not a node. food 4 thought. Yep, correct
                    }
                   
                    LastPeriGlobal = CA;
                    LastPeriGlobal_Distance = LastGlobalDistance.Length();
                    GD.Print("CA " + ((int)LastGlobalDistance.Length() / 100) / 10 + " [km]");

                }
            }
            //LastGlobalDistance = LS_P * (float)ScaleConversion("ToUnityUnits");
            MainSOIUpdate = MainSOISatellite;
            //ProjectOry.TrackStripMesh.SurfaceAddVertex(LS_P*(float)ScaleConversion("ToUnityUnits"));
            // GD.Print(LS_P * (float)ScaleConversion("ToUnityUnits"));
        };
        GD.Print("Projected");
        //GD.Print(LS_P * (float)ScaleConversion("ToUnityUnits"));
        // GD.Print("Done rendering");
        //GD.Print(ProjectOry.Trajectory);
        //GD.Print(ProjectOry.TrackStripMesh.SurfaceGetArrays();
        ProjectOry.TrackStripMesh.SurfaceEnd();
        ProjectOry.ObjectRef.TopLevel = true; // disable if relative to another body besides the main soi
        ParentRef.AddChild(ProjectOry.ObjectRef);
        ProjectOry.ObjectRef.Position = new Vector3();
        ProjectOry.ObjectRef.CastShadow = 0;
        // this.NBodyRef = Instantiate(NBodyRef, new Godot.Vector3(0, 0, 0), Godot.Quaternion.identity);
    }
    public void RunBallisticTrack(

           NBodyAffected Vessel
    )
    {
        double OrbitalPeriod = CalculateOrbital(
            new Godot.Vector3(),
            Vessel.StateVectors.PosCartesian,
            Vessel.StateVectors.VelCartesian,
            new Godot.Vector3(),
            Reference.SOI.MainReference.GravitationalParameter
            );
        GD.Print((OrbitalPeriod));
        Vessel.Trajectory = new ProjectOry(
            Vessel.ObjectRef,
            "Ballistic Trajectory",
            "",
            Vessel.StateVectors.PosCartesian,
            Vessel.StateVectors.VelCartesian,
            new Godot.Vector3(),
            (int)Reference.Dynamics.MET,
            60*60*24*20
            //(int)(OrbitalPeriod * 1.5)
            );
        SetUpProjectOry(ref Vessel.Trajectory, Vessel.ObjectRef);
    }
    public class NBodyAffected // ballistic and nonballistic object
    {
        public string Name;
        public string Description;

        public Node3D ObjectRef;

        //public int FirstMET_Timestamp = 0; // can be anything, since vessels can be instantialized at any given point -- nvm, kind of redundant
        public StateVectors StateVectors = new StateVectors();

        public ProjectOry Trajectory;

        public NBodyAffected(
            Node3D ObjectRef,
            string Name,
            string Description,
            Godot.Vector3 PosCartesian,
            Godot.Vector3 VelCartesian,
            Godot.Vector3 InstantaneousAccel


            )
        {
            this.ObjectRef = ObjectRef;
            this.Name = Name;
            this.Description = Description;
            this.StateVectors.PosCartesian = PosCartesian;
            this.StateVectors.VelCartesian = VelCartesian;
           // this.StateVectors.InstantaneousAccel = InstantaneousAccel;


            // this.NBodyRef = Instantiate(NBodyRef, new Godot.Vector3(0, 0, 0), Godot.Quaternion.identity);

        }
    }
    public class NLM_ReturnPacket
    {
        public Godot.MeshInstance3D LineObj; // the track itself
        public Godot.ImmediateMesh TrackStripMesh;

        public NLM_ReturnPacket(
            Godot.MeshInstance3D LineObj,
            Godot.ImmediateMesh TrackStripMesh
        )
        {
            this.LineObj = LineObj;
            this.TrackStripMesh = TrackStripMesh;
        }
    }

    public class NSE_ReturnPacket
    {
       
        public NSE_ReturnPacket(
            
        )
        {
           
        }
    }
    public class CelestialRender
    {
        public string Name;
        public string Description;

        public Node3D ObjectRef;//   = GetNode<Node>("Global/Earth");
        public MeshInstance3D Surface;

        public MeshInstance3D TrackRef;

        //public int FirstMET_Timestamp = 0; // since celestials are set up immediately on run -- redundant lmfao
        public DiscreteTimestep EphemerisMET_Last = new DiscreteTimestep();
        public Dictionary<int, DiscreteTimestep> Ephemeris = new Dictionary<int, DiscreteTimestep>(10000000); // the integer key is going to be the MET of the timestep, just to make shit easier.



        // default is moon

        public double Mass = 7.35 * Mathf.Pow(10, 22); //kg
        public double GravitationalParameter = 4.904 * Mathf.Pow(10, 12); // m^3 s^-2
        public double SOI = 64300;


        public double SMA = 384400 * 1000; //meters lmao
        public double Inclination = 5.145 * AstroProp_Runtime.Reference.Dynamics.DegToRads; //degrees to the ecliptic, in rads
        public double Eccentricity = .0549; // just eccentricity
        public double ArgPeri = 0 * AstroProp_Runtime.Reference.Dynamics.DegToRads;
        public double LongAscen = 0 * AstroProp_Runtime.Reference.Dynamics.DegToRads;
        public double MeanAnom = 0;

        public bool TidalLocked = false;
        public void ObjectSurfaceAssign(ref Node3D ObjectRef, ref  string ObjectRefString)
        {

        }

        public CelestialRender(
            string Name,
            string Description,
            Node3D ObjectRef,
            MeshInstance3D SurfaceRef,
            double Mass,
            double GravitationalParameter,
            double SOI,
            double SMA,
            double Inclination,
            double Eccentricity,
            double ArgPeri,
            double LongAscen,
            double MeanAnom,
            bool TidalLocked
            )
        {

            this.Name = Name;
            this.Description = Description;
            this.ObjectRef = ObjectRef;
            this.Surface = SurfaceRef;
            this.Mass = Mass;
            this.GravitationalParameter = GravitationalParameter;
            this.SOI = SOI;
            this.SMA = SMA;
            this.Inclination = -Inclination;
            this.Eccentricity = Eccentricity;
            this.ArgPeri = ArgPeri;
            this.LongAscen = LongAscen;
            this.MeanAnom = MeanAnom;
            this.TidalLocked = TidalLocked;
            
        }
    }

    public void ProjectCelestial(ref CelestialRender SOI) // run this only once
    {
        DiscreteTimestep StartFrame = new DiscreteTimestep();
        //GD.Print(StartFrame.MET);
        ModStateVector_Kep(SOI, StartFrame.MET, ref StartFrame.PosCartesian, ref StartFrame.VelCartesian);
        SOI.Ephemeris.Add((StartFrame.MET),StartFrame);
        int Period = (int)CalculateOrbital(new Godot.Vector3(), StartFrame.PosCartesian, new Godot.Vector3(), StartFrame.VelCartesian, Reference.SOI.MainReference.GravitationalParameter);
        int MaxFrames = (int)(Period / Reference.Dynamics.TimeStep);
        DiscreteTimestep LastFrame = StartFrame.Clone();

        Godot.OrmMaterial3D LineMat = new Godot.OrmMaterial3D();
        LineMat.ShadingMode = BaseMaterial3D.ShadingModeEnum.PerPixel; //LineMat.ShadingMode = BaseMaterial3D.ShadingModeEnum.Unshaded;
        LineMat.EmissionEnabled = true;
        LineMat.DisableAmbientLight = true;
        LineMat.DisableReceiveShadows = true;
        LineMat.Emission = Color.FromHtml("#00FFFF");
        LineMat.EmissionIntensity = 10;
        LineMat.AlbedoColor = Color.FromHtml("#00FFFF");
        LineMat.DistanceFadeMaxDistance = 50;
        LineMat.DistanceFadeMinDistance = -10;
        LineMat.DistanceFadeMode = BaseMaterial3D.DistanceFadeModeEnum.PixelAlpha;
        SOI.TrackRef = new Godot.MeshInstance3D(); // the track itself
        MeshInstance3D TrackRef = SOI.TrackRef;
        ImmediateMesh TrackStripMesh = new Godot.ImmediateMesh();
        TrackStripMesh.SurfaceBegin(Godot.Mesh.PrimitiveType.LineStrip, LineMat);
        TrackRef.Mesh = TrackStripMesh;

        Godot.Vector3 LastVertexPosGlobalSpace = StartFrame.PosCartesian * (float)ScaleConversion("ToUnityUnits");
        TrackStripMesh.SurfaceAddVertex(StartFrame.PosCartesian * (float)ScaleConversion("ToUnityUnits"));

        for (int i = 1; i <= Reference.Dynamics.EphemerisCeiling; i++)
        {
            DiscreteTimestep NewFrame = LastFrame.Clone(); //you are SHALLOW-COPYING here, need to DEEP-COPY -- NVM, SOLVED

             NewFrame.MET += (int)Reference.Dynamics.TimeStep;
            //GD.Print(LastFrame.MET + "  " + NewFrame.MET);
            
            SY4_Host(ref NewFrame.PosCartesian, ref NewFrame.VelCartesian, NewFrame.MET);
            SOI.Ephemeris.Add((NewFrame.MET), NewFrame);
            LastFrame = NewFrame.Clone();
            SOI.EphemerisMET_Last = LastFrame.Clone();

            Godot.Vector3 NewGlobalSpace = NewFrame.PosCartesian * (float)ScaleConversion("ToUnityUnits");
            
            if (System.Math.Abs((NewGlobalSpace - LastVertexPosGlobalSpace).Length()) > .15)
            {
                LastVertexPosGlobalSpace = NewGlobalSpace;
                TrackStripMesh.SurfaceAddVertex(NewGlobalSpace);
                
            }

           

        }

     

        
        

            
        GD.Print("Projected");
        //GD.Print(LS_P * (float)ScaleConversion("ToUnityUnits"));
        // GD.Print("Done rendering");
        //GD.Print(ProjectOry.Trajectory);
        //GD.Print(ProjectOry.TrackStripMesh.SurfaceGetArrays();
        TrackStripMesh.SurfaceEnd();
        TrackRef.TopLevel = true; // disable if relative to another body besides the main soi
        SOI.ObjectRef.AddChild(TrackRef);
        TrackRef.Position = new Vector3();
        TrackRef.CastShadow = 0;
        // this.NBodyRef = Instantiate(NBodyRef, new Godot.Vector3(0, 0, 0), Godot.Quaternion.identity);
        // GD.Print(SOI.Ephemeris.ElementAt(1).Value.PosCartesian); // wtf??
        //DiscreteTimestep Banzai = SOI.Ephemeris.ElementAt(1); // wtf??
        //GD.Print(SOI.Ephemeris.ElementAt); // wtf??
        //GD.Print(SOI.Ephemeris[1].PosCartesian);
        // GD.Print(SOI.Ephemeris[2].PosCartesian);
        // GD.Print(SOI.Ephemeris[30].PosCartesian); // wot?

        //StartFrame.PosCartesian;

        //Emphemeris.Add(StartFrame);
    }

    public static DiscreteTimestep FindStateSOI(CelestialRender SOI, int MET) //we're using dictionary indexing here, hella faster than list lookup (its not iterative like lists)
    {
        //GD.Print(MET);
       // GD.Print(SOI, SOI.Ephemeris[MET], MET);
        DiscreteTimestep FoundStep = SOI.Ephemeris[MET].Clone(); 
        
        //ElementAt(index) has to loop through every value before it in order to look up an index??? WTF
        //EZ fix, just index using dict[index]. Holy based
        
        //as the MET increases, so does the lag. with an index of 1, it is very performant??? after frame 30000, it may have to rehash the table?
        // DiscreteTimestep FoundStep = SOI.Ephemeris.ElementAt((int)(MET/Reference.Dynamics.TimeStep)).Value.Clone();
        //GD.Print(MET, (int)(MET / Reference.Dynamics.TimeStep));
        //GD.Print(MET + " " + FoundStep.MET + " Position: " + FoundStep.PosCartesian);
        //GD.Print(MET + " " + FoundStep.MET); //not good
        // GD.Print(FoundStep.MET);
        return FoundStep;

        // y did I make this a func????
    }
     public void METtoString(ref string MET)
    {
        float MET_float = MET.ToFloat();
        int Days = (int)System.Math.Floor((MET_float) / (60 * 60 * 24));
        int Hours = (int)System.Math.Floor((MET_float - Days * (60 * 60 * 24)) / (60 * 60));
        int Minutes = (int)System.Math.Floor((MET_float - Days * (60 * 60 * 24) - Hours * (60 * 60)) / (60));
        int Seconds = (int)System.Math.Floor((MET_float - Days * (60 * 60 * 24) - Hours * (60 * 60) - Minutes * 60));

        string D0 = "";
        string H0 = "";
        string M0 = "";
        string S0 = "";

        if (Days < 10)
        {
            D0 = "0";
        }
        if (Hours < 10)
        {
            H0 = "0";
        }
        if (Minutes < 10)
        {
            M0 = "0";
        }
        if (Seconds < 10)
        {
            S0 = "0";
        }


        // Time.Text = S0 + Seconds.ToString() + " S :" + M0 + Minutes.ToString() +  " M :" + H0 + Hours.ToString()+ " H :" + D0 + Days.ToString() + " D "; // code here for secs to mins and hours and days
        MET = D0 + Days.ToString() + ":" + H0 + Hours.ToString() + ":" + M0 + Minutes.ToString() + ":" + S0 + Seconds.ToString(); // + " [DD:HH:MM:SS]"; // code here for secs to mins and hours and days
    }
     public void UpdateTemporal()
    {
        Godot.Control Control = GetNode<Control>("Control");
        Godot.MarginContainer Temporal = Control.GetNode<Godot.MarginContainer>("Temporal");
        Godot.VBoxContainer VBX = Temporal.GetNode<Godot.VBoxContainer>("VBX");
        Godot.Label Time = VBX.GetNode<Godot.Label>("Time");
        Godot.Label TC = VBX.GetNode<Godot.Label>("TC");




        // Time.Text = S0 + Seconds.ToString() + " S :" + M0 + Minutes.ToString() +  " M :" + H0 + Hours.ToString()+ " H :" + D0 + Days.ToString() + " D "; // code here for secs to mins and hours and days
        string MTS_PlaceHolder = Reference.Dynamics.MET.ToString();
        METtoString(ref MTS_PlaceHolder);
        Time.Text =  MTS_PlaceHolder  + " [DD:HH:MM:SS]"; // code here for secs to mins and hours and days

        TC.Text = Reference.Dynamics.TimeCompression + "X " + "[Simulated/Real]";
        // 00:00:00:00

        EmitSignal(SignalName.FrameUpdate);
        SpatialEventUpdate();
    }
    public class Reference
    {
        
        public class Dynamics
        {
            public static double StandardGravParam = (6.67 * Mathf.Pow(10, -11)); // Unmodifiable

            public static int TimeStep = 1 / 1; // seconds per timestep
            public static double MET = 0; //mean elapsed time

            public static double RandomAssConstant = 8.564471763787176;//9, 8.4 for some odd reason
            public static double TimeCompression = (10000); //100000; // default is 1, 2360448 is 1 lunar month per second

            public static double DegToRads = Math.PI / 180;

            public static int EphemerisCeiling = (60 * 60 * 24 * 27)/TimeStep;
        };

        public class SOI
        {
            // first class is always the origin soi
            public class MainReference
            {
                public Node3D ObjectRef; //= GetNode<Mesh>("Global/Earth");
                    //GetNode<Mesh>("Global/Earth");

                public static double Mass = 5.97 * Mathf.Pow(10, 24); //kg
                public static double GravitationalParameter = 3.98 * Mathf.Pow(10, 14); // m^3 s^-2
                public static double Radius = 6378.1 * 1000; //Meters
            };





            //public object Moon = new CelestialRender();
        };

       
    };
    public class SpatialEventData
    {
        public Godot.Node3D Address;
        public float MET;
    }

    List<CelestialRender> KeplerContainers = new List<CelestialRender>(100); //List<CelestialRender> KeplerContainers = new List<CelestialRender>(1);
    List<NBodyAffected> NByContainers = new List<NBodyAffected>(30);
    public List<SpatialEventData> SpatialEventManager = new List<SpatialEventData>(30);

    bool SEU = true; //debug
    public void SpatialEventUpdate()
    {
        if (SEU)
        {


            for (int i = 0; i < SpatialEventManager.Count; i++)
            {
                if ((SpatialEventManager[i] != null))//& IsInstanceIdValid(SpatialEventManager[i].Address.GetInstanceId()))// IsInsideTree

                {
                    //GD.Print((SpatialEventManager[i].Address.IsNodeReady()), SpatialEventManager[i].Address.IsInsideTree());
                    if (SpatialEventManager[i].Address.IsNodeReady() == false)//(SpatialEventManager[i].Address.IsQueuedForDeletion()) // garbage collections
                    {
                        SpatialEventManager[i].Address.QueueFree();
                        //GD.Print("Qeued One");
                        SpatialEventManager[i] = null;
                    }
                    else
                    {
                        SpatialEventData SED = SpatialEventManager[i];


                        Godot.Node3D ShowHide = SED.Address.GetNode<Godot.Node3D>("ShowHide");


                        Godot.Label3D TimeLabel = ShowHide.GetNode<Godot.Label3D>("Timestamp");
                        Godot.Label3D EventLabel = ShowHide.GetNode<Godot.Label3D>("EventLabel");

                        Godot.Label3D Details = ShowHide.GetNode<Godot.Label3D>("Details");



                        bool Pinned = (bool)ShowHide.GetMeta("Pinned");
                        float LocalTransparency = TimeLabel.Transparency;



                        float T_Till = (float)System.Math.Abs(SED.MET - Reference.Dynamics.MET);
                        string Sign = "-";
                        if ((SED.MET - Reference.Dynamics.MET) < 0)
                        {
                            Sign = "+";
                            TimeLabel.Modulate = Details.Modulate;
                            EventLabel.Modulate = Details.Modulate;
                        }

                        if ((Pinned == false) & (Sign != "-"))
                        {
                            LocalTransparency += (((float)0.9 - LocalTransparency) * (float).05);
                        }
                        else
                        {
                            float METgap = SpatialEventManager[i].MET - (float)Reference.Dynamics.MET;
                            if ((METgap < 60 * 60 * 24) & (METgap > 0))
                            {
                                float MGCoeff = METgap / (60 * 60 * 24);
                                //LocalTransparency += (((float)(MGCoeff*.2) - LocalTransparency) * (float).01);
                                LocalTransparency = (float)(System.Math.Sin(Time.GetUnixTimeFromSystem() * 10) * .5);
                            }
                            else
                            {
                                LocalTransparency += (((float)0.2 - LocalTransparency) * (float).01);
                            }

                        }
                        TimeLabel.Transparency = LocalTransparency;
                        EventLabel.Transparency = LocalTransparency;

                        Details.Transparency = (float)(.8 + LocalTransparency * .2);
                        string Elapsed = T_Till.ToString();
                        METtoString(ref Elapsed);
                        TimeLabel.Text = "T" + Sign + " " + Elapsed;
                    }

                }
            }
        }
    }

        //List<CelestialRender> KeplerContainers = new List<CelestialRender>();

        public void ReturnEccentricAnomaly(double M, double e, ref double E)
    {
        //M = E - e*MathF.Sin(E);

        double Tolerance = Mathf.Pow(10, -3);

        // float e = (float) e; //.ToString(); // gives 2.9257


        for (int i = 0; i <= 100; i = i + 2)
        {
            //if (Mathf.Floor(i/20) == i/20)
            double Residual = E - (e * System.Math.Sin(E)) - M;

            double Derivative = 1 - (e * System.Math.Sin(E));

            E = E - Residual / Derivative;
            if (System.Math.Abs(Residual) < Tolerance)
            {
                break;
            }
        }
    }

    public void ModStateVector_Kep(CelestialRender SOI, double MET_Frame, ref Godot.Vector3 PosCartesian, ref Godot.Vector3 VelCartesian)
    {
        double E = 0;

        double dtT = MET_Frame;

        double M = dtT*(System.Math.Sqrt(SOI.GravitationalParameter / System.Math.Pow(SOI.SMA, 3)));
        // mean anom
        ReturnEccentricAnomaly(M, SOI.Eccentricity, ref E);
        // Class Keplerian = SOI.GetType()

        double TrueAnom = 2 * System.Math.Atan2(System.Math.Sqrt(1 + SOI.Eccentricity) * System.Math.Sin(E / 2), System.Math.Sqrt(1 - SOI.Eccentricity) * System.Math.Cos(E / 2));

        double NewRadius = SOI.SMA * (1 - SOI.Eccentricity * Math.Cos(E));

        // SOI.Inclination -= 90 * Reference.Dynamics.DegToRads;

        // Debug.Log(TrueAnom.ToString());
        // Debug.Log(NewRadius.ToString());

        Godot.Vector3 O = new Godot.Vector3(
            (float)(Math.Cos(TrueAnom) * NewRadius),
            (float)(Math.Sin(TrueAnom) * NewRadius),
            0 // UpVector in perifocal, so no value
            );

        double O_Dot_OP = Math.Sqrt(SOI.GravitationalParameter * SOI.SMA) / NewRadius;

        Godot.Vector3 O_Dot = new Godot.Vector3(
            (float)(-Math.Sin(E) * O_Dot_OP),
            (float)(Math.Sqrt(1 - Math.Pow(((float)(SOI.Eccentricity)), ((float)(2)))) * Math.Cos(E) * O_Dot_OP),
            0 // UpVector in perifocal, so no value
            );
        // Debug.Log(O_Dot.ToString());
        PosCartesian = new Godot.Vector3(
           (float)(O.X * (Math.Cos(SOI.ArgPeri) * Math.Cos(SOI.LongAscen) - Math.Sin(SOI.ArgPeri) * Math.Cos(SOI.Inclination) * Math.Sin(SOI.LongAscen)))
           -
           (float)(O.Y * (Math.Sin(SOI.ArgPeri) * Math.Cos(SOI.LongAscen) + Math.Cos(SOI.ArgPeri) * Math.Cos(SOI.Inclination) * Math.Sin(SOI.LongAscen)))
           ,
           (float)(O.X * (Math.Cos(SOI.ArgPeri) * Math.Sin(SOI.LongAscen) + Math.Sin(SOI.ArgPeri) * Math.Cos(SOI.Inclination) * Math.Cos(SOI.LongAscen)))
           +
           (float)(O.Y * (Math.Cos(SOI.ArgPeri) * Math.Cos(SOI.Inclination) * Math.Cos(SOI.LongAscen) - Math.Sin(SOI.ArgPeri) * Math.Sin(SOI.LongAscen)))
            ,
           (float)(O.X * Math.Sin(SOI.ArgPeri) * Math.Sin(SOI.Inclination)) + (float)(O.Y * Math.Cos(SOI.ArgPeri) * Math.Sin(SOI.Inclination))
           );

        VelCartesian = new Godot.Vector3(
           (float)(O_Dot.X * (Math.Cos(SOI.ArgPeri) * Math.Cos(SOI.LongAscen) - Math.Sin(SOI.ArgPeri) * Math.Cos(SOI.Inclination) * Math.Sin(SOI.LongAscen)))
           -
           (float)(O_Dot.Y * (Math.Sin(SOI.ArgPeri) * Math.Cos(SOI.LongAscen) + Math.Cos(SOI.ArgPeri) * Math.Cos(SOI.Inclination) * Math.Sin(SOI.LongAscen)))
           ,
           (float)(O_Dot.X * (Math.Cos(SOI.ArgPeri) * Math.Sin(SOI.LongAscen) + Math.Sin(SOI.ArgPeri) * Math.Cos(SOI.Inclination) * Math.Cos(SOI.LongAscen)))
           +
           (float)(O_Dot.Y * (Math.Cos(SOI.ArgPeri) * Math.Cos(SOI.Inclination) * Math.Cos(SOI.LongAscen) - Math.Sin(SOI.ArgPeri) * Math.Sin(SOI.LongAscen)))
            ,
           (float)(O_Dot.X * Math.Sin(SOI.ArgPeri) * Math.Sin(SOI.Inclination)) + (float)(O_Dot.Y * Math.Cos(SOI.ArgPeri) * Math.Sin(SOI.Inclination))
           )*((float)Reference.Dynamics.RandomAssConstant);
        //GD.Print("Moon vel debug" + VelCartesian.Length());
        //GD.Print(VelCartesian.Length());

    }
    public void GravityMain_SOI(Godot.Vector3 PosCartesian, double MET_Frame, ref Godot.Vector3 Acceleration)
    {



        Godot.Vector3 SOI_PosCartesian = new Godot.Vector3(0, 0, 0);







        Godot.Vector3 FromProjToSOI = SOI_PosCartesian - PosCartesian;

        double DistanceExponent = Math.Pow(FromProjToSOI.Length(), 2);

        Godot.Vector3 GravityUnit = FromProjToSOI / FromProjToSOI.Length();

        double GravityAccelerant = (Reference.SOI.MainReference.GravitationalParameter / DistanceExponent);

        Acceleration = GravityUnit * (float)GravityAccelerant;

        // return FromProjToSOI;
    }
    public void GravityGradient(CelestialRender SOI, Godot.Vector3 PosCartesian, double MET_Frame, ref Godot.Vector3 Acceleration)
    {

        DiscreteTimestep LastState = FindStateSOI(SOI, (int)MET_Frame);
        //GD.Print(LastState.MET); // ur issue is that the staet lookup is unable to find a state with the current met (same state output)
        Godot.Vector3 SOI_PosCartesian = LastState.PosCartesian;
        Godot.Vector3 SOI_VelCartesian = LastState.VelCartesian;

       // ModStateVector_Kep(SOI, MET_Frame, ref SOI_PosCartesian, ref SOI_VelCartesian);





        Godot.Vector3 FromProjToSOI = (SOI_PosCartesian - PosCartesian);
        

        double DistanceExponent = Math.Pow(FromProjToSOI.Length() ,2);

        Godot.Vector3 GravityUnit = FromProjToSOI / FromProjToSOI.Length();

        double GravityAccelerant = (SOI.GravitationalParameter / DistanceExponent);

        Acceleration = GravityUnit * (float)GravityAccelerant;

        // return FromProjToSOI;
    }
    

    public static double ScaleConversion(string Units)
    {
        // Earth Radius is 6378.1 * 1000 meters
        // earth diameter in the astrogation viewport is 10 meters, so radius is 5
        double Coefficient = 1;
        double UnityDefaulUnit_To_RealMeters = (6378.1 * 1000) / 5;
        if (Units == "ToUnityUnits")
        {
            Coefficient = 1 / UnityDefaulUnit_To_RealMeters;
        }
        else
        {
            Coefficient = UnityDefaulUnit_To_RealMeters;
        }
        return Coefficient;
    }

    public static double CalculateOrbital(Godot.Vector3 SOI_Pos, Godot.Vector3 Subject_Pos, Godot.Vector3 SOI_Vel, Godot.Vector3 Subject_Vel, double SOI_Mu)
    {
        double Relative_Vel = (Subject_Vel - SOI_Vel).Length();
        double Distance = (Subject_Pos - SOI_Pos).Length();

        double SMA = -(SOI_Mu*Distance)/(Distance*Relative_Vel-2*SOI_Mu);
        double Period = 2 * System.Math.PI * System.Math.Sqrt(Math.Pow(SMA, 3)/SOI_Mu);
       // GD.Print(Period);
        return Period;
    }
   
    public static NLM_ReturnPacket NewLineMesh(Godot.Color Color)
    {
        Godot.OrmMaterial3D LineMat = new Godot.OrmMaterial3D();
        LineMat.ShadingMode = BaseMaterial3D.ShadingModeEnum.PerPixel;

        //LineMat.ShadingMode = BaseMaterial3D.ShadingModeEnum.Unshaded;
        //LineMat.DistanceFadeMaxDistance = 100;
        //LineMat.DistanceFadeMinDistance = -80;
        //LineMat.DistanceFadeMode = BaseMaterial3D.DistanceFadeModeEnum.PixelAlpha;

        LineMat.EmissionEnabled = true;
        LineMat.DisableAmbientLight = true;
        LineMat.DisableReceiveShadows = true;
        LineMat.Emission = Color;
        LineMat.EmissionIntensity = 10;
        LineMat.Roughness = 1;
        LineMat.MetallicSpecular = 0;
        LineMat.AlbedoColor = Color;


        Godot.MeshInstance3D LineObj = new Godot.MeshInstance3D(); // the track itself
        Godot.ImmediateMesh TrackStripMesh = new Godot.ImmediateMesh();
        TrackStripMesh.SurfaceBegin(Godot.Mesh.PrimitiveType.LineStrip, LineMat);
        LineObj.Mesh = TrackStripMesh;

        

        return new NLM_ReturnPacket(LineObj, TrackStripMesh);
    }
    public async void SpatialUpdateEnumerable(Godot.Node3D NodeRef, int MET)
    {
        /*
        if (true)
        {
            return; // safety is on rn
        }
        while (!NodeRef.IsNodeReady())
        {
            Task.Delay(500);
        }
            int LastMET = (int)Reference.Dynamics.MET;
        while (NodeRef.IsNodeReady())
        {
            Task.Delay(500);
            if (LastMET != (int)Reference.Dynamics.MET)
            {
                // SpatialEventData SED = SpatialEventManager[i];

                Godot.Node3D Address = NodeRef;

                Godot.Node3D ShowHide = Address.GetNode<Godot.Node3D>("ShowHide");


                Godot.Label3D TimeLabel = ShowHide.GetNode<Godot.Label3D>("Timestamp");
                Godot.Label3D EventLabel = ShowHide.GetNode<Godot.Label3D>("EventLabel");

                Godot.Label3D Details = ShowHide.GetNode<Godot.Label3D>("Details");



                bool Pinned = (bool)ShowHide.GetMeta("Pinned");
                float LocalTransparency = TimeLabel.Transparency;



                float T_Till = (float)System.Math.Abs(MET - Reference.Dynamics.MET);
                string Sign = "-";
                if ((MET - Reference.Dynamics.MET) < 0)
                {
                    Sign = "+";
                    TimeLabel.Modulate = Details.Modulate;
                    EventLabel.Modulate = Details.Modulate;
                }

                if ((Pinned == false) & (Sign != "-"))
                {
                    LocalTransparency += (((float)0.9 - LocalTransparency) * (float).05);
                }
                else
                {
                    float METgap = MET - (float)Reference.Dynamics.MET;
                    if ((METgap < 60 * 60 * 24) & (METgap > 0))
                    {
                        float MGCoeff = METgap / (60 * 60 * 24);
                        //LocalTransparency += (((float)(MGCoeff*.2) - LocalTransparency) * (float).01);
                        LocalTransparency = (float)(System.Math.Sin(Time.GetUnixTimeFromSystem() * 10) * .5);
                    }
                    else
                    {
                        LocalTransparency += (((float)0.2 - LocalTransparency) * (float).01);
                    }

                }
                TimeLabel.Transparency = LocalTransparency;
                EventLabel.Transparency = LocalTransparency;

                Details.Transparency = (float)(.8 + LocalTransparency * .2);
                string Elapsed = T_Till.ToString();
                METtoString(ref Elapsed);
                TimeLabel.Text = "T" + Sign + " " + Elapsed;
                //await (LastMET != (int)Reference.Dynamics.MET))
            }
        }

            NodeRef.QueueFree();
            //GD.Print("Qeued One");
        
           */
        
    }

    public static Godot.Node3D NewSpatialEvent(string EventName, int MET, string Description, Color Color)
    {
        //'Lite' NSE's are just going to hide everything but the spatial aid, so just make the description, time, and name invisible. Declutter.
        // We will make an event manager later, which manages all of the count-down's and event visbilities each second.
        //This will just be a list of all of the SE's, iterated through and managed



        //var packed_scene = load("res://something.tscn")
        //var scene_node = packed_scene.instance()
        // var root = get_tree().get_root()
        //root.add_child(scene_node)
        bool StillActive = true;

        Godot.PackedScene NSE = (PackedScene)ResourceLoader.Load("res://Prefabs/TrackSpatialEvent.tscn"); //you forgot the filetype at the end kek (tscn)
        Godot.Node3D SE = (Godot.Node3D)NSE.Instantiate();

        Godot.Node3D ShowHide = (Godot.Node3D)SE.GetNode("ShowHide");
        Godot.Label3D EventLabel = (Godot.Label3D)ShowHide.GetNode("EventLabel");
        Godot.Label3D Timestamp = (Godot.Label3D)ShowHide.GetNode("Timestamp");
        Godot.Label3D Details = (Godot.Label3D)ShowHide.GetNode("Details");
        Godot.Sprite3D SpatialAid = (Godot.Sprite3D)SE.GetNode("SpatialAid");

        EventLabel.Text = EventName;
        //Timestamp.Text = ""; // worry about this later, probably attach it to an event-manager. - DONE!
        Details.Text = Description;

        EventLabel.Modulate = Color;
        Timestamp.Modulate = Color;
        SpatialAid.Modulate = Color;

        ShowHide.Visible = true;

       

        return SE; //leave everything else (position, parent) to whatever called the method
        //Godot.PackedScene NSE = AstroProp_Runtime.GetNode<PackedScene>("res://Prefabs/NewSpatialEvent");
    }

    public void MoveNBy(NBodyAffected Object, double MET)
    {
        float LocalScale = (float)ScaleConversion("ToUnityUnits");

        Godot.Vector3 PosCartesian = Object.StateVectors.PosCartesian;
        Godot.Vector3 VelCartesian = Object.StateVectors.VelCartesian;
        //double MET_Frame = MET;
        Object.StateVectors.PrevPosLerp = PosCartesian;
        CelestialRender PlaceholderRef = null;
        SY4(ref PosCartesian, ref VelCartesian, Object.StateVectors.InstantaneousAccel, MET, ref PlaceholderRef);

       
        // LocalScale = (float)(LocalScale);

        
        // SOI.ObjectRef.Translate(PosCartesian);
        //Object.ObjectRef.Position = (new Godot.Vector3((float)(PosCartesian.X * LocalScale), (float)(PosCartesian.Y * LocalScale), (float)(PosCartesian.Z * LocalScale)));


        Object.StateVectors.PosCartesian = PosCartesian;
        Object.StateVectors.VelCartesian = VelCartesian;
        // GD.Print(PosCartesian);
        //.transform.position = PosCartesian;
    }
    public void GrowTrack(NBodyAffected Object, int MET)
    {
         // simple, straightforwards.
    }
    public void TrimTrack(NBodyAffected Object, int MET)
    {
        // you will need to entirely reassemble the mesh if you want to remove one vertice
    }
    void BeginStepOps()
    {
        


        // Begin to do the Celestials 
        //foreach (var CelestialRender in KeplerContainers)
        bool debugKep = false; //lag not even caused by memory leaks

        for (int i = 0; i < KeplerContainers.Count; i++) // do NOT EVER use foreach, it is read only.
        {
            //CelestialRender Overwrite = KeplerContainers[i];
            if (!debugKep)
            {
                DiscreteTimestep NewFrame = KeplerContainers[i].EphemerisMET_Last;
                //GD.Print(CelestialRender.EphemerisMET_Last.MET);
                NewFrame.MET += (int)Reference.Dynamics.TimeStep;
                SY4_Host(ref NewFrame.PosCartesian, ref NewFrame.VelCartesian, NewFrame.MET);
                KeplerContainers[i].Ephemeris[NewFrame.MET] = NewFrame; //LAG NOT EVEN CAUSED BY ADDING TO DICTIONARY?
                //KeplerContainers[i].Ephemeris.Add((NewFrame.MET),NewFrame);
                //GD.Print(NewFrame.MET, NewFrame.GetHashCode()); //MAKE A HASHCODE OVERRIDE
                KeplerContainers[i].EphemerisMET_Last = NewFrame;

            }
           

            DiscreteTimestep LastState = FindStateSOI(KeplerContainers[i], (int)Reference.Dynamics.MET); // somewhere in here, lag 
           //GD.Print()
            //GD.Print(LastState.PosCartesian);
            KeplerContainers[i].ObjectRef.Position = LastState.PosCartesian*(float)ScaleConversion("ToUnityUnits");
            if (KeplerContainers[i].TidalLocked == true)
            {
                KeplerContainers[i].Surface.LookAt(-KeplerContainers[i].ObjectRef.Position, new Godot.Vector3(0, 1, 0));
                
                //GetNode<Node3D>("Global/Moon")
            }
            //KeplerContainers[i].Ephemeris.Remove(KeplerContainers[i].Ephemeris.ElementAt((int)Reference.Dynamics.MET).Key);
            //KeplerContainers[i].EphemerisMET_Last = LastState;
            // code here for 1-body dynamics
            //CelestialRender.Ephemeris.LastIndexOf();
            //CelestialRender.Ephemeris

            //GD.Print(Reference.Dynamics.MET + " DictCount: " + KeplerContainers[i].Ephemeris.Count() + " DictCapacity: " + KeplerContainers[i].Ephemeris.EnsureCapacity(KeplerContainers[i].Ephemeris.Count()));

            //some kind of trim function too??
        }


        // Perform N-Body Ballistics

        foreach (var NBodyAffected in NByContainers)
        {
            MoveNBy(NBodyAffected, Reference.Dynamics.MET);
            //Debug.Log(CelestialRender.Name.ToString());
        }

    }

    // Start is called before the first frame update
    public override void _Ready()
    {
        //Reference.Dynamics.TimeCompression = 1;
        // GD.Print("Boostrapper Startup");
        // Line ends after this, wtf???
        //GD.Print(GetNode<Node3D>("Global/Moon").Position);
        //Task.Delay(10000);
        GetNode<Node3D>("Global/Moon").Position = (new Vector3(0, 0, 0));
       // GD.Print(GetNode<Node3D>("Global/Moon").Position);
        //=(new Godot.Vector3(10, 0, 0));
       // GD.Print("Can Translate");                                           //(new Godot.Vector3(10, 0, 0));
                                                                        // Reference.SOI.MainReference. = GetNode<Node>("Global/Earth");
                                                                        //Print("AstroPropV2");
                                                                        //MotionContainer.
                                                                        //KeplerContainers.Clear();
        KeplerContainers.Add(new CelestialRender(
            "Moon",
            "Our closest rock",
            GetNode<Node3D>("Global/Moon"),
            GetNode<MeshInstance3D>("Global/Moon/Surface"),
            7.35 * System.Math.Pow(10, 22),
            4.904 * System.Math.Pow(10, 12),
            64300,
            384400 * 1000,
            (5.145 + 90) * AstroProp_Runtime.Reference.Dynamics.DegToRads,
            .0549, //.0549
            0 * AstroProp_Runtime.Reference.Dynamics.DegToRads,
            0 * AstroProp_Runtime.Reference.Dynamics.DegToRads,
            0,
            true

        ));
        NByContainers.Add(new NBodyAffected(
            GetNode<Node3D>("Global/Sagitta"),
            "Sagitta",
            "a spaceship",
            new Godot.Vector3(6789000, 0, 0), //6789000 iss altitude meters
            new Godot.Vector3(0, 0, 7600 + 3000) * (float)1.012385, //-6576 iss velocity m/s
            new Godot.Vector3(0, 0, 0) // zero propulsion


        ));
        
        for (int i = 0; i < KeplerContainers.Count; i++) // do NOT EVER use foreach, it is read only.
        {
            CelestialRender Overwrite = KeplerContainers[i];
            ProjectCelestial(ref Overwrite); //OK, whatever is happening in this function is not creating dictionary keys and values 
         
            KeplerContainers[i] = Overwrite;
            //GD.Print(Overwrite.Ephemeris[1].PosCartesian);
           // GD.Print(Overwrite.Ephemeris[2].PosCartesian);
            //GD.Print(Overwrite.Ephemeris[30].PosCartesian);
        }
        //GD.Print(KeplerContainers[1].Ephemeris[1].MET);
       // GD.Print(KeplerContainers[1].Ephemeris[2].MET);
       // GD.Print(KeplerContainers[1].Ephemeris[30].MET);
        foreach (var NBodyAffected in NByContainers)
        {
            RunBallisticTrack(NBodyAffected);
        }
        // Debug.Log(Reference.SOI.KeplerContainer);
        // Reference.SOI.PrintProperties()

    }
    public double LastStep = 0;
    public double NextStep = Time.GetUnixTimeFromSystem() + Reference.Dynamics.TimeStep / (Reference.Dynamics.TimeCompression);
    
    public override void _Process(double Delta)
    {
       // Reference.Dynamics.TimeCompression = GetNode<Control>("Control").GetMeta("TimeCompression", new float = 1.0)); just change from witin another script
        double RealTimeInterpolate = Reference.Dynamics.MET;
        bool debug = false;

        
       // GD.Print((int)Math.Ceiling((Time.GetUnixTimeFromSystem() - NextStep) / (1 / (Reference.Dynamics.TimeCompression))));
        if (Time.GetUnixTimeFromSystem() >= NextStep & (! debug)) 
        {
            int OverfillFrame = (int)Math.Ceiling((Time.GetUnixTimeFromSystem() - NextStep) / (1 / (Reference.Dynamics.TimeCompression))); //(int)Math.Ceiling(Time.time - NextStep);
          // overfillframe just gives UnixTime for some reason?? fixed --10052023 jcr
            LastStep = Time.GetUnixTimeFromSystem();
            NextStep = (Time.GetUnixTimeFromSystem()) + Reference.Dynamics.TimeStep / (Reference.Dynamics.TimeCompression);
            for (int i = 0; i < OverfillFrame; i++)
            {
                Reference.Dynamics.MET += Reference.Dynamics.TimeStep;
                BeginStepOps();

            };

            RealTimeInterpolate = Reference.Dynamics.MET;
        }
        else
        {
            RealTimeInterpolate = Reference.Dynamics.MET + (Time.GetUnixTimeFromSystem() - NextStep);

        }
       // Reference.Dynamics.TimeCompression = 2;
        foreach (var NBodyAffected in NByContainers)
        {
            //GD.Print(RealTimeInterpolate);
            float LerpFloat = (float)(1-(Reference.Dynamics.MET-RealTimeInterpolate));
            Godot.Vector3 LerpV3 = NBodyAffected.StateVectors.PosCartesian - NBodyAffected.StateVectors.PrevPosLerp;
           // GD.Print(LerpFloat,NBodyAffected.StateVectors.PrevPosLerp);
            NBodyAffected.ObjectRef.Position = (float)ScaleConversion("ToUnityUnits")*(NBodyAffected.StateVectors.PrevPosLerp + LerpV3 * LerpFloat);



            //NBodyAffected.ObjectRef.Position = NBodyAffected.StateVectors.PosCartesian*(float)ScaleConversion("ToUnityUnits") + LerpV3 * LerpFloat;
            //GD.Print(NBodyAffected.StateVectors.PosCartesian);
            //Debug.Log(CelestialRender.Name.ToString());
        }

        foreach (var CelestialRender in KeplerContainers)
        {
            //eventually you will have to interpolate the celestials

            //MoveCelestial(CelestialRender, RealTimeInterpolate);
            //Debug.Log(CelestialRender.Name.ToString());
        }
        UpdateTemporal();
        // Debug.Log((RealTimeInterpolate));

        // AstroProp_Runtime.Reference.Dynamics.StandardGravParam += 1;
        // Debug.Log(AstroProp_Runtime.Reference.Dynamics.StandardGravParam.ToString()); ;
    }

    
}

