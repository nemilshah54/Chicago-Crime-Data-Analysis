using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Threading.Tasks;  
using Microsoft.AspNetCore.Mvc;  
using Microsoft.AspNetCore.Mvc.RazorPages;  
using System.Data;
  
namespace crimes.Pages  
{  
    public class Top10ByAreaModel : PageModel  
    {  
        public List<Models.Crime> CrimeList { get; set; }
        public string Input { get; set; }
        public int NumCrimes { get; set; }
	    public Exception EX { get; set; }
        
        public static double RoundUp(double input, int places)
        {
            double multiplier = Math.Pow(10, Convert.ToDouble(places));
            return Math.Ceiling(input * multiplier) / multiplier;
        }
  
        public void OnGet(string input)  
        {  
				  List<Models.Crime> crimes = new List<Models.Crime>();
					
                    
                    // make input available to web page:
				//	Input = input;
                    
                    
					// clear exception:
					EX = null;
					
					try
					{
                    //
						// Do we have an input argument?  If so, we do a lookup:
						//
						if (input == null)
						{
							//
							// there's no page argument, perhaps user surfed to the page directly?  
							// In this case, nothing to do.
							//
						}
						else  
						{
							// 
							// Lookup movie(s) based on input, which could be id or a partial name:
							// 
							int id;
							string sql;
                            string sql2;
                            double sum= 0.0;
                            double temp= 0.0;
                           double crimePer = 0.0;

							if (System.Int32.TryParse(input, out id))
							{
								// lookup movie by movie id:
								sql = string.Format(@"
	SELECT  TOP 10  Crimes.IUCR,  Count(Crimes.IUCR) AS TotalCrimes, PrimaryDesc, SecondaryDesc, ROUND(AVG(CONVERT(float,Arrested))*100.0,  2) AS arrest,  ROUND((CONVERT(float,COUNT(*)) * 100.0 / (Select count(*) from Crimes)),2) AS percentage
FROM Crimes
INNER JOIN Codes ON Crimes.IUCR = Codes.IUCR
INNER JOIN Areas ON Crimes.Area= Areas.Area
WHERE Areas.Area = '{0}'
GROUP BY Crimes.IUCR, PrimaryDesc, SecondaryDesc
ORDER BY TotalCrimes DESC;
	", id);
    
   sql2 = string.Format(@"

SELECT Crimes.IUCR,  Count(Crimes.IUCR) AS TotalCrimes, PrimaryDesc, SecondaryDesc, ROUND(AVG(CONVERT(float,Arrested))*100.0,  2) AS arrest, ROUND((CONVERT(float,COUNT(*)) * 100.0 / (Select count(*) from Crimes)),2) AS percentage
FROM Crimes
INNER JOIN Codes ON Crimes.IUCR = Codes.IUCR
INNER JOIN Areas ON Crimes.Area= Areas.Area
WHERE Areas.Area = '{0}'
GROUP BY Crimes.IUCR, PrimaryDesc, SecondaryDesc
	", id);
    
    
							}
							else
							{
								// lookup movie(s) by partial name match:
								input = input.Replace("'", "''");

								sql = string.Format(@"
	SELECT  TOP 10  Crimes.IUCR,  Count(Crimes.IUCR) AS TotalCrimes, PrimaryDesc, SecondaryDesc, ROUND(AVG(CONVERT(float,Arrested))*100.0,  2) AS arrest,  ROUND((CONVERT(float,COUNT(*)) * 100.0 / (Select count(*) from Crimes)),2) AS percentage
FROM Crimes
INNER JOIN Codes ON Crimes.IUCR = Codes.IUCR
INNER JOIN Areas ON Crimes.Area= Areas.Area
WHERE Areas.AreaName = '{0}'
GROUP BY Crimes.IUCR, PrimaryDesc, SecondaryDesc
ORDER BY TotalCrimes DESC;
	", input);
    
    
       sql2 = string.Format(@"

SELECT Crimes.IUCR,  Count(Crimes.IUCR) AS TotalCrimes, PrimaryDesc, SecondaryDesc, ROUND(AVG(CONVERT(float,Arrested))*100.0,  2) AS arrest,  ROUND((CONVERT(float,COUNT(*)) * 100.0 / (Select count(*) from Crimes)),2) AS percentage
FROM Crimes
INNER JOIN Codes ON Crimes.IUCR = Codes.IUCR
INNER JOIN Areas ON Crimes.Area= Areas.Area
WHERE Areas.AreaName = '{0}'
GROUP BY Crimes.IUCR, PrimaryDesc, SecondaryDesc
	", input);
							}
                            
                             DataSet ds = DataAccessTier.DB.ExecuteNonScalarQuery(sql);
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
					}
					catch(Exception ex)
					{
					  EX = ex;
					}
					finally
					{
             CrimeList = crimes;
             NumCrimes = crimes.Count;
				  }
        }   
				
    }//class
}//namespace