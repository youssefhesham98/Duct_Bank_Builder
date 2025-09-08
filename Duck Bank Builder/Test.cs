using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Duck_Bank_Builder
{
    public class Test
    {
        public static void Run(Document doc, UIDocument uidoc, PipingSystemType systemType, PipeType pipeType/*,List<int> count_list*/)
        {
            //StringBuilder sb = new StringBuilder();

            Reference pickedRef = uidoc.Selection.PickObject(ObjectType.Element, "Select a structural framing element");
            Element element = doc.GetElement(pickedRef);

            #region Draft
            //// get the type (FamilySymbol) of the selected element
            //FamilySymbol symbol = doc.GetElement(element.GetTypeId()) as FamilySymbol;

            //if (symbol != null)
            //{
            //    Family family = symbol.Family;

            //    // get all symbols (types) belonging to this family
            //    IList<ElementId> symbolIds = family.GetFamilySymbolIds();
            //    List<FamilySymbol> allSymbols = new List<FamilySymbol>();

            //    foreach (ElementId id in symbolIds)
            //    {
            //        FamilySymbol fs = doc.GetElement(id) as FamilySymbol;
            //        if (fs != null)
            //        {
            //            allSymbols.Add(fs);
            //            TaskDialog.Show("Family Type", $"Type Name: {fs.Name}");
            //        }
            //    }
            //}
            #endregion

            List<XYZ> startorigins = new List<XYZ>();
            //List<XYZ> uniquestartorigins = new List<XYZ>();
            List<XYZ> endorigins = new List<XYZ>();
            //List<XYZ> uniqueendorigins = new List<XYZ>();
            //var index = 0;
            double tolerance = 1e-6;

            foreach (var geometryinstance in element.get_Geometry(new Options()).OfType<GeometryInstance>())
            {
                Solid solid = geometryinstance.GetInstanceGeometry().OfType<Solid>().FirstOrDefault(s => s.Volume > 0);
                //var faces = solid.Faces;    
                var origins = solid.Faces.OfType<CylindricalFace>();
                List<CylindricalFace> uniqueCylFaces = new List<CylindricalFace>();
                var origins_count = origins.Count() / 2;

                #region Draft
                //for (int i = 0; i < origins_count; i++)
                //{
                //    startorigins.Add(origins.ToList()[i].Origin);

                //}
                //for (int i = origins_count; i < origins.Count(); i++)
                //{
                //    endorigins.Add(origins.ToList()[i].Origin);
                //}
                #endregion


                // keep only unique cylinder axes (avoid duplicate faces)
                foreach (var face in origins)
                {
                    XYZ origin = face.Origin;
                    XYZ axis = face.Axis.Normalize();

                    bool exists = uniqueCylFaces.Any(f =>
                        f.Origin.IsAlmostEqualTo(origin, tolerance) &&
                        f.Axis.Normalize().IsAlmostEqualTo(axis, tolerance));

                    if (!exists)
                    {
                        uniqueCylFaces.Add(face);
                    }
                }

                foreach (CylindricalFace cylFace in uniqueCylFaces)
                {
                    XYZ axisDir = cylFace.Axis.Normalize();
                    XYZ origin = cylFace.Origin;

                    // get the param bounds of the cylinder
                    BoundingBoxUV bb = cylFace.GetBoundingBox();
                    UV min = bb.Min;
                    UV max = bb.Max;

                    // axis points at both ends
                    XYZ axisStart = origin + axisDir * min.V;
                    XYZ axisEnd = origin + axisDir * max.V;

                    startorigins.Add(axisStart);
                    endorigins.Add(axisEnd);
                }

                #region Draft
                //for (int i = 0; i < origins_count; i++)
                //{
                //    startorigins.Add(origins.ToList()[i].Origin);

                //}
                //for (int i = origins_count; i < origins.Count(); i++)
                //{
                //    endorigins.Add(origins.ToList()[i].Origin);
                //}

                //foreach (XYZ pt in startorigins)
                //{
                //    bool exists = startorigins.Any(u =>
                //         Math.Abs(u.X - pt.X) < tolerance &&
                //         Math.Abs(u.Y - pt.Y) < tolerance &&
                //         Math.Abs(u.Z - pt.Z) < tolerance);

                //    if (!exists)
                //    {
                //        uniquestartorigins.Add(pt);
                //    }
                //}
                //foreach (XYZ pt in endorigins)
                //{
                //    bool exists = endorigins.Any(u =>
                //         Math.Abs(u.X - pt.X) < tolerance &&
                //         Math.Abs(u.Y - pt.Y) < tolerance &&
                //         Math.Abs(u.Z - pt.Z) < tolerance);

                //    if (!exists)
                //    {
                //        uniqueendorigins.Add(pt);
                //    }
                //}
                #endregion

                // Step 5: Get PipeType, SystemType, Level
                //PipeType pipeType = new FilteredElementCollector(doc)
                //    .OfClass(typeof(PipeType))
                //    .Cast<PipeType>()
                //    .FirstOrDefault();

                //var pipeTypes = new FilteredElementCollector(doc)
                // .OfClass(typeof(PipeType))
                // .Cast<PipeType>().OrderBy(pt => pt.Name).ToList();

                //foreach (var item in pipeTypes)
                //{
                //    Data.pipetypes.Add(item);
                //}

                //MEPSystemType systemType = new FilteredElementCollector(doc)
                //    .OfClass(typeof(PipingSystemType))
                //    .Cast<PipingSystemType>()
                //    .FirstOrDefault(st => st.SystemClassification == MEPSystemClassification.DomesticColdWater);

                //var systemTypes = new FilteredElementCollector(doc)
                //   .OfClass(typeof(PipingSystemType))
                //   .Cast<PipingSystemType>().OrderBy(st => st.Name).ToList();

                //foreach (var item in systemTypes)
                //{
                //   Data.pipingsys.Add(item);
                //}

                Level level = new FilteredElementCollector(doc)
                    .OfClass(typeof(Level))
                    .Cast<Level>()
                    .OrderBy(l => l.Elevation)
                    .FirstOrDefault();

                if (pipeType == null || systemType == null || level == null)
                {
                    TaskDialog.Show("Error", "Could not find required PipeType, SystemType, or Level.");
                    return;
                }

                //var x = origins.ToList().FirstOrDefault().Origin;
                //var y = origins.ToList().LastOrDefault().Origin;

                startorigins.OrderBy(pt => pt.X).ThenBy(pt => pt.Y).ThenBy(pt => pt.Z);
                endorigins.OrderBy(pt => pt.X).ThenBy(pt => pt.Y).ThenBy(pt => pt.Z);

                // Step 6: Create pipe between start and end points
                using (Transaction tx = new Transaction(doc, "Create Pipe from Void Axis"))
                {
                    tx.Start();

                    for (int i = 0; i < origins.Count() / 2/*count_list.Count*/; i++)
                    {
                        Pipe pipe = Pipe.Create(doc, systemType.Id, pipeType.Id, level.Id, startorigins[i], endorigins[i]);
                    }

                    tx.Commit();
                }
                //TaskDialog.Show("Coordinates", origins.Count().ToString());
            }
        }
    }

    #region Draft
    //public static string X { get; set; }
    //public static string Y { get; set; }
    //public static string Z { get; set; }
    //public static string angle { get; set; }
    //public static string InternalOriginX { get; set; }
    //public static string InternalOriginY { get; set; }

    //public static void GettheCenters(UIDocument uidoc, Document doc/*,double x,double y,double z,double angl*/)
    //{
    //    int i = 0;
    //    try
    //    {
    //        // Pick first face
    //        Reference faceRef1 = uidoc.Selection.PickObject(ObjectType.Face, "Pick first face containing circular voids");
    //        if (faceRef1 == null) TaskDialog.Show("Error", "False");

    //        // Pick second face
    //        Reference faceRef2 = uidoc.Selection.PickObject(ObjectType.Face, "Pick second face containing circular voids");
    //        if (faceRef2 == null) TaskDialog.Show("Error", "False");

    //        Data.face01.Add(faceRef1);
    //        Data.face02.Add(faceRef2);

    //        //List<Reference> faceRefs = new List<Reference> { faceRef1, faceRef2 };

    //        List<XYZ> face01pts = RetListofCenters(doc, Data.face01/*,x,y,z,angl*/);
    //        List<XYZ> face02pts = RetListofCenters(doc, Data.face02/*, x, y, z, angl*/);

    //        //Data.pt01 = RetListofCenters(doc, Data.face01);
    //        //Data.pt02 = RetListofCenters(doc, Data.face02);

    //        // Example: pick some defaults
    //        MEPSystemType systemType = new FilteredElementCollector(doc)
    //            .OfClass(typeof(PipingSystemType))
    //            .Cast<PipingSystemType>()
    //            .FirstOrDefault();

    //        PipeType pipeType = new FilteredElementCollector(doc)
    //            .OfClass(typeof(PipeType))
    //            .Cast<PipeType>()
    //            .FirstOrDefault();

    //        Level lvl = GetLevelAtElevation(doc, face01pts[0].Z);

    //        foreach (XYZ face in face01pts)
    //        {
    //            if (systemType != null && pipeType != null /*&& level != null*/)
    //            {
    //                XYZ p1 = face01pts[i];
    //                XYZ p2 = face02pts[i];

    //                //Level level = new FilteredElementCollector(doc)
    //                //     .OfClass(typeof(Level))
    //                //     .Cast<Level>()
    //                //     .OrderBy(l => Math.Abs(l.Elevation - p1.Z))
    //                //     .FirstOrDefault();

    //                Pipe newPipe = CreatePipeBetweenPoints(
    //                    doc,
    //                    p1,
    //                    p2,
    //                    systemType.Id,
    //                    pipeType.Id,
    //                    lvl.Id
    //                );
    //            }
    //            i++;
    //        }
    //        #region MyRegion
    //        // User selects a structural framing element
    //        //Reference pickedRef = uidoc.Selection.PickObject(ObjectType.Element, "Pick a structural framing element");
    //        //if (pickedRef == null) TaskDialog.Show("Error", "False"); ;

    //        //Element element = doc.GetElement(pickedRef);

    //        //Options geomOptions = new Options();
    //        //geomOptions.ComputeReferences = true;
    //        //geomOptions.DetailLevel = ViewDetailLevel.Fine;

    //        //GeometryElement geomElem = element.get_Geometry(geomOptions);

    //        //List<Face> verticalFaces = new List<Face>();

    //        //foreach (GeometryObject geomObj in geomElem)
    //        //{
    //        //    Solid solid = geomObj as Solid;
    //        //    if (solid == null || solid.Faces.Size == 0) continue;

    //        //    foreach (Face face in solid.Faces)
    //        //    {
    //        //        PlanarFace pf = face as PlanarFace;
    //        //        if (pf == null) continue;

    //        //        XYZ normal = pf.FaceNormal.Normalize();

    //        //        // Check vertical face (normal perpendicular to Z axis)
    //        //        if (Math.Abs(normal.Z) < 1e-6)
    //        //        {
    //        //            verticalFaces.Add(pf);
    //        //        }
    //        //    }
    //        //}

    //        //// Pick the two smallest vertical faces
    //        //var smallestFaces = verticalFaces
    //        //    .OrderBy(f => f.Area)
    //        //    .Take(2)
    //        //    .ToList();

    //        //StringBuilder sb = new StringBuilder();
    //        //int faceIndex = 1;
    //        #endregion

    //    }
    //    catch (Exception ex)
    //    {
    //        TaskDialog.Show("Error", ex.Message);
    //    }
    //}
    //public static List<XYZ> RetListofCenters(Document doc, List<Reference> faceRefs/*,double x,double y,double z,double angl*/)
    //{
    //    StringBuilder sb = new StringBuilder();
    //    int faceIndex = 1;
    //    double tolerance = 1e-6;
    //    List<XYZ> uniqcenters = new List<XYZ>();
    //    //List<XYZ> transcenters = new List<XYZ>();

    //    //Transform transform = doc.ActiveProjectLocation.GetTotalTransform();


    //    //// 1. Get transform from Internal Origin → Survey Point
    //    //Transform internalToSurvey = doc.ActiveProjectLocation.GetTransform();



    //    foreach (Reference faceRef in faceRefs)
    //    {
    //        Face pickedFace = doc.GetElement(faceRef).GetGeometryObjectFromReference(faceRef) as Face;
    //        if (pickedFace == null) continue;

    //        sb.AppendLine($"--- Vertical Face {faceIndex} ---");

    //        List<XYZ> centers = new List<XYZ>();

    //        EdgeArrayArray edgeLoops = pickedFace.EdgeLoops;
    //        foreach (EdgeArray edgeArray in edgeLoops)
    //        {
    //            foreach (Edge edge in edgeArray)
    //            {
    //                Curve curve = edge.AsCurve();

    //                if (curve is Arc arc && arc.IsBound && arc.IsCyclic)
    //                {
    //                    // Check if full circle
    //                    if (arc.IsBound /*&& Math.Abs(arc.Length - 2 * Math.PI * arc.Radius) < 1e-6*/)
    //                    {
    //                        //XYZ srcenter = ToSurveyPoint(doc, arc.Center);
    //                        //XYZ ppcenter = ToProjectBasePoint(doc, arc.Center);
    //                        XYZ center = arc.Center;
    //                        if (centers.Contains(center)) continue;
    //                        else
    //                        {
    //                            //centers.Add(srcenter);
    //                            centers.Add(center);
    //                            //centers.Add(ppcenter);
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //        foreach (XYZ pt in centers)
    //        {
    //            bool exists = uniqcenters.Any(u =>
    //                 Math.Abs(u.X - pt.X) < tolerance &&
    //                 Math.Abs(u.Y - pt.Y) < tolerance &&
    //                 Math.Abs(u.Z - pt.Z) < tolerance);

    //            if (!exists)
    //            {
    //                uniqcenters.Add(pt);
    //            }
    //        }
    //        if (uniqcenters.Count == 0)
    //        {
    //            sb.AppendLine("No circular voids found.");
    //        }
    //        else
    //        {
    //            int i = 1;
    //            foreach (XYZ pt in uniqcenters)
    //            {
    //                sb.AppendLine($"Circle {i}: X={pt.X:F3}, Y={pt.Y:F3}, Z={pt.Z:F3}");
    //                i++;
    //            }
    //        }

    //        faceIndex++;
    //        //sb.AppendLine();
    //    }

    //    #region MyRegion
    //    //foreach (XYZ p in uniqcenters)
    //    //{
    //    //    XYZ pRelToPBP = TransformPoint(p, doc, "PBP");
    //    //    XYZ pRelToSP = TransformPoint(p, doc, "SP");
    //    //    XYZ pRelToIO = TransformPoint(p, doc, "IO"); // basically original

    //    //    transcenters.Add(pRelToPBP);

    //    //    //TaskDialog.Show("Coords",
    //    //    //    $"Internal Origin: ({p.X:F3},{p.Y:F3},{p.Z:F3})\n" +
    //    //    //    $"Project Base Point: ({pRelToPBP.X:F3},{pRelToPBP.Y:F3},{pRelToPBP.Z:F3})\n" +
    //    //    //    $"Survey Point: ({pRelToSP.X:F3},{pRelToSP.Y:F3},{pRelToSP.Z:F3})");
    //    //}
    //    #endregion

    //    // 2. Transform points
    //    //List<XYZ> surveyPoints = uniqcenters
    //    //    .Select(p => internalToSurvey.OfPoint(p))
    //    //    .ToList();

    //    //transcenters = transformPts(uniqcenters,x,y,z,angl);

    //    TaskDialog.Show("Circle Centers", sb.ToString());

    //    return uniqcenters;
    //}
    //private static Pipe CreatePipeBetweenPoints(Document doc, XYZ start, XYZ end, ElementId systemTypeId, ElementId pipeTypeId, ElementId levelId)
    //{
    //    // Transform from Internal Origin (IO) → Shared coordinates (Survey Point)
    //    //Transform sharedTransform = doc.ActiveProjectLocation.GetTotalTransform();
    //    Pipe pipe = null;


    //    using (Transaction trans = new Transaction(doc, "Create Pipe"))
    //    {
    //        trans.Start();
    //        //var pt01 = TransformPoint(start, sharedTransform);
    //        //var pt02 = TransformPoint(end, sharedTransform);

    //        pipe = Pipe.Create(doc, systemTypeId, pipeTypeId, levelId, start, end);


    //        //, TransformPoint(start, doc/*,"IO"*/), TransformPoint(end, doc/*,"IO"*/)
    //        trans.Commit();
    //    }

    //    return pipe;
    //}
    //private static XYZ ToSurveyPoint(Document doc, XYZ internalPoint)
    //{
    //    Transform toShared = doc.ActiveProjectLocation.GetTransform();
    //    return toShared.OfPoint(internalPoint);
    //}
    //private static XYZ ToProjectBasePoint(Document doc, XYZ internalPoint)
    //{
    //    BasePoint pbp = new FilteredElementCollector(doc)
    //        .OfCategory(BuiltInCategory.OST_ProjectBasePoint)
    //        .OfClass(typeof(BasePoint))
    //        .Cast<BasePoint>()
    //        .FirstOrDefault();

    //    if (pbp == null)
    //        return internalPoint; // fallback if not found

    //    double eastWest = pbp.get_Parameter(BuiltInParameter.BASEPOINT_EASTWEST_PARAM).AsDouble();
    //    double northSouth = pbp.get_Parameter(BuiltInParameter.BASEPOINT_NORTHSOUTH_PARAM).AsDouble();
    //    double elevation = pbp.get_Parameter(BuiltInParameter.BASEPOINT_ELEVATION_PARAM).AsDouble();

    //    XYZ offset = new XYZ(eastWest, northSouth, elevation);

    //    // Internal coords → relative to Project Base Point
    //    return internalPoint - offset;
    //}
    //private void GetPointInfo(XYZ p1, XYZ p2, out double distance, out double slopeAngleDeg, out double planAngleDeg)
    //{
    //    // Distance in 3D (always in feet in Revit)
    //    distance = p1.DistanceTo(p2);

    //    // Horizontal projection vector (XY plane)
    //    double dx = p2.X - p1.X;
    //    double dy = p2.Y - p1.Y;
    //    double dz = p2.Z - p1.Z;

    //    // Angle in XY plane (bearing, radians → degrees)
    //    planAngleDeg = Math.Atan2(dy, dx) * (180.0 / Math.PI);

    //    // Slope angle relative to horizontal (radians → degrees)
    //    double horizDist = Math.Sqrt(dx * dx + dy * dy);
    //    slopeAngleDeg = Math.Atan2(dz, horizDist) * (180.0 / Math.PI);
    //}
    //private static XYZ TransformPoint(XYZ point, Document doc/*, string targetSystem = "PBP",*/ /*double x, double y, double z*/)
    //{
    //    // Get project location
    //    ProjectLocation location = doc.ActiveProjectLocation;

    //    // Transform: Project Base Point (PBP) → Survey Point (SP) ----- (Neglect)
    //    Transform pbpToSp = location.GetTransform();

    //    // Maps points from PBP to IO
    //    Transform pbpToInternalTransform = location.GetTotalTransform();

    //    // Inverse transform
    //    Transform internalToPbpTransform = pbpToInternalTransform.Inverse;

    //    // Apply of point
    //    /*XYZ internalPoint = new XYZ(x, y, z);*/ // Your point in internal coordinates
    //    XYZ pbpRelativePoint = internalToPbpTransform.OfPoint(point);
    //    XYZ position = new XYZ();

    //    // Collect all base points
    //    var basePoints = new FilteredElementCollector(doc)
    //                        .OfClass(typeof(BasePoint))
    //                        .Cast<BasePoint>();

    //    foreach (var bp in basePoints)
    //    {
    //        // The Survey Point is the shared one
    //        bool isShared = bp.IsShared;

    //        if (isShared)
    //        {
    //            // Get location in Revit internal coordinates
    //            position = bp.Position;

    //            //TaskDialog.Show("Survey Point",
    //            //    $"Survey Point Location:\nX = {position.X}\nY = {position.Y}\nZ = {position.Z}");
    //        }
    //    }


    //    // Get SP
    //    //ProjectLocation surveyPoint = new FilteredElementCollector(doc).OfClass(typeof(ProjectLocation))
    //    //                                          .Cast<ProjectLocation>()
    //    //                                          .FirstOrDefault(p => p.Name == "Survey Point"); // Or identify by other means;

    //    //Get SP position
    //    //XYZ surveyPointLocation = surveyPoint.GetProjectPosition();

    //    /*XYZ internalPoint = new XYZ(x, y, z);*/ // Your point in internal coordinates
    //    XYZ surveyRelativePoint = point - /*surveyPointLocation*/ position;
    //    return surveyRelativePoint;

    //    // Handle the cases
    //    //switch (targetSystem.ToUpper())
    //    //{
    //    //    case "IO": // Already in Internal Origin
    //    //        return point;

    //    //    case "PBP":
    //    //        // Convert from Internal Origin to Project Base Point
    //    //        // Inverse of IO→PBP (which is pbpToSp.Inverse * (IO→SP))
    //    //        // Trick: IO→SP = pbpToSp * (IO→PBP)
    //    //        // So IO→PBP = pbpToSp.Inverse * (IO→SP)
    //    //        // But IO is the "truth", so: apply inverse of PBP->SP to get back to PBP
    //    //        return pbpToSp.Inverse.OfPoint(point);

    //    //    case "SP":
    //    //        // Convert from Internal Origin to Survey Point
    //    //        return pbpToSp.OfPoint(point);

    //    //    default:
    //    //        throw new ArgumentException("Invalid target system. Use IO, PBP, or SP.");
    //    //}
    //}
    //private static Level GetLevelAtElevation(Document doc, double elevationFeet)
    //{
    //    List<Level> levels = new FilteredElementCollector(doc)
    //        .OfClass(typeof(Level))
    //        .Cast<Level>()
    //        .OrderBy(l => l.Elevation)
    //        .ToList();

    //    // Find level at or just below the point's elevation
    //    Level hostingLevel = levels.LastOrDefault(l => l.Elevation <= elevationFeet);

    //    // If none, fall back to lowest
    //    if (hostingLevel == null)
    //        hostingLevel = levels.FirstOrDefault();

    //    return hostingLevel;
    //}
    //public static List<XYZ> transformPts(List<XYZ> points, double offsetX, double offsetY, double offsetZ, double angleDeg)
    //{
    //    // Example points (internal origin coordinates)
    //    //   List<XYZ> internalPoints = new List<XYZ>
    //    //   {
    //    //       new XYZ(13.90, -3.97, 0),   // Point 1
    //    //    new XYZ(20.46, -3.97, 0),  // Point 2
    //    //    new XYZ(25.71, -3.97, 0)   // Point 3
    //    //};

    //    // --- Step 1: Project Base Point offsets ---
    //    offsetX = 100.0; // E/W
    //    offsetY = 100.0; // N/S
    //    offsetX = 100.0; // Elevation

    //    // --- Step 2: Rotation (angle to true north) ---
    //    offsetX = 45.0;
    //    double angleRad = angleDeg * Math.PI / 180.0;

    //    Transform rotation = Transform.CreateRotation(XYZ.BasisZ, angleRad);
    //    Transform translation = Transform.CreateTranslation(new XYZ(offsetX, offsetY, offsetZ));

    //    // Combined transform: Rotate then translate
    //    Transform pbpTransform = translation.Multiply(rotation);

    //    // --- Step 3: Apply Survey Point shift ---
    //    double surveyX = 0;
    //    double surveyY = 0;
    //    double surveyZ = 0;

    //    Transform surveyShift = Transform.CreateTranslation(new XYZ(surveyX, surveyY, surveyZ));

    //    // Final transform = Survey shift * (PBP rotation + translation)
    //    Transform finalTransform = surveyShift.Multiply(pbpTransform);

    //    // --- Step 4: Transform points ---
    //    List<XYZ> realWorldPoints = new List<XYZ>();
    //    foreach (XYZ p in points)
    //    {
    //        realWorldPoints.Add(finalTransform.OfPoint(p));
    //    }

    //    return realWorldPoints;

    //    // Show results
    //    //StringBuilder sb = new StringBuilder();
    //    //sb.AppendLine("Manual Transformed Points (Internal → Real World):");

    //    //for (int i = 0; i < realWorldPoints.Count; i++)
    //    //{
    //    //    sb.Append("P").Append(i + 1).Append(": ")
    //    //      .Append("X=").Append(realWorldPoints[i].X.ToString("F2")).Append(", ")
    //    //      .Append("Y=").Append(realWorldPoints[i].Y.ToString("F2")).Append(", ")
    //    //      .Append("Z=").Append(realWorldPoints[i].Z.ToString("F2"))
    //    //      .AppendLine();
    //    //}

    //    //TaskDialog.Show("Manual Real Coordinates", sb.ToString());
    //}

    //public static void Get(Document doc, UIDocument uidoc)
    //{
    //    try
    //    {
    //        // 1. Pick a structural framing element
    //        Reference pickedRef = uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element, "Pick a structural framing family with openings");
    //        FamilyInstance fi = doc.GetElement(pickedRef) as FamilyInstance;

    //        if (fi == null)
    //        {
    //            TaskDialog.Show("Error", "Please pick a structural framing instance.");
    //            //return Result.Failed;
    //        }

    //        // 2. Get instance transform (local → model coords)
    //        Transform instTransform = fi.GetTransform();

    //        // 3. Extract geometry
    //        Options opt = new Options { ComputeReferences = true, DetailLevel = ViewDetailLevel.Fine };
    //        GeometryElement geomElem = fi.get_Geometry(opt);

    //        List<XYZ> circleCenters = new List<XYZ>();

    //        foreach (GeometryObject geomObj in geomElem)
    //        {
    //            if (geomObj is Solid solid && solid.Faces.Size > 0)
    //            {
    //                foreach (Face face in solid.Faces)
    //                {
    //                    // Loop through face edges
    //                    EdgeArrayArray edgeLoops = face.EdgeLoops;
    //                    foreach (EdgeArray loop in edgeLoops)
    //                    {
    //                        foreach (Edge edge in loop)
    //                        {
    //                            Curve c = edge.AsCurve();
    //                            if (c is Arc arc && arc.IsCyclic) // It's a full circle
    //                            {
    //                                XYZ centerLocal = arc.Center;

    //                                // 4. Transform local → model coords
    //                                XYZ centerModel = instTransform.OfPoint(centerLocal);

    //                                // 5. Apply Survey Point transform
    //                                XYZ centerSurvey = ConvertToSurveyPoint(doc, centerModel);

    //                                circleCenters.Add(centerSurvey);
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }

    //        // 6. Output results
    //        StringBuilder sb = new StringBuilder();
    //        sb.AppendLine($"Found {circleCenters.Count} circular openings:");
    //        int idx = 1;
    //        foreach (XYZ p in circleCenters)
    //        {
    //            sb.AppendLine($"Circle {idx++}: X={p.X:F2}, Y={p.Y:F2}, Z={p.Z:F2}");
    //        }

    //        TaskDialog.Show("Circle Centers", sb.ToString());
    //    }
    //    catch (Exception ex)
    //    {
    //        TaskDialog.Show("Error", ex.Message);
    //        //return Result.Failed;
    //    }

    //    //return Result.Succeeded;
    //}

    //// Helper: Convert model XYZ → Survey Point coordinates
    //public static XYZ ConvertToSurveyPoint(Document doc, XYZ modelPoint)
    //{
    //    BasePoint surveyPoint = new FilteredElementCollector(doc)
    //        .OfClass(typeof(BasePoint))
    //        .Cast<BasePoint>()
    //        .FirstOrDefault(x => x.IsShared);

    //    if (surveyPoint == null) return modelPoint;

    //    XYZ surveyOrigin = surveyPoint.Position;
    //    double surveyRotation = surveyPoint.get_Parameter(BuiltInParameter.BASEPOINT_ANGLETON_PARAM).AsDouble();

    //    Transform rotation = Transform.CreateRotationAtPoint(XYZ.BasisZ, -surveyRotation, surveyOrigin);
    //    XYZ surveyPointCoords = rotation.OfPoint(modelPoint) + surveyOrigin;

    //    return surveyPointCoords;
    //}


    //// ---------------------- HELP

    //public static XYZ ConvertPoint(XYZ local)
    //{
    //    var surveyOrigin = new XYZ(double.Parse(X), double.Parse(Y), 0);

    //    var internalorigin = new XYZ(double.Parse(InternalOriginX), double.Parse(InternalOriginY), 0);

    //    double angleRadians = double.Parse(angle) * Math.PI / 180;


    //    // Translate
    //    XYZ pt = local + surveyOrigin - internalorigin;

    //    // Rotate around Z at surveyOrigin if needed
    //    if (angleRadians != 0.0)
    //    {
    //        // move to origin
    //        XYZ v = pt - surveyOrigin;
    //        // rotate about Z axis
    //        Transform rot = Transform.CreateRotation(XYZ.BasisZ, angleRadians);
    //        XYZ vRot = rot.OfVector(v);
    //        // move back
    //        pt = surveyOrigin + vRot;
    //    }

    //    return pt;
    //}

    //public static XYZ SumSigned(XYZ basePoint, XYZ offset)
    //{
    //    //double SumCoord(double a, double b)
    //    //{
    //    //    return (b >= 0)
    //    //      ? a + b        // positive offset → move forwards
    //    //      : a - Math.Abs(b);  // negative offset → move backwards
    //    //}

    //    //return new XYZ(
    //    //  SumCoord(basePoint.X, offset.X),
    //    //  SumCoord(basePoint.Y, offset.Y),
    //    //  SumCoord(basePoint.Z, offset.Z)
    //    //);
    //    return new XYZ(basePoint.X + offset.X, basePoint.Y + offset.Y, basePoint.Z + offset.Z);


    //}

    //public static XYZ RotateAboutZ(XYZ pt, XYZ origin, double angleRadians)
    //{
    //    if (angleRadians == 0.0)
    //        return pt;

    //    // translate to origin, rotate, translate back
    //    XYZ v = pt - origin;
    //    Transform rot = Transform.CreateRotation(XYZ.BasisZ, angleRadians);
    //    XYZ vRot = rot.OfVector(v);
    //    return origin + vRot;
    //}

    //public static XYZ ConvertPoint(XYZ local, XYZ locationPoint)
    //{
    //    // parse your survey origin & angle however you like…
    //    var surveyOrigin = new XYZ(
    //      double.Parse(X),
    //      double.Parse(Y),
    //      0.0
    //    );

    //    var internalOrigin = new XYZ(double.Parse(InternalOriginX), double.Parse(InternalOriginY), 0.0);

    //    double angleRadians = double.Parse(angle) * Math.PI / 180;

    //    // 1) combine the “local” and “user‑entered location” offsets
    //    XYZ combinedOffset = SumSigned(local, locationPoint) - internalOrigin;

    //    // 2) apply survey origin offset on top
    //    XYZ worldPt = SumSigned(surveyOrigin, combinedOffset);

    //    // 3) rotate the result around Z at surveyOrigin
    //    return RotateAboutZ(worldPt, surveyOrigin, angleRadians);
    //}

    //public static XYZ TransformPoint(XYZ point, Transform transform)
    //{
    //    double x = point.X;
    //    double y = point.Y;
    //    double z = point.Z;

    //    //transform basis of the old coordinate system in the new coordinate // system
    //    XYZ b0 = transform.get_Basis(0);
    //    XYZ b1 = transform.get_Basis(1);
    //    XYZ b2 = transform.get_Basis(2);
    //    XYZ origin = transform.Origin;

    //    //transform the origin of the old coordinate system in the new 
    //    //coordinate system
    //    double xTemp = x * b0.X + y * b1.X + z * b2.X + origin.X;
    //    double yTemp = x * b0.Y + y * b1.Y + z * b2.Y + origin.Y;
    //    double zTemp = x * b0.Z + y * b1.Z + z * b2.Z + origin.Z;

    //    return new XYZ(xTemp, yTemp, zTemp);
    //}

    //public static void Circles(Document doc, UIDocument uidoc)
    //{
    //    StringBuilder sb = new StringBuilder();
    //    int index = 1;
    //    // Pick first face
    //    Reference faceRef = uidoc.Selection.PickObject(ObjectType.Face, "Pick first face containing circular voids");
    //    if (faceRef == null) TaskDialog.Show("Error", "False");

    //    Face pickedFace = doc.GetElement(faceRef).GetGeometryObjectFromReference(faceRef) as Face;
    //    if (pickedFace == null) TaskDialog.Show("False", "Error");

    //    List<XYZ> centers = new List<XYZ>();

    //    // Loop over edge loops on the face
    //    foreach (EdgeArray edgeArray in pickedFace.EdgeLoops)
    //    {
    //        foreach (Edge edge in edgeArray)
    //        {
    //            // Convert edge to curve
    //            Curve curve = edge.AsCurve();

    //            // Check if it's a full circle
    //            if (curve is Arc arc && arc.IsBound && arc.IsCyclic)
    //            {
    //                // Add the center point of the circle
    //                centers.Add(arc.Center);
    //                XYZ center = arc.Center;
    //                sb.Append("Circle ");
    //                sb.Append(index);
    //                sb.Append(": X=");
    //                sb.Append(center.X.ToString("F3"));
    //                sb.Append(", Y=");
    //                sb.Append(center.Y.ToString("F3"));
    //                sb.Append(", Z=");
    //                sb.Append(center.Z.ToString("F3"));
    //                sb.AppendLine();

    //                index++;
    //            }
    //        }
    //    }

    //    TaskDialog.Show("Result", sb.ToString());

    //}

    //-----------------------------------------ClassificationEntries

    //using Autodesk.Revit.DB;
    //using System;
    //using System.Collections.Generic;
    //using System.Linq;
    //using System.Text;
    //using System.Threading.Tasks;

    //namespace Duck_Bank_Builder
    //{
    //    internal class CircleOnFace
    //    {
    //        public XYZ Center;
    //        public double Radius;
    //        public UV UV; // for ordering on face
    //    }
    //}

    //-----------------------------------------ClassificationEntries

    //using Autodesk.Revit.DB;
    //using System;
    //using System.Collections.Generic;
    //using System.Linq;
    //using System.Text;
    //using System.Threading.Tasks;

    //namespace Duck_Bank_Builder
    //{
    //    internal class FaceData
    //    {
    //        public PlanarFace Face;
    //        public List<CircleOnFace> Circles = new List<CircleOnFace>();
    //    }
    //}

    #endregion
}
