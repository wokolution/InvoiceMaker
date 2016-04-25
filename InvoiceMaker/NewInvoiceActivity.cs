
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Webkit;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Print;
using Android.Views.InputMethods;
using Android.Content.Res;

namespace InvoiceMaker
{
	[Activity (Label = "Rechnung", MainLauncher = true, Icon = "@mipmap/icon")]			
	public class NewInvoiceActivity : Activity 
	{
		private decimal betragEinzel;
		private decimal betragGesamt;
		private List<Buchungszeilen> buchungen = new List<Buchungszeilen> ();

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.NewInvoice);
			// Create your application here



			EditText editTextFirma = FindViewById<EditText> (Resource.Id.editTextFirma);
			EditText editTextAnzahl = FindViewById<EditText> (Resource.Id.editTextAnzahl);
			EditText editTextPreis = FindViewById<EditText> (Resource.Id.editTextPreis);
			EditText editTextProdukt = FindViewById<EditText> (Resource.Id.editTextProdukt);
			EditText editTextDatum = FindViewById<EditText> (Resource.Id.editTextDatum);
			EditText editTextNummer = FindViewById<EditText> (Resource.Id.editTextNummer);

			editTextNummer.Text = DateTime.Now.Year.ToString () + DateTime.Now.Month.ToString ().PadLeft(2,'0');

			editTextDatum.Text = DateTime.Now.ToShortDateString ();

			var webView = FindViewById<WebView> (Resource.Id.webViewRechnung);
			webView.SetWebViewClient (new WebViewClient ());

			PrintManager printMgr = (PrintManager)GetSystemService (Context.PrintService);

			Button button = FindViewById<Button> (Resource.Id.buttonCreateInvoice);
			button.Click += delegate {
				
				var htmlDocument = DocumentManager.buildHTMLInvoice (editTextFirma.Text,this.Assets);

				webView.LoadDataWithBaseURL (null, htmlDocument, "text/HTML", "UTF-8", null);
				printMgr.Print ("Rechnung", webView.CreatePrintDocumentAdapter (), null);

				
			};

			Button buttonConfirm = FindViewById<Button> (Resource.Id.buttonConfirm);
			buttonConfirm.Click += delegate {
				try{
				betragEinzel = Convert.ToDecimal (editTextPreis.Text.Replace ('.', ',')) * Convert.ToInt32 (editTextAnzahl.Text);
				betragGesamt += betragEinzel;

				buchungen.Add (new Buchungszeilen {
					Preis = Convert.ToDecimal (editTextPreis.Text),
					Produkt = editTextProdukt.Text,
					Anzahl = Convert.ToInt32 (editTextAnzahl.Text),
					BetragEinzel = betragEinzel
				});
				var htmlDocument = DocumentManager.buildBuchungszeilen (buchungen, betragGesamt.ToString ());
				webView.LoadDataWithBaseURL (null, htmlDocument, "text/HTML", "UTF-8", null);

				}catch{
					AlertDialog.Builder alert  = new AlertDialog.Builder(this);
					alert.SetMessage("Produkt, Anzahl u. Preis müssen ausgefüllt sein!");
					alert.SetTitle("Hinweis");
					alert.SetCancelable(true);
					alert.Show();
				}


			};
		
	}
		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.InvoiceMakerMenu, menu);
			return base.OnCreateOptionsMenu(menu);
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			switch (item.ItemId)
			{
			case Resource.Id.actionNew:
				this.Finish ();
				StartActivity(typeof(NewInvoiceActivity));
				return true;
			case Resource.Id.actionClose:
				this.Finish ();
				return true;
			
			default :
				return base.OnOptionsItemSelected(item);
			}
		}
	}}

