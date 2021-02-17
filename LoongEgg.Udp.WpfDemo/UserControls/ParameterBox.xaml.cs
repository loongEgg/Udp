using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LoongEgg.Udp.WpfDemo
{
    /// <summary>
    /// ParameterBox.xaml 的交互逻辑
    /// </summary>
    public partial class ParameterBox : UserControl
    {
        public static ParameterBox DesignInstance { get; } = new ParameterBox();

        public ParameterBox()
        {
            InitializeComponent();
        }
         
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(
                nameof(Label), typeof(string), typeof(ParameterBox),
                new PropertyMetadata("[Label]"));


        public double LabelWidth
        {
            get { return (double)GetValue(LabelWidthProperty); }
            set { SetValue(LabelWidthProperty, value); }
        }
        public static readonly DependencyProperty LabelWidthProperty =
            DependencyProperty.Register(
                nameof(LabelWidth), typeof(double), typeof(ParameterBox),
                new PropertyMetadata(90d));


        public object Value
        {
            get { return (object)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                nameof(Value), typeof(object), typeof(ParameterBox),
                new PropertyMetadata("[Value]"));

        public bool Editable
        {
            get { return (bool)GetValue(EditableProperty); }
            set { SetValue(EditableProperty, value); }
        }
        public static readonly DependencyProperty EditableProperty =
            DependencyProperty.Register(
                nameof(Editable), typeof(bool), typeof(ParameterBox),
                new PropertyMetadata(true));
         
        public string Unit
        {
            get { return (string)GetValue(UnitProperty); }
            set { SetValue(UnitProperty, value); }
        }
        public static readonly DependencyProperty UnitProperty =
            DependencyProperty.Register(
                nameof(Unit), typeof(string), typeof(ParameterBox),
                new PropertyMetadata(""));
         
        public double UnitWidth
        {
            get { return (double)GetValue(UnitWidthProperty); }
            set { SetValue(UnitWidthProperty, value); }
        }
        public static readonly DependencyProperty UnitWidthProperty =
            DependencyProperty.Register(
                nameof(UnitWidth), typeof(double), typeof(ParameterBox),
                new PropertyMetadata(0d));

    }
}
