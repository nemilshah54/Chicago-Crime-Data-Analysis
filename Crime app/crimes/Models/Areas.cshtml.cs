using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Threading.Tasks;  
using Microsoft.AspNetCore.Mvc;  
using Microsoft.AspNetCore.Mvc.RazorPages;  
using System.Data;
  
namespace crimes.Pages  
{  
    public class AreasModel : PageModel  
    {  
        public List<Models.Area> CrimeList { get; set; }
		public Exception EX { get; set; }
        
        public static double RoundUp(double input, int places)
        {
            double multiplier = Math.Pow(10, Convert.ToDouble(places));
            return Math.Ceiling(input * multiplier) / multiplier;
        }
  
        public void OnGet()  
        {  
				  List<Models.Area> areas = new List<Models.Area>();
					
					// clear exception:
					EX = null;
					
					try
					{
						string sql = string.Format(@"
	
SELECT Areas.Area,  AreaName, Count(Crimes.Area) AS TotalCrimes,  ROUND((CONVERT(float,COUNT(*)) * 100.0 / (Select count(*) from Crimes)),2) AS percentage
FROM Areas
INNER JOIN Crimes ON Crimes.Area = Areas.Area
GROUP BY Areas.Area, AreaName
ORDER BY AreaName ASC;
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
							Models.Area c = new Models.Area();

							c.areaNo = Convert.ToInt32(row["Area"]);
							c.TotalCrimes= Convert.ToInt32(row["TotalCrimes"]);
                            c.areaName = Convert.ToString(row["AreaName"]);
                            temp = Convert.ToDouble(row["TotalCrimes"]);
                            
                            crimePer=  (temp/sum)*100;    
                            crimePer = RoundUp( crimePer, 2);
                    //        c.crimePercentage =  crimePer;
                            c.crimePercentage  =  Convert.ToDouble(row["percentage"]);
							 areas.Add(c);
						}
					}
					catch(Exception ex)
					{
					  EX = ex;
					}
					finally
					{
                      CrimeList = areas;
				  }
        }  
				
    }//class
}//namespace