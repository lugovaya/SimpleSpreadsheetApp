using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace SimpleSpreadsheetApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ValidateCellContent(object sender, DevExpress.XtraSpreadsheet.SpreadsheetCellValidatingEventArgs e)
        {
            var nonNegativeNumbers = @"\d+"; // <non-negative number>
            var cellReference = @"\w\d"; // <cell reference > = <letter> <digit> 
            var operation = @"(\+|\-|\*|/)"; // <operation> = ‘+’ | ‘-‘ | ‘*’ | ‘/’             
            var term = string.Format("({0}|{1})", nonNegativeNumbers, cellReference); // <term> = <cell reference> | <non-negative number> 

            //  Cell may contain: 
            // 1. Nothing 
            var nothingMask = "(^$)";

            // 2. Non-negative integer numbers
            var nonNegativeNumbersMask = @"(^\d+$)";

            // 3. Text labels, which is started with ' symbol
            var textMask = @"(^'\w+$)";

            // 4. Expression, which is started with '=' symbol and may contain non-negative integers, cell references, and simple arithmetic operations.  
            // <expression> = ‘=’ <term> { <operation> <term> } 
            var exprassionMask = "(^=" + term + "(" + operation + term + ")*$)";

            var summaryPattern = string.Format("{0}|{1}|{2}|{3}", nothingMask, nonNegativeNumbersMask, textMask, exprassionMask);

            var content = e.EditorText;

            var match = Regex.Match(content, summaryPattern, RegexOptions.IgnoreCase);

            if (!match.Success)
            {
                MessageBox.Show("Invalid input. See input requirements.");
                e.Cell.FillColor = System.Drawing.Color.Red;
            }
            else
            {
                e.Cell.FillColor = System.Drawing.Color.Transparent;
            }
        }
    }
}
