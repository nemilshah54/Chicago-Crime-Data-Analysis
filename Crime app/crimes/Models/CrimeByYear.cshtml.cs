// Nemil Shah
// netid: nshah213
// UIN : 670897116.
// 

using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Threading.Tasks;  
using Microsoft.AspNetCore.Mvc;  
using Microsoft.AspNetCore.Mvc.RazorPages;  
using System.Data;
  
namespace crimes.Pages  
{  
    public class CrimeByYearModel : PageModel  
    {  
           public List<int> Years;
           public List<int> NumCrimes;
           public Exception EX { get; set; }
        
   
  
        public void OnGet(string input)  
        {  
				  Years  = new List<int>();
                  NumCrimes  = new List<int>();
					
                    
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
 
  

							if (System.Int32.TryParse(input, out id))
							{
								// lookup movie by movie id:
								sql = string.Format(@"
SELECT Year, Count(Year) AS Total
FROM Crimes
INNER JOIN Areas ON Crimes.Area= Areas.Area
WHERE Areas.Area = '{0}'
GROUP BY Year
ORDER BY Year ASC
	", id);

							}
							else
							{
								// lookup movie(s) by partial name match:
								input = input.Replace("'", "''");
	sql = string.Format(@"
	SELECT Year, Count(Year) AS Total
FROM Crimes
INNER JOIN Areas ON Crimes.Area= Areas.Area
WHERE Areas.AreaName= '{0}'
GROUP BY Year
ORDER BY Year ASC
	", input);

							}
                            
                             DataSet ds = DataAccessTier.DB.ExecuteNonScalarQuery(sql);
        
                        int i=0;
            
						foreach (DataRow row in ds.Tables["TABLE"].Rows)
						{
							int year = 0;
                            int numCrime = 0;
                            
                           
                            year= Convert.ToInt32(row["Year"]);
                            
                        //    System.Console.WriteLine("year is : {0}", year);  
                            
                            numCrime = Convert.ToInt32(row["Total"]);
                        //    System.Console.WriteLine("Num of Crimes is : {0}",  numCrime );
                            
                            Years.Add( year);
                            NumCrimes.Add(numCrime);
                            
                            i++;
						}
						}
                        
					}
					catch(Exception ex)
					{
					  EX = ex;
					}
					finally
					{
        //     CrimeList = crimes;
        //     NumCrimes = crimes.Count;
				  }
        }   
				
    }//class
}//namespace