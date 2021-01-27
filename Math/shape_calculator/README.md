### shape calculator(C#)

* Purpose:   
Support build common shapes : quadrilateral, parallelogram, rhombus, kite, square, rectangle, triangle, circle, regular polygon and set by coordinates.
With required and correct arguments, this program could calculate unknown area, perimeter and other infos for other purpose.
The *LeastCompleteShapetoFill* method could calculates the least number of certain shapes to fill an area considering the direction.


* Concepts:   
Use dictionary to record different restriction of arguments and properties for each shape.  A further validation by Euler's quadrilateral theorem will be implemented if it's a quadrilateral.
The accessibility of properties, which's achieved by get/ set accessor,  blocks modification from user to ensure the correctness of result. The switch statements in each methods make the code more readable and eliminating the need to test another condition.

* Preview:   
![demo of shape calculaor 1](https://github.com/Elie-Yen/demo_project/blob/master/Math/shape_calculator/demo_shapebuilder.png?raw=true)

```
>> var ex2 = RegularPolygon(6, 1); // hexagon
>> int n = ex2.LeastCompleteShapetoFill(6, 4);
Total: 12, odd-row * 2, even-row * 1, odd-col * 4, even-col * 4

>> Console.WriteLine(n);
12
```

![demo of shape calculaor 2](https://github.com/Elie-Yen/demo_project/blob/master/Math/shape_calculator/demo_shape_calculator_2.png?raw=true)

```
>> var triangle = new Triangle(5, 6, 7);
>> var kite = new Kite(
                new double[]{2 * Math.Sqrt(2), Math.Sqrt(13)}, // sides
                new double[]{4, 5}); // diagonals
>> var rectangle = new Rectangle(4, 6);
>> Console.WriteLine(triangle);

===== Info of triangle =====
Area: 14.70
Perimeter: 18.00
Edges: 3
Length: [ 5.00, 6.00, 7.00 ]

>> Console.WriteLine(kite);

===== Info of kite =====
Area: 10.00
Perimeter: 12.87
Edges: 4
Length: [ 2.83, 3.61, 2.83, 3.61 ]
Diagonals: [ 4.00, 5.00 ]
Angle of diagonals: 90.00

>> Console.WriteLine(rectangle);

===== Info of rectangle =====
Area: 24.00
Perimeter: 20.00
Edges: 4
Length: [ 4.00, 6.00, 4.00, 6.00 ]
Diagonals: [ 7.21, 7.21 ]
Angle of diagonals: 112.62

```
