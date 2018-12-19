

using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Threading.Tasks;  
using Microsoft.AspNetCore.Mvc;  
using Microsoft.AspNetCore.Mvc.RazorPages;  
using System.Data;
  
namespace crimes.Pages  
{  
    public class CrimesTop10Model : PageModel  
    {  
        public List<Models.Crime> CrimeList { get; set; }
				public Exception EX { get; set; }
                
                
       public static double RoundUp(double input, int places)
      {
            double multiplier = Math.Pow(10, Convert.ToDouble(places));
            return Math.Ceiling(input * multiplier) / multiplier;
       }
  
        public void OnGet()  
        {  
				  List<Models.Crime> crimes = new List<Models.Crime>();
					
					// clear exception:
					EX = null;
					
					try
					{
						string sql = string.Format(@"
	
SELECT  TOP 10  Crimes.IUCR,  Count(Crimes.IUCR) AS TotalCrimes, PrimaryDesc, SecondaryDesc, ROUND(AVG(CONVERT(float,Arrested))*100.0,  2) AS arrest,  ROUND((CONVERT(float,COUNT(*)) * 100.0 / (Select count(*) from Crimes)),2) AS percentage
FROM Crimes
INNER JOIN Codes ON Crimes.IUCR = Codes.IUCR
GROUP BY Crimes.IUCR, PrimaryDesc, SecondaryDesc
ORDER BY TotalCrimes DESC;
	");
    
    DataSet ds = DataAccessTier.DB.ExecuteNonScalarQuery(sql);
    
string sql2 = string.Format(@"

SELECT Crimes.IUCR,  Count(Crimes.IUCR) AS TotalCrimes, PrimaryDesc, SecondaryDesc
FROM Crimes
INNER JOIN Codes ON Crimes.IUCR = Codes.IUCR
GROUP BY Crimes.IUCR, PrimaryDesc, SecondaryDesc
	");

                        double sum= 0.0;
                        double temp= 0.0;
                        double crimePer = 0.0;
                        
                        DataSet ds2 = DataAccessTier.DB.ExecuteNonScalarQuery(sql2);
                        
                        foreach (DataRow row in ds2.Tables["TABLE"].Rows)
						{
                                     temp = Convert.ToDouble(row["TotalCrimes"]);
                                     sum = sum + temp;
						}
 

						foreach (DataRow row in ds.Tables["TABLE"].Rows)
						{
							Models.Crime c = new Models.Crime();

							c.IUCR = Convert.ToString(row["IUCR"]);
							c.TotalCrimes = Convert.ToInt32(row["TotalCrimes"]);
                            temp = Convert.ToDouble(row["TotalCrimes"]);
                            c.PrimaryDesc = Convert.ToString(row["PrimaryDesc"]);
                            c.SecondaryDesc = Convert.ToString(row["SecondaryDesc"]);
                            c.arrestPercentage = Convert.ToDouble(row["arrest"]);
                            
                            crimePer=  (temp/sum)*100; 
                            
                            crimePer = RoundUp( crimePer, 2);
                            c.crimePercentage =  Convert.ToDouble(row["percentage"]);
                            
							crimes.Add(c);
						}
					}
					catch(Exception ex)
					{
					  EX = ex;
					}
					finally
					{
             CrimeList = crimes;
				  }
        }  
				
    }//class
}//namespace