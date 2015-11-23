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

namespace RestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private byte[,] place = new byte[10,10];


        public MainWindow()
        {
            InitializeComponent();
            bool first = true;
            for (int i = 0; i < place.GetLength(0); i++)
            {
                GridPlace.ColumnDefinitions.Add(new ColumnDefinition());
                for (int j = 0; j < place.GetLength(1); j++)
                {
                    if(first) GridPlace.RowDefinitions.Add(new RowDefinition());
                    var border = new Border();
                    border.SetValue(Grid.RowProperty, i);
                    border.SetValue(Grid.ColumnProperty, j);
                    border.MouseLeftButtonUp += ChangeToSeat;
                    border.MouseRightButtonUp += ChangeToTable;
                    border.Style = GridPlace.FindResource("Empty") as Style;
                    GridPlace.Children.Add(border);
                }
                first = false;
            }
        }


        private void ChangeToSeat(object sender, RoutedEventArgs e)
        {
            var border = sender as Border;
            if (border == null) return;

            int row = (int)border.GetValue(Grid.RowProperty);
            int column = (int)border.GetValue(Grid.ColumnProperty);

            if (place[row, column] != 1)
            {
                border.Style = GridPlace.FindResource("Seat") as Style;
                place[row, column] = 1;
            }
            else
            {
                border.Style = GridPlace.FindResource("Empty") as Style;
                place[row, column] = 0;
            }

        }

        private void ChangeToTable(object sender, MouseButtonEventArgs e)
        {
            var border = sender as Border;
            if (border == null) return;

            int row = (int)border.GetValue(Grid.RowProperty);
            int column = (int)border.GetValue(Grid.ColumnProperty);

            if (place[row, column] != 2)
            {
                border.Style = GridPlace.FindResource("Table") as Style;
                place[row, column] = 2;
            }
            else
            {
                border.Style = GridPlace.FindResource("Empty") as Style;
                place[row, column] = 0;
            }
            

        }
    }
}