using System;

namespace InvoiceMaker
{
	public class Buchungszeilen
	{
		public Buchungszeilen ()
		{
		}

		public string Produkt{ get;set;}
		public int Anzahl{ get; set;}
		public decimal Preis{ get; set;}
		public decimal BetragEinzel{ get; set;}
	}
}

