/*-------------------
Elie Yen
C#
Shape calculator
--------------------*/
class ShapeBuilder{
    
    // some fixed data
    private static Dictionary<string, int[]> Restriction = new Dictionary<string, int[]> {
        // each represent edges/ length of sides/ diagonals/ least attributes to compute
        // val < 0 if shape doesn't need/ has specific requirement of that attribute 
        ["quadrilateral"] = new int[] {4, 4, 2, 3}, ["kite"] = new int[] {4, 2, 2, 2},
        ["parallelogram"] = new int[] {4, 2, 2, 3}, ["rhombus"] = new int[] {4, 1, 2, 2},
        ["square"] = new int[] {4, 1, 0, 1}, ["rectangle"] = new int[] {4, 2, 0, 1},
        ["triangle"] = new int[] {3, 3, 0, 1}, ["set_by_coordinates"] = new int[] {-3, -3, 0, 1}, 
        ["circle"] = new int[] {-1, -1, 0, 1}, ["regular_polygon"] = new int[] {-2, 1, 0, 2}
    };
    private static string[] msgSet = new string[]{
        "\n(V) [{0}] set successfully!",
        "\n[.LenofSides] to set {0} length of sides",
        "\n[.Vertexs] to set at least 3 coordinates of Vertexs",
        "\n[.Diagonals] to set 2 diagonals (value > 0)", 
        "\n[.AngleofDiagonals] to set angle between 2 diagonals (0 < value < 180)",
        "[!] The shape is not support this attribute",
        "[!] {0} is not supported, try another",
        "[!] Invalid value, check again\n{0}",
        "[!] This attribute can't be modified, use [Reset] if you want to reset all attributes",
        "[!] Not set yet.",
        "calculation error (incorrect/ insufficient attributes)",
    };
    
    // initialized attributes for setting & auto compute
    private string _Shape = msgSet[9];
    private double[] _LenofSides = new double[]{-1};
    private double _Radius = -1; // for circle only
    private double[] _Diagonals = new double[]{-1};
    private double _AngleofDiagonals = -1;
    private double DegreeofDiagonals = -1; // hidden, just for calcuation
    private double[][] _Vertexs = new double[][]{ new double[]{-1, -1} };
    private int Verified = -1; // initialize, hidden, represent shape is not set yet
    private string _Requirement = msgSet[9];
    private string _Info = msgSet[9];
    private double _Area = -1;
    private double _Perimeter = -1;
    private int _Edge = -1;
    private bool[] show_Info = new bool[]{
        // Area, Perimeter, Radius, Edge, LenofSides, Diagonals, AngleofDiagonals, Vertexs
        true, true, false, true, true, false, false, false
    }; 
    
    // set by user
    public string Shape {
        get => _Shape;
        set{
            if (! Restriction.ContainsKey(value.ToLower())){
                Console.WriteLine(String.Format(msgSet[6], value));
            }
            else if (_Shape != msgSet[9]){
                // try to modify the shape
                Console.WriteLine(msgSet[8]);
            }
            else{
                _Shape = value.ToLower();
                Verified = 0; // still need to reach requirement for calculation
                // initialize requirment, info content, edge (except coordinates and regular polygon)
                _Requirement = String.Format(msgSet[1], Restriction[Shape][1]);
                _Info = $"===== Info of {Shape} =====\n";
                _Info += "Area: {0}\nPerimeter: {1}\nEdges: {2}\nLength: {3}\n";

                // check by num of edge in restriction
                switch (Restriction[Shape][0]) {
                    case -1: // circle
                        _Requirement = "[.Radius] to set radius.";
                        _Info = "Area: {0}\nPerimeter: {1}\nRadius: {2}";
                        show_Info[2] = true;
                        show_Info[3] = false;
                        show_Info[4] = false;
                        _Edge = 0;
                        break;
                    case -2: // regular_polygon
                        _Requirement += " [Edge] to set edges";
                         break;
                    case -3: // set by_coordinates
                         _Requirement = msgSet[2];
                         _Info += "Vertexs: {4}\n";
                         show_Info[7] = true;
                         break;
                    case 4: 
                        if (Restriction[Shape][2] == 2){
                            // for some quadrilateral need diagonal & angle
                            _Requirement += (Restriction[Shape][3] == 2)? msgSet[3]:
                                            msgSet[3] + msgSet[4];
                        }
                        // all quadrilateral
                        _Info += "Diagonals: {4}\nAngle of diagonals: {5}";
                        show_Info[5] = true;
                        show_Info[6] = true;
                        _Edge = 4;
                        break; 
                    default: _Edge = 3; break;
                }
                Console.WriteLine(String.Format(msgSet[0], Shape));
                Console.WriteLine("In order to calculate, please set following attribute(s)!");
                Console.WriteLine(Requirement);
            }
            
        }
    }
    public int Edge {
        get => _Edge;
        set{
            if (Shape == "regular_polygon"){
                if (value > 2){
                    _Edge = value;
                    Verified ++;
                    Console.WriteLine(String.Format(msgSet[0], "Edge"));
                }
                else {
                    Console.WriteLine(String.Format(msgSet[7], value));
                }
            }
            else {
                Console.WriteLine(msgSet[5]);
            }
        }
    }
    public double[] LenofSides{
        get => _LenofSides;
        set {
            string res = String.Format(msgSet[0], "Len");
            if (Verified < 0 || Shape == "set_by_coordinates" || Shape == "circle"){
                res = msgSet[5];
            }
            else if (value.Length != Restriction[Shape][1] ||
                    ! Array.TrueForAll(value, x => x > 0) ){
                res = String.Format(msgSet[7], String.Format(msgSet[1], Restriction[Shape][1]));
            }
            else {
                Verified ++;
                if (Restriction[Shape][0] != 4){
                    _LenofSides = value;
                }
                else{
                    // quadrilateral check if valid by Euler's quadrilateral theorem 
                    // a^2 + b^2 + c^2 + d^2 >= p^2 + q^2
                    double p = (_Diagonals[0] == -1) ? 0 :
                        _Diagonals[0] * _Diagonals[0] + _Diagonals[1] + _Diagonals[1];
                    
                    // avoid directly calculate with p leading overflow
                    double sumofspower = 0;
                    double[] tmp = new double[4];
                    for (int i = 0; i < 4; i++){
                        tmp[i] = (i < value.Length) ? value[i]: value[i % value.Length];
                        sumofspower += tmp[i] * tmp[i];
                    }
                    if (sumofspower - p < 0){
                        Verified --;
                        res = String.Format(msgSet[7], String.Format(msgSet[1], Restriction[Shape][1]));
                    }
                    else {
                        _LenofSides = tmp;
                    }
                }
            }
            Console.WriteLine(res);
        }
    }
    public double Radius{
        get => _Radius;
        set {
            string res = String.Format(msgSet[0], "Radius");
            if (Shape != "circle"){
                res = msgSet[5];
            }
            else if (value < 0){
                res = String.Format(msgSet[7], String.Format(msgSet[1], Restriction[Shape][1]));
            }
            else {
                Verified ++;
                _Radius = value;
            }
            Console.WriteLine(res);
        }
    }
    public double[] Diagonals{
        get => _Diagonals;
        set{
            if (Verified < 0 || Restriction[Shape][2] == 0){
                Console.WriteLine(msgSet[5]);
            }
            else if (value.Length != 2 || !Array.TrueForAll(value, x => x > 0)){
                Console.WriteLine(String.Format(msgSet[7], msgSet[3]));
            }
            else {
                Verified ++;
                string res = String.Format(msgSet[0], "diagonals");
                if (_LenofSides.Length == 4){
                    // quadrilateral check if valid by Euler's quadrilateral theorem 
                    // a^2 + b^2 + c^2 + d^2 >= p^2 + q^2
                    double p =  value[0] * value[0] + value[1] * value[1];
                    
                    // avoid directly calculate with p leading overflow
                    double sumofspower = 0; 
                    foreach (double side in LenofSides){
                        sumofspower += side * side;
                    }
                    if (sumofspower - p < 0){
                        Verified --;
                        res = String.Format(msgSet[7], msgSet[3]);
                    }
                    else {
                        _Diagonals = value;
                    }
                }
                else {
                    _Diagonals = value;
                }
                Console.WriteLine(res);
            }
        }
    }
    public double AngleofDiagonals{
        get => _AngleofDiagonals;
        set {
            if (Verified < 0 || Restriction[Shape][2] == 0){
                Console.WriteLine(msgSet[5]);
            }
            else if (! (value > 0 && value < 180) ){
                Console.WriteLine(String.Format(msgSet[7], msgSet[4]));
            }
            else {
                Verified ++;
                _AngleofDiagonals = value;
                DegreeofDiagonals = (value * Math.PI / 180);
                Console.WriteLine(String.Format(msgSet[0], "Angle"));
            }
        }
    }
    public double[][] Vertexs{
        get => _Vertexs;
        set {
            if (Shape != "set_by_coordinates"){
                Console.WriteLine(msgSet[5]);
            }
            else if (value.Length < 3 || 
                !Array.TrueForAll(value, x => x.Length == 2)){
                Console.WriteLine(String.Format(msgSet[7], msgSet[2]));
            }
            else {
                Verified ++;
                _Vertexs = value;
                _Edge = value.Length;
                Console.WriteLine(String.Format(msgSet[0], "Vertexs"));
            }
        }
        
    }

    // constructors with different perameters
    public ShapeBuilder(){}
    public ShapeBuilder(string sp){ this.Shape = sp; }

    private void GetAreaPerimeter(){
        double s = LenofSides[0];
        switch (Shape){
            case "regular_polygon": 
                _Perimeter = Edge * s;
                _Area = 0.25 * Edge * s * s / Math.Tan(Math.PI / Edge);    
                break;
            case "triangle":
                _Perimeter = LenofSides[0] + LenofSides[1] + LenofSides[2];
                // Heron's formula
                s = _Perimeter / 2;
                _Area = Math.Pow(s * (s - LenofSides[0]) * 
                    (s - LenofSides[1]) * (s - LenofSides[2]), 0.5);
                break;
            case "circle":
                _Area = Radius * Radius * Math.PI;
                _Perimeter = Radius * 2 * Math.PI;
                break;
            case "square":
                // update diagonals, area, perimeter if it's rectangle or square
                double d1 = _LenofSides[0] * Math.Sqrt(2);
                _Diagonals = new double[]{d1, d1};
                _AngleofDiagonals = 90;
                DegreeofDiagonals = Math.PI / 2;
                _Area = _LenofSides[0] * _LenofSides[0];
                _Perimeter = 4 * _LenofSides[0]; 
                break;
            case "rectangle": 
                double d2 = Math.Sqrt(_LenofSides[0] * + _LenofSides[0] +
                                    _LenofSides[1] * _LenofSides[1]);
                _Diagonals = new double[]{d2, d2};
                DegreeofDiagonals = Math.PI - 2 * Math.Asin(_LenofSides[0] / d2);
                _AngleofDiagonals = (DegreeofDiagonals * 180) / Math.PI;
                _Area = _LenofSides[0] * _LenofSides[1];
                _Perimeter = 2 * (_LenofSides[0] + _LenofSides[1]);
                break;
            case "kite":
                _AngleofDiagonals = 90;
                DegreeofDiagonals = Math.PI / 2;
                _Area = 0.5 * Diagonals[0] * Diagonals[1];
                _Perimeter = 2 * (LenofSides[0] + LenofSides[1]);
                break;
            case "rhombus":
                _AngleofDiagonals = 90;
                DegreeofDiagonals = Math.PI / 2;
                _Area = 0.5 * Diagonals[0] * Diagonals[0];
                _Perimeter = 4 * LenofSides[0];
                break;
            case "set_by_coordinates":
                int n = Edge;
                _LenofSides = new double[n];
                _Area = (0.5 * Math.Abs( Vertexs[0][0] * Vertexs[n - 1][1] 
                                    - Vertexs[0][1] * Vertexs[n - 1][0]));
                _LenofSides[0] = Math.Pow(Vertexs[0][0] - Vertexs[n - 1][0], 2) + 
                                Math.Pow(Vertexs[0][1] - Vertexs[n - 1][1], 2);
                _Perimeter = _LenofSides[0];
                for (int i = 0; i < n - 1; i++ ){
                    _Area += 0.5 * Math.Abs(Vertexs[i][0] * Vertexs[i + 1][1] - 
                                            Vertexs[i][1] * Vertexs[i + 1][0]);
                    _LenofSides[i + 1] = Math.Pow(Vertexs[i][0] - Vertexs[i + 1][0], 2) +
                                        Math.Pow(Vertexs[i][1] - Vertexs[i + 1][1], 2);
                    _Perimeter += _LenofSides[i + 1];
                }
                break;
            default:
                // parallelogram and quadrilateral use the same way
                _Area = 0.5 * Math.Sin(DegreeofDiagonals) * Diagonals[0] * Diagonals[1];
                _Perimeter = LenofSides[0] + LenofSides[1] + LenofSides[2] + LenofSides[3];
                break;
        }
    }
    private string Print(object objx){
        string res = "[";
        switch (objx.GetType().Name){
            case "Double[]":
                double[] obj = (double[])objx;
                if (obj[0] < 0){
                    return "--";
                }
                foreach (double x in obj){
                    res += String.Format(" {0:N2}," , x);
                }
                res = res.TrimEnd(',');
                res += " ]";
                return res; 
            case "Double[][]":
                double[][] dd = (double[][])objx;
                if (dd.Length < 3){
                    return "--";
                }
                foreach (double[] x in dd){
                    try { res += String.Format(" ({0:N2}, {1:N2})," , x[0], x[1]); }
                    catch (IndexOutOfRangeException){
                        Console.WriteLine("Error, format is wrong");
                        return "";
                    }
                }
                res = res.TrimEnd(',');
                res += " ]";
                return res;
            case "Double":
                double d = (double)objx;
                return (d < 0)? "--" : String.Format("{0:N2}", d);
            case "Int32":
                int i = (int)objx;
                return (i < 0)? "--" : i.ToString();
            default: break;
        }
        return objx.ToString();
    }
    
    // auto compute read-only arributes
    public string Requirement { get => _Requirement; }
    public double Area {
        get{
            if (Verified < 0){
                Console.WriteLine(String.Format(msgSet[6], Shape));
            }
            else if (Restriction[Shape][3] > Verified){
                // not enough data to compute
                Console.WriteLine(msgSet[10]);
                Console.WriteLine(Requirement);
            }
            else {
                GetAreaPerimeter();
            }
            return _Area;
        }
    }
    public double Perimeter {
        get{
            if (Verified < 0){
                Console.WriteLine(String.Format(msgSet[6], Shape));
            }
            else if (Restriction[Shape][3] > Verified){
                // not enough data to compute
                Console.WriteLine(msgSet[10]);
                Console.WriteLine(Requirement);
            }
            else {
                GetAreaPerimeter();
            }
            return _Perimeter;
        }
    }
    public string Info {
        get{
            object[] tmp = new object[]{
                Area, Perimeter, Radius, Edge, LenofSides, Diagonals, AngleofDiagonals, Vertexs};
            List<string> Info_content = new List<string>();
            for (int i = 0; i < tmp.Length; i++){
                if (show_Info[i]){
                    Info_content.Add(Print(tmp[i]));
                }
            }
            // transform to array for formatting
            return String.Format(_Info, Info_content.ToArray());
        } 
    }
    public override string ToString(){ return this.Info; }

    // other assistance program
    public void Reset(){
        // like the initialization part
        _Shape = msgSet[9];
        _LenofSides = new double[]{-1};
        _Diagonals = new double[]{-1};
        _AngleofDiagonals = -1;
        DegreeofDiagonals = -1;
        _Vertexs = new double[][]{ new double[]{-1, -1} };
        Verified = -1;
        _Requirement = msgSet[9];
        _Info = msgSet[9];
        _Area = -1;
        _Perimeter = -1;
        _Edge = -1;
        show_Info = new bool[]{
        // Area, Perimeter, Radius, Edge, LenofSides, Diagonals, AngleofDiagonals, Vertexs
        true, true, false, true, true, false, false, false
        }; 
        Console.WriteLine(String.Format(msgSet[0], "Reset"));
    }
    public int LeastCompleteShapetoFill(double width, double height){
        // for regular triangle, hexagon, square, rectangle, rhombus only
        double s = LenofSides[0];
        string res = "Total: {0}, rows: {1}, cols: {2}";

        int assist_cal(double sw, double sh, double initial_w, int num_of_single_row){
            // for calculate rectangle, rhombus, regular triangle
            // which directions matters, and simple to calculate (compare to hexagon)
            // each stands for total number, num of row/ col

            /* C# does not support co-variant array conversion from int[] to object[]
            therefore whole array is considered as object,
            hence Format(res, array[]) overload with a single parameter is called. */

            int[] comb1 = new int[3];
            int[] comb2 = new int[3];

            comb1[1] = (int)Math.Ceiling(height / sh);
            comb1[2] = (int)Math.Ceiling((width + initial_w) / sw);
            comb1[0] = comb1[1] * comb1[2] * num_of_single_row;
            
            // rotate
            comb2[2] = (int)Math.Ceiling(width / sh);
            comb2[1] = (int)Math.Ceiling((height + initial_w) / sw);
            comb2[0] = comb1[1] * comb1[2] * num_of_single_row;

            if (comb1[0] <= comb2[0]){
                Console.WriteLine(String.Format(res, comb1[0], comb1[1], comb1[2]));
                return comb1[0];
            }
            Console.WriteLine(String.Format(res, comb2[0], comb2[1], comb2[2]) +
                            " (rotate 90 degree)");
            return comb2[0];
        }
        switch (Shape){
            case "square":
                if (s < 0){
                    Console.WriteLine(String.Format(msgSet[6], Shape));
                }
                else {
                    int[] comb = new int[3];
                    comb[1] = (int)Math.Ceiling(height / s);
                    comb[2] = (int)Math.Ceiling(width / s);
                    comb[0] = comb[1] * comb[2];
                    Console.WriteLine(String.Format(res, comb[0], comb[1], comb[2]));
                    return comb[0];
                }
                break;
            case "rectangle":
                if (s < 0){
                    Console.WriteLine(String.Format(msgSet[6], Shape));
                }
                else{
                    return assist_cal(s, LenofSides[1], 0, 1);
                }
                break;
            case "rhombus":
                if (s < 0 || Diagonals[0] < 0){
                    Console.WriteLine(String.Format(msgSet[6], Shape));
                }
                else {
                    double shortd = Math.Min(Diagonals[0], Diagonals[1]);
                    double sh = 0.5 * Diagonals[0] * Diagonals[1] / s;
                    double fw = s - Math.Sqrt(shortd * shortd - sh * sh);
                    return assist_cal(s, sh, fw, 1);
                }
                break;
            case "triangle":
                if (s < 0 || ! (LenofSides[0] == LenofSides[1] && LenofSides[2] == LenofSides[1]) ){
                    Console.WriteLine(String.Format(msgSet[6], Shape));
                }
                else {
                    return assist_cal(s, s * 0.5 * Math.Sqrt(3), s * 0.5, 2);
                }
                break;
            case "regular_polygon":
               if (Edge != 6 || s < 0){
                    Console.WriteLine(String.Format(msgSet[6], Shape));
                }
                else {
                    // each stands for total number, num for odd/even-row, odd/even-cols
                    int[] comb1 = new int[5];
                    int[] comb2 = new int[5]; // rotate 90 degree
                    string test = "{0} {1} {2} {3} {4}";

                    res = "Total: {0}, odd-row * {1}, even-row * {2}, odd-col * {3}, even-col * {4}";
                    double sw = s * Math.Sqrt(3);
                    double sh = s * 3;
                    double evensh = s * 2;

                    comb1[1] = (int)Math.Ceiling( (height + s * 0.5) / sh); // odd_h
                    comb1[2] = (comb1[1] + evensh * (comb1[1] - 1) >= height)?
                                 comb1[1] - 1: comb1[1]; // even_h
                    comb1[3] = (int)Math.Ceiling(width / sw); // odd_w
                    comb1[4] = (int)Math.Ceiling((width + sw * 0.5) / sw); // even_w
                    comb1[0] = comb1[1] * comb1[3] + comb1[2] * comb1[4];

                    // rotate 90 degree, put sharp side horizontally
                    comb2[3] = (int)Math.Ceiling( (width + s * 0.5) / sh); // odd_w
                    comb2[4] = (comb2[3] + evensh * (comb2[3] - 1) >= width)?
                                 comb2[3] - 1: comb2[3]; // even_w
                    comb2[1] = (int)Math.Ceiling(height / sw); // odd_h
                    comb2[2] = (int)Math.Ceiling((height + sw * 0.5) / sw); // even_h
                    comb2[0] = comb2[1] * comb2[3] + comb2[2] * comb2[4];
                    
                    // for test
                    Console.WriteLine(String.Format(test, comb1[0], comb1[1], comb1[2], comb1[3], comb1[4]));
                    Console.WriteLine(String.Format(test, comb2[0], comb2[1], comb2[2], comb2[3], comb2[4]));

                    if (comb1[0] <= comb2[0]){
                        Console.WriteLine(
                            String.Format(res, comb1[0], comb1[1], comb1[2], comb1[3], comb1[4]));
                        return comb1[0];
                    }
                    Console.WriteLine(
                        String.Format(res, comb2[0], comb2[1], comb2[2], comb2[3], comb2[4]) +
                        " (rotate 90 degree, sharp side horizontally)");
                    return comb2[0];

                }
                break;
            default:
                Console.WriteLine(String.Format(msgSet[6], Shape));
                break;
        }
        return -1;
    }
}


// constructor with fixed shape
class Triangle: ShapeBuilder{
    public Triangle(params double[] len){ this.Shape = "triangle"; this.LenofSides = len; }
}
class Circle: ShapeBuilder{
    public Circle(double len){ this.Shape = "circle"; this.Radius = len; }
}
class Square: ShapeBuilder{
    public Square(double len){ this.Shape = "square"; this.LenofSides = new double[]{len}; }
}
class Rectangle: ShapeBuilder{
    public Rectangle(params double[] len){ this.Shape = "rectangle"; this.LenofSides = len; }
}
class Rhombus: ShapeBuilder{
    public Rhombus(double len, double[] dg){
        this.Shape = "rhombus";
        this.LenofSides = new double[]{len};
        this.Diagonals = dg;
    }
}
class Kite: ShapeBuilder{
    public Kite(double[] len, double[] dg){
        this.Shape = "kite";
        this.LenofSides = len;
        this.Diagonals = dg;
    }
}
class Parallelogram: ShapeBuilder{
    public Parallelogram(double[] len, double[] dg, double ag){
        this.Shape = "Parallelogram";
        this.LenofSides = len;
        this.Diagonals = dg;
        this.AngleofDiagonals = ag;
    }
}
class quadrilateral: ShapeBuilder{
    public quadrilateral(double[] len, double[] dg, double ag){
        this.Shape = "quadrilateral";
        this.LenofSides = len;
        this.Diagonals = dg;
        this.AngleofDiagonals = ag;
    }
}
class SetbyCoordinates: ShapeBuilder{
    public SetbyCoordinates(double[][] vt){ this.Shape = "set_by_coordinates"; this.Vertexs = vt; }
}
class RegularPolygon: ShapeBuilder{
    public RegularPolygon(int n, double len){
        this.Shape = "regular_polygon";
        this.Edge = n;
        this.LenofSides = new double[]{len};
    }
}

class Test {
    static void Main(){
        double[][] vt = new double[][]{
            new double[]{0,0},
            new double[]{1.2,2.5},
            new double[]{3.1,3.8},
            new double[]{1.2,3},
            new double[]{0.5,1.5}
        };
        var ex1 = new ShapeBuilder("set_by_coordinates");
        ex1.Vertexs = vt;
        Console.WriteLine(ex1);
        /*-------------------
        ===== Info of set_by_coordinates =====
        Area: 4.12
        Perimeter: 22.48
        Edges: 5
        Length: [ 2.50, 7.69, 5.30, 4.25, 2.74 ]
        Vertexs: [ (0.00, 0.00), (1.20, 2.50), (3.10, 3.80), (1.20, 3.00), (0.50, 1.50) ]
        -------------------*/

        var ex2 = RegularPolygon(6, 1); // hexagon
        int n = ex2.LeastCompleteShapetoFill(6, 4);
        // output: Total: 12, odd-row * 2, even-row * 1, odd-col * 4, even-col * 4
        Console.WriteLine(n); // 12
    }
}
