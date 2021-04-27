using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reklama5_Scrapper
{
    public partial class Form1 : Form
    {
        static string titleS;
        static string cenaS;

        static int s = 0;
        static int end = 0;

        string model;
        string godinaOd;
        string godinaDo;
        string cenaOd;
        string cenaDo;
        string kmOd;
        string kmDo;
        static IWebDriver Driver; //chrome driver
        Thread Car;

        static object lockObject = new object();  //Read data form a thread with timer
        static Boolean stopLock = false;







        public Form1()
        {
            InitializeComponent();

            dataGridView1.Columns.Add("ID", "ID");
            dataGridView1.Columns.Add("Оглас", "Оглас");
            dataGridView1.Columns.Add("Цена", "Цена");





        }



        public void CarScrap()
        {
            IWebElement element;
            Driver = new ChromeDriver();
            Driver.Navigate().GoToUrl("https://reklama5.mk/Search?q=&city=&sell=0&sell=1&buy=0&buy=1&trade=0&trade=1&includeOld=0&includeOld=1&includeNew=0&includeNew=1&f31=&priceFrom=9000&priceTo=12000&f33_from=&f33_to=&f36_from=&f36_to=&f35=&f37=&f138=&f10016_from=&f10016_to=&private=0&company=0&page=1&SortByPrice=1&zz=1&cat=24");

            if(godinaOd!=null)
                (new OpenQA.Selenium.Support.UI.SelectElement(Driver.FindElement(By.Id("f33_from")))).SelectByValue(godinaOd);
            try
            {


                (new OpenQA.Selenium.Support.UI.SelectElement(Driver.FindElement(By.Id("f33_to")))).SelectByText(godinaDo);
            }catch (Exception e) { }

            try
            {
                (new OpenQA.Selenium.Support.UI.SelectElement(Driver.FindElement(By.Id("f31")))).SelectByText(model);
            }
            catch (Exception e) { }

            if(cenaOd!=null)
                (new OpenQA.Selenium.Support.UI.SelectElement(Driver.FindElement(By.Id("priceFrom")))).SelectByValue(cenaOd);

            if (cenaDo != null)
                (new OpenQA.Selenium.Support.UI.SelectElement(Driver.FindElement(By.Id("priceTo")))).SelectByValue(cenaDo);

            if (kmOd != null)
                (new OpenQA.Selenium.Support.UI.SelectElement(Driver.FindElement(By.Id("f36_from")))).SelectByValue(kmOd);


            if (kmDo != null)
                (new OpenQA.Selenium.Support.UI.SelectElement(Driver.FindElement(By.Id("f36_to")))).SelectByValue(kmDo);

            element = Driver.FindElement(By.CssSelector("input.btn.btn-xs.btn-primary"));
            element.Click();

            IList<IWebElement> test = Driver.FindElements(By.ClassName("OglasResults"));
            
            IWebElement temp;

            Thread.Sleep(2000);



            int pageLock = 0;
            int endLock = 0;
            try
            {
                while ((pageLock != 1)||(endLock != 1))
                {
                    string[] strana = null;
                    try
                    {
                        temp = Driver.FindElement(By.ClassName("number-of-pages"));

                         strana = temp.Text.Split(' ');
                    }
                    catch (Exception e) {
                        pageLock = 1;
                    }
                    string title;
                    string cena;
                    test = Driver.FindElements(By.ClassName("OglasResults"));
                    foreach (IWebElement element1 in test)
                    {
                        //Console.WriteLine(element.Text);
                        temp = element1.FindElement(By.ClassName("SearchAdTitle"));
                        //Console.WriteLine(element.GetAttribute()));
                        title = temp.Text;
                        temp = element1.FindElement(By.CssSelector("div.text-left.text-success"));
                        cena = temp.Text;


                        this.Invoke((MethodInvoker)delegate
                        {
                            dataGridView1.Rows.Add(s++, title, cena);

                        });


                        lock (lockObject)
                        {
                            // That can be any code that calculates text variable,
                            // I'm using DateTime for demonstration:
                            titleS = title;
                            cenaS = cena;
                        }

                    }
                    Thread.Sleep(500);
                    //
                    //next = Driver.FindElement(By.XPath("//*/fieldset[@class="openable"]"));
                    //next = Driver.FindElement(By.CssSelector(".icon fa-chevron-right fa-fw[aria-hidden='true']"));
                    // next = Driver.FindElement(By.CssSelector("li.arrow.next"));
                    // next.Click();

                    if (strana[1].Equals(strana[3]) == false)
                    {

                        // MessageBox.Show(strana[0]+strana[1]+strana[2]+strana[3]);
                        temp = Driver.FindElement(By.ClassName("prev-nextPage"));
                        temp.Click();
                        Thread.Sleep(5000);
                    }
                    else {
                        endLock = 1;
                        MessageBox.Show("Завршено");

                        break;
                    }



                }
            }
            catch (Exception e) { MessageBox.Show("Завршено"); }




        }

        private void button1_Click(object sender, EventArgs e)
        {
            stopLock = true;

            model = comboBox1.Text;

            godinaOd = comboBox2.Text;
            godinaDo = comboBox3.Text;

            cenaOd = comboBox5.Text;
            cenaDo = comboBox4.Text;

            kmOd = comboBox7.Text;
            kmDo = comboBox6.Text;
            
            Car = new Thread(CarScrap);
            Car.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            {
                dataGridView1.Rows.Add('1', '2', '4');
            }

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
