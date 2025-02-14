using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class CategoryDAO
    {
        public static List<Category> GetCategories()
        {
            var ListCategory = new List<Category>();    
            try
            {
                using (var context = new eStoreDbContext())
                {
                    ListCategory = context.categories.ToList();
                }
            }catch (Exception ex) { 
            throw new Exception(ex.Message);
            }return ListCategory;
        }
       
        

      


    }
}
