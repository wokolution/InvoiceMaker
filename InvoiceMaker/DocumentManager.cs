using System;
using System.Collections.Generic;
using Android.Webkit;
using Android.Print;
using Android.App;
using Android.Widget;
using Android.OS;
using System.Text;
using Android.Content.Res;
using System.IO;

namespace InvoiceMaker
{
	public class DocumentManager:NewInvoiceActivity
	{

		public DocumentManager ()
		{
		}

		public static string buildHTMLInvoice (string firma,AssetManager asset)
		{
			string content = String.Empty;
			using (StreamReader sr = new StreamReader (asset.Open ("index.html")))
			{
				content = sr.ReadToEnd ();
			}
			content = content.Replace ("TEST", "OK");
			String htmlDocument =content ;//"<html><body><h1>Rechnung</h1><p>" + firma + "</p></body></html>";
			return htmlDocument;
		}

		public static string buildBuchungszeilen (List<Buchungszeilen> buchungszeile,string betragGesamt)
		{
			StringBuilder htmlDocument = new StringBuilder();
			htmlDocument.Append("<html><body>");
			htmlDocument.Append("<table style=\"width:100%\">");
			htmlDocument.Append("<tr>");
			htmlDocument.Append("<td>Produkt</td>");
			htmlDocument.Append("<td>Anzahl</td>");
			htmlDocument.Append("<td>Preis</td>");
			htmlDocument.Append("<td>Summe</td>");
			htmlDocument.Append("</tr>");
			foreach (Buchungszeilen b in buchungszeile) {
				
				htmlDocument.Append("<tr>");
				htmlDocument.Append("<td>"+b.Produkt+"</td>");
				htmlDocument.Append("<td>"+b.Anzahl+"</td>");
				htmlDocument.Append("<td>"+b.Preis+"</td>");
				htmlDocument.Append("<td>"+b.BetragEinzel+"</td>");
				htmlDocument.Append("</tr>");	

							}
			htmlDocument.Append("<tr>");
			htmlDocument.Append("<td></td>");
			htmlDocument.Append("<td></td>");
			htmlDocument.Append("<td></td>");
			htmlDocument.Append("<td><b>"+betragGesamt+"<b></td>");
			htmlDocument.Append("</tr>");	
			htmlDocument.Append("</table>");
			htmlDocument.Append("</body></html>");
			return htmlDocument.ToString();
		}



	}
}

