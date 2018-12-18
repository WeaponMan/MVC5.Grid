using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ConsoleApp1
{

    class Test
    {
       
        public string Ahoj { get; set; }

        public bool? test2;
    }

    class Program
    {
        

        static void Main(string[] args)
        {

            var list = new List<Test>(){
                new Test{
                  //  Test = "dssda",
                    test2 = null
                },
                new Test{
                    //Test = "dssdass",
                   test2 = true
                }
            };


            var test = 0.9650000000;
            var h = 100.0;
            var round = Decimal.Round((decimal)test * (decimal)h, 1);
            Console.WriteLine(round);

           



            Console.ReadKey();
        }

    }
}