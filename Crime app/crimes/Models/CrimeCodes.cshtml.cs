using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Threading.Tasks;  
using Microsoft.AspNetCore.Mvc;  
using Microsoft.AspNetCore.Mvc.RazorPages;  
using System.Data;
  
namespace crimes.Pages  
{  
    public class CrimeCodesModel : PageModel  
    {  
        public List<Models.CrimeCode> CrimeList { get; set; }
		public Exception EX { get; set; }
  
        public void OnGet()  
        {  
				  List<Models.CrimeCode> codes = new List<Models.CrimeCode>();
					
					// clear exception:
					EX = null;
					
					try
					{
						string sql = string.Format(@"
	
SELECT Crimes.IUCR,  Count(Crimes.IUCR) AS TotalCrimes, PrimaryDesc, SecondaryDesc
FROM Crimes
RIGHT JOIN Codes ON Crimes.IUCR = Codes.IUCR
GROUP BY Crimes.IUCR, PrimaryDesc, SecondaryDesc
ORDER BY PrimaryDesc ASC;
	");
    
    DataSet ds = DataAccessTier.DB.ExecuteNonScalarQuery(sql);

						foreach (DataRow row in ds.Tables["TABLE"].Rows)
						{
							Models.CrimeCode c = new Models.CrimeCode();

							c.IUCR = Convert.ToString(row["IUCR"]);
							c.TotalCrimes = Convert.ToInt32(row["TotalCrimes"]);
                            c.PrimaryDesc = Convert.ToString(row["PrimaryDesc"]);
                            c.SecondaryDesc = Convert.ToString(row["SecondaryDesc"]);

							codes.Add(c);
						}
					}
					catch(Exception ex)
					{
					  EX = ex;
					}
					finally
					{
             CrimeList = codes;
				  }
        }  
				
    }//class
}//namespace