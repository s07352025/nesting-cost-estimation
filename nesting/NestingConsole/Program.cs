using NestingLibPort;
using NestingLibPort.Data;
using NestingLibPort.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace NestingConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            NestPath bin = new NestPath();
            double binWidth =1000;
            double binHeight = 1000;
            bin.add(0, 0);
            bin.add(binWidth, 0);
            bin.add(binWidth, binHeight);
            bin.add(0, binHeight);
            Console.WriteLine("Bin Size : Width = " + binWidth + " Height=" + binHeight);
            var nestPaths = SvgUtil.transferSvgIntoPolygons("test.xml");
            Console.WriteLine("Reading File = test.xml");
            Console.WriteLine("No of parts = " + nestPaths.Count);
            Config config = new Config();
            Console.WriteLine("Configuring Nest");
            Nest nest = new Nest(bin, nestPaths, config, 9);
            Console.WriteLine("Performing Nest");
            List<List<Placement>> appliedPlacement = nest.startNest();
            Console.WriteLine("Nesting Completed");
            //
            string nestP = string.Join(Environment.NewLine,nestPaths);
            //Console.WriteLine(nestP);//數據
            String nestP_count = Regex.Matches(nestP, "id").Count.ToString(); 
            Console.WriteLine(nestP_count);
            //
            var svgPolygons =  SvgUtil.svgGenerator(nestPaths, appliedPlacement, binWidth, binHeight);
            //
            string svgP = string.Join(Environment.NewLine,svgPolygons);
            Console.WriteLine(svgP);//數據
            String svgP_count = Regex.Matches(svgP, "<path").Count.ToString();
            Console.WriteLine(svgP_count);
            
            SvgUtil.saveXMLFile(svgP);
            //
            while(nestP_count != svgP_count){
                nestP_count="0";
                svgP_count="1";
                Console.WriteLine("Bin Size : Width = " + binWidth + " Height=" + binHeight);
                nestPaths = SvgUtil.transferSvgIntoPolygons("test.xml");
                Console.WriteLine("Reading File = test.xml");
                Console.WriteLine("No of parts = " + nestPaths.Count);
                config = new Config();
                Console.WriteLine("Configuring Nest");
                nest = new Nest(bin, nestPaths, config, 9);
                Console.WriteLine("Performing Nest");
                appliedPlacement = nest.startNest();
                Console.WriteLine("Nesting Completed");
                nestP = string.Join(Environment.NewLine,nestPaths);
                nestP_count = Regex.Matches(nestP, "id").Count.ToString(); 
                Console.WriteLine(nestP_count);
                svgPolygons =  SvgUtil.svgGenerator(nestPaths, appliedPlacement, binWidth, binHeight);
                svgP = string.Join(Environment.NewLine,svgPolygons);
                svgP_count = Regex.Matches(svgP, "<path").Count.ToString(); 
                Console.WriteLine(svgP_count);
            }
            //
            Console.WriteLine("Converted to SVG format");
            SvgUtil.saveSvgFile(svgPolygons, "output.svg");            
            Console.WriteLine("Saved svg file..Opening File");
            Process.Start("output.svg");
            Console.ReadLine();
        }
    }
}
