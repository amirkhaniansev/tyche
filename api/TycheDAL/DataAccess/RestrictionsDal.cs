using Tyche.TycheDAL.Context;

namespace Tyche.TycheDAL.DataAccess
{
    public class RestrictionsDal : BaseDal
    {
        public RestrictionsDal(string connectionString, TycheContext context = null) 
            : base(connectionString, context)
        {
        }
    }
}