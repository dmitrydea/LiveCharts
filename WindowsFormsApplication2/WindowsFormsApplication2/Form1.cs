using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;

namespace WindowsFormsApplication2
{
   
    public partial class Form1 : Form
    {
        private double _axisMax;
        private double _axisMin;
        List<string> lableX = new List<string>();

        public Form1()
        {
            InitializeComponent();
            var mapper = Mappers.Xy<MeasureModel>()   //use DateTime.Ticks as X
               .Y(model => model.Value);           //use the value property as Y

            //lets save the mapper globally.
            Charting.For<MeasureModel>(mapper);

            //the ChartValues property will store our values array
            ChartValues = new ChartValues<MeasureModel>();
            //cartesianChart1.DisableAnimations = true;
            cartesianChart1.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Values = ChartValues,
                    PointGeometrySize = 18,
                    LineSmoothness = 0
                   
                }
            };
            cartesianChart1.AxisX.Add(new Axis
            {
                DisableAnimations = true,
                Labels = new string[] {"1","2","3","4","5","6","7","8","9"},
                Separator = new Separator
                {
                    Step = 1
                }
            });

            //SetAxisLimits(System.DateTime.Now);

            //The next code simulates data changes every 500 ms
            Timer = new Timer
            {
                Interval = 1000
            };
            Timer.Tick += TimerOnTick;
            R = new Random();
            Timer.Start();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public ChartValues<MeasureModel> ChartValues { get; set; }
        public Timer Timer { get; set; }
        public Random R { get; set; }

        private void SetAxisLimits(System.DateTime now)
        {        
            cartesianChart1.AxisX[0].MaxValue = now.Ticks + TimeSpan.FromSeconds(1).Ticks; // lets force the axis to be 100ms ahead
            cartesianChart1.AxisX[0].MinValue = now.Ticks - TimeSpan.FromSeconds(8).Ticks; //we only care about the last 8 seconds
        }

        private void TimerOnTick(object sender, EventArgs eventArgs)
        {
            var now = System.DateTime.Now;

            ChartValues.Add(new MeasureModel
            {
                Value = R.Next(0, 10)
            });

            lableX.Add(now.ToShortTimeString()+":"+now.Second);
            cartesianChart1.AxisX[0].Labels = lableX;

            //MessageBox.Show(TimeSpan.FromSeconds(1).Ticks.ToString(),"");
            //SetAxisLimits(now);
            cartesianChart1.Refresh();
            //lets only use the last 30 values
            if (ChartValues.Count > 10)
            {
                ChartValues.RemoveAt(0);
                lableX.RemoveAt(0);
            }
        }
    }

    public class MeasureModel
    {
        public double Value { get; set; }
    } 

}

