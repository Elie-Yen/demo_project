/*-------------------
Elie Yen
C#
Area calculator
--------------------*/
class ShapeBuilder{
    public string Shape { get; set; } // can't change after setting
    public double[] LenofSides { get; set; }
    public double[] Diagonals { get; set; }
    public double DegreeofDiagonals { get; set; }
    public double[ , ] Verticles { get; set; }

    // some auto computed properties 
    public double Perimeter { get{return GetAreaPerimeter()[1];} set{} }
    public double Area { get{return GetAreaPerimeter()[0];} set{} }
    public StringBuilder Requirement { get; set; } // requirement of this shape
    public String Info { get{return GetInfo();} set{} } // info of current status

    private Dictionary<string, int[]> Restriction { get; set; } // least sides/diagonals/attributes to have
    private int Num_Attributes { get; set; } // check how many attributes the shape has
    private string[] msgSet { get; set; } // some msg about setting
    
    public ShapeBuilder(string sp){
        // initialize // rhombus 菱形
        Restriction = new Dictionary<string, int[]> {
            ["quadrilateral"] = new int[] {4, 2, 3}, ["kite"] = new int[] {2, 2, 2},
            ["parallelogram"] = new int[] {2, 2, 3}, ["rhombus"] = new int[] {1, 2, 2},
            ["square"] = new int[] {1, 0, 1}, ["rectangle"] = new int[] {2, 0, 1},
            ["set_by_coordinates"] = new int[] {3, 0, 1}, ["triangle"] = new int[] {3, 0, 1},
            ["circle"] = new int[] {1, 0, 1}, ["regular_polygon"] = new int[] {5, 0, 1}
            };
        msgSet = new string[]{
            "\n(V) [ {0} ] set successfully!",
            "\nUse [.SetLenofSides(value)] to set Length of {0} Sides (value > 0)",
            "\nUse [.SetVerticles(value)] to set at least 3 coordinates of Verticles",
            "\nUse [.SetDiagonals(value)] to set 2 Diagonals (value > 0)", 
            "\nUse [.SetDegreeofDiagonals(value)] to set 1 Degree between 2 diagonals (0 < value < 180)",
            "[!] The shape is not support this attribute",
            "[!] {0} is not supported! try another!",
            "[!] Invalid value, check again!\n{0}",
            "calculation error (incorrect/ insufficient attributes)"
            };
        Requirement = new StringBuilder("");
        LenofSides = new double[]{-1};
        Verticles = new double[,]{ {-1, -1} };
        Diagonals = new double[]{-1};
        DegreeofDiagonals = -1;

        if (! Restriction.ContainsKey(sp.ToLower())){
            Num_Attributes = -1000;
            Shape = sp;
            Console.WriteLine(String.Format(msgSet[6], Shape));
        }
        else {
            Num_Attributes = 0;
            Shape = sp.ToLower();
            Console.WriteLine(String.Format(msgSet[0], Shape));
            Console.WriteLine("In order to calculate, please set following attribute(s)!");
            if (Shape == "regular_polygon"){
                msgSet[1] = "\nUse [.SetLenofSides(value)] to set {0}+ edges and length of side (value > 0)";
                Requirement.Append(String.Format(msgSet[1], Restriction[Shape][0]));
            }
            else if (Shape == "circle"){
                msgSet[1] = "\nUse [.SetLenofSides(value)] to set {0} radius (value > 0)";
                Requirement.Append(String.Format(msgSet[1], Restriction[Shape][0]));
            }
            else if (Shape == "set_by_coordinates"){
                Requirement.Append(msgSet[2]);
            } 
            // for squares and any quadrilaterals
            else {
                Requirement.Append(String.Format(msgSet[1], Restriction[Shape][0])); 
                if (Restriction[Shape][1] == 2){
                    Requirement.Append(msgSet[3]);
                    Diagonals = new double[]{-1, -1};
                    if (Shape == "quadrilateral" || Shape == "parallelogram"){
                        Requirement.Append(msgSet[4]);
                    }
                }
                LenofSides = new double[]{-1, -1, -1, -1};
            }
            Console.WriteLine(Requirement);
        }
    }

    public void SetLenofSides(params double[] bd){
        string res = String.Format(msgSet[0], "Len");
        if (! Restriction.ContainsKey(Shape) || Shape == "set_by_coordinates"){
            res = msgSet[5];
        }
        else if (Shape == "regular_polygon" && (bd.Length != 2 || bd[0] < 5 || bd[0] > (int)bd[0]) || 
            (Shape != "regular_polygon" && bd.Length != Restriction[Shape][0]) ||
            ! Array.TrueForAll(bd, x => x > 0) ){
            res = String.Format(msgSet[7], String.Format(msgSet[1], Restriction[Shape][0]));
        }
        else if (Shape == "regular_polygon"){
            LenofSides = new double[(int)bd[0]];
            Array.Fill(LenofSides, bd[1]);
        }
        else {
            Num_Attributes ++;
            if (Restriction[Shape][0] > 2 || Shape == "circle"){
                LenofSides = bd;
            }
            else{
                double[] tmp = new double[4];
                for (int i = 0; i < 4; i++){
                    tmp[i] = (i < bd.Length) ? bd[i]: bd[i % bd.Length];
                }
                // quadrilateral check if valid by Euler's quadrilateral theorem 
                // a^2 + b^2 + c^2 + d^2 >= p^2 + q^2
                double p = (Restriction[Shape][1] == 2 && Diagonals[0] > 0 && Diagonals[1] > 0) ?
                    (tmp[0] * tmp[0] + tmp[1] * tmp[1] + tmp[2] * tmp[2] + tmp[3] * tmp[3] -
                    (Diagonals[0] * Diagonals[0] + Diagonals[1] + Diagonals[1])) : 0;
                if (p < 0){
                    Num_Attributes --;
                    res = String.Format(msgSet[7], String.Format(msgSet[1], Restriction[Shape][0]));
                }
                else {
                    LenofSides = tmp;
                }
            }
        }
        Console.WriteLine(res);
    }
    
    public void SetVerticles(double[ , ] vt){
        if (Shape != "set_by_coordinates"){
            Console.WriteLine(msgSet[5]);
        }
        // multi dimensional array need to test len of dimension
        else if (vt.GetUpperBound(0) + 1 < 3 || vt.GetUpperBound(1) + 1 != 2){
            Console.WriteLine(String.Format(msgSet[7], msgSet[2]));
        }
        else {
            Num_Attributes ++;
            Verticles = vt;
            Console.WriteLine(String.Format(msgSet[0], "Verticles"));
        }
    }

    public void SetDiagonals(params double[] dias){
        if (Num_Attributes < 0 || Restriction[Shape][1] == 0){
            Console.WriteLine(msgSet[5]);
        }
        else if (dias.Length != 2 || !Array.TrueForAll(dias, x => x > 0)){
            Console.WriteLine(String.Format(msgSet[7], msgSet[3]));
        }
        else {
            Num_Attributes ++;
            string res = String.Format(msgSet[0], "diagonals");
            // quadrilateral check if valid by Euler's quadrilateral theorem 
            // a^2 + b^2 + c^2 + d^2 >= p^2 + q^2
            double a = LenofSides[0];
            double b = LenofSides[1];
            double c = LenofSides[2];
            double d = LenofSides[3];
            double p = a * a + b * b + c * c + d * d - dias[0] * dias[0] + dias[1] + dias[1];
            if (Array.TrueForAll(LenofSides, x => x > 0) && p < 0){
                Num_Attributes --;
                res = String.Format(msgSet[7], msgSet[3]);
            }
            else {
                Diagonals = dias;
            }
            Console.WriteLine(res);
        }
    }

    public void SetDegreeofDiagonals(double ag){
        if (Num_Attributes < 0 || Restriction[Shape][1] == 0){
            Console.WriteLine(msgSet[5]);
        }
        else if (! (ag > 0 && ag < 180) ){
            Console.WriteLine(String.Format(msgSet[7], msgSet[4]));
        }
        else {
            Num_Attributes ++;
            DegreeofDiagonals = (ag * Math.PI / 180);
            Console.WriteLine(String.Format(msgSet[0], "Degree"));
        }
    }

    private double[] GetAreaPerimeter(){
        double[] res = new double[2]{-1, -1}; // Area & Perimeter
        if (Num_Attributes < 0){
            Console.WriteLine(String.Format(msgSet[6], Shape));
        }
        else if (Restriction[Shape][2] > Num_Attributes){
            Console.WriteLine(msgSet[8]);
            Console.WriteLine(Requirement);
        }
        else if (Shape == "triangle"){
            res[1] = LenofSides[0] + LenofSides[1] + LenofSides[2];
            // Heron's formula
            double s = Perimeter / 2;
            res[0] = Math.Pow(s * (s - LenofSides[0]) * 
                (s - LenofSides[1]) * (s - LenofSides[2]), 0.5);
        }
        else if (Shape == "set_by_coordinates"){
            int n = Verticles.GetUpperBound(0);
            LenofSides = new double[n + 1];
            res[0] = (0.5 * Math.Abs(Verticles[0, 0] * Verticles[n, 1] 
                                    - Verticles[0, 1] * Verticles[n, 0]));
            LenofSides[0] = Math.Pow(Verticles[0, 0] - Verticles[n, 0], 2) + 
                    Math.Pow(Verticles[0, 1] - Verticles[n, 1], 2);
            res[1] = LenofSides[0];
            
            for (int i = 0; i < n; i++ ){
                res[0] += 0.5 * Math.Abs(Verticles[i, 0] * Verticles[i + 1, 1] - 
                                        Verticles[i, 1] * Verticles[i + 1, 0]);
                LenofSides[i + 1] = Math.Pow(Verticles[i, 0] - Verticles[i + 1, 0], 2) +
                                     Math.Pow(Verticles[i, 1] - Verticles[i + 1, 1], 2);
                res[1] += LenofSides[i + 1];
            }
        }
        else if (Shape == "circle"){ 
            res[0] = LenofSides[0] * LenofSides[0] * Math.PI;
            res[1] = LenofSides[0] * 2 * Math.PI;
        }
        else if (Shape == "regular_polygon"){
            int n = LenofSides.Length;
            double s = LenofSides[0];
            res[1] = n * s;
            res[0] = 0.25 * n * s / Math.Tan(Math.PI / n);
        }
        else{
            if ((Restriction[Shape][0] == 1 || Shape == "kite")){
                    DegreeofDiagonals = 0.5 * Math.PI;
            }
            if (Shape == "square"){
                double d = LenofSides[0] * Math.Sqrt(2);
                Diagonals = new double[]{d, d};
            }
            else if (Shape == "rectangle"){
                double d = Math.Sqrt(LenofSides[0] * + LenofSides[0] + LenofSides[1] * LenofSides[1]);
                Diagonals = new double[]{d, d};
                DegreeofDiagonals = Math.PI - 2 * Math.Asin(LenofSides[0] / d);
            }
            res[0] = (Restriction[Shape][1] == 2) ? 
                    0.5 * Math.Sin(DegreeofDiagonals) * Diagonals[0] * Diagonals[1] 
                    : LenofSides[0] * LenofSides[1];
            res[1] = LenofSides[0] + LenofSides[1] + LenofSides[2] + LenofSides[3];
        }
        return res;
    }

    private string GetInfo(){
        string empty_attr = "not set yet";
        string _Area = (Area > 0)? Area.ToString(): msgSet[8];
        string _Perimeter = (Perimeter > 0)? Perimeter.ToString(): msgSet[8];
        string len = (LenofSides[0] > 0)? Print1DArray(LenofSides) : empty_attr;
        string n = (LenofSides.Length != 1)? LenofSides.Length.ToString() : empty_attr;
        StringBuilder res = 
            new StringBuilder($"===== Info of {Shape} =====\nArea: {_Area}\nPerimeter: {_Perimeter}");
        string common = (Shape == "circle") ? $"\nLen of radius: {len}"
                        : $"\nNum of edges: {n}\nLen of each side: {len}";
        res.Append(common);
        if (Num_Attributes < 0){
            res = new StringBuilder(String.Format(msgSet[6], Shape));
        }
        else if (Shape == "set_by_coordinates"){
            string vt = (Verticles.Length > 4) ? Print2DArray(Verticles) : empty_attr;
            res.Append($"\nCoordinate: {vt}");
        }
        else if (LenofSides.Length == 4){
            string d = (Diagonals.Length == 2) ? Print1DArray(Diagonals) : empty_attr;
            string angle = (DegreeofDiagonals > 0) ?
                            (180 * DegreeofDiagonals / Math.PI).ToString() : empty_attr;
            res.Append($"\nLen of Diagonals: {d}\nAngle of Diagonals: {angle}");
        }
        return res.ToString();
    }

    private string Print1DArray(double[] arr){
        StringBuilder res = new StringBuilder($"[{arr[0]}");
        for (int i = 1; i < arr.Length; i++){
            res.Append($", {arr[i]}");
        }
        res.Append(']');
        return res.ToString();
    }

    private string Print2DArray(double[ , ] arr){
        StringBuilder res = new StringBuilder($"[[{arr[0, 0]}, {arr[0, 1]}]");
        for (int i = 1; i < arr.GetUpperBound(0) + 1; i++){
            res.Append($", [{arr[i, 0]}, {arr[i, 1]}]");
        }
        res.Append(']');
        return res.ToString();
    }

}

class TestShapeBuilder{
    static void Main() {
        double[,] vt = new double[,]{
            {0,0},
            {1.2,2.5},
            {3.1,3.8},
            {1.2,3},
            {0.5,1.5}
        };
        var a1 = new ShapeBuilder("set_by_coordinates");
        a1.SetVerticles(vt);
        Console.WriteLine(a1.Info);
        Console.WriteLine("---");
        var b1 = new ShapeBuilder("rhombus");
        b1.SetLenofSides(10);
        Console.WriteLine(b1.Area);
        b1.SetDiagonals(12, 16);
        Console.WriteLine(b1.Area);
        b1.SetLenofSides(5);
        Console.WriteLine(b1.Info);
        
    }
}