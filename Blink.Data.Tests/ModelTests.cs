using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Blink.Data.Model;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Blink.Data.Tests
{
    [TestClass]
    public class ModelTests
    {
        [TestMethod]
        public void TestElements()
        {
            var root = new Root();
            var root2 = new Root();

            var folder = new Folder();
            var note = new Note();
            var text = new Text();
            var list = new List();

            list.Values.Add(text);
            note.Values.Add(list);
            folder.Values.Add(note);
            root.Children.Add(folder);




            //var c00 = new Composite();
            //var c01 = new Composite();
            //var s = new Material<string>();

            //c00.Add(c01);
            //c00.Add(s);

            //c01.Add(s);

            //((Composite)c00[1]).Add(s);
        }
    }
}
