using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

using Wexman.Design;
using System.Drawing.Design;
using System.Drawing;
using System.Windows.Forms.Design;

namespace Example
{
    public class ExampleClass
    {
        public ExampleClass()
        {
            Example1 = new Dictionary<string, string>();
            Example2 = new Dictionary<int, Color>();
            Example3 = new Dictionary<StringAlignment,ExampleClass>();
        }

        [Editor(typeof(GenericDictionaryEditor<string,string>), typeof(UITypeEditor))]
        [GenericDictionaryEditor(ValueEditorType = typeof(FileNameEditor), Title="Where are your files located?", KeyDisplayName="Keyword", ValueDisplayName="Filename")]
        [Description("A Dictionary<string,string> that uses a FileNameEditor for the value")]
        public Dictionary<string, string> Example1 { get; set; }

        [Editor(typeof(GenericDictionaryEditor<int, Color>), typeof(UITypeEditor))]
        [GenericDictionaryEditor(Title="Color your numbers", KeyDefaultProviderType=typeof(FourtyTwo), ValueDefaultProviderType=typeof(Red))]
        [Description("A Dictionary<int, Color> that has custom DefaultProviders for key and value")]
        public Dictionary<int, Color> Example2 { get; set; }

        [Editor(typeof(GenericDictionaryEditor<StringAlignment, ExampleClass>), typeof(UITypeEditor))]
        [GenericDictionaryEditor(ValueConverterType=typeof(ExpandableObjectConverter))]
        [Description("A Dictionary<StringAlignment, ExampleClass> with an ExpandableObjectConverter for the value")]
        public Dictionary<StringAlignment, ExampleClass> Example3 { get; set; }

    }
  
}
