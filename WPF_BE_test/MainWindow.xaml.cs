using System;
using System.Collections.Generic;
using System.Data.Entity;
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

namespace WPF_BE_test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Context context;

        public MainWindow()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledExceptionHandler);

            context = new Context();
            InitializeComponent();

            context.NotSorted.Load();
            context.Sorted.Load();

            Grid1.ItemsSource = context.NotSorted.Local.ToBindingList();
            Grid2.ItemsSource = context.Sorted.Local.ToBindingList();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            //Clear tables
            ClearSet(context.NotSorted);
            ClearSet(context.Sorted);


            GetRandomNmbs(20).ForEach(n =>
            {
                context.NotSorted.Add(new NotSortedItem { Number = n });
                //Save after each change to preserve add order
                context.SaveChanges();
            });
        }

        private void SortAndCopy_Click(object sender, RoutedEventArgs e)
        {
            //Clear destination table
            ClearSet(context.Sorted);

            //Loop through sorted entities
            foreach (NotSortedItem nsi in context.NotSorted.ToList().OrderBy(i => i.Number))
            {
                context.Sorted.Add(new SortedItem() { Number = nsi.Number });
                //Save after each change to preserve add order
                context.SaveChanges();
            }
        }

        private void ClearSet<T>(DbSet<T> set) where T : BaseItem
        {
            //Bad idea to clear table that way, but for our purposes.... :-)
            set.RemoveRange(set.AsEnumerable());
            context.SaveChanges();
        }


        //Getting random number sequence
        private List<int> GetRandomNmbs(int count)
        {
            List<int> rslt = new List<int>();
            Random r = new Random();
            int nmb;
            do
            {
                nmb = r.Next(1, count + 1);
                if (!rslt.Contains(nmb))
                    rslt.Add(nmb);
            } while (rslt.Count < count);
            return rslt;
        }

        static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            MessageBox.Show(e.Message, "Opps something goes wrong...", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }
}
