using System;
using System.Xml;
using System.Xml.Schema;

namespace XSDVal
{
  class Program
  {

    // xsdval.exe "path to xsd" "path to xml"
    static void Main(string[] args)
    {
      try
      {

        Console.WriteLine("XSDVal - Validates an XML file against an XSD.");
        Console.WriteLine("\tUSAGE - xsdval.exe [path_to_XSD] [path_to_XML]");

        if(args.Length < 2){
            Console.WriteLine("ERROR - Please provide [path_to_XSD] and [path_to_XML].");
            return;
        }

        var xsdPath = args[0];
        var xmlPath = args[1];
        XSDValidator.Validate(xsdPath, xmlPath);
      }
      catch (Exception ex)
      {
        Console.WriteLine("ERROR - " + ex.Message);
      }
    }
  }

  public class XSDValidator
  {
    public static void Validate(string xsdPath, string xmlPath)
    {
      XmlReaderSettings schemaSettings = new XmlReaderSettings();
      schemaSettings.Schemas.Add(null, xsdPath);
      schemaSettings.ValidationType = ValidationType.Schema;
      schemaSettings.ValidationEventHandler += new ValidationEventHandler(SchemaValidationEventHandler);

      schemaSettings.ValidationFlags =
          XmlSchemaValidationFlags.ReportValidationWarnings |
          XmlSchemaValidationFlags.ProcessIdentityConstraints |
          XmlSchemaValidationFlags.ProcessInlineSchema |
          XmlSchemaValidationFlags.ProcessSchemaLocation;

      XmlReader xml = XmlReader.Create(xmlPath, schemaSettings);

      while (xml.Read()) { }
    }

    static void SchemaValidationEventHandler(object sender, ValidationEventArgs e)
    {
      string text = $"[Line: {e.Exception?.LineNumber}, Column: {e.Exception?.LinePosition}]: {e.Message}";
      if (e.Severity == XmlSeverityType.Warning)
      {
        Console.Write("WARNING: ");
        Console.WriteLine(text);
      }
      else if (e.Severity == XmlSeverityType.Error)
      {
        Console.Write("ERROR: ");
        Console.WriteLine(text);
      }
    }
  }
}

