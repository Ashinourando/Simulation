using System;
using System.Numerics;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

class Object{

  public float Math;
  public Vector2 Velocity;
  public Vector2 Location;

  public Object(float m, Vector2 v, Vector2 l){
    this.Math = m;
    this.Velocity = v;
    this.Location = l;
  }

  public void Move(float time, Vector2 acceleration){

    Location = Vector2.Add(Location, Vector2.Add(Vector2.Multiply(Velocity, time), Vector2.Multiply(acceleration, time * time / 2)));
    Velocity = Vector2.Add(Velocity, Vector2.Multiply(acceleration, time));
  }
}

class Simulation : Form{

  public List<Object> ObjectList = new List<Object>();
  Timer t = new Timer();
  public SolidBrush Green;
  float MATH_EARTH;
  float MATH_MOON;
  float MATH_SUN;
  float MATH_VENUS;
  float MATH_MARS;
  float MATH_MERCURY;
  float VERTICAL_CORRECTION;
  float HORIZONTAL_CORRECTION;
  float SCALE;
  float GRAVITY;

  public Vector2 Acceleration(Object o1, Object o2){

    return Vector2.Multiply(Vector2.Add(o2.Location, Vector2.Negate(o1.Location)), GRAVITY * o2.Math * (float)Math.Pow(Vector2.Distance(o1.Location, o2.Location), -3));
  }

  public void TimerEvent(object Sender, EventArgs e){
    this.Invalidate();
    for(int i = 0; i < this.ObjectList.Count; i++){
      for(int j = 0; j < this.ObjectList.Count; j++){
        if(j != i){
          this.ObjectList[i].Move((float)0.01, Acceleration(ObjectList[i], ObjectList[j]));
        }
      }
    }
  }

  protected override void OnLoad(EventArgs e){
    this.DoubleBuffered = true;
    this.Green = new SolidBrush(Color.FromArgb(90, 127, 11));
    this.VERTICAL_CORRECTION = 400;
    this.HORIZONTAL_CORRECTION = 640;
    this.SCALE = (float)Math.Pow(10, -8);
    this.MATH_EARTH = (float)(5.972 * Math.Pow(10, 24));
    this.MATH_MOON = (float)(7.34 * Math.Pow(10, 22));
    this.MATH_SUN = (float)(1.989 * Math.Pow(10, 30));
    this.MATH_VENUS = (float)(4.869 * Math.Pow(10, 24));
    this.MATH_MARS = (float)(6.419 * Math.Pow(10, 23));
    this.MATH_MERCURY = (float)(3.302 * Math.Pow(10, 23));
    this.GRAVITY = (float)(6.6743 * Math.Pow(10, -11));
    t.Interval = 10;
    t.Tick += new EventHandler(TimerEvent);
    this.ObjectList.Add(new Object(MATH_SUN, new Vector2(0, 0), new Vector2(0, 0)));
    this.ObjectList.Add(new Object(MATH_MERCURY, new Vector2(0, -46000), new Vector2(57900000000, 0)));
    this.ObjectList.Add(new Object(MATH_VENUS, new Vector2(0, -35040), new Vector2(108160000000, 0)));
    this.ObjectList.Add(new Object(MATH_EARTH, new Vector2(0, -29790), new Vector2(149590000000, 0)));
    this.ObjectList.Add(new Object(MATH_MARS, new Vector2(0, -24130), new Vector2(227946000000, 0)));
    t.Start();
  }

  protected override void OnPaint(PaintEventArgs e){
    for(int i = 0; i < this.ObjectList.Count; i++){
      e.Graphics.FillRectangle(Brushes.Black, HORIZONTAL_CORRECTION + this.ObjectList[i].Location.X * SCALE, VERTICAL_CORRECTION + this.ObjectList[i].Location.Y * SCALE, 10, 10);
    }
  }
}

class Program{

  static void Main(){

    Application.Run(new Simulation());
  }
}

//c:\windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe /r:System.Numerics.dll C:\Users\tomom\OneDrive\ドキュメント\c#\Simulation\Simulation.cs
