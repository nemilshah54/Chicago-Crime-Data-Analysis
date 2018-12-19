//
// One movie
//

namespace crimes.Models
{

  public class Crime
	{
	
		// data members with auto-generated getters and setters:
	 // public int MovieID { get; set; }
		public string IUCR { get; set; }
		public int TotalCrimes { get; set; }
		public string PrimaryDesc { get; set; }
		public string SecondaryDesc  { get; set; }
       public double crimePercentage  { get; set; }
       public double arrestPercentage  { get; set; }
       
	
		// default constructor:
		public Crime()
		{ }
		
		// constructor:
		public Crime( string IUCR, int  TotalCrimes, string  PrimaryDesc, string SecondaryDesc, double crimePercentage, double arrestPercentage   )
		{
			this.IUCR= IUCR;
            this.TotalCrimes= TotalCrimes;
            this.PrimaryDesc= PrimaryDesc;
            this.SecondaryDesc= SecondaryDesc;
             this.crimePercentage = crimePercentage;
            this. arrestPercentage =  arrestPercentage ;
     
		}
		
	}//class

}//namespace