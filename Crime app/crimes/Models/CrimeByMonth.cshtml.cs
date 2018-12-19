
// Nemil Shah
// netid: nshah213
// UIN : 670897116.

using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Threading.Tasks;  
using Microsoft.AspNetCore.Mvc;  
using Microsoft.AspNetCore.Mvc.RazorPages;  
using System.Data;
  
namespace crimes.Pages  
{  
    public class CrimeByMonthModel : PageModel  
    {  
      public List<string> Months;
      public List<int> NumCrimes;
	  public Exception EX { get; set; }

        public void OnGet()  
        {   
                    Months  = new List<string>();
                    NumCrimes  = new List<int>();
     
					// clear exception:
					EX = null;
					
					try
					{
                    
                    // Query Builder
						string sql = string.Format(@"
	
SELECT MONTH (CrimeDate) AS CrimeMonth, Count (MONTH (CrimeDate)) AS Total
FROM Crimes
GROUP BY MONTH (CrimeDate)
ORDER BY MONTH (CrimeDate) ASC;
	");
    
    DataSet ds = DataAccessTier.DB.ExecuteNonScalarQuery(sql);
    
    // Array String of Months here.
    
    
     string[] m = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September",
                                 "October", "November", "December"};
                        
                        int i =0;
						foreach (DataRow row in ds.Tables["TABLE"].Rows)
						{
                            
							String c = null;;
                            int numCrime = 0;    
                            c = m[i];
                            numCrime = Convert.ToInt32(row["Total"]);
                            Months.Add( c);
                            NumCrimes.Add(numCrime);
                            i++;
						}
					}
					catch(Exception ex)
					{
					  EX = ex;
					}
					finally
					{
                       //  Months = months;
                        // NumCrimes = numCrimes;
				  }
        }  
				
    }//class
}//namespace