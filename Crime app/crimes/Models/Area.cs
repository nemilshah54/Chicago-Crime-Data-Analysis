//
// One movie
//

namespace crimes.Models
{

  public class Area
	{
	
		// data members with auto-generated getters and setters:
        public int areaNo { get; set; }
		public string areaName { get; set; }
		public int TotalCrimes { get; set; }
        public double crimePercentage  { get; set; }
       
		// default constructor:
		public Area()
		{ }
		
		// constructor:
		public Area( int areaNo, string areaName, int  TotalCrimes,  double crimePercentage/*, double arrestPercentage */   )
		{
			this.areaNo= areaNo;
            this.areaName= areaName;
            this.TotalCrimes= TotalCrimes;
            this.crimePercentage = crimePercentage;
		}
		
	}//class

}//namespace