using System;
using System.Windows.Forms;
using Modbus.IntegrationTests;

namespace Test_Compact_Framework
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();

			try
			{
				//textBox1.Text = "Testing serial...";
				//TestCases.Serial();
				textBox1.Text = "Testing TCP...";
				TestCases.Tcp();
				textBox1.Text = "Testing UDP...";
				TestCases.Udp();

				textBox1.Text = "Tests completed successfully.";
			}
			catch (Exception e)
			{
				textBox1.Text = e.ToString();
			}
		}
	}
}